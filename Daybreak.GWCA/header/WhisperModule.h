#pragma once
#include "httplib.h"

namespace Daybreak::Modules::WhisperModule {
    void PostWhisper(const httplib::Request&, httplib::Response& res);
}