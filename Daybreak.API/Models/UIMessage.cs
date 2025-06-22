namespace Daybreak.API.Models;

public enum UIMessage : uint
{
    None = 0x0,
    Resize = 0x8,
    InitFrame = 0x9,
    DestroyFrame = 0xb,
    KeyDown = 0x1e, // wparam = UIPacket::kKeyAction*
    KeyUp = 0x20, // wparam = UIPacket::kKeyAction*
    MouseClick = 0x22, // wparam = UIPacket::kMouseClick*
    MouseClick2 = 0x2e, // wparam = UIPacket::kMouseAction*
    MouseAction = 0x2f, // wparam = UIPacket::kMouseAction*
    SetLayout = 0x33,
    FrameMessage_0x47 = 0x47, // Multiple uses depending on frame
    UpdateAgentEffects = 0x10000000 | 0x9,
    RerenderAgentModel = 0x10000000 | 0x7, // wparam = uint32_t agent_id
    AgentSpeechBubble = 0x10000000 | 0x17,
    ShowAgentNameTag = 0x10000000 | 0x19, // wparam = AgentNameTagInfo*
    HideAgentNameTag = 0x10000000 | 0x1A,
    SetAgentNameTagAttribs = 0x10000000 | 0x1B, // wparam = AgentNameTagInfo*
    ChangeTarget = 0x10000000 | 0x20, // wparam = UIPacket::kChangeTarget*
    AgentStartCasting = 0x10000000 | 0x27, // wparam = UIPacket::kAgentStartCasting*
    ShowMapEntryMessage = 0x10000000 | 0x29, // wparam = { wchar_t* title, wchar_t* subtitle }
    SetCurrentPlayerData = 0x10000000 | 0x2A, // fired after setting the worldcontext player name
    PostProcessingEffect = 0x10000000 | 0x34, // Triggered when drunk. wparam = UIPacket::kPostProcessingEffect
    HeroAgentAdded = 0x10000000 | 0x38, // hero assigned to agent/inventory/ai mode
    HeroDataAdded = 0x10000000 | 0x39, // hero info received from server (name, level etc)
    ShowXunlaiChest = 0x10000000 | 0x40,
    MinionCountUpdated = 0x10000000 | 0x46,
    MoraleChange = 0x10000000 | 0x47, // wparam = {agent id, morale percent }
    LoginStateChanged = 0x10000000 | 0x50, // wparam = {bool is_logged_in, bool unk }
    EffectAdd = 0x10000000 | 0x55, // wparam = {agent_id, GW::Effect*}
    EffectRenew = 0x10000000 | 0x56, // wparam = GW::Effect*
    EffectRemove = 0x10000000 | 0x57, // wparam = effect id
    SkillActivated = 0x10000000 | 0x5b, // wparam ={ uint32_t agent_id , uint32_t skill_id }
    UpdateSkillbar = 0x10000000 | 0x5E, // wparam ={ uint32_t agent_id , ... }
    UpdateSkillsAvailable = 0x10000000 | 0x5f, // Triggered on a skill unlock, profession change or map load
    TitleProgressUpdated = 0x10000000 | 0x65, // wparam = title_id
    ExperienceGained = 0x10000000 | 0x66, // wparam = experience amount
    WriteToChatLog = 0x10000000 | 0x7E, // wparam = UIPacket::kWriteToChatLog*. Triggered by the game when it wants to add a new message to chat.
    WriteToChatLogWithSender = 0x10000000 | 0x7f, // wparam = UIPacket::kWriteToChatLogWithSender*. Triggered by the game when it wants to add a new message to chat.
    AllyOrGuildMessage = 0x10000000 | 0x80, // wparam = UIPacket::kAllyOrGuildMessage*
    PlayerChatMessage = 0x10000000 | 0x81, // wparam = UIPacket::kPlayerChatMessage*
    FloatingWindowMoved = 0x10000000 | 0x83, // wparam = frame_id
    FriendUpdated = 0x10000000 | 0x89, // wparam = { GW::Friend*, ... }
    MapLoaded = 0x10000000 | 0x8A,
    OpenWhisper = 0x10000000 | 0x90, // wparam = wchar* name
    Logout = 0x10000000 | 0x9b, // wparam = { bool unknown, bool character_select }
    CompassDraw = 0x10000000 | 0x9c, // wparam = UIPacket::kCompassDraw*
    OnScreenMessage = 0x10000000 | 0xA0, // wparam = wchar_** encoded_string
    DialogBody = 0x10000000 | 0xA4, // wparam = DialogBodyInfo*
    DialogButton = 0x10000000 | 0xA1, // wparam = DialogButtonInfo*
    TargetNPCPartyMember = 0x10000000 | 0xB1, // wparam = { uint32_t unk, uint32_t agent_id }
    TargetPlayerPartyMember = 0x10000000 | 0xB2, // wparam = { uint32_t unk, uint32_t player_number }
    VendorWindow = 0x10000000 | 0xB3, // wparam = UIPacket::kVendorWindow
    VendorItems = 0x10000000 | 0xB7, // wparam = UIPacket::kVendorItems
    VendorTransComplete = 0x10000000 | 0xB9, // wparam = *TransactionType
    VendorQuote = 0x10000000 | 0xBB, // wparam = UIPacket::kVendorQuote
    StartMapLoad = 0x10000000 | 0xC0, // wparam = { uint32_t map_id, ...}
    WorldMapUpdated = 0x10000000 | 0xC5, // Triggered when an area in the world map has been discovered/updated
    GuildMemberUpdated = 0x10000000 | 0xD8, // wparam = { GuildPlayer::name_ptr }
    ShowHint = 0x10000000 | 0xDF, // wparam = { uint32_t icon_type, wchar_t* message_enc }
    WeaponSetSwapComplete = 0x10000000 | 0xE7, // wparam = UIPacket::kWeaponSwap*
    WeaponSetSwapCancel = 0x10000000 | 0xE8,
    WeaponSetUpdated = 0x10000000 | 0xE9,
    UpdateGoldCharacter = 0x10000000 | 0xEA, // wparam = { uint32_t unk, uint32_t gold_character }
    UpdateGoldStorage = 0x10000000 | 0xEB, // wparam = { uint32_t unk, uint32_t gold_storage }
    InventorySlotUpdated = 0x10000000 | 0xEC, // undocumented. Triggered when an item is moved into a slot
    EquipmentSlotUpdated = 0x10000000 | 0xED, // undocumented. Triggered when an item is moved into a slot
    InventorySlotCleared = 0x10000000 | 0xEF, // undocumented. Triggered when an item has been removed from a slot
    EquipmentSlotCleared = 0x10000000 | 0xF0, // undocumented. Triggered when an item has been removed from a slot
    PvPWindowContent = 0x10000000 | 0xF8,
    PreStartSalvage = 0x10000000 | 0x100, // { uint32_t item_id, uint32_t kit_id }
    TradePlayerUpdated = 0x10000000 | 0x103, // wparam = GW::TraderPlayer*
    ItemUpdated = 0x10000000 | 0x104, // wparam = UIPacket::kItemUpdated*
    MapChange = 0x10000000 | 0x10F, // wparam = map id
    CalledTargetChange = 0x10000000 | 0x113, // wparam = { player_number, target_id }
    ErrorMessage = 0x10000000 | 0x117, // wparam = { int error_index, wchar_t* error_encoded_string }
    PartyHardModeChanged = 0x10000000 | 0x118, // wparam = { int is_hard_mode }
    PartyAddHenchman = 0x10000000 | 0x119,
    PartyRemoveHenchman = 0x10000000 | 0x11a,
    PartyAddHero = 0x10000000 | 0x11c,
    PartyRemoveHero = 0x10000000 | 0x11d,
    PartyAddPlayer = 0x10000000 | 0x122,
    PartyRemovePlayer = 0x10000000 | 0x124,
    DisableEnterMissionBtn = 0x10000000 | 0x128, // wparam = boolean (1 = disabled, 0 = enabled)
    ShowCancelEnterMissionBtn = 0x10000000 | 0x12b,
    PartyDefeated = 0x10000000 | 0x12d,
    PartySearchInviteReceived = 0x10000000 | 0x135, // wparam = UIPacket::kPartySearchInviteReceived*
    PartySearchInviteSent = 0x10000000 | 0x137,
    PartyShowConfirmDialog = 0x10000000 | 0x138, // wparam = UIPacket::kPartyShowConfirmDialog
    PreferenceEnumChanged = 0x10000000 | 0x13E, // wparam = UiPacket::kPreferenceEnumChanged
    PreferenceFlagChanged = 0x10000000 | 0x13F, // wparam = UiPacket::kPreferenceFlagChanged
    PreferenceValueChanged = 0x10000000 | 0x140, // wparam = UiPacket::kPreferenceValueChanged
    UIPositionChanged = 0x10000000 | 0x141, // wparam = UIPacket::kUIPositionChanged
    QuestAdded = 0x10000000 | 0x149, // wparam = { quest_id, ... }
    QuestDetailsChanged = 0x10000000 | 0x14A, // wparam = { quest_id, ... }
    ClientActiveQuestChanged = 0x10000000 | 0x14C, // wparam = { quest_id, ... }. Triggered when the game requests the current quest to change
    ServerActiveQuestChanged = 0x10000000 | 0x14E, // wparam = UIPacket::kServerActiveQuestChanged*. Triggered when the server requests the current quest to change
    UnknownQuestRelated = 0x10000000 | 0x14F,
    DungeonComplete = 0x10000000 | 0x151, // undocumented 
    MissionComplete = 0x10000000 | 0x152, // undocumented
    VanquishComplete = 0x10000000 | 0x154, // undocumented
    ObjectiveAdd = 0x10000000 | 0x155, // wparam = UIPacket::kObjectiveAdd*
    ObjectiveComplete = 0x10000000 | 0x156, // wparam = UIPacket::kObjectiveComplete*
    ObjectiveUpdated = 0x10000000 | 0x157, // wparam = UIPacket::kObjectiveUpdated*
    TradeSessionStart = 0x10000000 | 0x160, // wparam = { trade_state, player_number }
    TradeSessionUpdated = 0x10000000 | 0x166, // no args
    TriggerLogoutPrompt = 0x10000000 | 0x16C, // no args
    ToggleOptionsWindow = 0x10000000 | 0x16D, // no args
    CheckUIState = 0x10000000 | 0x170, // Undocumented
    RedrawItem = 0x10000000 | 0x172, // wparam = uint32_t item_id
    CloseSettings = 0x10000000 | 0x174, // Undocumented
    ChangeSettingsTab = 0x10000000 | 0x175, // wparam = uint32_t is_interface_tab
    GuildHall = 0x10000000 | 0x177, // wparam = gh key (uint32_t[4])
    LeaveGuildHall = 0x10000000 | 0x179,
    Travel = 0x10000000 | 0x17A,
    OpenWikiUrl = 0x10000000 | 0x17B, // wparam = char* url
    AppendMessageToChat = 0x10000000 | 0x189, // wparam = wchar_t* message
    HideHeroPanel = 0x10000000 | 0x197, // wparam = hero_id
    ShowHeroPanel = 0x10000000 | 0x198, // wparam = hero_id
    GetInventoryAgentId = 0x10000000 | 0x19c, // wparam = 0, lparam = uint32_t* agent_id_out. Used to fetch which agent is selected
    EquipItem = 0x10000000 | 0x19d, // wparam = { item_id, agent_id }
    MoveItem = 0x10000000 | 0x19e, // wparam = { item_id, to_bag, to_slot, bool prompt }
    InitiateTrade = 0x10000000 | 0x1A0,
    InventoryAgentChanged = 0x10000000 | 0x1b0, // Triggered when inventory needs updating due to agent change; no args
    OpenTemplate = 0x10000000 | 0x1B9, // wparam = GW::UI::ChatTemplate*

