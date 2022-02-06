using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;

namespace Reason.Results
{
    /// <summary>
    /// Success when all results have succeeded.
    /// </summary>
    public class AllSuccessResult : AggregatedResult
    {
        public AllSuccessResult(IEnumerable<Result> aggregatedResults) : base(aggregatedResults, null)
        {
        }

        public AllSuccessResult(IEnumerable<Result> aggregatedResults, FailedReason reason) : base(aggregatedResults, reason)
        {
        }

        protected override ReasonBase? GetReasonCore()
        {
            List<Result> failedResults = NextResults.Where(r => r.IsFailed()).ToList();
            if (failedResults.Count <= 0) return new FailedReasonNotFailed();

            if (Reason != null) return Reason;

            return new FailedReasonCollection(failedResults.Select(r => r.GetReason()).Where(r => r is FailedReason).Select(r => (FailedReason)r));
        }
    }
}
