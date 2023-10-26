#include "pch.h"
#include "PathingModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/PathingPayload.h>
#include <json.hpp>
#include <queue>
#include <GWCA/GameEntities/Pathing.h>

namespace Daybreak::Modules::PathingModule {
    std::queue<std::promise<PathingPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    PathingPayload GetPayload() {
        PathingPayload pathingPayload;
        pathingPayload.Trapezoids.clear();
        pathingPayload.AdjacencyList.clear();
        if (!GW::Map::GetIsMapLoaded()) {
            return pathingPayload;
        }

        auto pathingMap = GW::Map::GetPathingMap();
        std::list<PathingTrapezoid> trapezoids;
        std::map<int, std::list<int>> adjacencyMap;
        for (int i = 0; i < pathingMap->size(); i++) {
            auto gwPathingMap = pathingMap->at(i);
            for (int j = 0; j < gwPathingMap.trapezoid_count; j++) {
                auto gwTrapezoid = gwPathingMap.trapezoids[j];
                PathingTrapezoid trapezoid;
                trapezoid.Id = gwTrapezoid.id;
                trapezoid.PathingMapId = i;
                trapezoid.XTL = gwTrapezoid.XTL;
                trapezoid.XBL = gwTrapezoid.XBL;
                trapezoid.XTR = gwTrapezoid.XTR;
                trapezoid.XBR = gwTrapezoid.XBR;
                trapezoid.YT = gwTrapezoid.YT;
                trapezoid.YB = gwTrapezoid.YB;

                trapezoids.push_back(trapezoid);
                for (int k = 0; k < 4; k++) {
                    auto adjacentTrapezoid = gwTrapezoid.adjacent[k];
                    if (adjacentTrapezoid) {
                        adjacencyMap[trapezoid.Id].push_back(adjacentTrapezoid->id);
                    }
                }
            }
        }

        std::list<std::list<int>> adjacencyList;
        for (const auto& pair : adjacencyMap) {
            adjacencyList.push_back(pair.second);
        }

        pathingPayload.Trapezoids = trapezoids;
        pathingPayload.AdjacencyList = adjacencyList;
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
                        promise->set_value(GetPayload());
                    }
                    catch (...) {
                        PathingPayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetPathingData(const httplib::Request&, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
        auto response = new std::promise<PathingPayload>;

        EnsureInitialized();
        PromiseQueue.emplace(response);
        json responsePayload = response->get_future().get();

        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}