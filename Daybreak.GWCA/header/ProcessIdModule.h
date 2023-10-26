#pragma once
#include "httplib.h"

namespace http {
    namespace modules {
        void HandleProcessId(const httplib::Request&, httplib::Response& res);
    }
}