#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <string>

namespace Daybreak::Modules {
    class WhisperModule : public DaybreakModule<bool, std::wstring> {
    public:
        std::string ApiUri() override;
    protected:
        std::optional<std::wstring> GetContext(const httplib::Request& req, httplib::Response& res) override;
        std::optional<bool> GetPayload(const std::wstring message) override;
        std::tuple<std::string, std::string> ReturnPayload(bool) override;
    };
}