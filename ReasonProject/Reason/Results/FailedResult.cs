using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;

namespace Reason.Results
{
    /// <summary>
    /// Class which represents a failed result.
    /// </summary>
    public class FailedResult : Result
    {
        public FailedResult(FailedReason reason) : base(new List<Result>(), reason)
        {
            this.reason = reason;
        }

        public FailedResult(FailedReason reason, IEnumerable<Result> nextResults) : base(nextResults, reason) { this.reason = reason; }

        private FailedReason reason;

        protected override FailedReason GetReasonCore()
        {
            return this.reason;
        }
    }

}
