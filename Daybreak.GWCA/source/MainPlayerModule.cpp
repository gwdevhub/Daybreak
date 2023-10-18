#include "pch.h"
#include "MainPlayerModule.h"
#include <GWCA/Managers/GameThreadMgr.h>
#include <GWCA/Managers/MapMgr.h>
#include <GWCA/Managers/AgentMgr.h>
#include <GWCA/Managers/PartyMgr.h>
#include <GWCA/Managers/PlayerMgr.h>
#include <GWCA/Managers/SkillbarMgr.h>
#include <GWCA/Managers/QuestMgr.h>
#include <GWCA/Context/WorldContext.h>
#include <GWCA/GameContainers/Array.h>
#include <GWCA/GameEntities/Skill.h>
#include <GWCA/GameEntities/Quest.h>
#include <GWCA/GameEntities/Agent.h>
#include <GWCA/GameEntities/Title.h>
#include <GWCA/GameEntities/Party.h>
#include <GWCA/GameEntities/Attribute.h>
#include <GWCA/GameEntities/Player.h>
#include <GWCA/GameEntities/NPC.h>
#include <GWCA/Constants/Constants.h>
#include <temp/GWPlayer.h>
#include <future>
#include <payloads/MainPlayer.h>
#include <json.hpp>
#include <queue>
#include <algorithm>

namespace Daybreak::Modules::MainPlayerModule {
    std::queue<std::promise<MainPlayer>*> PromiseQueue;
    std::mutex GameThreadMutex;
    GW::HookEntry GameThreadHook;
    volatile bool initialized = false;

    std::string GetString(wchar_t* chars) {
        std::string str(64, '\0');
        auto length = std::wcstombs(&str[0], chars, 64);
        str.resize(length);
        return str;
    }

    std::list<QuestMetadata> GetQuestLog() {
        std::list<QuestMetadata> questMetadatas;
        auto questLog = GW::QuestMgr::GetQuestLog();
        for (auto& quest : *questLog) {
            QuestMetadata metadata;
            metadata.FromId = (uint32_t)quest.map_from;
            metadata.ToId = (uint32_t)quest.map_to;
            metadata.Id = (uint32_t)quest.quest_id;
            metadata.PosX = (uint32_t)quest.marker.x;
            metadata.PosY = (uint32_t)quest.marker.y;
            questMetadatas.push_back(metadata);
        }

        return questMetadatas;
    }

    std::vector<GW::MapAgent> GetMapEntities() {
        std::vector<GW::MapAgent> entities;
        auto worldContext = GW::GetWorldContext();
        for (auto& agent : worldContext->map_agents) {
            entities.push_back(agent);
        }

        return entities;
    }

    std::list<GW::PartyAttribute> GetPartyAttributes() {
        std::list<GW::PartyAttribute> attributes;
        auto worldContext = GW::GetWorldContext();
        for (auto& attribute : worldContext->attributes) {
            attributes.push_back(attribute);
        }

        return attributes;
    }

    std::list<GW::ProfessionState> GetProfessions() {
        std::list<GW::ProfessionState> professions;
        auto worldContext = GW::GetWorldContext();
        for (auto& profession : worldContext->party_profession_states) {
            professions.push_back(profession);
        }

        return professions;
    }

    std::list<GW::AgentLiving> GetLivingAgents() {
        std::list<GW::AgentLiving> agentList;
        const GW::AgentArray* agents_ptr = GW::Agents::GetAgentArray();
        GW::AgentArray* agents = GW::Agents::GetAgentArray();
        for (auto* a : *agents) {
            const GW::AgentLiving* agent = a ? a->GetAsAgentLiving() : nullptr;
            if (agent) {
                agentList.push_back(*agent);
            }
        }

        return agentList;
    }

    std::list<GW::NPC> GetNpcs() {
        std::list<GW::NPC> npcs;
        auto worldContext = GW::GetWorldContext();
        for (auto& npc : worldContext->npcs) {
            npcs.push_back(npc);
        }

        return npcs;
    }

