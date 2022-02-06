using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Results;
using Reason.Reasons;

namespace ReasonProject
{
    internal class SampleFailedReasonDividedByZero : FailedReason
    {
        public override string Message { get => "Can't divide by zero."; }
    } 

    internal class SampleCalcClass
    {
        public Result<double> Divide(double numerator, double denominator)
        {
            if (denominator == 0) return Result<double>.MakeFailedFirst(new SampleFailedReasonDividedByZero());

            return Result<double>.MakeSuccessFirst(numerator / denominator);
        }
    }
}
