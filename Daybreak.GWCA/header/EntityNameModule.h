#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/NamePayload.h>

namespace Daybreak::Modules {
    class EntityNameModule : public DaybreakModule<NamePayload, uint32_t> {
        public:
            std::string ApiUri() override;
        protected:
            std::optional<uint32_t> GetContext(const httplib::Request& req, httplib::Response& res) override;
            std::optional<NamePayload> GetPayload(uint32_t context) override;
            bool CanReturn(const httplib::Request& req, httplib::Response& res, const NamePayload& payload) override;
            std::tuple<std::string, std::string> ReturnPayload(NamePayload payload) override;
        private:
            std::wstring name;
    };
}