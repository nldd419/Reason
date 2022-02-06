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
        public Result<double> Divide(double numerator, double denominator)
        {
            if (denominator == 0) return Result<double>.MakeFailedFirst(new FailedReasonDividedByZero());

            return Result<double>.MakeSuccessFirst(numerator / denominator);
        }
    }
}
