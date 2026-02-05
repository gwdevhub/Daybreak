# Daybreak API

## Summary

- **Daybreak API** is a lightweight HTTP service
- Daybreak injects this service into Guild Wars on launch, when **Focus View** is enabled
- Built as a NativeAOT–compiled C# DLL
- Automatically loaded into the game process and hosts a REST+WebSocket API on `http://localhost:5080-5100`
- All endpoints are documented via Swagger UI at [/swagger/index.html](http://localhost:5080/swagger/index.html)
- Can query character select info, in-game state, builds, party loadouts, PvP stats, quest logs, and more—making it easy to build external tools, overlays or bots that integrate seamlessly with your running Guild Wars session

### Base URLs

- `/api/v1/rest/...`
- `/api/v1/ws/...`

### Character Select

`GET api/v1/rest/character-select`
Fetches the list of characters and the one you’re currently on

`POST api/v1/rest/character-select/{identifier}`
Switches to another character (by UUID or name)

### Login

`GET api/v1/rest/login`
Retrieves your current login info

### Main Player

`GET api/v1/rest/main-player/state`
Returns a snapshot of your live in-game state

`GET api/v1/rest/main-player/info`
Account & PvP stats

`GET api/v1/rest/main-player/quest-log`
Your current quest and quest backlog

`GET api/v1/rest/main-player/build`
Fetches your equipped build

`POST api/v1/rest/main-player/build?code={buildString}`
Applies a new build by build template

`GET api/v1/rest/main-player/instance-info`
Details about your current map instance

`GET api/v1/rest/main-player/title`
Title progress information (e.g. Luxon/Kurzick)

`GET api/v1/ws/main-player/state`
Connect with a websocket to receive state payloads on each game thread proc

### Party

`GET api/v1/rest/party/loadout`
Get your party loadout. This includes current builds for the player and heroes, hero positioning and hero behavior

`POST api/v1/rest/party/loadout`
Sets your party loadout, including builds, hero positionings and hero behaviors

### Inventory

`GET api/v1/rest/inventory`
Fetches your current inventory items. This includes item IDs, quantities and names

### Service Health

`GET api/v1/rest/health`
Checks that Daybreak.API is up and all sub-components are healthy

### Component Schemas

Schema | Purpose
-- | --
AttributeEntry | Single attribute (id, basePoints, totalPoints)
BuildEntry | Your build export: professions, attributes, skill IDs
CharacterSelectEntry | One character on your account (uuid, name, professions, level, map, etc.)
CharacterSelectInformation | Wrapper for current + available characters
LoginInfo | Your email & active character name
MainPlayerState | Live state: XP, level, currencies, HP/energy, position
MainPlayerInformation | PvP stats: wins, losses, rating, rank, qualifier & reward points
QuestInformation | One quest link (from-map/to-map)
QuestLogInformation | Current quest + quest list
InstanceInfo | Current map instance details
TitleInfo | Title progression (points, tiers)
PartyLoadout | Collection of PartyLoadoutEntry
PartyLoadoutEntry | One hero: hero ID, behavior, embedded BuildEntry
HealthCheckResponse | Overall service health summary
HealthCheckEntryResponse | Per-component health details (status, description, data, tags)
InventoryInformation | List of bags with their items, quantities and names
