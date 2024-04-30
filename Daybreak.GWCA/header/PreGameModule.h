#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/PreGamePayload.h>

namespace Daybreak::Modules {
    class PreGameModule : public DaybreakModule<PreGamePayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<PreGamePayload> GetPayload(const uint32_t) override;
    };
}