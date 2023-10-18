#include "pch.h"
#include "AliveModule.h"

namespace http {
    namespace modules {
        void HandleAlive(const httplib::Request&, httplib::Response& res) {
            res.set_content("Yes!", "text/plain");
        }
    }
}