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

namespace Daybreak::Modules {
    std::optional<SessionPayload> SessionModule::GetPayload(const uint32_t) {
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

    std::string SessionModule::ApiUri()
    {
        return "/session";
    }

    std::optional<uint32_t> SessionModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return NULL;
    }
}