#pragma once
#include "httplib.h"

namespace Daybreak::Modules::MapModule {
    void GetMapInfo(const httplib::Request&, httplib::Response& res);
}