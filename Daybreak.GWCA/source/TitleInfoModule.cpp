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
    const int MaxTries = 20;
    std::vector<std::tuple<TitleInfoPayload, std::promise<TitleInfoPayload>*, std::wstring*, int>> WaitingList;
    std::queue<std::tuple<uint32_t, std::promise<TitleInfoPayload>>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    std::wstring* GetAsyncName(uint32_t titleTierIndex) {
        const auto worldContext = GW::GetWorldContext();
        const auto tiers = &worldContext->title_tiers;
        const auto tier = &tiers->at(titleTierIndex);
        auto description = new std::wstring();
        GW::UI::AsyncDecodeStr(tier->tier_name_enc, description);
        return description;
    }

    TitleInfoPayload GetPayload(uint32_t id) {
        TitleInfoPayload payload;
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

        payload.TitleId = id;
        payload.TitleTierId = tierIndex;
        payload.CurrentPoints = title->current_points;
        payload.PointsNeededNextRank = title->points_needed_next_rank;
        payload.IsPercentageBased = title->is_percentage_based();
        payload.CurrentTier = tier->tier_number;
        return payload;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promiseRequest = PromiseQueue.front();
                    std::promise<TitleInfoPayload> &promise = std::get<1>(*promiseRequest);
                    uint32_t id = std::get<0>(*promiseRequest);
                    PromiseQueue.pop();
                    try {
                        const auto payload = GetPayload(id);
                        if (payload.TitleId == 0 &&
                            payload.CurrentPoints == 0 &&
                            payload.PointsNeededNextRank == 0 &&
                            payload.TitleTierId == 0) {
                            TitleInfoPayload payload;
                            promise.set_value(payload);
                            continue;
                        }

                        auto name = GetAsyncName(payload.TitleTierId);
                        if (!name) {
                            continue;
                        }

                        WaitingList.emplace_back(payload, &promise, name, 0);
                    }
                    catch (...) {
                        TitleInfoPayload payload;
                        promise.set_value(payload);
                    }
                }

                for (auto i = 0; i < WaitingList.size(); ) {
                    auto item = &WaitingList[i];
                    auto name = std::get<2>(*item);
                    auto& tries = std::get<3>(*item);
                    if (name->empty() &&
                        tries < MaxTries) {
                        tries += 1;
                        i++;
                        continue;
                    }

                    auto promise = std::get<1>(*item);
                    auto payload = std::get<0>(*item);
                    payload.TitleName = Utils::WStringToString(*name);
                    WaitingList.erase(WaitingList.begin() + i);
                    delete(name);
                    promise->set_value(payload);
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetTitleInfo(const httplib::Request& req, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
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

        auto response = new std::tuple<uint32_t, std::promise<TitleInfoPayload>>();
        std::get<0>(*response) = id;
        std::promise<TitleInfoPayload>& promise = std::get<1>(*response);

        EnsureInitialized();
        PromiseQueue.emplace(response);

        json responsePayload = promise.get_future().get();
        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}