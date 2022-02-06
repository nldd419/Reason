using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;

namespace ReasonProject.Samples.Basic
{
    internal class FailedReasonDividedByZero : FailedReason
    {
        public override string Message { get => "You can't divide by zero."; }
    }
}
