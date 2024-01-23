#include <cstdint>
#include <payloads/Attribute.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const Attribute& p) {
        j = json
        {
            {"Id", p.Id},
            {"BaseLevel", p.BaseLevel},
            {"ActualLevel", p.ActualLevel},
            {"DecrementPoints", p.DecrementPoints},
            {"IncrementPoints", p.IncrementPoints},
        };
    }
}