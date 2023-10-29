#include <cstdint>
#include <json.hpp>
#include <payloads/StatePayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const StatePayload& p) {
        j = json
        {
            {"Id", p.Id},
            {"PosX", p.PosX},
            {"PosY", p.PosY},
            {"State", p.State},
            {"Health", p.Health},
            {"Energy", p.Energy}
        };
    }
}