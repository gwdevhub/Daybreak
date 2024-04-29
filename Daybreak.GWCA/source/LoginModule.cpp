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
    LoginPayload* GetPayload() {
        const auto context = GW::GetCharContext();
        auto loginPayload = new LoginPayload();
        if (!context) {
            return loginPayload;
        }

        std::wstring playerEmail(context->player_email);
        std::wstring playerName(context->player_name);
        loginPayload->Email = Daybreak::Utils::WStringToString(playerEmail);
        loginPayload->PlayerName = Daybreak::Utils::WStringToString(playerName);

        return loginPayload;
    }

    void GetLoginInfo(const httplib::Request&, httplib::Response& res) {
        LoginPayload* payload = NULL;
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
            printf("[Login Module] Encountered exception: {%s}", ex->what());
            res.set_content(std::format("Encountered exception: {}", ex->what()), "text/plain");
            res.status = 500;
        }
    }
}