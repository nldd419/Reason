using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Reason.Exceptions
{
    // I compromised on throwing exceptions when specific cases.
    // I wish to do it as minimum as possible though.

    [Serializable()]
    public sealed class ReasonCustomException : Exception
    {
        public ReasonCustomException()
        {
        }

        public ReasonCustomException(string? message) : base(message)
        {
        }

        public ReasonCustomException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        private ReasonCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
