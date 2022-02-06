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
    /// Class which represents a success result with a value.
    /// </summary>
    /// <remarks>
    /// <see cref="Result.IsFailed"/> of this class never return true, and this class is never throwing a <see cref="ReasonCustomException"/> when calling <see cref="Result{T}.Get"/>,
    /// because it's guaranteed to have a value.
    /// </remarks>
    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T val) : base(val, null, new List<Result>()) { }
        public SuccessResult(T val, IEnumerable<Result> nextResults) : base(val, null, nextResults) { }

        protected override ReasonBase GetReasonCore()
        {
            return new FailedReasonNotFailed();
        }
    }

}
