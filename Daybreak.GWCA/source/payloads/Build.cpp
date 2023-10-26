#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/Build.h>
#include <payloads/Attribute.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const Build& p) {
        j = json
        {
            {"Primary", p.Primary},
            {"Secondary", p.Secondary},
            {"Attributes", p.Attributes},
            {"Skills", p.Skills}
        };
    }
}