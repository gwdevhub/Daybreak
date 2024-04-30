#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/LoginPayload.h>

namespace Daybreak::Modules {
    class LoginModule : public DaybreakModule<LoginPayload, uint32_t>{
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<LoginPayload> GetPayload(const uint32_t) override;
    };
}