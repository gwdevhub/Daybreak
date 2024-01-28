#pragma once
#include <cstdint>
#include <json.hpp>
#include <payloads/StatePayload.h>
#include <payloads/Camera.h>

using json = nlohmann::json;

namespace Daybreak {
    struct GameStatePayload {
        Camera Camera;
        std::list<StatePayload> States;
    };

    void to_json(json& j, const GameStatePayload& p);
}