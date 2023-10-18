#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/PathingTrapezoid.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const PathingTrapezoid& p) {
        j = json
        {
            {"Id", p.Id},
            {"XTL", p.XTL},
            {"XTR", p.XTR},
            {"XBL", p.XBL},
            {"XBR", p.XBR},
            {"YT", p.YT},
            {"YB", p.YB},
        };
    }
}