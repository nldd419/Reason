using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;
using Reason.Exceptions;

namespace Reason.Results
{
    /// <summary>
    /// Class which represents an exception result.
    /// </summary>
    public class ExceptionResult<E> : FailedResult where E : Exception
    {
        public ExceptionResult(FailedReasonException<E> reason) : base(reason, new List<Result>()) { this.ExceptionReason = reason; }
        public ExceptionResult(FailedReasonException<E> reason, IEnumerable<Result> nextResults) : base(reason, nextResults) { this.ExceptionReason = reason; }

        public readonly FailedReasonException<E> ExceptionReason;
    }

}
