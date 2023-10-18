#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/QuestMetadata.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const QuestMetadata& p) {
        j = json
        {
            {"Id", p.Id},
            {"PosX", p.PosX},
            {"PosY", p.PosY},
            {"FromId", p.FromId},
            {"ToId", p.ToId},
        };
    }
}