#include <cstdint>
#include <json.hpp>
#include <payloads/GameStatePayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const GameStatePayload& p) {
        j = json
        {
            {"States", p.States},
        };
    }
}