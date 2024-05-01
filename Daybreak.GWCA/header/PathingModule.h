#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/PathingPayload.h>

namespace Daybreak::Modules {
    class PathingModule : public DaybreakModule<PathingPayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<PathingPayload> GetPayload(const uint32_t) override;
    };
}