#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/Bag.h>
#include <payloads/InventoryPayload.h>

using json = nlohmann::json;

namespace Daybreak {
    void to_json(json& j, const InventoryPayload& p) {
        j = json
        {
            {"GoldInStorage", p.GoldInStorage},
            {"GoldOnCharacter", p.GoldOnCharacter},
            {"Backpack", p.Backpack},
            {"BeltPouch", p.BeltPouch},
            {"Bags", p.Bags},
            {"EquipmentPack", p.EquipmentPack},
            {"MaterialStorage", p.MaterialStorage},
            {"UnclaimedItems", p.UnclaimedItems},
            {"StoragePanes", p.StoragePanes},
            {"EquippedItems", p.EquippedItems},
        };
    }
}