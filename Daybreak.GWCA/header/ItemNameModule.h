#pragma once
#include "httplib.h"

namespace Daybreak::Modules::ItemNameModule {
    void GetName(const httplib::Request& req, httplib::Response& res);
}