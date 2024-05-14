#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/DebugPayload.h>

namespace Daybreak::Modules {
    class DebugModule : public DaybreakModule<DebugPayload, uint32_t> {
    public:
        std::string ApiUri() override;
    protected:
        std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
        std::optional<DebugPayload> GetPayload(uint32_t context) override;
    };
}