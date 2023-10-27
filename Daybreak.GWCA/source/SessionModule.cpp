#include "pch.h"
#include "SessionModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/SessionPayload.h>
#include <json.hpp>
#include <queue>
#include <GWCA/Context/AgentContext.h>
#include <GWCA/Context/WorldContext.h>

namespace Daybreak::Modules::SessionModule {
    std::queue<std::promise<SessionPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    SessionPayload GetPayload() {
        SessionPayload sessionPayload;
        sessionPayload.MapId = (uint32_t)GW::Map::GetMapID();
        sessionPayload.InstanceType = (uint32_t)GW::Map::GetInstanceType();

        const auto worldContext = GW::GetWorldContext();
        if (!worldContext) {
            return sessionPayload;
        }

        sessionPayload.FoesKilled = worldContext->foes_killed;
        sessionPayload.FoesToKill = worldContext->foes_to_kill;
        
        const auto agentContext = GW::GetAgentContext();
        if (!agentContext) {
            return sessionPayload;
        }

        sessionPayload.InstanceTimer = agentContext->instance_timer;
        return sessionPayload;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promise = PromiseQueue.front();
                    PromiseQueue.pop();
                    try {
                        auto payload = GetPayload();
                        promise->set_value(payload);
                    }
                    catch (...) {
                        SessionPayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetSessionInfo(const httplib::Request&, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
        auto response = new std::promise<SessionPayload>;

        EnsureInitialized();
        PromiseQueue.emplace(response);
        json responsePayload = response->get_future().get();

        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}