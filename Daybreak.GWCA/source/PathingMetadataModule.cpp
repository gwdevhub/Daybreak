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
    PathingMetadataPayload* GetPayload() {
        auto pathingPayload = new PathingMetadataPayload();
        pathingPayload->TrapezoidCount = 0;
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

        pathingPayload->TrapezoidCount = count;
        return pathingPayload;
    }

    void GetPathingMetadata(const httplib::Request&, httplib::Response& res) {
        PathingMetadataPayload* payload = NULL;
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
            printf("[Pathing Metadata Module] Encountered exception: {%s}", ex->what());
            res.set_content(std::format("Encountered exception: {}", ex->what()), "text/plain");
            res.status = 500;
        }
    }
}