#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct UserPayload {
        std::string Email;
        uint32_t CurrentKurzickPoints;
        uint32_t TotalKurzickPoints;
        uint32_t CurrentLuxonPoints;
        uint32_t TotalLuxonPoints;
        uint32_t CurrentImperialPoints;
        uint32_t TotalImperialPoints;
        uint32_t CurrentBalthazarPoints;
        uint32_t TotalBalthazarPoints;
        uint32_t CurrentSkillPoints;
        uint32_t TotalSkillPoints;
        uint32_t MaxKurzickPoints;
        uint32_t MaxLuxonPoints;
        uint32_t MaxImperialPoints;
        uint32_t MaxBalthazarPoints;
    };

    void to_json(json& j, const UserPayload& p);
}