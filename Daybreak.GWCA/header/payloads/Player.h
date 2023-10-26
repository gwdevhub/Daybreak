#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/LivingEntity.h>
#include <payloads/Build.h>

using json = nlohmann::json;

namespace Daybreak {
    struct Player : public LivingEntity {
        std::list<uint32_t> UnlockedProfession;
        uint32_t NpcDefinition = 0;
        Build Build;
    };

    void to_json(json& j, const Player& p);
}