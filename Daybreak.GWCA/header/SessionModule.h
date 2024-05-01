#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/SessionPayload.h>

namespace Daybreak::Modules {
    class SessionModule : public DaybreakModule<SessionPayload, uint32_t> {
    public:
        std::string ApiUri() override;
    protected:
        std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
        std::optional<SessionPayload> GetPayload(const uint32_t) override;
    };
}