#pragma once
#include "httplib.h"

namespace Daybreak::Modules::MainPlayerModule {
    void GetMainPlayer(const httplib::Request&, httplib::Response& res);
}