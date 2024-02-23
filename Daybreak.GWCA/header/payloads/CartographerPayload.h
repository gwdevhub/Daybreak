#pragma once
#include <cstdint>
#include <json.hpp>
#include <temp/Size2D.h>

using json = nlohmann::json;

namespace Daybreak {
    struct CartographerPayload {
        Temp::Size2D MapSize;
        float MapX;
        float MapY;
        std::vector<uint32_t> CartographedAreas;
    };

    void to_json(json& j, const CartographerPayload& p);
}