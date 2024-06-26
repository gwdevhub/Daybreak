// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "httplib.h"
#include <GWCA/GWCA.h>
#include <GWCA/Utilities/Scanner.h>
#include <GWCA/Utilities/Hook.h>
#include <GWCA/Utilities/Hooker.h>
#include <GWCA/Managers/RenderMgr.h>
#include <GWCA/Managers/MemoryMgr.h>
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
#include "TitleInfoModule.h"
#include "WhisperModule.h"
#include "DebugModule.h"
#include <mutex>

volatile bool initialized;
volatile WNDPROC oldWndProc;
std::mutex startupMutex;
HMODULE dllmodule;
HANDLE serverThread;
static FILE* stdout_proxy;
static FILE* stderr_proxy;

void Terminate()
{
    http::server::StopServer();
    if (serverThread) {
        CloseHandle(serverThread);
    }

#ifdef BUILD_TYPE_DEBUG
    if (stdout_proxy)
        fclose(stdout_proxy);
    if (stderr_proxy)
        fclose(stderr_proxy);
    FreeConsole();
#endif
}


static DWORD WINAPI StartHttpServer(LPVOID)
{
    // This is a new thread so you should only initialize GWCA and setup the hook on the game thread.
    // When the game thread hook is setup (i.e. SetRenderCallback), you should do the next operations
    // on the game from within the game thread.
    const auto titleInfoModule = new Daybreak::Modules::TitleInfoModule();
    const auto entityNameModule = new Daybreak::Modules::EntityNameModule();
    const auto gameModule = new Daybreak::Modules::GameModule();
    const auto gameStateModule = new Daybreak::Modules::GameStateModule();
    const auto inventoryModule = new Daybreak::Modules::InventoryModule();
    const auto itemNameModule = new Daybreak::Modules::ItemNameModule();
    const auto loginModule = new Daybreak::Modules::LoginModule();
    const auto mainPlayerModule = new Daybreak::Modules::MainPlayerModule();
    const auto mapModule = new Daybreak::Modules::MapModule();
    const auto pathingMetadataModule = new Daybreak::Modules::PathingMetadataModule();
    const auto pathingModule = new Daybreak::Modules::PathingModule();
    const auto preGameModule = new Daybreak::Modules::PreGameModule();
    const auto sessionModule = new Daybreak::Modules::SessionModule();
    const auto userModule = new Daybreak::Modules::UserModule();
    const auto whisperModule = new Daybreak::Modules::WhisperModule();
    const auto debugModule = new Daybreak::Modules::DebugModule();

    http::server::SetLogger(http::ConsoleLogger);
    http::server::Get("/alive", http::modules::HandleAlive);
    http::server::Get("/id", http::modules::HandleProcessId);
    http::server::Get(mapModule->ApiUri(), [&mapModule](const httplib::Request& req, httplib::Response& res) { mapModule->HandleApiCall(req, res); });
    http::server::Get(loginModule->ApiUri(), [&loginModule](const httplib::Request& req, httplib::Response& res) { loginModule->HandleApiCall(req, res); });
    http::server::Get(pathingMetadataModule->ApiUri(), [&pathingMetadataModule](const httplib::Request& req, httplib::Response& res) { pathingMetadataModule->HandleApiCall(req, res); });
    http::server::Get(pathingModule->ApiUri(), [&pathingModule](const httplib::Request& req, httplib::Response& res) { pathingModule->HandleApiCall(req, res); });
    http::server::Get(preGameModule->ApiUri(), [&preGameModule](const httplib::Request& req, httplib::Response& res) { preGameModule->HandleApiCall(req, res); });
    http::server::Get(userModule->ApiUri(), [&userModule](const httplib::Request& req, httplib::Response& res) { userModule->HandleApiCall(req, res); });
    http::server::Get(gameModule->ApiUri(), [&gameModule](const httplib::Request& req, httplib::Response& res) { gameModule->HandleApiCall(req, res); });
    http::server::Get(inventoryModule->ApiUri(), [&inventoryModule](const httplib::Request& req, httplib::Response& res) { inventoryModule->HandleApiCall(req, res); });
    http::server::Get(mainPlayerModule->ApiUri(), [&mainPlayerModule](const httplib::Request& req, httplib::Response& res) { mainPlayerModule->HandleApiCall(req, res); });
    http::server::Get(gameStateModule->ApiUri(), [&gameStateModule](const httplib::Request& req, httplib::Response& res) { gameStateModule->HandleApiCall(req, res); });
    http::server::Get(sessionModule->ApiUri(), [&sessionModule](const httplib::Request& req, httplib::Response& res) { sessionModule->HandleApiCall(req, res); });
    http::server::Get(entityNameModule->ApiUri(), [&entityNameModule](const httplib::Request& req, httplib::Response& res) { entityNameModule->HandleApiCall(req, res); });
    http::server::Get(itemNameModule->ApiUri(), [&itemNameModule](const httplib::Request& req, httplib::Response& res) { itemNameModule->HandleApiCall(req, res); });
    http::server::Get(titleInfoModule->ApiUri(), [&titleInfoModule](const httplib::Request& req, httplib::Response& res) { titleInfoModule->HandleApiCall(req, res); });
    http::server::Post(whisperModule->ApiUri(), [&whisperModule](const httplib::Request& req, httplib::Response& res) { whisperModule->HandleApiCall(req, res); });
    http::server::Get(debugModule->ApiUri(), [&debugModule](const httplib::Request& req, httplib::Response& res) { debugModule->HandleApiCall(req, res); });
    http::server::StartServer();
    return 0;
}

