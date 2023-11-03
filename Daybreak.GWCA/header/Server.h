#pragma once
#include "httplib.h"
#include "pch.h"

namespace http {
    namespace server {
        bool StartServer();
        void StopServer();
        void SetLogger(httplib::Logger logger);
        void Get(const std::string& pattern, httplib::Server::Handler handler);
    }
}