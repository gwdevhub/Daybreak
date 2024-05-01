#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/InventoryPayload.h>

namespace Daybreak::Modules {
    class InventoryModule : public DaybreakModule<InventoryPayload, uint32_t> {
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<InventoryPayload> GetPayload(const uint32_t) override;
        public:
            std::string ApiUri() override;
    };
}