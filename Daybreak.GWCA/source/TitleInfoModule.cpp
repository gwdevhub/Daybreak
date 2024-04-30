#include "pch.h"
#include "TitleInfoModule.h"
#include "payloads/TitleInfoPayload.h"
#include <DaybreakModule.h>
#include <Utils.h>
#include <GWCA/GameContainers/Array.h>
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Managers/PlayerMgr.h>
#include <GWCA/Constants/AgentIDs.h>
#include <GWCA/GameEntities/Title.h>
#include <GWCA/Context/WorldContext.h>
#include <GWCA/Constants/Constants.h>
#include <GWCA/Managers/UIMgr.h>

namespace Daybreak::Modules {
    std::string TitleInfoModule::ApiUri()
    {
        return "/titles/info";
    }

    std::optional<TitleInfoPayload> TitleInfoModule::GetPayload(const uint32_t context) {
        TitleInfoPayload payload;
        if (!GW::Map::GetIsMapLoaded()) {
            return std::optional<TitleInfoPayload>();
        }

        const auto title = GW::PlayerMgr::GetTitleTrack((GW::Constants::TitleID)context);
        if (!title) {
            return std::optional<TitleInfoPayload>();
        }

        if (title->current_points == 0 &&
            title->current_title_tier_index == 0 &&
            title->next_title_tier_index == 0 &&
            title->max_title_rank == 0) {
            return std::optional<TitleInfoPayload>();
        }

        const auto tierIndex = title->current_title_tier_index;
        const auto worldContext = GW::GetWorldContext();
        const auto tiers = &worldContext->title_tiers;
        const auto tier = &tiers->at(tierIndex);

        payload.TitleId = context;
        payload.TitleTierId = tierIndex;
        payload.CurrentPoints = title->current_points;
        payload.PointsNeededNextRank = title->points_needed_next_rank;
        payload.IsPercentageBased = title->is_percentage_based();
        payload.CurrentTier = tier->tier_number;
        GW::UI::AsyncDecodeStr(tier->tier_name_enc, &this->name);
        return payload;
    }

    bool TitleInfoModule::CanReturn(const httplib::Request& req, httplib::Response& res, const TitleInfoPayload& payload) {
        return !name.empty();
    }

    std::tuple<std::string, std::string> TitleInfoModule::ReturnPayload(TitleInfoPayload payload) {
        payload.TitleName = Daybreak::Utils::WStringToString(name);
        return std::make_tuple(static_cast<json>(payload).dump(), "application/json");
    }

    std::optional<uint32_t> TitleInfoModule::GetContext(const httplib::Request& req, httplib::Response& res) {
        uint32_t id = 0;
        auto it = req.params.find("id");
        if (it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing id parameter", "text/plain");
            return std::optional<uint32_t>();
        }
        else {
            auto idStr = it->second;
            size_t pos = 0;
            auto result = std::stoul(idStr, &pos);
            if (pos != idStr.size()) {
                res.status = 400;
                res.set_content("Invalid id parameter", "text/plain");
                return std::optional<uint32_t>();
            }

            id = static_cast<uint32_t>(result);
        }

        return id;
    }
}