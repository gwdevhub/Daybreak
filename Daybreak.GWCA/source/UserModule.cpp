#include "pch.h"
#include "UserModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/UserPayload.h>
#include <json.hpp>
#include <queue>
#include <GWCA/Context/CharContext.h>
#include <GWCA/Context/WorldContext.h>

namespace Daybreak::Modules::UserModule {
    std::queue<std::promise<UserPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    UserPayload GetPayload() {
        UserPayload userPayload;
        auto charContext = GW::GetCharContext();
        auto worldContext = GW::GetWorldContext();
        std::string emailstr(64, '\0');
        auto length = std::wcstombs(&emailstr[0], charContext->player_email, 64);
        emailstr.resize(length);
        userPayload.Email = emailstr;
        userPayload.CurrentKurzickPoints = worldContext->current_kurzick;
        userPayload.CurrentLuxonPoints = worldContext->current_luxon;
        userPayload.CurrentImperialPoints = worldContext->current_imperial;
        userPayload.CurrentBalthazarPoints = worldContext->current_balth;
        userPayload.CurrentSkillPoints = worldContext->current_skill_points;
        userPayload.TotalKurzickPoints = worldContext->total_earned_kurzick;
        userPayload.TotalLuxonPoints = worldContext->total_earned_luxon;
        userPayload.TotalImperialPoints = worldContext->total_earned_imperial;
        userPayload.TotalBalthazarPoints = worldContext->total_earned_balth;
        userPayload.TotalSkillPoints = worldContext->total_earned_skill_points;
        userPayload.MaxKurzickPoints = worldContext->max_kurzick;
        userPayload.MaxLuxonPoints = worldContext->max_luxon;
        userPayload.MaxImperialPoints = worldContext->max_imperial;
        userPayload.MaxBalthazarPoints = worldContext->max_balth;

        return userPayload;
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
                        UserPayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetUserInfo(const httplib::Request&, httplib::Response& res) {
        auto response = std::promise<UserPayload>();

        EnsureInitialized();
        PromiseQueue.emplace(&response);
        json responsePayload = response.get_future().get();

        res.set_content(responsePayload.dump(), "text/json");
    }
}