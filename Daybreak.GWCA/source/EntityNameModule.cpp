#include "pch.h"
#include "EntityNameModule.h"
#include "payloads/NamePayload.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/GameEntities/Agent.h>
#include <future>
#include <json.hpp>
#include <tuple>
#include <queue>
#include <GWCA/GWCA.h>
#include <GWCA/Context/PreGameContext.h>
#include <GWCA/Managers/AgentMgr.h>
#include <Windows.h>
#include <cstdint>
#include <limits>
#include "Utils.h"

namespace Daybreak::Modules::EntityNameModule {
    std::vector<std::tuple<uint32_t, std::promise<NamePayload>*, std::wstring*>> WaitingList;
    std::queue<std::tuple<uint32_t, std::promise<NamePayload>>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    std::wstring* GetAsyncName(uint32_t id) {
        NamePayload namePayload;
        auto agent = GW::Agents::GetAgentByID(id);
        if (!agent) {
            return nullptr;
        }

        auto agentLiving = agent->GetAsAgentLiving();
        if (!agentLiving) {
            return nullptr;
        }

        auto name = new std::wstring();
        if (!GW::Agents::AsyncGetAgentName(agentLiving, *name)) {
            delete(name);
            return nullptr;
        }

        return name;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promiseRequest = PromiseQueue.front();
                    std::promise<NamePayload> &promise = std::get<1>(*promiseRequest);
                    uint32_t id = std::get<0>(*promiseRequest);
                    PromiseQueue.pop();
                    try {
                        auto name = GetAsyncName(id);
                        if (!name) {
                            continue;
                        }

                        WaitingList.emplace_back(id, &promise, name);
                    }
                    catch (const std::exception& e) {
                        printf("[Entity Name Module] Encountered exception: {%s}", e.what());
                        NamePayload payload;
                        promise.set_value(payload);
                    }
                }

                for (auto i = 0U; i < WaitingList.size(); ) {
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

    void GetName(const httplib::Request& req, httplib::Response& res) {
        uint32_t id = 0;
        auto it = req.params.find("id");
        if (it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing id parameter", "text/plain");
            return;
        }
        else {
            auto idStr = it->second;
            size_t pos = 0;
            auto result = std::stoul(idStr, &pos);
            if (pos != idStr.size()) {
                res.status = 400;
                res.set_content("Invalid id parameter", "text/plain");
                return;
            }

            id = static_cast<uint32_t>(result);
        }

        auto response = std::tuple<uint32_t, std::promise<NamePayload>>();
        std::get<0>(response) = id;
        std::promise<NamePayload>& promise = std::get<1>(response);

        EnsureInitialized();
        PromiseQueue.emplace(&response);

        json responsePayload = promise.get_future().get();
        res.set_content(responsePayload.dump(), "text/json");
    }
}