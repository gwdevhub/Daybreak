#pragma once
#include <string>

namespace Daybreak::Utils {
    std::string WStringToString(const std::wstring& wstr);
    bool StringToInt(const std::string& str, int& outValue);
}