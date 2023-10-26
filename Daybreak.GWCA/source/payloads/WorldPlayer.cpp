#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/Player.h>
#include <payloads/Title.h>
#include <payloads/WorldPlayer.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const WorldPlayer& p) {
        to_json(j, (Player)p);
        j["Title"] = p.Title;
        j["Name"] = p.Name;
    }
}