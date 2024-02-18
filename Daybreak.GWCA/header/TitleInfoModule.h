#pragma once
#include "httplib.h"

namespace Daybreak::Modules::TitleInfoModule {
    void GetTitleInfo(const httplib::Request& req, httplib::Response& res);
}