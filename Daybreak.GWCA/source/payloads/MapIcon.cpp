#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/MapIcon.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const MapIcon& p) {
        j = json
        {
            {"Id", p.Id},
            {"PosX", p.PosX},
            {"PosY", p.PosY},
            {"Affiliation", p.Affiliation},
        };
    }
}