#include "pch.h"
#include "WhisperModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Managers/ChatMgr.h>
#include <queue>
#include <string>
#include <Utils.h>

namespace Daybreak::Modules {
    std::optional<bool> WhisperModule::GetPayload(std::wstring message) {
        if (!GW::Map::GetIsMapLoaded()) {
            return std::optional<bool>();
        }

        GW::Chat::WriteChat(GW::Chat::CHANNEL_WHISPER, message.c_str(), L"Daybreak", false);
        return true;
    }

    std::string WhisperModule::ApiUri()
    {
        return "/whisper";
    }

    std::optional<std::wstring> WhisperModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        auto wMessage = Utils::StringToWString(req.body);
        return wMessage;
    }

    std::tuple<std::string, std::string> WhisperModule::ReturnPayload(bool) {
        return std::make_tuple("", "text/plain");
    }
}