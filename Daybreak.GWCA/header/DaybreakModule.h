#pragma once

#include <httplib.h>
#include <GWCA/Managers/GameThreadMgr.h>
#include <thread>
#include <json.hpp>
#include <semaphore>
#include <future>
#include <memory>
#include <string>
#include <optional>

using nlohmann::json;

namespace Daybreak {
    template <typename TPayload, typename TContext>
    class DaybreakModule {
        protected:
            virtual std::chrono::milliseconds GetTimeout() {
                return std::chrono::milliseconds(200);
            }
            std::future<std::optional<TPayload>> EnqueueWithReturn(const TContext context) {
                std::shared_ptr<std::promise<std::optional<TPayload>>> promise = std::make_shared<std::promise<std::optional<TPayload>>>();
                std::future<std::optional<TPayload>> ret = promise->get_future();
                GW::GameThread::Enqueue([this, context, promise]() {
                    try {
                        const std::optional<TPayload> result = this->GetPayload(context);
                        promise->set_value(result);
                    }
                    catch (...) {
                        promise->set_exception(std::current_exception());
                    }
                    });

                return ret;
            }
            virtual std::optional<TContext> GetContext(const httplib::Request& req, httplib::Response& res) = 0;
            virtual bool CanReturn(const httplib::Request& req, httplib::Response& res, const TPayload& payload) {
                return true;
            }
            virtual std::tuple<std::string, std::string> ReturnPayload(TPayload payload) {
                return std::make_tuple(static_cast<json>(payload).dump(), "application/json");
            }
            virtual void OnNoValue(httplib::Response& res) {
                res.status = 500;
                res.body = "No payload";
            }
            virtual std::optional<TPayload> GetPayload(const TContext context) = 0;
        public:
            virtual std::string ApiUri() = 0;
            void HandleApiCall(const httplib::Request& req, httplib::Response& res) {
                const auto startTime = std::chrono::steady_clock::now();
                const std::optional<TContext> context = this->GetContext(req, res);
                if (!context.has_value()) {
                    return;
                }

                std::future<std::optional<TPayload>> future = this->EnqueueWithReturn(context.value());
                std::optional<TPayload> payload = future.get();
                if (!payload.has_value()) {
                    this->OnNoValue(res);
                    return;
                }

                /*
                * Wait until timeout. Normally timeout should be handled differently, but since the modules
                * wait only for strings to populate, we can return the rest of the payload just fine and let
                * Daybreak handle the missing string
                */
                while (!this->CanReturn(req, res, payload.value()) &&
                    std::chrono::steady_clock::now() - startTime < this->GetTimeout()) {
                    std::this_thread::sleep_for(std::chrono::milliseconds(1));
                }

                std::tuple<std::string, std::string> contentResponse = this->ReturnPayload(payload.value());
                res.status = 200;
                res.set_content(std::get<0>(contentResponse), std::get<1>(contentResponse));
            }
    };
}