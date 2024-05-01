#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/MainPlayer.h>

namespace Daybreak::Modules {
    class MainPlayerModule : public DaybreakModule<MainPlayer, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<MainPlayer> GetPayload(const uint32_t) override;
    };
}