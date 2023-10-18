#include <cstdint>
#include <json.hpp>
#include <payloads/PreGamePayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const PreGamePayload& p) {
        j = json
        {
            {"ChosenCharacterIndex", p.ChosenCharacterIndex},
            {"Characters", p.Characters},
        };
    }
}