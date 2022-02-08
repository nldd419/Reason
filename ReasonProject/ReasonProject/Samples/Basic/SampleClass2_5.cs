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
    internal class SampleClass2_5 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Exception";

        public string Title => "7.Safety 'Get'";

        public string[] Description => new string[]
        {
            "This library has a mechanism that forces programmers to check the result before getting values.",
            "Which is that throwing an exception if you try to get a value before calling a check method 'IsFailed'",
            "even if the result has a value at the moment. Therefore you can notice the potential issue that you",
            "forgot checking the result at a testing time.",
            "On another note, 'Result.Whether' method does check it implicitly, so you don't have to care",
            "the checking work if you set the global configuration 'AutomaticCatch' to true.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("This 'Div' returns a 'Result' object which has a value.", indent);
            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result<decimal> div = Div(1, 2, indent);", indent);

            Utils.WriteLine("", indent);
            Result<decimal> div = Div(1, 2, indent);

            Utils.WriteLine("", indent);
            Utils.WriteLine("To call 'Get' before calling 'IsFailed' causes an exception.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "Result result = Result.CatchAll(() =>",
                "{",
                "    decimal val = div.Get();",
                "    return Result.MakeSuccessFirst();",
                "}, useMessagePropertyAsMessage: true);");

            Result result = Result.CatchAll(() =>
            {
                decimal val = div.Get();
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

            Utils.WriteLine("", indent);
            Utils.WriteLine("Once you check the result, then you can get the value without an exception.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "if (!div.IsFailed())",
                "{",
                "    Utils.WriteLine($\"The value is '{div.Get()}'\", indent);",
                "}");

            Utils.WriteLine("", indent);
            if (!div.IsFailed())
            {
                Utils.WriteLine($"The value is {div.Get()}", indent);
            }
        }
    }
}
