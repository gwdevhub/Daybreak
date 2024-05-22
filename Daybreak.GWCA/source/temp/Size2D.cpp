#include <cstdint>
#include <temp/Size2D.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak::Temp {
    void to_json(json& j, const Size2D& p) {
        j = json
        {
            {"X", p.x},
            {"Y", p.y},
        };
    }
}