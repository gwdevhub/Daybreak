// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "httplib.h"
#include <GWCA/GWCA.h>
#include "AliveModule.h"
#include "ProcessIdModule.h"
#include "HttpLogger.h"
#include "Server.h"
#include "MapModule.h"
#include "LoginModule.h"
#include "PathingMetadataModule.h"
#include "PathingModule.h"
#include "PreGameModule.h"
#include "InventoryModule.h"
#include "UserModule.h"
#include "GameModule.h"
#include "MainPlayerModule.h"
#include "SessionModule.h"
#include "EntityNameModule.h"
#include "GameStateModule.h"
#include "ItemNameModule.h"

static FILE* stdout_proxy;
static FILE* stderr_proxy;


static DWORD WINAPI ThreadProc(LPVOID lpModule)
{
    // This is a new thread so you should only initialize GWCA and setup the hook on the game thread.
    // When the game thread hook is setup (i.e. SetRenderCallback), you should do the next operations
    // on the game from within the game thread.

    HMODULE hModule = static_cast<HMODULE>(lpModule);
#ifdef BUILD_TYPE_DEBUG
    AllocConsole();
    SetConsoleTitleA("Daybreak.GWCA Console");
    freopen_s(&stdout_proxy, "CONOUT$", "w", stdout);
    freopen_s(&stderr_proxy, "CONOUT$", "w", stderr);
#endif
    GW::Initialize();
    http::server::SetLogger(http::ConsoleLogger);
    http::server::Get("/alive", http::modules::HandleAlive);
    http::server::Get("/id", http::modules::HandleProcessId);
    http::server::Get("/map", Daybreak::Modules::MapModule::GetMapInfo);
    http::server::Get("/login", Daybreak::Modules::LoginModule::GetLoginInfo);
    http::server::Get("/pathing/metadata", Daybreak::Modules::PathingMetadataModule::GetPathingMetadata);
    http::server::Get("/pathing", Daybreak::Modules::PathingModule::GetPathingData);
    http::server::Get("/pregame", Daybreak::Modules::PreGameModule::GetPreGameInfo);
    http::server::Get("/user", Daybreak::Modules::UserModule::GetUserInfo);
    http::server::Get("/game", Daybreak::Modules::GameModule::GetGameInfo);
    http::server::Get("/inventory", Daybreak::Modules::InventoryModule::GetInventoryInfo);
    http::server::Get("/game/mainplayer", Daybreak::Modules::MainPlayerModule::GetMainPlayer);
    http::server::Get("/game/state", Daybreak::Modules::GameStateModule::GetGameStateInfo);
    http::server::Get("/session", Daybreak::Modules::SessionModule::GetSessionInfo);
    http::server::Get("/entities/name", Daybreak::Modules::EntityNameModule::GetName);
    http::server::Get("/items/name", Daybreak::Modules::ItemNameModule::GetName);
    http::server::StartServer();

#ifdef BUILD_TYPE_DEBUG
    if (stdout_proxy)
        fclose(stdout_proxy);
    if (stderr_proxy)
        fclose(stderr_proxy);
    FreeConsole();
#endif

    FreeLibraryAndExitThread(hModule, EXIT_SUCCESS);
}


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    DisableThreadLibraryCalls(hModule);

    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {
        HANDLE handle = CreateThread(0, 0, ThreadProc, hModule, 0, 0);
        CloseHandle(handle);
    }

    return TRUE;
}