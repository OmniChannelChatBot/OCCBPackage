using System;

namespace OCCBPackage.Exceptions
{
    [Serializable]
    public class GatewayTimeoutException : ApiException
    {
        public GatewayTimeoutException(string message)
            : base(message)
        {
        }

        public GatewayTimeoutException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
