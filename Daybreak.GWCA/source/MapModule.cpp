#include "pch.h"
#include "MapModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/MapPayload.h>
#include <json.hpp>
#include <queue>

namespace Daybreak::Modules::MapModule {
    MapPayload* GetPayload() {
        auto mapPayload = new MapPayload();
        auto isLoaded = GW::Map::GetIsMapLoaded();
        if (!isLoaded) {
            return mapPayload;
        }

        auto instanceType = GW::Map::GetInstanceType();
        auto instanceTime = GW::Map::GetInstanceTime();
        auto mapInfo = GW::Map::GetCurrentMapInfo();
        auto isInCinematic = GW::Map::GetIsInCinematic();
        auto region = GW::Map::GetRegion();
        auto mapId = GW::Map::GetMapID();
        if (!mapInfo) {
            return mapPayload;
        }

        mapPayload->Campaign = static_cast<uint32_t>(mapInfo->campaign);
        mapPayload->Continent = static_cast<uint32_t>(mapInfo->continent);
        mapPayload->Region = mapInfo->region;
        mapPayload->InstanceType = (uint32_t)instanceType;
        mapPayload->IsLoaded = isLoaded;
        mapPayload->Timer = instanceTime;
        mapPayload->Id = (uint32_t)mapId;

        return mapPayload;
    }

    void GetMapInfo(const httplib::Request&, httplib::Response& res) {
        MapPayload* payload = NULL;
        std::exception* ex = NULL;
        volatile bool executing = true;
        GW::GameThread::Enqueue([&res, &executing, &ex, &payload]
            {
                try {
                    payload = GetPayload();

                }
                catch (std::exception e) {
                    ex = &e;
                }

                executing = false;
            });

        while (executing) {
            Sleep(4);
        }

        if (payload) {
            const auto json = static_cast<nlohmann::json>(*payload);
            const auto dump = json.dump();
            res.set_content(dump, "text/json");
            delete(payload);
        }
        else if (ex) {
            printf("[Map Module] Encountered exception: {%s}", ex->what());
            res.set_content(std::format("Encountered exception: {}", ex->what()), "text/plain");
            res.status = 500;
        }
    }
}