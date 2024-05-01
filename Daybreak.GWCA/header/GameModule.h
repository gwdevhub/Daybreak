#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/GamePayload.h>

namespace Daybreak::Modules {
    class GameModule : public DaybreakModule<GamePayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<GamePayload> GetPayload(const uint32_t) override;
    };
}