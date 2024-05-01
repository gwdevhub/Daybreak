#include "pch.h"
#include "MapModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/MapPayload.h>
#include <json.hpp>
#include <queue>

namespace Daybreak::Modules {
    std::optional<MapPayload> MapModule::GetPayload(const uint32_t) {
        MapPayload mapPayload;
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

        mapPayload.Campaign = static_cast<uint32_t>(mapInfo->campaign);
        mapPayload.Continent = static_cast<uint32_t>(mapInfo->continent);
        mapPayload.Region = mapInfo->region;
        mapPayload.InstanceType = (uint32_t)instanceType;
        mapPayload.IsLoaded = isLoaded;
        mapPayload.Timer = instanceTime;
        mapPayload.Id = (uint32_t)mapId;

        return mapPayload;
    }

    std::string MapModule::ApiUri()
    {
        return "/map";
    }

    std::optional<uint32_t> MapModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return NULL;
    }
}