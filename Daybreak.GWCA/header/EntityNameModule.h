#pragma once
#include "httplib.h"

namespace Daybreak::Modules::EntityNameModule {
    void GetName(const httplib::Request& req, httplib::Response& res);
}