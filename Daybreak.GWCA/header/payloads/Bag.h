#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <payloads/BagContent.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct Bag {
        std::list<BagContent> Items;
    };

    void to_json(json& j, const Bag& p);
}