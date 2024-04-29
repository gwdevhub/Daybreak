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

    void GetSessionInfo(const httplib::Request&, httplib::Response& res) {
        SessionPayload payload;
        std::exception ex;
        volatile bool executing = true;
        volatile bool exception = false;
        GW::GameThread::Enqueue([&res, &executing, &ex, &payload, &exception]
            {
                try {
                    payload = GetPayload();

                }
                catch (std::exception e) {
                    ex = e;
                    exception = true;
                }

                executing = false;
            });

        while (executing) {
            Sleep(4);
        }

        if (!exception) {
            const auto json = static_cast<nlohmann::json>(payload);
            const auto dump = json.dump();
            res.set_content(dump, "text/json");
        }
        else {
            printf("[Session Module] Encountered exception: {%s}", ex.what());
            res.set_content(std::format("Encountered exception: {}", ex.what()), "text/plain");
            res.status = 500;
        }
    }
}