#include "pch.h"
#include "MapModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/PreGamePayload.h>
#include <json.hpp>
#include <GWCA/Context/PreGameContext.h>

namespace Daybreak::Modules::PreGameModule {

    PreGamePayload GetPayload() {
        PreGamePayload preGamePayload;
        if (GW::Map::GetIsMapLoaded()) {
            preGamePayload.Characters.clear();
            preGamePayload.ChosenCharacterIndex = 0;
            return preGamePayload;
        }

        const auto context = GW::GetPreGameContext();
        if (!context) {
            return preGamePayload;
        }

        for (const auto& loginChar : context->chars) {
            char charName[20];
            int result = WideCharToMultiByte(CP_UTF8, 0, loginChar.character_name, -1, charName, sizeof(charName), NULL, NULL);
            if (result == 0) {
                // handle error, use GetLastError() to get more info
            }
            preGamePayload.Characters.emplace_back(charName);
        }

        preGamePayload.ChosenCharacterIndex = context->index_1;
        return preGamePayload;
    }

    void GetPreGameInfo(const httplib::Request&, httplib::Response& res) {
        GW::GameThread::Enqueue([&res] {
            const auto payload = GetPayload();
            const auto ret_json = static_cast<json>(payload);
            res.set_content(ret_json, "text/json");
        });
    }
}