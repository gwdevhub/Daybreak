#include "pch.h"
#include "GameStateModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/AgentMgr.h>
#include <GWCA/Managers/CameraMgr.h>
#include <GWCA/GameEntities/Agent.h>
#include <GWCA/GameEntities/Camera.h>
#include <GWCA/Managers/MapMgr.h>
#include <future>
#include <payloads/GameStatePayload.h>
#include <json.hpp>
#include <queue>
#include <algorithm>

namespace Daybreak::Modules::GameStateModule {
    std::list<GW::AgentLiving> GetLivingAgents() {
        std::list<GW::AgentLiving> agentList;
        const GW::AgentArray* agents_ptr = GW::Agents::GetAgentArray();
        GW::AgentArray* agents = GW::Agents::GetAgentArray();
        if (!agents) {
            return agentList;
        }

        for (const auto* a : *agents) {
            const GW::AgentLiving* agent = a ? a->GetAsAgentLiving() : nullptr;
            if (agent) {
                agentList.push_back(*agent);
            }
        }

        return agentList;
    }

    std::list<StatePayload> GetStates(
        std::list<GW::AgentLiving> agents) {
        std::list<StatePayload> states;
        for (auto& agent : agents) {
            StatePayload state;
            state.Id = agent.agent_id;
            state.PosX = agent.pos.x;
            state.PosY = agent.pos.y;
            state.State = agent.type_map;
            state.Health = agent.hp * (agent.max_hp > 0 ? agent.max_hp : 1);
            state.Energy = agent.energy * (agent.max_energy > 0 ? agent.max_energy : 1);
            state.RotationAngle = agent.rotation_angle;
            states.push_back(state);
        }

        return states;
    }

    GameStatePayload* GetPayload() {
        auto gamePayload = new GameStatePayload();
        if (!GW::Map::GetIsMapLoaded()) {
            return gamePayload;
        }

        auto agents = GetLivingAgents();
        if (agents.empty()) {
            return gamePayload;
        }

        auto states = GetStates(agents);
        gamePayload->States = states;

        auto camera = GW::CameraMgr::GetCamera();
        Daybreak::Camera cameraPayload{};
        cameraPayload.Pitch = camera->pitch;
        cameraPayload.Yaw = camera->yaw;
        gamePayload->Camera = cameraPayload;
        return gamePayload;
    }

    void GetGameStateInfo(const httplib::Request&, httplib::Response& res) {
        GameStatePayload* payload = NULL;
        std::exception* ex = NULL;
        volatile bool executing = true;
        GW::GameThread::Enqueue([&res, &executing, &ex, &payload]
            {
                try {
                    payload = GetPayload();

                }
                catch (std::exception e) {
                    ex = &e;
                }

                executing = false;
            });

        while (executing) {
            Sleep(4);
        }

        if (payload) {
            const auto json = static_cast<nlohmann::json>(*payload);
            const auto dump = json.dump();
            res.set_content(dump, "text/json");
            delete(payload);
        }
        else if (ex) {
            printf("[Game State Module] Encountered exception: {%s}", ex->what());
            res.set_content(std::format("Encountered exception: {}", ex->what()), "text/plain");
            res.status = 500;
        }
    }
}