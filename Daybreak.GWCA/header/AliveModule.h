#pragma once
#include "httplib.h"

namespace http {
    namespace modules {
        void HandleAlive(const httplib::Request&, httplib::Response& res);
    }
}