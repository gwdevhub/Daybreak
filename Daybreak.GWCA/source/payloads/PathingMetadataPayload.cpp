#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/PathingMetadataPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const PathingMetadataPayload& p) {
        j = json
        {
            {"TrapezoidCount", p.TrapezoidCount},
        };
    }
}