#include "pch.h"
#include <iostream>
#include <limits>
#include <windows.h>
#include <string>
#include <cstdint>

namespace Daybreak::Utils {
    std::wstring StringToWString(const std::string& str) {
        if (str.empty()) return std::wstring();

        int size_needed = MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), NULL, 0);
        std::wstring wstrTo(size_needed, 0);
        MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), &wstrTo[0], size_needed);

        return wstrTo;
    }

    std::string WStringToString(const std::wstring& wstr)
    {
        if (wstr.empty()) return std::string();

        int size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
        std::string strTo(size_needed, 0);
        WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], size_needed, NULL, NULL);

        return strTo;
    }

    bool StringToInt(const std::string& str, int& outValue) {
        size_t pos = 0;
        unsigned long result = std::stoul(str, &pos);

        // Ensure the entire string was processed
        if (pos != str.size()) {
            return false;
        }

        outValue = static_cast<int>(result);
        return true;
    }
}