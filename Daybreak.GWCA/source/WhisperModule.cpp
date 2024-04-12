#include "pch.h"
#include "WhisperModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Managers/ChatMgr.h>
#include <queue>
#include <string>
#include <Utils.h>

namespace Daybreak::Modules::WhisperModule {
    std::queue<std::wstring> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    void PostMessage(std::wstring message) {
        if (!GW::Map::GetIsMapLoaded()) {
            return;
        }

        GW::Chat::WriteChat(GW::Chat::CHANNEL_WHISPER, message.c_str(), L"Daybreak", false);
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto message = PromiseQueue.front();
                    PromiseQueue.pop();
                    try {
                        PostMessage(message);
                    }
                    catch (...) {
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void PostWhisper(const httplib::Request& req, httplib::Response& res) {
        EnsureInitialized();
        auto wMessage = Utils::StringToWString(req.body);
        PromiseQueue.emplace(wMessage);
        res.set_content("Okay", "text/plain");
    }
}