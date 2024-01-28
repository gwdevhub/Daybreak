#include <cstdint>
#include <json.hpp>
#include <payloads/GameStatePayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const GameStatePayload& p) {
        j = json
        {
            {"Camera", p.Camera},
            {"States", p.States},
        };
    }
}