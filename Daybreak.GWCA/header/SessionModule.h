#pragma once
#include "httplib.h"

namespace Daybreak::Modules::SessionModule {
    void GetSessionInfo(const httplib::Request&, httplib::Response& res);
}