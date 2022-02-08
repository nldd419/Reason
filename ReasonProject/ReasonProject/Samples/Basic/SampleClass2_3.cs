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
    internal class SampleClass2_3 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Exception";

        public string Title => "5.Catch Exception And Show Custom Message";

        public string[] Description => new string[]
        {
            "If you want to customize the message of 'ExceptionResult', you can pass a function which takes",
            "one parameter as Exception to the 'Result.CatchAll's second parameter.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("This time, we pass a function which returns a message about an exception type.", indent);

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
                "}, (ex) =>",
                "{",
                "    return $\"This excaption type is { ex.GetType() }\";",
                "});");

            Result result = Result.CatchAll(() =>
            {
                Utils.WriteLine("", indent);
                Utils.WriteLine($"Calculating ( 1 / 0 )...", indent);

                // This causes throwing an exception.
                decimal d = 0;
                decimal tmp = 1 / d;

                return Result.MakeSuccessFirst();
            }, (ex) =>
            {
                return $"This excaption type is {ex.GetType()}";
            });

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
