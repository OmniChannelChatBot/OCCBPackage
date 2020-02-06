using System;

namespace OCCBPackage.Exceptions
{
    [Serializable]
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
