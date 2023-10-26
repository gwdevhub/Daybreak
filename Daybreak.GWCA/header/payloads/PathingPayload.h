#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <payloads/PathingTrapezoid.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct PathingPayload {
        std::list<PathingTrapezoid> Trapezoids;
        std::list<std::list<int>> AdjacencyList;
    };

    void to_json(json& j, const PathingPayload& p);
}