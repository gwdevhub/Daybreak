#pragma once
#include <string>

namespace Daybreak::Utils {
    std::string WStringToString(const std::wstring& wstr);
    std::wstring StringToWString(const std::string& str);
    bool StringToInt(const std::string& str, int& outValue);
}