    std::vector<GW::TitleTier> GetTitleTiers() {
        std::vector<GW::TitleTier> titles;
        auto worldContext = GW::GetWorldContext();
        for (auto& title : worldContext->title_tiers) {
            titles.push_back(title);
        }

        return titles;
    }

    std::list<GW::Title> GetTitles() {
        std::list<GW::Title> titles;
        auto worldContext = GW::GetWorldContext();
        for (auto& title : worldContext->titles) {
            titles.push_back(title);
        }

        return titles;
    }

    std::list<Temp::GWPlayer> GetPlayers() {
        std::list<Temp::GWPlayer> players;
        auto worldContext = GW::GetWorldContext();
        Temp::GWPlayer *tempPlayerArray = (Temp::GWPlayer*)worldContext->players.m_buffer;
        for (auto i = 0; i < worldContext->players.m_size; i++) {
            if (tempPlayerArray->agent_id != 0) {
                players.push_back(*tempPlayerArray);
            }
            tempPlayerArray++;
        }

        return players;
    }

    std::map<uint32_t, std::list<uint32_t>> GetSkillbars() {
        std::map<uint32_t, std::list<uint32_t>> skillbarMap;
        auto skillbars = GW::SkillbarMgr::GetSkillbarArray();
        for (auto& skillbar : *skillbars) {
            std::list<uint32_t> skillList;
            skillList.push_back((uint32_t)skillbar.skills[0].skill_id);
            skillList.push_back((uint32_t)skillbar.skills[1].skill_id);
            skillList.push_back((uint32_t)skillbar.skills[2].skill_id);
            skillList.push_back((uint32_t)skillbar.skills[3].skill_id);
            skillList.push_back((uint32_t)skillbar.skills[4].skill_id);
            skillList.push_back((uint32_t)skillbar.skills[5].skill_id);
            skillList.push_back((uint32_t)skillbar.skills[6].skill_id);
            skillList.push_back((uint32_t)skillbar.skills[7].skill_id);
            skillbarMap[skillbar.agent_id] = skillList;
        }

        return skillbarMap;
    }

    std::list<LivingEntity> GetLivingEntities(
        std::list<GW::AgentLiving> agents) {
        std::list<LivingEntity> entities;
        for (auto& agent : agents) {
            LivingEntity entity;
            entity.PrimaryProfessionId = agent.primary;
            entity.SecondaryProfessionId = agent.secondary;
            entity.EntityState = agent.type_map;
            entity.EntityAllegiance = (uint32_t)agent.allegiance;
            entity.NpcDefinition = agent.player_number;
            entity.Id = agent.agent_id;
            entity.Timer = agent.timer;
            entity.Level = agent.level;
            entity.PosX = agent.pos.x;
            entity.PosY = agent.pos.y;
            entities.push_back(entity);
        }

        return entities;
    }

