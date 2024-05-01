#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/MapPayload.h>

namespace Daybreak::Modules {
    class MapModule : public DaybreakModule<MapPayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<MapPayload> GetPayload(const uint32_t) override;
    };
}