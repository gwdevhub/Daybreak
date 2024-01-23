#pragma once
#include "httplib.h"

namespace Daybreak::Modules::InventoryModule {
    void GetInventoryInfo(const httplib::Request&, httplib::Response& res);
}