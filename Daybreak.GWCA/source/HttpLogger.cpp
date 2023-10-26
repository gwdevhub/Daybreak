#include "HttpLogger.h"

namespace http {
    void ConsoleLogger(const httplib::Request req, httplib::Response res) {
        std::cout << req.method << " " << req.path << std::endl << res.status << std::endl;
    }
}