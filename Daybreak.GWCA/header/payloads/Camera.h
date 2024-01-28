#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct Camera {
        float Yaw;
        float Pitch;
    };

    void to_json(json& j, const Camera& p);
}