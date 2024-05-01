#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/PathingMetadataPayload.h>

namespace Daybreak::Modules {
    class PathingMetadataModule : public DaybreakModule<PathingMetadataPayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<PathingMetadataPayload> GetPayload(const uint32_t) override;
    };
}