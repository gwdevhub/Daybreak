#include "pch.h"
#include "PathingMetadataModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/GameEntities/Pathing.h>
#include <future>
#include <payloads/PathingMetadataPayload.h>
#include <json.hpp>
#include <queue>

namespace Daybreak::Modules {
    std::optional<PathingMetadataPayload> PathingMetadataModule::GetPayload(const uint32_t) {
        PathingMetadataPayload pathingPayload;
        pathingPayload.TrapezoidCount = 0;
        if (!GW::Map::GetIsMapLoaded()) {
            return pathingPayload;
        }

        auto pathingMap = GW::Map::GetPathingMap();
        if (!pathingMap) {
            return pathingPayload;
        }

        int count = 0;
        for (auto i = 0U; i < pathingMap->size(); i++) {
            auto gwPathingMap = pathingMap->at(i);
            count += gwPathingMap.trapezoid_count;
        }

        pathingPayload.TrapezoidCount = count;
        return pathingPayload;
    }

    std::string PathingMetadataModule::ApiUri()
    {
        return "/pathing/metadata";
    }

    std::optional<uint32_t> PathingMetadataModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return NULL;
    }
}