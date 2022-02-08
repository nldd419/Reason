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
    internal class SampleClass2_2 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Exception";

        public string Title => "4.Catch Exception And Show Stacktrace";

        public string[] Description => new string[]
        {
            "Besides exception messages, you can show the stacktrace either.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("This time, we set 'useMessagePropertyAsMessage' to false.", indent);

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
                "}, useMessagePropertyAsMessage: false);");

            Result result = Result.CatchAll(() =>
            {
                Utils.WriteLine("", indent);
                Utils.WriteLine($"Calculating ( 1 / 0 )...", indent);

                // This causes throwing an exception.
                decimal d = 0;
                decimal tmp = 1 / d;

                return Result.MakeSuccessFirst();
            }, useMessagePropertyAsMessage: false);

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
