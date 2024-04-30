#include "pch.h"
#include "ItemNameModule.h"
#include "payloads/NamePayload.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/ItemMgr.h>
#include <GWCA/GameEntities/Item.h>
#include <future>
#include <json.hpp>
#include <tuple>
#include <queue>
#include <GWCA/GWCA.h>
#include <Windows.h>
#include <cstdint>
#include <limits>
#include <Utils.h>

namespace Daybreak::Modules {
    namespace ItemNameModuleInternal {
        std::vector<std::string> SplitAndRemoveSpaces(const std::string& s) {
            std::vector<std::string> result;
            std::stringstream ss(s);
            std::string item;

            while (getline(ss, item, ',')) {
                item.erase(std::remove(item.begin(), item.end(), ' '), item.end());
                result.push_back(item);
            }

            return result;
        }
    }

    std::string ItemNameModule::ApiUri() {
        return "/items/name";
    }

    std::optional<GW::Item*> ItemNameModule::GetContext(const httplib::Request& req, httplib::Response& res) {
        uint32_t id = 0;
        std::vector<uint32_t> modifiers;

        auto id_it = req.params.find("id");
        if (id_it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing id parameter", "text/plain");
            return std::optional<GW::Item*>();
        }
        else {
            auto& idStr = id_it->second;
            int parsedId;
            if (!Daybreak::Utils::StringToInt(idStr, parsedId)) {
                res.status = 400;
                res.set_content("Invalid id parameter", "text/plain");
                return std::optional<GW::Item*>();
            }

            id = static_cast<uint32_t>(parsedId);
        }

        auto mod_it = req.params.find("modifiers");
        if (mod_it == req.params.end()) {
            res.status = 400;
            res.set_content("Missing modifiers parameter", "text/plain");
            return std::optional<GW::Item*>();
        }
        else {
            auto& modifiersStr = mod_it->second;
            const auto tokens = ItemNameModuleInternal::SplitAndRemoveSpaces(modifiersStr);
            for (const auto& token : tokens) {
                int parsedModifier;
                if (!Daybreak::Utils::StringToInt(token, parsedModifier)) {
                    res.status = 400;
                    res.set_content("Invalid modifier parameter", "text/plain");
                    return std::optional<GW::Item*>();
                }

                modifiers.push_back(static_cast<uint32_t>(parsedModifier));
            }
        }

        GW::ItemModifier parsedModifiers[64];
        for (auto i = 0U; i < modifiers.size(); i++) {
            GW::ItemModifier parsedModifier;
            const auto mod = modifiers.at(i);
            parsedModifier.mod = mod;
            parsedModifiers[i] = parsedModifier;
        }

        auto item = GW::Items::GetItemByModelIdAndModifiers(id, parsedModifiers, modifiers.size(), 1, 23);
        if (!item) {
            return std::optional<GW::Item*>();
        }

        return item;
    }

    std::optional<NamePayload> ItemNameModule::GetPayload(GW::Item* context) {
        NamePayload payload;
        if (!context) {
            return std::optional<NamePayload>();
        }

        payload.Id = context->item_id;
        GW::Items::AsyncGetItemName(context, this->name);
        return payload;
    }

    bool ItemNameModule::CanReturn(const httplib::Request& req, httplib::Response& res, const NamePayload& payload) {
        return !name.empty();
    }

    std::tuple<std::string, std::string> ItemNameModule::ReturnPayload(NamePayload payload) {
        payload.Name = Daybreak::Utils::WStringToString(this->name);
        return std::make_tuple(static_cast<json>(payload).dump(), "application/json");
    }
}