LRESULT CALLBACK WndProc(const HWND hWnd, const UINT Message, const WPARAM wParam, const LPARAM lParam) {
    if (Message == WM_CLOSE || (Message == WM_SYSCOMMAND && wParam == SC_CLOSE)) {
        Terminate();
    }

    return CallWindowProc(oldWndProc, hWnd, Message, wParam, lParam);
}

LRESULT CALLBACK SafeWndProc(const HWND hWnd, const UINT Message, const WPARAM wParam, const LPARAM lParam) noexcept
{
    __try {
        return WndProc(hWnd, Message, wParam, lParam);
    }
    __except (EXCEPTION_EXECUTE_HANDLER) {
        return CallWindowProc(oldWndProc, hWnd, Message, wParam, lParam);
    }
}

void OnWindowCreated(IDirect3DDevice9*) {
    startupMutex.lock();
    if (initialized) {
        startupMutex.unlock();
        return;
    }

#ifdef BUILD_TYPE_DEBUG
    AllocConsole();
    SetConsoleTitleA("Daybreak.GWCA Console");
    freopen_s(&stdout_proxy, "CONOUT$", "w", stdout);
    freopen_s(&stderr_proxy, "CONOUT$", "w", stderr);
#endif
    auto handle = GW::MemoryMgr::GetGWWindowHandle();
    oldWndProc = reinterpret_cast<WNDPROC>(SetWindowLongPtrW(handle, GWL_WNDPROC, reinterpret_cast<LONG>(SafeWndProc)));
    serverThread = CreateThread(
        NULL,
        0,
        StartHttpServer,
        NULL,
        0,
        NULL);
    startupMutex.unlock();
    initialized = true;
    return;
}


static DWORD WINAPI Init(LPVOID)
{
    printf("Init: Setting up scanner\n");
    GW::Scanner::Initialize();
    printf("Init: Setting up hook base\n");
    GW::HookBase::Initialize();
    printf("Init: Setting up GWCA\n");
    if (!GW::Initialize()) {
        printf("Init: Failed to set up GWCA\n");
        return 0;
    }

    printf("Init: Enabling hooks\n");
    GW::HookBase::EnableHooks();
    printf("Init: Set up render callback\n");
    GW::Render::SetRenderCallback(OnWindowCreated);
    printf("Init: Returning success\n");
    FreeLibraryAndExitThread(dllmodule, EXIT_SUCCESS);
    return 0;
}


// DLL entry point, dont do things in this thread unless you know what you are doing.
BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    DisableThreadLibraryCalls(hModule);

    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {
        HANDLE handle = CreateThread(0, 0, Init, hModule, 0, 0);
        CloseHandle(handle);
    }

    return TRUE;
}