#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/GameStatePayload.h>

namespace Daybreak::Modules {
    class GameStateModule : public DaybreakModule<GameStatePayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<GameStatePayload> GetPayload(const uint32_t) override;
    };
}