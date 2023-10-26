#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct SessionPayload {
        uint32_t FoesKilled = 0;
        uint32_t FoesToKill = 0;
        uint32_t MapId = 0;
        uint32_t InstanceTimer = 0;
        uint32_t InstanceType = 0;
    };

    void to_json(json& j, const SessionPayload& p);
}