#pragma once
#include "httplib.h"

namespace Daybreak::Modules::UserModule {
    void GetUserInfo(const httplib::Request&, httplib::Response& res);
}