    // GWCA Client to Server commands. Only added the ones that are used for hooks, everything else goes straight into GW

    SendLoadSkillTemplate = 0x30000000 | 0x3,  // wparam = SkillbarMgr::SkillTemplate*
    SendPingWeaponSet = 0x30000000 | 0x4,  // wparam = UIPacket::kSendPingWeaponSet*
    SendMoveItem = 0x30000000 | 0x5,  // wparam = UIPacket::kSendMoveItem*
    SendMerchantRequestQuote = 0x30000000 | 0x6,  // wparam = UIPacket::kSendMerchantRequestQuote*
    SendMerchantTransactItem = 0x30000000 | 0x7,  // wparam = UIPacket::kSendMerchantTransactItem*
    SendUseItem = 0x30000000 | 0x8,  // wparam = UIPacket::kSendUseItem*
    SendSetActiveQuest = 0x30000000 | 0x9,  // wparam = uint32_t quest_id
    SendAbandonQuest = 0x30000000 | 0xA, // wparam = uint32_t quest_id
    SendChangeTarget = 0x30000000 | 0xB, // wparam = UIPacket::kSendChangeTarget* // e.g. tell the gw client to focus on a different target
    SendCallTarget = 0x30000000 | 0x13, // wparam = { uint32_t call_type, uint32_t agent_id } // also used to broadcast morale, death penalty, "I'm following X", etc
    SendDialog = 0x30000000 | 0x16, // wparam = dialog_id // internal use


    StartWhisper = 0x30000000 | 0x17, // wparam = UIPacket::kStartWhisper*
    GetSenderColor = 0x30000000 | 0x18, // wparam = UIPacket::kGetColor* // Get chat sender color depending on channel, output object passed by reference
    GetMessageColor = 0x30000000 | 0x19, // wparam = UIPacket::kGetColor* // Get chat message color depending on channel, output object passed by reference
    SendChatMessage = 0x30000000 | 0x1B, // wparam = UIPacket::kSendChatMessage*
    LogChatMessage = 0x30000000 | 0x1D, // wparam = UIPacket::kLogChatMessage*. Triggered when a message wants to be added to the persistent chat log.
    RecvWhisper = 0x30000000 | 0x1E, // wparam = UIPacket::kRecvWhisper*
    PrintChatMessage = 0x30000000 | 0x1F, // wparam = UIPacket::kPrintChatMessage*. Triggered when a message wants to be added to the in-game chat window.
    SendWorldAction = 0x30000000 | 0x20, // wparam = UIPacket::kSendWorldAction*
    SetRendererValue = 0x30000000 | 0x21 // wparam = UIPacket::kSetRendererValue
};
