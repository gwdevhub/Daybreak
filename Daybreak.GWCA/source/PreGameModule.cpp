#include "pch.h"
#include "MapModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/PreGamePayload.h>
#include <json.hpp>
#include <queue>
#include <GWCA/GWCA.h>
#include <GWCA/Context/PreGameContext.h>

namespace Daybreak::Modules::PreGameModule {
    std::queue<std::promise<PreGamePayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    PreGamePayload GetPayload() {
        PreGamePayload preGamePayload;
        if (GW::Map::GetIsMapLoaded()) {
            preGamePayload.Characters.clear();
            preGamePayload.ChosenCharacterIndex = 0;
            return preGamePayload;
        }

        auto context = GW::GetPreGameContext();
        for (const auto& loginChar : context->chars) {
            std::string charName(20, '\0');
            auto length = std::wcstombs(&charName[0], loginChar.character_name, 20);
            charName.resize(length);
            preGamePayload.Characters.push_back(charName);
        }

        preGamePayload.ChosenCharacterIndex = context->index_1;
        return preGamePayload;
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
                        PreGamePayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetPreGameInfo(const httplib::Request&, httplib::Response& res) {
        auto response = std::promise<PreGamePayload>();

        EnsureInitialized();
        PromiseQueue.emplace(&response);
        json responsePayload = response.get_future().get();

        res.set_content(responsePayload.dump(), "text/json");
    }
}