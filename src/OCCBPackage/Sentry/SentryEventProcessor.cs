using Microsoft.AspNetCore.Http;
using Sentry;
using Sentry.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCCBPackage.Sentry
{
    public class SentryEventProcessor : ISentryEventProcessor
    {
        private readonly IHttpContextAccessor _httpContext;

        public SentryEventProcessor(IHttpContextAccessor httpContext) => _httpContext = httpContext;

        public SentryEvent Process(SentryEvent @event)
        {
            // Here I can modify the event, while taking dependencies via DI

            @event.SetExtra("Service name", Environments.GetServiceName());
            return @event;
        }
    }
}
