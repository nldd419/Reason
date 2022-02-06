using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;

namespace Reason.Results
{
    public class AggregatedResult : Result
    {
        protected AggregatedResult(IEnumerable<Result> aggregatedResults, ReasonBase? reason) : base(aggregatedResults, reason)
        {
        }
    }
}
