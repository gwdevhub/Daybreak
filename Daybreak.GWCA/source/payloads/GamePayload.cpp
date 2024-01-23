#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/MainPlayer.h>
#include <payloads/MapIcon.h>
#include <payloads/GamePayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const GamePayload& p) {
        j = json
        {
            {"MainPlayer", p.MainPlayer},
            {"Party", p.Party},
            {"WorldPlayers", p.WorldPlayers},
            {"LivingEntities", p.LivingEntities},
            {"MapIcons", p.MapIcons},
            {"TargetId", p.TargetId},
        };
    }
}