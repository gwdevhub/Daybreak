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
    std::wstring* GetAsyncName(uint32_t id, std::vector<uint32_t> modifiers) {
        GW::ItemModifier parsedModifiers[64];
        for (auto i = 0U; i < modifiers.size(); i++) {
            GW::ItemModifier parsedModifier;
            const auto mod = modifiers.at(i);
            parsedModifier.mod = mod;
            parsedModifiers[i] = parsedModifier;
        }

        auto item = GW::Items::GetItemByModelIdAndModifiers(id, parsedModifiers, modifiers.size(), 1, 23);
        if (!item) {
            return nullptr;
        }

        auto name = new std::wstring();
        GW::Items::AsyncGetItemName(item, *name);
        return name;
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
        uint32_t id = 0;
        std::vector<uint32_t> modifiers;
        
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

        std::wstring* name;
        std::exception ex;
        volatile bool executing = true;
        volatile bool exception = false;
        GW::GameThread::Enqueue([&res, &executing, &ex, &name, &id, &modifiers, &exception]
            {
                try {
                    name = GetAsyncName(id, modifiers);
                }
                catch (std::exception e) {
                    ex = e;
                    exception = true;
                }

                executing = false;
            });

        // Wait while executing the name request or while the name has been requested but has not yet been populated
        while (executing ||
            (name && name->empty())) {
            Sleep(4);
        }

        if (!exception && name) {
            NamePayload namePayload;
            namePayload.Name = Daybreak::Utils::WStringToString(*name);
            namePayload.Id = id;
            const auto json = static_cast<nlohmann::json>(namePayload);
            const auto dump = json.dump();
            res.set_content(dump, "text/json");
            delete(name);
        }
        else {
            printf("[Item Name Module] Encountered exception: {%s}", ex.what());
            res.set_content(std::format("Encountered exception: {}", ex.what()), "text/plain");
            res.status = 500;
        }
    }
}