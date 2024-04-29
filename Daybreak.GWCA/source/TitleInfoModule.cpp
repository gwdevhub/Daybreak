#include "pch.h"
#include "TitleInfoModule.h"
#include "payloads/TitleInfoPayload.h"
#include <GWCA/GameContainers/Array.h>
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Managers/PlayerMgr.h>
#include <GWCA/Constants/AgentIDs.h>
#include <GWCA/GameEntities/Title.h>
#include <GWCA/Context/WorldContext.h>
#include <GWCA/Constants/Constants.h>
#include <GWCA/Managers/UIMgr.h>
#include <future>
#include <json.hpp>
#include <tuple>
#include <queue>
#include <Windows.h>
#include <cstdint>
#include <limits>
#include "Utils.h"

namespace Daybreak::Modules::TitleInfoModule {
    std::wstring* GetAsyncName(uint32_t titleTierIndex) {
        const auto worldContext = GW::GetWorldContext();
        const auto tiers = &worldContext->title_tiers;
        const auto tier = &tiers->at(titleTierIndex);
        auto description = new std::wstring();
        GW::UI::AsyncDecodeStr(tier->tier_name_enc, description);
        return description;
    }

    TitleInfoPayload* GetPayload(uint32_t id) {
        auto payload = new TitleInfoPayload();
        if (!GW::Map::GetIsMapLoaded()) {
            return payload;
        }

        const auto title = GW::PlayerMgr::GetTitleTrack((GW::Constants::TitleID)id);
        if (!title) {
            return payload;
        }

        if (title->current_points == 0 &&
            title->current_title_tier_index == 0 &&
            title->next_title_tier_index == 0 &&
            title->max_title_rank == 0) {
            return payload;
        }

        const auto tierIndex = title->current_title_tier_index;
        const auto worldContext = GW::GetWorldContext();
        const auto tiers = &worldContext->title_tiers;
        const auto tier = &tiers->at(tierIndex);

        payload->TitleId = id;
        payload->TitleTierId = tierIndex;
        payload->CurrentPoints = title->current_points;
        payload->PointsNeededNextRank = title->points_needed_next_rank;
        payload->IsPercentageBased = title->is_percentage_based();
        payload->CurrentTier = tier->tier_number;
        return payload;
    }

    void GetTitleInfo(const httplib::Request& req, httplib::Response& res) {
        uint32_t id = 0;
        auto it = req.params.find("id");
        if (it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing id parameter", "text/plain");
            return;
        }
        else {
            auto idStr = it->second;
            size_t pos = 0;
            auto result = std::stoul(idStr, &pos);
            if (pos != idStr.size()) {
                res.status = 400;
                res.set_content("Invalid id parameter", "text/plain");
                return;
            }

            id = static_cast<uint32_t>(result);
        }

        TitleInfoPayload* payload = NULL;
        std::wstring* name = NULL;
        std::exception* ex = NULL;
        volatile bool executing = true;
        GW::GameThread::Enqueue([&res, &executing, &ex, &payload, &name, &id]
            {
                try {
                    payload = GetPayload(id);
                    name = GetAsyncName(id);
                }
                catch (std::exception e) {
                    ex = &e;
                }

                executing = false;
            });

        // Wait while executing the name request or while the name has been requested but has not yet been populated
        while (executing ||
            (name && name->empty())) {
            Sleep(4);
        }

        if (name && payload) {
            payload->TitleName = Daybreak::Utils::WStringToString(*name);
            const auto json = static_cast<nlohmann::json>(*payload);
            const auto dump = json.dump();
            res.set_content(dump, "text/json");
            delete(name);
            delete(payload);
        }
        else if (ex) {
            printf("[Item Name Module] Encountered exception: {%s}", ex->what());
            res.set_content(std::format("Encountered exception: {}", ex->what()), "text/plain");
            res.status = 500;
        }
    }
}