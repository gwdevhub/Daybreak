#include "pch.h"
#include "EntityNameModule.h"
#include "payloads/NamePayload.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/GameEntities/Agent.h>
#include <future>
#include <json.hpp>
#include <tuple>
#include <queue>
#include <GWCA/GWCA.h>
#include <GWCA/Context/PreGameContext.h>
#include <GWCA/Managers/AgentMgr.h>
#include <Windows.h>
#include <cstdint>
#include <limits>
#include "Utils.h"

namespace Daybreak::Modules {
    std::string EntityNameModule::ApiUri()
    {
        return "/entities/name";
    }

    std::optional<uint32_t> EntityNameModule::GetContext(const httplib::Request& req, httplib::Response& res)
    {
        uint32_t id = 0;
        auto it = req.params.find("id");
        if (it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing id parameter", "text/plain");
            return std::optional<uint32_t>();
        }
        else {
            auto idStr = it->second;
            size_t pos = 0;
            auto result = std::stoul(idStr, &pos);
            if (pos != idStr.size()) {
                res.status = 400;
                res.set_content("Invalid id parameter", "text/plain");
                return std::optional<uint32_t>();
            }

            id = static_cast<uint32_t>(result);
        }

        return id;
    }

    std::optional<NamePayload> EntityNameModule::GetPayload(const uint32_t context)
    {
        NamePayload namePayload;
        auto agent = GW::Agents::GetAgentByID(context);
        if (!agent) {
            return std::optional<NamePayload>();
        }

        auto agentLiving = agent->GetAsAgentLiving();
        if (!agentLiving) {
            return std::optional<NamePayload>();
        }

        if (!GW::Agents::AsyncGetAgentName(agentLiving, this->name)) {
            return std::optional<NamePayload>();
        }

        namePayload.Id = context;
        return namePayload;
    }

    bool EntityNameModule::CanReturn(const httplib::Request& req, httplib::Response& res, const NamePayload& payload) {
        return !name.empty();
    }

    std::tuple<std::string, std::string> EntityNameModule::ReturnPayload(NamePayload payload) {
        payload.Name = Daybreak::Utils::WStringToString(this->name);
        return std::make_tuple(static_cast<json>(payload).dump(), "application/json");
    }
}