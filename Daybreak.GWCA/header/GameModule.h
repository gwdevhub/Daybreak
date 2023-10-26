#pragma once
#include "httplib.h"

namespace Daybreak::Modules::GameModule {
    void GetGameInfo(const httplib::Request&, httplib::Response& res);
}