#include "pch.h"
#include <iostream>
#include <windows.h>
#include <vector>
#include <thread>

namespace PipeServer {
    // Function to handle a single client connection
    void HandleClient(HANDLE hPipe)
    {
        while (true)
        {
            char buffer[1024];
            DWORD bytesRead;

            if (!ReadFile(hPipe, buffer, sizeof(buffer), &bytesRead, NULL) || bytesRead == 0)
            {
                break; // Error or client closed the connection
            }

            // Process data received from the client (e.g., display or manipulate it)
            std::cout << "Received from client: " << buffer << std::endl;

            // Send a response back to the client (optional)
            const char* response = "Hello from server!";
            DWORD bytesWritten;
            WriteFile(hPipe, response, static_cast<DWORD>(strlen(response) + 1), &bytesWritten, NULL);
        }

        // Close the pipe handle when the client disconnects
        CloseHandle(hPipe);
    }

    void StartServer()
    {
        DWORD processId = GetCurrentProcessId();
        // Create a buffer to store the formatted process ID as a wide string
        wchar_t processIdStr[12]; // Assuming DWORD can be represented in 10 characters

        // Format the process ID into the buffer as a wide string
        swprintf_s(processIdStr, L"%u", processId);
        std::wstring pipeName = L"\\\\.\\pipe\\Daybreak";
        pipeName += processIdStr;

        HANDLE hPipe = CreateNamedPipeW(
            pipeName.c_str(), // Use the generated pipe name
            PIPE_ACCESS_DUPLEX,
            PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT,
            1,
            1024, 1024,
            1000,
            NULL
        );

        if (hPipe == INVALID_HANDLE_VALUE)
        {
            std::cerr << "Error: CreateNamedPipe failed with error code " << GetLastError() << std::endl;
            return;
        }

        std::cout << "Created named pipe ";
        wprintf(L"%s\n", pipeName.c_str());
        std::cout << "Waiting for clients to connect..." << std::endl;
        // Wait for client connections and handle them in separate threads
        std::vector<std::thread> clientThreads;
        HANDLE io_event = CreateEvent(NULL, FALSE, FALSE, NULL);
        OVERLAPPED overlapped;
        if (io_event == NULL) {
            std::cout << "Failed to create event" << std::endl;
            return;
        }

        while (true)
        {
            overlapped.hEvent = io_event;
            if (ConnectNamedPipe(hPipe, &overlapped))
            {
                // A client has connected, create a thread to handle it
                std::cout << "Client connected" << std::endl;
                clientThreads.emplace_back(HandleClient, hPipe);
                clientThreads.back().detach();
            }
            else {
                DWORD err = GetLastError();
                std::cout << "Failed to connect named pipe " << err << std::endl;
                if (err == ERROR_PIPE_CONNECTED) {
                    std::cout << "Pipe connection was successful. Client connected" << std::endl;
                    clientThreads.emplace_back(HandleClient, hPipe);
                    clientThreads.back().detach();
                }

                if (err == ERROR_IO_PENDING)
                {
                    std::cout << "Error IO Pending" << std::endl;
                }
                if (err == ERROR_PIPE_LISTENING)
                {
                    std::cout << "Error Pipe Listening" << std::endl;
                }
            }
        }

        CloseHandle(hPipe); // Close the server pipe when done
        return;
    }
}