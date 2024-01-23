#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/MapPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const MapPayload& p) {
        j = json
        {
            {"IsLoaded", p.IsLoaded},
            {"Id", p.Id},
            {"InstanceType", p.InstanceType},
            {"Timer", p.Timer},
            {"Campaign", p.Campaign},
            {"Continent", p.Continent},
            {"Region", p.Region},
            {"Type", p.Type},
        };
    }
}