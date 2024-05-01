#pragma once
#include "httplib.h"
#include <DaybreakModule.h>
#include <payloads/NamePayload.h>
#include <GWCA/GameEntities/Item.h>

namespace Daybreak::Modules {
    class ItemNameModule : public DaybreakModule<NamePayload, GW::Item*> {
    public:
        std::string ApiUri() override;
    protected:
        std::optional<GW::Item*> GetContext(const httplib::Request& req, httplib::Response& res) override;
        std::optional<NamePayload> GetPayload(GW::Item* context) override;
        bool CanReturn(const httplib::Request& req, httplib::Response& res, const NamePayload& payload) override;
        std::tuple<std::string, std::string> ReturnPayload(NamePayload payload) override;
    private:
        std::wstring name;
    };
}