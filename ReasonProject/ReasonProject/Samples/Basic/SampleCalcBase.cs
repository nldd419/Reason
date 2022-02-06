using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReasonProject.Samples;
using Reason.Results;
using Reason.Reasons;

namespace ReasonProject.Samples.Basic
{
    internal class SampleCalcBase
    {
        public string CategoryBase => "Basic/";
        public virtual string CategoryName => "";
        public string Category => CategoryBase + CategoryName;

        protected Result Div(double numerator, double denominator, int indent)
        {
            Calculator calc = new Calculator();

            Result<double> result = calc.Divide(numerator, denominator);

            Result ret = result.Whether((v) =>
            {
                Utils.WriteLine($"Result is {v}", indent);
                return Result.MakeSuccessFirst();
            },
            (r) =>
            {
                Utils.WriteLine($"Failed reason is \"{r.Message}\"", indent);
                return result;
            });

            return ret;
        }

        protected double DivThrowingException(double numerator, double denominator)
        {
            Calculator calc = new Calculator();

            Result<double> result = calc.Divide(numerator, denominator);

            // To call 'Get' before calling 'IsFailed' causes a exception.
            return result.Get();
        }

        protected Result MakeNestedResult()
        {
            /*
             * [14]
             *  |_______________
             *  |               |
             * [6]             [13]
             *  |_______        |_______
             *  |       |       |       |
             * [2]     [5]     [9]     [12]
             *  |___    |___    |___    |____
             *  |   |   |   |   |   |   |    |
             * [0] [1] [3] [4] [7] [8] [10] [11]
             *  
             */

            Result result0 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={0}"));
            Result result1 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={1}"));
            Result result2 = Result.MakeAllSuccess(new List<Result> { result0, result1 }, new FailedReasonWithMessage($"Id={2}"));

            Result result3 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={3}"));
            Result result4 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={4}"));
            Result result5 = Result.MakeAllSuccess(new List<Result> { result3, result4 }, new FailedReasonWithMessage($"Id={5}"));

            Result result6 = Result.MakeAllSuccess(new List<Result> { result2, result5 }, new FailedReasonWithMessage($"Id={6}"));

            Result result7 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={7}"));
            Result result8 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={8}"));
            Result result9 = Result.MakeAllSuccess(new List<Result> { result7, result8 }, new FailedReasonWithMessage($"Id={9}"));

            Result result10 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={10}"));
            Result result11 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={11}"));
            Result result12 = Result.MakeAllSuccess(new List<Result> { result10, result11 }, new FailedReasonWithMessage($"Id={12}"));

            Result result13 = Result.MakeAllSuccess(new List<Result> { result9, result12 }, new FailedReasonWithMessage($"Id={13}"));

            Result result14 = Result.MakeAllSuccess(new List<Result> { result6, result13 }, new FailedReasonWithMessage($"Id={14}"));

            return result14;
        }

        protected Result MakeNestedResultShare()
        {
            /*
             * [4]
             *  |___ ___
             *  |   |   |
             *  |  [3] [2]
             *  |       |___
             *  |_______|   |
             *  |           |
             * [0]         [1]
             *  
             */

            Result result0 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={0}"));
            Result result1 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={1}"));
            Result result2 = Result.MakeAllSuccess(new List<Result> { result0, result1 }, new FailedReasonWithMessage($"Id={2}"));

            Result result3 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={3}"));
            Result result4 = Result.MakeAllSuccess(new List<Result> { result0, result3, result2 }, new FailedReasonWithMessage($"Id={4}"));

            return result4;
        }
    }
}
