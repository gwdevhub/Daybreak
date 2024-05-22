#pragma once
#include "httplib.h"
#include <payloads/CartographerPayload.h>
#include <DaybreakModule.h>

namespace Daybreak::Modules {
    class CartographerModule : public DaybreakModule<CartographerPayload, uint32_t> {
    public:
        std::string ApiUri() override;
    protected:
        std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
        std::optional<CartographerPayload> GetPayload(uint32_t context) override;
    };
}