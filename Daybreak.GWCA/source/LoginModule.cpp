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
    std::queue<std::promise<LoginPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    LoginPayload GetPayload() {
        auto context = GW::GetCharContext();
        LoginPayload loginPayload;
        std::wstring playerEmail(context->player_email);
        std::wstring playerName(context->player_name);
        loginPayload.Email = Daybreak::Utils::WStringToString(playerEmail);
        loginPayload.PlayerName = Daybreak::Utils::WStringToString(playerName);

        return loginPayload;
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
                        LoginPayload payload;
                        promise->set_value(payload);
                    }
                }
            });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetLoginInfo(const httplib::Request&, httplib::Response& res) {
        auto response = std::promise<LoginPayload>();

        EnsureInitialized();
        PromiseQueue.emplace(&response);
        json responsePayload = response.get_future().get();

        res.set_content(responsePayload.dump(), "text/json");
    }
}