#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <payloads/PathingTrapezoid.h>
#include <json.hpp>
#include <payloads/PathingPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const PathingPayload& p) {
        j = json
        {
            {"Trapezoids", p.Trapezoids},
            {"AdjacencyList", p.AdjacencyList}
        };
    }
}