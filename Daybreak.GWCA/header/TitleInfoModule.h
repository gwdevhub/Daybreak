#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <TitleInfoModule.h>
#include <payloads/TitleInfoPayload.h>
#include <tuple>
#include <string>

namespace Daybreak::Modules {
    class TitleInfoModule : public DaybreakModule<TitleInfoPayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<TitleInfoPayload> GetPayload(const uint32_t context) override;
            std::tuple<std::string, std::string> ReturnPayload(TitleInfoPayload payload) override;
            bool CanReturn(const httplib::Request& req, httplib::Response& res, const TitleInfoPayload& payload) override;
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
        private:
            std::wstring name;
    };
}