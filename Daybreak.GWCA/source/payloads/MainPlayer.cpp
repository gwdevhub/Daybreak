#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/WorldPlayer.h>
#include <payloads/QuestMetadata.h>
#include <payloads/MainPlayer.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const MainPlayer& p) {
        to_json(j, (WorldPlayer)p);
        j["CurrentQuest"] = p.CurrentQuest;
        j["QuestLog"] = p.QuestLog;
        j["HardModeUnlocked"] = p.HardModeUnlocked;
        j["Experience"] = p.Experience;
        j["Morale"] = p.Morale;
        j["CurrentHp"] = p.CurrentHp;
        j["MaxHp"] = p.MaxHp;
        j["CurrentEnergy"] = p.CurrentEnergy;
        j["MaxEnergy"] = p.MaxEnergy;
    }
}