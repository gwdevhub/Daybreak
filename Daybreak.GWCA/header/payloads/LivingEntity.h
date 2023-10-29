#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct LivingEntity {
        uint32_t Id = 0;
        uint32_t Timer = 0;
        float PosX = 0;
        float PosY = 0;
        uint32_t NpcDefinition = 0;
        uint32_t PrimaryProfessionId = 0;
        uint32_t SecondaryProfessionId = 0;
        uint32_t Level = 0;
        uint32_t EntityState = 0;
        uint32_t EntityAllegiance = 0;
        float Health;
        float Energy;
    };

    void to_json(json& j, const LivingEntity& p);
}