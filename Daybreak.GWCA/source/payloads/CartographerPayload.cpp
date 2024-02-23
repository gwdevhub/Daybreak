#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/CartographerPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const CartographerPayload& p) {
        j = json
        {
            {"MapSize", p.MapSize},
            {"CartographedAreas", p.CartographedAreas},
            {"MapX", p.MapX},
            {"MapY", p.MapY},
        };
    }
}