    void PopulatePlayer(
        Player& player,
        int playerId,
        GW::ProfessionState* professionState,
        std::list<uint32_t> skillbar,
        GW::PartyAttribute* partyAttribute,
        GW::AgentLiving* agent) {
        
        std::list<uint32_t> unlockedProfessions;
        if (professionState) {
            for (auto i = 0; i < 11; i++) {
                auto prof = (GW::Constants::Profession)i;
                if (professionState->IsProfessionUnlocked(prof)) {
                    unlockedProfessions.push_back(i);
                }
            }

            unlockedProfessions.push_back((uint32_t)professionState->primary);
        }

        Build build;
        std::list<Attribute> attributes;
        if (partyAttribute) {
            for (const auto& partyAttr : partyAttribute->attribute) {
                if (((uint32_t)partyAttr.id < 0) ||
                    ((uint32_t)partyAttr.id > 44) ||
                    partyAttr.level <= 0) {
                    continue;
                }

                Attribute attribute;
                attribute.Id = (uint32_t)partyAttr.id;
                attribute.BaseLevel = partyAttr.level_base;
                attribute.ActualLevel = partyAttr.level;
                attribute.DecrementPoints = partyAttr.decrement_points;
                attribute.IncrementPoints = partyAttr.increment_points;
                attributes.push_back(attribute);
            }

            build.Attributes = attributes;
            if (agent) {
                build.Primary = (uint32_t)agent->primary;
                build.Secondary = (uint32_t)agent->secondary;
            }
            else if (professionState) {
                build.Primary = (uint32_t)professionState->primary;
                build.Secondary = (uint32_t)professionState->secondary;
            }

            build.Skills = skillbar;
            player.Build = build;
        }

        player.UnlockedProfession = unlockedProfessions;
        player.Id = playerId;        
        if (agent) {
            player.PosX = agent->pos.x;
            player.PosY = agent->pos.y;
            player.Timer = agent->timer;
            player.Level = agent->level;
            player.NpcDefinition = agent->player_number;
            player.PrimaryProfessionId = (uint32_t)agent->primary;
            player.SecondaryProfessionId = (uint32_t)agent->secondary;
        }
        else if (professionState) {
            player.PrimaryProfessionId = (uint32_t)professionState->primary;
            player.SecondaryProfessionId = (uint32_t)professionState->secondary;
        }
    }

    void PopulateWorldPlayer(
        std::list<GW::Title> titles,
        std::vector<GW::TitleTier> titleTiers,
        WorldPlayer& player,
        int playerId,
        Temp::GWPlayer gwPlayer,
        GW::ProfessionState* professionState,
        std::list<uint32_t> skillbar,
        GW::PartyAttribute* partyAttribute,
        GW::AgentLiving* agent) {

        PopulatePlayer(player, playerId, professionState, skillbar, partyAttribute, agent);
        GW::Title gwTitle;
        Title title;
        int titleId = 0;
        auto name = GetString(gwPlayer.name);
        player.Name = name;

        for (const auto &t : titles) {
            if (t.current_title_tier_index == gwPlayer.active_title_tier) {
                gwTitle = t;
                break;
            }

            titleId++;
        }

        if (gwTitle.current_title_tier_index >= titleTiers.size()) {
            return;
        }

        auto titleTier = titleTiers[gwTitle.current_title_tier_index];
        title.CurrentPoints = gwTitle.current_points;
        title.IsPercentage = gwTitle.is_percentage_based();
        title.PointsForCurrentRank = gwTitle.points_needed_current_rank;
        title.PointsForNextRank = titleTier.tier_number == gwTitle.max_title_rank ? gwTitle.current_points : gwTitle.points_needed_next_rank;
        title.TierNumber = titleTier.tier_number;
        title.MaxTierNumber = gwTitle.max_title_rank;
        title.Id = titleId;

        player.Title = title;
    }

    void PopulateMainPlayer(
        MainPlayer& player,
        int activeQuestId,
        std::list<QuestMetadata> questLog,
        std::list<GW::Title> titles,
        std::vector<GW::TitleTier> titleTiers,
        int playerId,
        Temp::GWPlayer gwPlayer,
        GW::ProfessionState* professionState,
        std::list<uint32_t> skillbar,
        GW::PartyAttribute* partyAttribute,
        GW::AgentLiving* agent) {
        PopulateWorldPlayer(titles, titleTiers, player, playerId, gwPlayer, professionState, skillbar, partyAttribute, agent);
        player.CurrentQuest = activeQuestId;
        player.QuestLog = questLog;
        player.HardModeUnlocked = GW::GetWorldContext()->is_hard_mode_unlocked == 1;
        player.Experience = GW::GetWorldContext()->experience;
        player.Morale = GW::GetWorldContext()->morale;
        if (agent) {
            player.CurrentEnergy = agent->max_energy * agent->energy;
            player.CurrentHp = agent->max_hp * agent->hp;
            player.MaxEnergy = agent->max_energy;
            player.MaxHp = agent->max_hp;
        }
    }

