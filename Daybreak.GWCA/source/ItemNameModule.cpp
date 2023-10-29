#include "pch.h"
#include "ItemNameModule.h"
#include "payloads/NamePayload.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/ItemMgr.h>
#include <GWCA/GameEntities/Item.h>
#include <future>
#include <json.hpp>
#include <tuple>
#include <queue>
#include <GWCA/GWCA.h>
#include <Windows.h>
#include <cstdint>
#include <limits>
#include <Utils.h>

namespace Daybreak::Modules::ItemNameModule {
    std::vector<std::tuple<uint32_t, std::promise<NamePayload>*, std::wstring*>> WaitingList;
    std::queue<std::tuple<uint32_t, std::list<uint32_t>, std::promise<NamePayload>>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;


    // TODO #442: Delete this once it is merged into GWCA
    GW::Item* GetItemByModelIdAndModifiers(uint32_t modelid, const std::list<GW::ItemModifier> modifiers, int bagStart, int bagEnd) {
        GW::Bag** bags = GW::Items::GetBagArray();
        GW::Bag* bag = NULL;

        for (int bagIndex = bagStart; bagIndex <= bagEnd; ++bagIndex) {
            bag = bags[bagIndex];
            if (!(bag && bag->items.valid())) continue;
            for (GW::Item* item : bag->items) {
                if (item && item->model_id == modelid && item->mod_struct_size == modifiers.size()) {
                    auto match = true;
                    auto listIt = modifiers.begin();
                    for (uint32_t i = 0; i < item->mod_struct_size; i++) {
                        // Compare each element from the array to the corresponding element in the list
                        if (!(item->mod_struct[i] == *listIt)) {
                            match = false;
                            break;
                        }
                        ++listIt;
                    }

                    if (match) {
                        return item;
                    }
                }
            }
        }

        return NULL;
    }

    std::wstring* GetAsyncName(uint32_t id, std::list<uint32_t> modifiers) {
        std::list<GW::ItemModifier> parsedModifiers;
        for (auto mod : modifiers) {
            GW::ItemModifier parsedModifier;
            parsedModifier.mod = mod;
            parsedModifiers.push_back(parsedModifier);
        }

        auto item = GetItemByModelIdAndModifiers(id, parsedModifiers, 1, 23);
        if (!item) {
            return nullptr;
        }

        auto name = new std::wstring();
        GW::Items::AsyncGetItemByName(item, *name);
        return name;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promiseRequest = PromiseQueue.front();
                    std::promise<NamePayload> &promise = std::get<2>(*promiseRequest);
                    uint32_t id = std::get<0>(*promiseRequest);
                    auto modifiers = std::get<1>(*promiseRequest);
                    PromiseQueue.pop();
                    try {
                        auto name = GetAsyncName(id, modifiers);
                        if (!name) {
                            continue;
                        }

                        WaitingList.emplace_back(id, &promise, name);
                    }
                    catch (...) {
                        NamePayload payload;
                        promise.set_value(payload);
                    }
                }

                for (size_t i = 0; i < WaitingList.size(); ) {
                    auto item = &WaitingList[i];
                    auto name = std::get<2>(*item);
                    if (name->empty()) {
                        i++;
                        continue;
                    }

                    auto promise = std::get<1>(*item);
                    auto id = std::get<0>(*item);
                    WaitingList.erase(WaitingList.begin() + i);
                    NamePayload payload;
                    payload.Id = id;
                    payload.Name = Daybreak::Utils::WStringToString(*name);
                    delete(name);
                    promise->set_value(payload);
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    std::vector<std::string> SplitAndRemoveSpaces(const std::string& s) {
        std::vector<std::string> result;
        std::stringstream ss(s);
        std::string item;

        while (getline(ss, item, ',')) {
            item.erase(std::remove(item.begin(), item.end(), ' '), item.end());
            result.push_back(item);
        }

        return result;
    }

    void GetName(const httplib::Request& req, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
        uint32_t id = 0;
        std::list<uint32_t> modifiers;
        
        auto id_it = req.params.find("id");
        if (id_it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing id parameter", "text/plain");
            return;
        }
        else {
            auto& idStr = id_it->second;
            int parsedId;
            if (!Daybreak::Utils::StringToInt(idStr, parsedId)){
                res.status = 400;
                res.set_content("Invalid id parameter", "text/plain");
                return;
            }

            id = static_cast<uint32_t>(parsedId);
        }

        auto mod_it = req.params.find("modifiers");
        if (mod_it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing modifiers parameter", "text/plain");
            return;
        }
        else {
            auto& modifiersStr = mod_it->second;
            const auto tokens = SplitAndRemoveSpaces(modifiersStr);
            for (const auto& token : tokens) {
                int parsedModifier;
                if (!Daybreak::Utils::StringToInt(token, parsedModifier)) {
                    res.status = 400;
                    res.set_content("Invalid modifier parameter", "text/plain");
                    return;
                }

                modifiers.push_back(static_cast<uint32_t>(parsedModifier));
            }
        }

        auto response = new std::tuple<uint32_t, std::list<uint32_t>, std::promise<NamePayload>>();
        std::get<0>(*response) = id;
        std::promise<NamePayload>& promise = std::get<2>(*response);
        std::get<1>(*response) = modifiers;

        EnsureInitialized();
        PromiseQueue.emplace(response);

        json responsePayload = promise.get_future().get();
        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}