#include "pch.h"
#include "LoginModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Context/CharContext.h>
#include <future>
#include <payloads/LoginPayload.h>
#include <json.hpp>
#include <queue>

namespace Daybreak::Modules::LoginModule {
    std::queue<std::promise<LoginPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    LoginPayload GetPayload() {
        auto context = GW::GetCharContext();
        LoginPayload loginPayload;
        std::string emailstr(64, '\0');
        auto length = std::wcstombs(&emailstr[0], context->player_email, 64);
        emailstr.resize(length);
        loginPayload.Email = emailstr;
    
        std::string playerstr(20, '\0');
        length = std::wcstombs(&playerstr[0], context->player_name, 20);
        playerstr.resize(length);
        loginPayload.PlayerName = playerstr;

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
        auto callbackEntry = new GW::HookEntry;
        auto response = new std::promise<LoginPayload>;

        EnsureInitialized();
        PromiseQueue.emplace(response);
        json responsePayload = response->get_future().get();

        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}