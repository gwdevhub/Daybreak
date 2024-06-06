#include "pch.h"
#include "DebugModule.h"
#include "payloads/DebugPayload.h"
#include <GWCA/GWCA.h>
#include <GWCA/Context/GameContext.h>

namespace Daybreak::Modules {
    std::string DebugModule::ApiUri()
    {
        return "/debug";
    }

    std::optional<uint32_t> DebugModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return 0;
    }

    std::optional<DebugPayload> DebugModule::GetPayload(const uint32_t)
    {
        char buffer[20];
        DebugPayload debugPayload;
        const auto gameContext = GW::GetGameContext();
        std::snprintf(buffer, 20, "%p", static_cast<void*>(gameContext));
        debugPayload.GameContextAddress = std::string(buffer);
        std::snprintf(buffer, 20, "%p", static_cast<void*>(gameContext->agent));
        debugPayload.AgentContextAddress = std::string(buffer);
        std::snprintf(buffer, 20, "%p", static_cast<void*>(gameContext->character));
        debugPayload.CharContextAddress = std::string(buffer);
        std::snprintf(buffer, 20, "%p", static_cast<void*>(gameContext->world));
        debugPayload.WorldContextAddress = std::string(buffer);
        std::snprintf(buffer, 20, "%p", static_cast<void*>(gameContext->map));
        debugPayload.MapContextAddress = std::string(buffer);

        return debugPayload;
    }
}