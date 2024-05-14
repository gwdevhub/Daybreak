#include <cstdint>
#include <json.hpp>
#include <payloads/DebugPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const DebugPayload& p) {
        j = json
        {
            {"GameContextAddress", p.GameContextAddress},
            {"WorldContextAddress", p.WorldContextAddress},
            {"MapContextAddress", p.MapContextAddress},
            {"AgentContextAddress", p.AgentContextAddress},
            {"CharContextAddress", p.CharContextAddress},
        };
    }
}