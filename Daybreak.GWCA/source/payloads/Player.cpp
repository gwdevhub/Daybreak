#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/LivingEntity.h>
#include <payloads/Build.h>
#include <payloads/Player.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const Player& p) {
        to_json(j, (LivingEntity)p);
        j["UnlockedProfession"] = p.UnlockedProfession;
        j["NpcDefinition"] = p.NpcDefinition;
        j["Build"] = p.Build;
    }
}