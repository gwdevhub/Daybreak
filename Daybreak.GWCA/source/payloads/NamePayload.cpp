#include <cstdint>
#include <json.hpp>
#include <payloads/NamePayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const NamePayload& p) {
        j = json
        {
            {"Id", p.Id},
            {"Name", p.Name}
        };
    }
}