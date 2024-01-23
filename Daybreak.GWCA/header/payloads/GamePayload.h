#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/MainPlayer.h>
#include <payloads/MapIcon.h>

using json = nlohmann::json;

namespace Daybreak {
    struct GamePayload {
        MainPlayer MainPlayer;
        std::list<Player> Party;
        std::list<WorldPlayer> WorldPlayers;
        std::list<LivingEntity> LivingEntities;
        std::list<MapIcon> MapIcons;
        uint32_t TargetId = 0;
    };

    void to_json(json& j, const GamePayload& p);
}