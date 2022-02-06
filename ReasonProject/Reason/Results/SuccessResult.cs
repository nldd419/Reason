using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;

namespace Reason.Results
{
    /// <summary>
    /// Class which represents a success result without a value.
    /// </summary>
    public class SuccessResult : Result
    {
        public SuccessResult() : base(new List<Result>(), null) { }
        public SuccessResult(IEnumerable<Result> nextResults) : base(nextResults, null) { }

        protected override ReasonBase GetReasonCore()
        {
            return new FailedReasonNotFailed();
        }
    }

}
