#include "pch.h"
#include "InventoryModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Context/ItemContext.h>
#include <GWCA/Managers/ItemMgr.h>
#include <GWCA/GameEntities/Item.h>
#include <future>
#include <payloads/inventoryPayload.h>
#include <json.hpp>
#include <queue>

namespace Daybreak::Modules {
    namespace InventoryModuleInternal {
        Daybreak::Bag GetBag(GW::Bag* bag) {
            Bag retBag;
            if (!bag) {
                return retBag;
            }

            for (const auto& item : bag->items) {
                BagContent bagContent;
                if (!item) {
                    continue;
                }

                bagContent.Id = item->model_id;
                bagContent.Slot = item->slot;
                bagContent.Count = item->quantity;
                auto modifier = item->mod_struct;
                for (auto i = 0U; i < item->mod_struct_size; i++) {
                    bagContent.Modifiers.push_back((uint32_t)(modifier->mod));
                    modifier++;
                }

                retBag.Items.push_back(bagContent);
            }

            return retBag;
        }
    }

    std::optional<InventoryPayload> InventoryModule::GetPayload(const uint32_t) {
        InventoryPayload inventoryPayload;
        if (!GW::Map::GetIsMapLoaded()) {
            return inventoryPayload;
        }

        auto inventory = GW::GetItemContext()->inventory;
        if (!inventory) {
            return inventoryPayload;
        }

        inventoryPayload.GoldInStorage = GW::Items::GetGoldAmountInStorage();
        inventoryPayload.GoldOnCharacter = GW::Items::GetGoldAmountOnCharacter();        
        inventoryPayload.Backpack = InventoryModuleInternal::GetBag(inventory->backpack);
        inventoryPayload.BeltPouch = (InventoryModuleInternal::GetBag(inventory->belt_pouch));
        inventoryPayload.Bags.push_back(InventoryModuleInternal::GetBag(inventory->bag1));
        inventoryPayload.Bags.push_back(InventoryModuleInternal::GetBag(inventory->bag2));
        inventoryPayload.EquipmentPack = (InventoryModuleInternal::GetBag(inventory->equipment_pack));
        inventoryPayload.MaterialStorage = (InventoryModuleInternal::GetBag(inventory->material_storage));
        inventoryPayload.UnclaimedItems = (InventoryModuleInternal::GetBag(inventory->unclaimed_items));
        inventoryPayload.EquippedItems = (InventoryModuleInternal::GetBag(inventory->equipped_items));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage1));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage2));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage3));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage4));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage5));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage6));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage7));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage8));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage9));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage10));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage11));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage12));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage13));
        inventoryPayload.StoragePanes.push_back(InventoryModuleInternal::GetBag(inventory->storage14));
        return inventoryPayload;
    }

    std::optional<uint32_t> InventoryModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return 0;
    }

    std::string InventoryModule::ApiUri()
    {
        return "/inventory";
    }
}