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
    UserPayload* GetPayload() {
        auto userPayload = new UserPayload();
        auto charContext = GW::GetCharContext();
        auto worldContext = GW::GetWorldContext();
        if (!charContext) {
            return userPayload;
        }

        if (!worldContext) {
            return userPayload;
        }

        std::string emailstr(64, '\0');
        auto length = std::wcstombs(&emailstr[0], charContext->player_email, 64);
        emailstr.resize(length);
        userPayload->Email = emailstr;
        userPayload->CurrentKurzickPoints = worldContext->current_kurzick;
        userPayload->CurrentLuxonPoints = worldContext->current_luxon;
        userPayload->CurrentImperialPoints = worldContext->current_imperial;
        userPayload->CurrentBalthazarPoints = worldContext->current_balth;
        userPayload->CurrentSkillPoints = worldContext->current_skill_points;
        userPayload->TotalKurzickPoints = worldContext->total_earned_kurzick;
        userPayload->TotalLuxonPoints = worldContext->total_earned_luxon;
        userPayload->TotalImperialPoints = worldContext->total_earned_imperial;
        userPayload->TotalBalthazarPoints = worldContext->total_earned_balth;
        userPayload->TotalSkillPoints = worldContext->total_earned_skill_points;
        userPayload->MaxKurzickPoints = worldContext->max_kurzick;
        userPayload->MaxLuxonPoints = worldContext->max_luxon;
        userPayload->MaxImperialPoints = worldContext->max_imperial;
        userPayload->MaxBalthazarPoints = worldContext->max_balth;

        return userPayload;
    }

    void GetUserInfo(const httplib::Request&, httplib::Response& res) {
        UserPayload* payload = NULL;
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
            printf("[Game Module] Encountered exception: {%s}", ex->what());
            res.set_content(std::format("Encountered exception: {}", ex->what()), "text/plain");
            res.status = 500;
        }
    }
}