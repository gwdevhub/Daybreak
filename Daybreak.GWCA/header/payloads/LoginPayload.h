#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct LoginPayload {
        std::string Email;
        std::string PlayerName;
    };

    void to_json(json& j, const LoginPayload& p);
}