#include "pch.h"
#include "MapModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Context/PreGameContext.h>
#include <payloads/PreGamePayload.h>
#include <json.hpp>

namespace Daybreak::Modules::PreGameModule {

    PreGamePayload GetPayload() {
        PreGamePayload preGamePayload;
        if (GW::Map::GetIsMapLoaded())
        {
            preGamePayload.Characters.clear();
            preGamePayload.ChosenCharacterIndex = 0;
            return preGamePayload;
        }

        const auto context = GW::GetPreGameContext();
        if (!context) 
        {
            return preGamePayload;
        }

        for (const auto& loginChar : context->chars)
        {
            char charName[20];
            int result = WideCharToMultiByte(CP_UTF8, 0, loginChar.character_name, -1, charName, sizeof(charName), NULL, NULL);
            if (result == 0)
            {
                // handle error, use GetLastError() to get more info
            }

            preGamePayload.Characters.emplace_back(charName);
        }

        preGamePayload.ChosenCharacterIndex = context->index_1;
        return preGamePayload;
    }

    void GetPreGameInfo(const httplib::Request&, httplib::Response& res) {
        PreGamePayload payload;
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
            printf("[Pre Game Module] Encountered exception: {%s}", ex.what());
            res.set_content(std::format("Encountered exception: {}", ex.what()), "text/plain");
            res.status = 500;
        }
    }
}