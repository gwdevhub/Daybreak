#pragma once
#include "httplib.h"

namespace Daybreak::Modules::PreGameModule {
    void GetPreGameInfo(const httplib::Request&, httplib::Response& res);
}