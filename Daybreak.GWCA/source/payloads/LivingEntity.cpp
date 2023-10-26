#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/LivingEntity.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const LivingEntity& p) {
        j = json
        {
            {"Id", p.Id},
            {"Timer", p.Timer},
            {"PosX", p.PosX},
            {"PosY", p.PosY},
            {"NpcDefinition", p.NpcDefinition},
            {"PrimaryProfessionId", p.PrimaryProfessionId},
            {"SecondaryProfessionId", p.SecondaryProfessionId},
            {"Level", p.Level},
            {"EntityState", p.EntityState},
            {"EntityAllegiance", p.EntityAllegiance},
        };
    }
}