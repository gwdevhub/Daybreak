#pragma once
#include <cstdint>
#include <GWCA/GameEntities/Map.h>
#include <json.hpp>
#include <payloads/Bag.h>

using json = nlohmann::json;

namespace Daybreak {
    struct InventoryPayload {
        uint32_t GoldInStorage;
        uint32_t GoldOnCharacter;
        Bag Backpack;
        Bag BeltPouch;
        Bag EquipmentPack;
        Bag MaterialStorage;
        Bag UnclaimedItems;
        Bag EquippedItems;
        std::list<Bag> Bags;
        std::list<Bag> StoragePanes;
    };

    void to_json(json& j, const InventoryPayload& p);
}