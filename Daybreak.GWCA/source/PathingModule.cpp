#include "pch.h"
#include "PathingModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/PathingPayload.h>
#include <json.hpp>
#include <queue>
#include <GWCA/GameEntities/Pathing.h>

namespace Daybreak::Modules {
    std::optional<PathingPayload> PathingModule::GetPayload(const uint32_t) {
        PathingPayload pathingPayload;
        pathingPayload.Trapezoids.clear();
        pathingPayload.AdjacencyList.clear();
        if (!GW::Map::GetIsMapLoaded()) {
            return pathingPayload;
        }

        auto pathingMap = GW::Map::GetPathingMap();
        if (!pathingMap) {
            return pathingPayload;
        }

        std::list<PathingTrapezoid> trapezoids;
        std::map<int, std::list<int>> adjacencyMap;
        for (auto i = 0U; i < pathingMap->size(); i++) {
            auto gwPathingMap = pathingMap->at(i);
            for (auto j = 0U; j < gwPathingMap.trapezoid_count; j++) {
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

    std::string PathingModule::ApiUri()
    {
        return "/pathing";
    }

    std::optional<uint32_t> PathingModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return NULL;
    }
}