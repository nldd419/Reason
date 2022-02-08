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
    internal class SampleClass2_1 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Exception";

        public string Title => "3.Catch Exception And Show Message";

        public string[] Description => new string[]
        {
            "You can catch an exception and convert it into 'ExceptionResult' object."
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("Give the statements you want to catch an exception to the 'Result.CatchAll' method parameter.", indent);
            Utils.WriteLine("You can use 'Result.Catch<E>' generic methods as well.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "Result result = Result.CatchAll(() =>",
                "{",
                "    Utils.WriteLine(\"\", indent);",
                "    Utils.WriteLine($\"Calculating(1 / 0)...\", indent);",
                "",
                "    // This causes throwing an exception.",
                "    decimal d = 0;",
                "    decimal tmp = 1 / d;",
                "",
                "    return Result.MakeSuccessFirst();",
                "}, useMessagePropertyAsMessage: true);");

            Result result = Result.CatchAll(() =>
            {
                Utils.WriteLine("", indent);
                Utils.WriteLine($"Calculating ( 1 / 0 )...", indent);

                // This causes throwing an exception.
                decimal d = 0;
                decimal tmp = 1 / d;

                return Result.MakeSuccessFirst();
            }, useMessagePropertyAsMessage: true);

            Utils.WriteLine("", indent);
            Utils.WriteLine("See the result.", indent);

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
        }
    }
}
