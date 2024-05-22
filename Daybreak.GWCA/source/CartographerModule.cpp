#include "pch.h"
#include "CartographerModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/GameContainers/Array.h>
#include <GWCA/Context/WorldContext.h>
#include <GWCA/Context/MapContext.h>
#include <GWCA/Context/AgentContext.h>
#include <future>
#include <payloads/CartographerPayload.h>
#include <json.hpp>
#include <queue>
#include <GWCA/GameEntities/Pathing.h>
#include <GWCA/Utilities/Scanner.h>
#include <temp/Size2D.h>

namespace Daybreak::Modules {
    std::string CartographerModule::ApiUri()
    {
        return "/cartographer";
    }

    std::optional<uint32_t> CartographerModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return 0;
    }

    std::optional<CartographerPayload> CartographerModule::GetPayload(const uint32_t)
    {
        CartographerPayload payload;
        const auto worldContext = GW::GetWorldContext();
        const auto agentContext = GW::GetAgentContext();
        for (const auto i : worldContext->cartographed_areas) {
            payload.CartographedAreas.push_back((uint32_t)i);
        }
        payload.MapSize.x = worldContext->h05B4[0];
        payload.MapSize.y = worldContext->h05B4[1];
        payload.MapDims = *(uint32_t*)((uint8_t*)agentContext + 0x124);
        payload.WorldDims.X0 = *(float*)((uint8_t*)agentContext + 0x1B0);
        payload.WorldDims.Y0 = *(float*)((uint8_t*)agentContext + 0x1B4);
        payload.WorldDims.X1 = *(float*)((uint8_t*)agentContext + 0x1B8);
        payload.WorldDims.Y1 = *(float*)((uint8_t*)agentContext + 0x1BC);

        return payload;
    }
}