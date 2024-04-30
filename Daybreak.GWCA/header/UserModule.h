#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/UserPayload.h>

namespace Daybreak::Modules {
    class UserModule : public DaybreakModule<UserPayload, uint32_t> {
    public:
        std::string ApiUri() override;
    protected:
        std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
        std::optional<UserPayload> GetPayload(const uint32_t) override;
    };
}