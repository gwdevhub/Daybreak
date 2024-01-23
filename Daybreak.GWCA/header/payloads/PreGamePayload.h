#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct PreGamePayload {
        int ChosenCharacterIndex;
        std::list<std::string> Characters;
    };

    void to_json(json& j, const PreGamePayload& p);
}