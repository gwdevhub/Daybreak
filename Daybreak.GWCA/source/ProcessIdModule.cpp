#include "pch.h"
#include "ProcessIdModule.h"

namespace http {
    namespace modules {
        static DWORD processId = 0;

        int GetProcessId() {
            if (processId > 0) {
                return processId;
            }

            processId = GetCurrentProcessId();
            return processId;
        }

        void HandleProcessId(const httplib::Request&, httplib::Response& res) {
            res.set_content(std::to_string(GetProcessId()), "text/plain");
        }
    }
}