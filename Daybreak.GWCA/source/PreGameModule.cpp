#include "pch.h"
#include <PreGameModule.h>
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Context/PreGameContext.h>
#include <payloads/PreGamePayload.h>
#include <json.hpp>

namespace Daybreak::Modules {
    std::optional<PreGamePayload> PreGameModule::GetPayload(const uint32_t) {
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

    std::string PreGameModule::ApiUri()
    {
        return "/pregame";
    }

    std::optional<uint32_t> PreGameModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        return NULL;
    }
}