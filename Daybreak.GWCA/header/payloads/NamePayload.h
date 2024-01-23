#pragma once
#include <cstdint>
#include <json.hpp>

using json = nlohmann::json;

namespace Daybreak {
    struct NamePayload {
        uint32_t Id = 0;
        std::string Name = "";
    };

    void to_json(json& j, const NamePayload& p);
}