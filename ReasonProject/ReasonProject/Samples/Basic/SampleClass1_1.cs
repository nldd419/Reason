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
    internal class SampleClass1_1 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Basic";

        public string Title => "1.Get Return Value";

        public string[] Description => new string[]
        {
                "Implementing logic such thet all functions always returing a result value and a message makes",
                "me feel better.",
                "However, if you're sure that the method never throws any exception and is always success,",
                "you can also simply return a primitive value.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("This 'Div' calculates {first arg} / {second arg} and then return 'Result' object.", indent);
            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result<decimal> result = Div(1, 2, indent);", indent);

            Utils.WriteLine("", indent);
            Result<decimal> result = Div(1, 2, indent);

            Utils.WriteLine("", indent);
            Utils.WriteLine("After that, you can retrieve the value using 'Result.Whether' method which takes some callback arguments.", indent);
            Utils.WriteLine("One is called when success, another is called when fail.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "result.Whether(",
                "(val) =>",
                "{",
                "    Utils.Write($\"The result is {val}.\");",
                "    return result; // You may delegate the result.",
                "},",
                "(failedReason) =>",
                "{",
                "    Utils.WriteLine(\"Operation Failed!\", indent);",
                "    Utils.WriteLine($\"The reason is '{failedReason.Message}'\", indent);",
                "    return result; // If you are not responsible for this failure, you may delegate the result as well.",
                "}); ");

            Utils.WriteLine("", indent);
            result.Whether(
            (val) =>
            {
                Utils.WriteLine($"The result is {val}.", indent);
                return result; // You may delegate the result.
            },
            (failedReason) =>
            {
                Utils.WriteLine("Operation Failed!", indent);
                Utils.WriteLine($"The reason is '{failedReason.Message}'", indent);
                return result; // If you are not responsible for this failure, you may delegate the result as well.
            });
        }
    }
}
