using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReasonProject.Samples;
using Reason.Results;
using Reason.Utils;

namespace ReasonProject.Samples.Basic
{
    internal class SampleClass1_2 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Basic";

        public string Title => "Handling Failure";

        public int Order => 2;

        public string[] Description => new string[]
        {
                "Handling a failed result is not complex, bacause the reason of the failure has already been set",
                "by the method you called.",
                "For example, calculating ( 1 / 0 ) causes an exception.",
                "Generally, it's hard to know why a method failed, because a method normally wants to return",
                "a value, not a message.",
                "Thanks to the method returning the 'Result' object, you can get the value when success, and can",
                "get the message when failure.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("This 'Div' has a checking logic whether the demoninator is 0. If so, it returns 'FailedResult' object.", indent);
            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result<decimal> result = Div(1, 0, indent);", indent);

            Utils.WriteLine("", indent);
            Result<decimal> result = Div(1, 0, indent);

            Utils.WriteLine("", indent);
            Utils.WriteLine("In this case, the method failed, so you can get the message it set.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "if (result.IsFailed())",
                "{",
                "    Utils.WriteLine(\"Operation Failed!\", indent);",
                "    Utils.WriteLine($\"The reason is '{result.GetReason().Message}'\", indent);",
                "}");

            Utils.WriteLine("", indent);
            if (result.IsFailed())
            {
                Utils.WriteLine("Operation Failed!", indent);
                Utils.WriteLine($"The reason is '{result.GetReason().Message}'", indent);
            }

            Utils.WriteLine("", indent);
            Utils.WriteLine("You can also use 'Result.Whether' method for handling a result. (See '1.Get Return Value' sample.)", indent);
        }
    }
}
