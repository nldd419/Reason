using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Results;

namespace ReasonProject.Samples.Basic
{
    internal class Calculator
    {
        public Result<decimal> Divide(decimal numerator, decimal denominator)
        {
            if (denominator == 0) return Result<decimal>.MakeFailedFirst(new FailedReasonDividedByZero());

            return Result<decimal>.MakeSuccessFirst(numerator / denominator);
        }
    }
}
