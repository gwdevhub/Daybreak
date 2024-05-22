#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak::Temp {
    struct Size2D { // total: 0x8/8
        /* +h0000 */ uint32_t x;
        /* +h0004 */ uint32_t y;
    };

    void to_json(json& j, const Size2D& p);
}