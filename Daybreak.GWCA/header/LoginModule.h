#pragma once
#include "httplib.h"

namespace Daybreak::Modules::LoginModule {
    void GetLoginInfo(const httplib::Request&, httplib::Response& res);
}