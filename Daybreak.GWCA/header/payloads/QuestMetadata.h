#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct QuestMetadata {
        uint32_t Id;
        float PosX;
        float PosY;
        uint32_t FromId;
        uint32_t ToId;
    };

    void to_json(json& j, const QuestMetadata& p);
}