#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/WorldPlayer.h>
#include <payloads/QuestMetadata.h>

using json = nlohmann::json;

namespace Daybreak {
    struct MainPlayer : public WorldPlayer {
        uint32_t CurrentQuest = 0;
        std::list<QuestMetadata> QuestLog;
        bool HardModeUnlocked = false;
        uint32_t Experience = 0;
        uint32_t Morale = 0;
        float CurrentHp = 0;
        uint32_t MaxHp = 0;
        float CurrentEnergy = 0;
        uint32_t MaxEnergy = 0;
    };

    void to_json(json& j, const MainPlayer& p);
}