#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/Player.h>
#include <payloads/Title.h>

using json = nlohmann::json;

namespace Daybreak {
    struct WorldPlayer : public Player {
        std::string Name = "";
        Title Title;
    };

    void to_json(json& j, const WorldPlayer& p);
}