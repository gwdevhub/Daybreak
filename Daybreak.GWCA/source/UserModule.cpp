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
    UserPayload GetPayload() {
        UserPayload userPayload;
        auto charContext = GW::GetCharContext();
        auto worldContext = GW::GetWorldContext();
        if (!charContext) {
            return userPayload;
        }

        if (!worldContext) {
            return userPayload;
        }

        char emailStr[64];
        int result = WideCharToMultiByte(CP_UTF8, 0, charContext->player_email, -1, emailStr, sizeof(emailStr), NULL, NULL);
        if (result == 0) {
            // handle error, use GetLastError() to get more info
        }
        userPayload.Email = emailStr;
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

    void GetUserInfo(const httplib::Request&, httplib::Response& res) {
        UserPayload payload;
        std::exception ex;
        volatile bool executing = true;
        volatile bool exception = false;
        GW::GameThread::Enqueue([&res, &executing, &ex, &payload, &exception]
            {
                try {
                    payload = GetPayload();

                }
                catch (std::exception e) {
                    ex = e;
                    exception = true;
                }

                executing = false;
            });

        while (executing) {
            Sleep(4);
        }

        if (!exception) {
            const auto json = static_cast<nlohmann::json>(payload);
            const auto dump = json.dump();
            res.set_content(dump, "text/json");
        }
        else {
            printf("[User Module] Encountered exception: {%s}", ex.what());
            res.set_content(std::format("Encountered exception: {}", ex.what()), "text/plain");
            res.status = 500;
        }
    }
}