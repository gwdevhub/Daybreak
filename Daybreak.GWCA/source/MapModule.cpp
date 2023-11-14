#include "pch.h"
#include "MapModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/MapPayload.h>
#include <json.hpp>
#include <queue>

namespace Daybreak::Modules::MapModule {
    std::queue<std::promise<MapPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    MapPayload GetPayload() {
        auto isLoaded = GW::Map::GetIsMapLoaded();
        auto instanceType = GW::Map::GetInstanceType();
        auto instanceTime = GW::Map::GetInstanceTime();
        auto mapInfo = GW::Map::GetCurrentMapInfo();
        auto isInCinematic = GW::Map::GetIsInCinematic();
        auto region = GW::Map::GetRegion();
        auto mapId = GW::Map::GetMapID();
        MapPayload mapPayload;
        mapPayload.Campaign = static_cast<uint32_t>(mapInfo->campaign);
        mapPayload.Continent = static_cast<uint32_t>(mapInfo->continent);
        mapPayload.Region = mapInfo->region;
        mapPayload.InstanceType = (uint32_t)instanceType;
        mapPayload.IsLoaded = isLoaded;
        mapPayload.Timer = instanceTime;
        mapPayload.Id = (uint32_t)mapId;

        return mapPayload;
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
                    catch (...) {
                        MapPayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetMapInfo(const httplib::Request&, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
        auto response = new std::promise<MapPayload>;

        EnsureInitialized();
        PromiseQueue.emplace(response);
        json responsePayload = response->get_future().get();

        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}