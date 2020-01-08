using System;

namespace OCCBPackage
{
    internal class Environments
    {
        public static string GetServiceName() => Environment.GetEnvironmentVariable("SERVICE_NAME")
            ?? throw new ApplicationException("Environment Variable SERVICE_NAME is null");
    }
}
