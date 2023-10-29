#pragma once
#include "httplib.h"

namespace Daybreak::Modules::GameStateModule {
    void GetGameStateInfo(const httplib::Request&, httplib::Response& res);
}