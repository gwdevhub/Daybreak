#pragma once
#include <cstdint>
#include <json.hpp>
#include <payloads/StatePayload.h>

using json = nlohmann::json;

namespace Daybreak {
    struct GameStatePayload {
        std::list<StatePayload> States;
    };

    void to_json(json& j, const GameStatePayload& p);
}