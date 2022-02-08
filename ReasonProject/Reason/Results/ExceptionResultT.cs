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
    public class ExceptionResult<E, T> : FailedResult<T> where E : Exception
    {
        public ExceptionResult(FailedReasonException<E> reason) : base(reason, new List<Result>()) { this.ExceptionReason = reason; }
        public ExceptionResult(FailedReasonException<E> reason, IEnumerable<Result> nextResults) : base(reason, nextResults) { this.ExceptionReason = reason; }
        /// <summary>
        /// Convert <see cref="ExceptionResult{E}"/> into <see cref="ExceptionResult{E, T}"/>. This is ok because 'Exception Result' always has no values.
        /// </summary>
        public ExceptionResult(ExceptionResult<E> exceptionResult) : this(exceptionResult.ExceptionReason, exceptionResult.NextResults) { }

        public readonly FailedReasonException<E> ExceptionReason;
    }

}
