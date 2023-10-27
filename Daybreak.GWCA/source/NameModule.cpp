#include "pch.h"
#include "NameModule.h"
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

namespace Daybreak::Modules::NameModule {
    std::queue<std::tuple<uint32_t, std::promise<NamePayload>>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    std::string WStringToString(const std::wstring& wstr)
    {
        if (wstr.empty()) return std::string();

        int size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
        std::string strTo(size_needed, 0);
        WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], size_needed, NULL, NULL);

        return strTo;
    }

    NamePayload GetAsyncName(uint32_t id) {
        NamePayload namePayload;
        auto name = new std::wstring();
        auto agent = GW::Agents::GetAgentByID(id);
        if (!agent) {
            return namePayload;
        }

        auto agentLiving = agent->GetAsAgentLiving();
        if (!agentLiving) {
            return namePayload;
        }

        if (!GW::Agents::AsyncGetAgentName(agentLiving, *name)) {
            return namePayload;
        }

        const auto namestr =  WStringToString(*name);
        namePayload.Id = id;
        namePayload.Name = namestr;
        delete(name);
        return namePayload;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promiseRequest = PromiseQueue.front();
                    std::promise<NamePayload>& promise = std::get<1>(*promiseRequest);
                    uint32_t id = std::get<0>(*promiseRequest);
                    PromiseQueue.pop();
                    try {
                        auto payload = GetAsyncName(id);
                        promise.set_value(payload);
                    }
                    catch (...) {
                        NamePayload payload;
                        promise.set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetName(const httplib::Request& req, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
        uint32_t id = 0;
        auto it = req.params.find("id");
        if (it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing id parameter", "text/plain");
            return;
        }
        else {
            auto idStr = it->second;
            size_t pos = 0;
            auto result = std::stoul(idStr, &pos);
            if (pos != idStr.size()) {
                res.status = 400;
                res.set_content("Invalid id parameter", "text/plain");
                return;
            }

            id = static_cast<uint32_t>(result);
        }

        auto response = new std::tuple<uint32_t, std::promise<NamePayload>>();
        std::get<0>(*response) = id;
        std::promise<NamePayload>& promise = std::get<1>(*response);

        EnsureInitialized();
        PromiseQueue.emplace(response);

        json responsePayload = promise.get_future().get();
        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}