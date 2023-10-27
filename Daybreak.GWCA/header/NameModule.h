#pragma once
#include "httplib.h"

namespace Daybreak::Modules::NameModule {
    void GetName(const httplib::Request& req, httplib::Response& res);
}