#include "pch.h"
#include "CartographerModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/GameContainers/Array.h>
#include <GWCA/Context/WorldContext.h>
#include <GWCA/Context/MapContext.h>
#include <future>
#include <payloads/CartographerPayload.h>
#include <json.hpp>
#include <queue>y
#include <GWCA/GameEntities/Pathing.h>
#include <GWCA/Utilities/Scanner.h>
#include <temp/Size2D.h>

namespace Daybreak::Modules::CartographerModule {
    //TODO: Delete union after debugging

    std::queue<std::promise<CartographerPayload>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    typedef Temp::Size2D* (__cdecl* GetDimsPt)();
    GetDimsPt GetDimsFunc = 0;

    CartographerPayload GetPayload() {
        CartographerPayload cartoPayload;
        if (!GW::Map::GetIsMapLoaded()) {
            return cartoPayload;
        }

        const auto dimensions = GetDimsFunc();
        //cartoPayload.MapSize = *dimensions;
        const auto worldContext = GW::GetWorldContext();
        const auto mapContext = GW::GetMapContext();
        cartoPayload.MapSize.x = worldContext->h05B4[0];
        cartoPayload.MapSize.y = worldContext->h05B4[1];
        const auto cartoArray = &worldContext->cartographed_areas;
        const auto castedArray = reinterpret_cast<const GW::Array<uint32_t>*>(cartoArray);
        for (auto i = 0; i < castedArray->size(); i++) {
            const auto areaBits = castedArray->at(i);
            cartoPayload.CartographedAreas.push_back(areaBits);
        }

        union {
            uint32_t intValue;
            float floatValue;
        }u{};

        u.intValue = mapContext->h0088[14];
        cartoPayload.MapX = u.floatValue;
        u.intValue = mapContext->h0088[15];
        cartoPayload.MapY = u.floatValue;
        return cartoPayload;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            // Get a pointer to the GetDims function
            GetDimsFunc = (GetDimsPt)GW::Scanner::Find("\xe8\x00\x00\x00\x00\x8b\x40\x2c\x05\xa4\x05\x00\x00\xc3", "x????xxxxxxxxx", 0x0);

            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promise = PromiseQueue.front();
                    PromiseQueue.pop();
                    try {
                        auto payload = GetPayload();
                        promise->set_value(payload);
                    }
                    catch (...) {
                        CartographerPayload payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetCartographerAreas(const httplib::Request&, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
        auto response = new std::promise<CartographerPayload>;

        EnsureInitialized();
        PromiseQueue.emplace(response);
        json responsePayload = response->get_future().get();

        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}