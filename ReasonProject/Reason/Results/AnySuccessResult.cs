using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;

namespace Reason.Results
{
    /// <summary>
    /// Success when any of child results have succeeded.
    /// </summary>
    public class AnySuccessResult : AggregatedResult
    {
        public AnySuccessResult(IEnumerable<Result> aggregatedResults) : base(aggregatedResults, null)
        {
        }

        public AnySuccessResult(IEnumerable<Result> aggregatedResults, FailedReason reason) : base(aggregatedResults, reason)
        {
        }

        protected override ReasonBase? GetReasonCore()
        {
            List<Result> failedResults = NextResults.Where(r => r.IsFailed()).ToList();
            if (failedResults.Count != NextResults.Count()) return new FailedReasonNotFailed();

            if (Reason != null) return Reason;

            return new FailedReasonCollection(failedResults.Select(r => r.GetReason()).Where(r => r is FailedReason).Select(r => (FailedReason)r));
        }
    }
}
