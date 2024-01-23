#pragma once
#include "httplib.h"

namespace Daybreak::Modules::PathingMetadataModule {
    void GetPathingMetadata(const httplib::Request&, httplib::Response& res);
}