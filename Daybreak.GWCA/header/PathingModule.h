#pragma once
#include "httplib.h"

namespace Daybreak::Modules::PathingModule {
    void GetPathingData(const httplib::Request&, httplib::Response& res);
}