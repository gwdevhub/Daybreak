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

namespace Daybreak::Modules::LoginModule {
    LoginPayload GetPayload() {
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

    void GetLoginInfo(const httplib::Request&, httplib::Response& res) {
        LoginPayload payload;
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
            printf("[Login Module] Encountered exception: {%s}", ex.what());
            res.set_content(std::format("Encountered exception: {}", ex.what()), "text/plain");
            res.status = 500;
        }
    }
}