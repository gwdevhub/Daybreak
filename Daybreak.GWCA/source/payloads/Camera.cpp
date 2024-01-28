#pragma once
#include <payloads/Camera.h>
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const Camera& p) {
        j = json
        {
            {"Yaw", p.Yaw},
            {"Pitch", p.Pitch}
        };
    }
}