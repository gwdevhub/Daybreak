#include "pch.h"
#include "PathingMetadataModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/PathingMetadataPayload.h>
#include <json.hpp>
#include <queue>
#include <GWCA/GameEntities/Pathing.h>

namespace Daybreak::Modules::PathingMetadataModule {
    std::queue<std::promise<PathingMetadataPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    PathingMetadataPayload GetPayload() {
        PathingMetadataPayload pathingPayload;
        pathingPayload.TrapezoidCount = 0;
        if (!GW::Map::GetIsMapLoaded()) {
            return pathingPayload;
        }

        auto pathingMap = GW::Map::GetPathingMap();
        int count = 0;
        for (int i = 0; i < pathingMap->size(); i++) {
            auto gwPathingMap = pathingMap->at(i);
            count += gwPathingMap.trapezoid_count;
        }

        pathingPayload.TrapezoidCount = count;
        return pathingPayload;
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
                        PathingMetadataPayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetPathingMetadata(const httplib::Request&, httplib::Response& res) {
        auto response = std::promise<PathingMetadataPayload>();

        EnsureInitialized();
        PromiseQueue.emplace(&response);
        json responsePayload = response.get_future().get();

        res.set_content(responsePayload.dump(), "text/json");
    }
}