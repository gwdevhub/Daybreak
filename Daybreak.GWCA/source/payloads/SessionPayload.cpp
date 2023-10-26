#include <cstdint>
#include <json.hpp>
#include <payloads/SessionPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const SessionPayload& p) {
        j = json
        {
            {"FoedKilled", p.FoesKilled},
            {"FoesToKill", p.FoesToKill},
            {"MapId", p.MapId},
            {"InstanceTimer", p.InstanceTimer},
            {"InstanceType", p.InstanceType},
        };
    }
}