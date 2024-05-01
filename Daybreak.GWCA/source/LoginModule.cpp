#include "pch.h"
#include "LoginModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Context/CharContext.h>
#include <future>
#include <payloads/LoginPayload.h>
#include <json.hpp>
#include <queue>
#include "Utils.h"

namespace Daybreak::Modules {
    std::optional<LoginPayload> LoginModule::GetPayload(const uint32_t) {
        const auto context = GW::GetCharContext();
        LoginPayload loginPayload;
        if (!context) {
            return loginPayload;
        }

        std::wstring playerEmail(context->player_email);
        std::wstring playerName(context->player_name);
        loginPayload.Email = Daybreak::Utils::WStringToString(playerEmail);
        loginPayload.PlayerName = Daybreak::Utils::WStringToString(playerName);

        return loginPayload;
    }

    std::optional<uint32_t> LoginModule::GetContext(const httplib::Request& req, httplib::Response& res) {
        return 0;
    }

    std::string LoginModule::ApiUri()
    {
        return "/login";
    }
}