using System;

namespace OCCBPackage.Exceptions
{
    [Serializable]
    public class PreconditionFailedException : ApiException
    {
        public PreconditionFailedException(string message)
            : base(message)
        {
        }

        public PreconditionFailedException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
