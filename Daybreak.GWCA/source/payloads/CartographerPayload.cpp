#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/CartographerPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const CartographerPayload& p) {
        j["MapSize"] = p.MapSize;
        j["CartographedAreas"] = p.CartographedAreas;
        j["MapX"] = p.MapX;
        j["MapY"] = p.MapY;
        j["WorldDims"]["X0"] = p.WorldDims.X0;
        j["WorldDims"]["Y0"] = p.WorldDims.Y0;
        j["WorldDims"]["X1"] = p.WorldDims.X1;
        j["WorldDims"]["Y1"] = p.WorldDims.Y1;
        j["MapDims"] = p.MapDims;
    }
}