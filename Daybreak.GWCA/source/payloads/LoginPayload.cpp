#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/LoginPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const LoginPayload& p) {
        j = json
        {
            {"Email", p.Email},
            {"PlayerName", p.PlayerName},
        };
    }
}