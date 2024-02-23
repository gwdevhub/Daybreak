#pragma once
#include "httplib.h"

namespace Daybreak::Modules::CartographerModule {
    void GetCartographerAreas(const httplib::Request&, httplib::Response& res);
}