using System;

namespace OCCBPackage.Exceptions
{
    [Serializable]
    public class AuthenticationException : ApiException
    {
        public AuthenticationException(string message)
            : base(message)
        {
        }

        public AuthenticationException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
