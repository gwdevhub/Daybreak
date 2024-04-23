#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct StatePayload {
        uint32_t Id = 0;
        float PosX = 0;
        float PosY = 0;
        uint32_t State = 0;
        float Health = 0;
        float Energy = 0;
        float RotationAngle = 0;
    };

    void to_json(json& j, const StatePayload& p);
}