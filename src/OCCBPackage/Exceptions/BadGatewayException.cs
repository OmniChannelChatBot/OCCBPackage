using System;

namespace OCCBPackage.Exceptions
{
    [Serializable]
    public class BadGatewayException : ApiException
    {
        public BadGatewayException(string message)
            : base(message)
        {
        }

        public BadGatewayException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
