#include "pch.h"
#include "InventoryModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Context/ItemContext.h>
#include <GWCA/Managers/ItemMgr.h>
#include <GWCA/GameEntities/Item.h>
#include <future>
#include <payloads/InventoryPayload.h>
#include <json.hpp>
#include <queue>

namespace Daybreak::Modules::InventoryModule {
    std::queue<std::promise<InventoryPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    Daybreak::Bag GetBag(GW::Bag* bag) {
        Bag retBag;
        if (!bag) {
            return retBag;
        }

        for (const auto &item : bag->items) {
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

    InventoryPayload GetPayload() {
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
        inventoryPayload.Backpack = GetBag(inventory->backpack);
        inventoryPayload.BeltPouch = (GetBag(inventory->belt_pouch));
        inventoryPayload.Bags.push_back(GetBag(inventory->bag1));
        inventoryPayload.Bags.push_back(GetBag(inventory->bag2));
        inventoryPayload.EquipmentPack = (GetBag(inventory->equipment_pack));
        inventoryPayload.MaterialStorage = (GetBag(inventory->material_storage));
        inventoryPayload.UnclaimedItems = (GetBag(inventory->unclaimed_items));
        inventoryPayload.EquippedItems = (GetBag(inventory->equipped_items));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage1));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage2));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage3));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage4));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage5));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage6));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage7));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage8));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage9));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage10));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage11));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage12));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage13));
        inventoryPayload.StoragePanes.push_back(GetBag(inventory->storage14));
        return inventoryPayload;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promise = PromiseQueue.front();
                    PromiseQueue.pop();
                    try {
                        auto payload = GetPayload();
                        promise->set_value(payload);
                    }
                    catch(...){
                        InventoryPayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetInventoryInfo(const httplib::Request&, httplib::Response& res) {
        auto response = std::promise<InventoryPayload>();

        EnsureInitialized();
        PromiseQueue.emplace(&response);
        json responsePayload = response.get_future().get();

        res.set_content(responsePayload.dump(), "text/json");
    }
}