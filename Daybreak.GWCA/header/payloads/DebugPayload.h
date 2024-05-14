#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/MainPlayer.h>
#include <payloads/MapIcon.h>

using json = nlohmann::json;

namespace Daybreak {
    struct DebugPayload {
        std::string GameContextAddress;
        std::string WorldContextAddress;
        std::string AgentContextAddress;
        std::string MapContextAddress;
        std::string CharContextAddress;
    };

    void to_json(json& j, const DebugPayload& p);
}