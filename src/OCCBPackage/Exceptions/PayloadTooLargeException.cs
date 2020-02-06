using System;

namespace OCCBPackage.Exceptions
{
    [Serializable]
    public class PayloadTooLargeException : ApiException
    {
        public PayloadTooLargeException(string message)
            : base(message)
        {
        }

        public PayloadTooLargeException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
