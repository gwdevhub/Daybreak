#include "pch.h"
#include "WhisperModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Managers/ChatMgr.h>
#include <queue>
#include <string>
#include <Utils.h>

namespace Daybreak::Modules::WhisperModule {
    void PostMessage(std::wstring message) {
        if (!GW::Map::GetIsMapLoaded()) {
            return;
        }

        GW::Chat::WriteChat(GW::Chat::CHANNEL_WHISPER, message.c_str(), L"Daybreak", false);
    }

    void PostWhisper(const httplib::Request& req, httplib::Response& res) {
        auto wMessage = Utils::StringToWString(req.body);
        GW::GameThread::Enqueue([&res, wMessage]
            {
                PostMessage(wMessage);
            });
    }
}