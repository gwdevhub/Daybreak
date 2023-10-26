#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/Attribute.h>

using json = nlohmann::json;

namespace Daybreak {
    struct Build {
        uint32_t Primary = 0;
        uint32_t Secondary = 0;
        std::list<Attribute> Attributes;
        std::list<uint32_t> Skills;
    };

    void to_json(json& j, const Build& p);
}