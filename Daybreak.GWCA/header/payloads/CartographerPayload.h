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
        struct WorldDims {
            float X0;
            float Y0;
            float X1;
            float Y1;
        } WorldDims;
        uint32_t MapDims;
    };

    void to_json(json& j, const CartographerPayload& p);
}