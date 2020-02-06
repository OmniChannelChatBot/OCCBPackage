using System;

namespace OCCBPackage.Exceptions
{
    [Serializable]
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
