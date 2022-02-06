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
    /// Class which represents a failed result.
    /// </summary>
    /// <remarks>The value might be non-null default, but trying to get the value is throwing a <see cref="ReasonCustomException"/> because it has a 'failed reason'.</remarks>
    public class FailedResult<T> : Result<T>
    {
        public FailedResult(FailedReason reason) : base(default, reason, new List<Result>())
        {
            this.reason = reason;
        }

        public FailedResult(FailedReason reason, IEnumerable<Result> nextResults) : base(default, reason, nextResults) { this.reason = reason; }

        private FailedReason reason;

        protected override FailedReason GetReasonCore()
        {
            return this.reason;
        }
    }

}
