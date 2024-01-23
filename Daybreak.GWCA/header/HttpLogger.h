#pragma once
#include "httplib.h"

namespace http {
    void ConsoleLogger(const httplib::Request req, httplib::Response res);
}