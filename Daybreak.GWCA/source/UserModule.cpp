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

namespace Daybreak::Modules {
    std::optional<UserPayload> UserModule::GetPayload(const uint32_t) {
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

    std::string UserModule::ApiUri()
    {
        return "/user";
    }

    std::optional<uint32_t> UserModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return NULL;
    }
}