    MainPlayer GetPayload() {
        MainPlayer gamePayload;
        if (!GW::Map::GetIsMapLoaded()) {
            return gamePayload;
        }
        
        auto worldContext = GW::GetWorldContext();
        auto activeQuestId = GW::QuestMgr::GetActiveQuestId();
        auto questLog = GetQuestLog();
        auto skillbars = GetSkillbars();
        if (skillbars.empty()) {
            return gamePayload;
        }

        auto agents = GetLivingAgents();
        if (agents.empty()) {
            return gamePayload;
        }

        auto titles = GetTitles();
        if (titles.empty()) {
            return gamePayload;
        }

        auto titleTiers = GetTitleTiers();
        if (titleTiers.empty()) {
            return gamePayload;
        }

        auto players = GetPlayers();
        if (players.empty()) {
            return gamePayload;
        }

        auto npcs = GetNpcs();
        if (npcs.empty()) {
            return gamePayload;
        }

        auto attributes = GetPartyAttributes();
        if (attributes.empty()) {
            return gamePayload;
        }

        auto professions = GetProfessions();
        if (professions.empty()) {
            return gamePayload;
        }

        auto mapAgents = GetMapEntities();
        if (mapAgents.empty()) {
            return gamePayload;
        }
        auto playerAgentId = GW::Agents::GetPlayerId();


        MainPlayer mainPlayer;
        auto foundMainAttr = std::find_if(attributes.begin(), attributes.end(), [&](GW::PartyAttribute attribute) {
            return playerAgentId == attribute.agent_id;
            });
        auto foundMainAgent = std::find_if(agents.begin(), agents.end(), [&](GW::AgentLiving agentLiving) {
            return playerAgentId == agentLiving.agent_id;
            });
        auto foundMainProfessionsState = std::find_if(professions.begin(), professions.end(), [&](GW::ProfessionState professionState) {
            return playerAgentId == professionState.agent_id;
            });
        auto foundMainGwPlayer = std::find_if(players.begin(), players.end(), [&](Temp::GWPlayer gwPlayer) {
            return playerAgentId == gwPlayer.agent_id;
            });
        GW::AgentLiving* mainAgentPtr;
        if (foundMainAgent == agents.end()) {
            mainAgentPtr = nullptr;
        }
        else {
            mainAgentPtr = &*foundMainAgent;
        }

        GW::PartyAttribute* mainAttrPtr;
        if (foundMainAttr == attributes.end()) {
            mainAttrPtr = nullptr;
        }
        else {
            mainAttrPtr = &*foundMainAttr;
        }

        GW::ProfessionState* mainProfPtr;
        if (foundMainProfessionsState == professions.end()) {
            mainProfPtr = nullptr;
        }
        else {
            mainProfPtr = &*foundMainProfessionsState;
        }

        PopulateMainPlayer(mainPlayer, (uint32_t)activeQuestId, questLog, titles, titleTiers, playerAgentId, *foundMainGwPlayer, mainProfPtr, skillbars[playerAgentId], mainAttrPtr, mainAgentPtr);
        return mainPlayer;
    }

    void EnsureInitialized() {
        GameThreadMutex.lock();
        if (!initialized) {
            GW::GameThread::RegisterGameThreadCallback(&GameThreadHook, [&](GW::HookStatus*) {
                while (!PromiseQueue.empty()) {
                    auto promise = PromiseQueue.front();
                    PromiseQueue.pop();
                    try {
                        auto payload = GetPayload();
                        promise->set_value(GetPayload());
                    }
                    catch (...) {
                        MainPlayer payload;
                        promise->set_value(payload);
                    }
                }
                });

            initialized = true;
        }

        GameThreadMutex.unlock();
    }

    void GetMainPlayer(const httplib::Request&, httplib::Response& res) {
        auto callbackEntry = new GW::HookEntry;
        auto response = new std::promise<MainPlayer>;

        EnsureInitialized();
        PromiseQueue.emplace(response);
        json responsePayload = response->get_future().get();

        delete callbackEntry;
        delete response;
        res.set_content(responsePayload.dump(), "text/json");
    }
}