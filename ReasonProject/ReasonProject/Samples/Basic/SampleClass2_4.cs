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
    internal class SampleClass2_4 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Exception";

        public string Title => "6.Automatic Exception Catch";

        public string[] Description => new string[]
        {
            "The other method for sending an error message to their caller is throwing an exception.",
            "Aside from performance, it has some issues in my opinion.",
            "First, you must enclose your statement in brackets of try-catch. It easily happens you to forget",
            "to do that, and a compiler doesn't raise any compile error.",
            "Another pattern is that writing try-catch at the root of your program and handling all exceptions there.",
            "This is good most of the time. However, what if your program is constructing a viewmodel on a GUI application",
            "and throwing an exception during initialization time?",
            "  public void OnInitialize() {",
            "    // Get some data from anywhere. Suppose this method causes an exception and forgot to handle.",
            "    this.Data = GetSomething();",
            "  }",
            "Bacause there is a root logic to handle all exceptions, you may be willing to ignore dealing with this.",
            "After the exception, the state of the viewmodel is unknown. If your root logic didn't properly finish",
            "the page, your end-users may push a 'Submit' button and send the undefined state to your database.",
            "",
            "My approach isn't a solution for forgetting something, but at least you're sure you don't throw any exception.",
            "You can catch an exception automatically while executing 'Result.Whether' method.",
            "That can be done by either specifying method parameters or setting global configurations.",
        };

        public void Exec(int indent)
        {
            // Save current settings.
            bool configAuto = Result.Configurations.AutomaticCatch;
            bool configUseMessage = Result.Configurations.UseMessagePropertyAsMessage;

            Utils.WriteLine("Set global configurations.", indent);
            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result.Configurations.AutomaticCatch = true;", indent);
            Utils.WriteLineForCode("Result.Configurations.UseMessagePropertyAsMessage = true;", indent);

            Result.Configurations.AutomaticCatch = true;
            Result.Configurations.UseMessagePropertyAsMessage = true;

            Utils.WriteLine("", indent);
            Utils.WriteLine("Create success result with a value of 0.", indent);
            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result<decimal> result = Result<decimal>.MakeSuccessFirst(0);", indent);

            Result<decimal> result = Result<decimal>.MakeSuccessFirst(0);

            Utils.WriteLine("", indent);
            Utils.WriteLine("Then call 'Result.Whether' method.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "result = result.Whether(",
                "(val) =>",
                "{",
                "    Utils.WriteLine($\"Calculating ( 1 / {val} )...\", indent);",
                "",
                "    // This causes throwing an exception, but it'ok because",
                "    // 'Result.Whether' method automatically catches the exception",
                "    // and converts it into 'ExceptionResult', then returns it.",
                "    decimal tmp = 1 / val;",
                "",
                "    return result.MakeSuccess(tmp);",
                "},",
                "(r) =>",
                "{",
                "    Utils.WriteLine($\"Failed reason is '\"{r.Message}\"'\", indent);",
                "    return result;",
                "});");

            result = result.Whether(
            (val) =>
            {
                Utils.WriteLine("", indent);
                Utils.WriteLine($"Calculating ( 1 / {val} )...", indent);

                // This causes throwing an exception, but it'ok because
                // 'Result.Whether' method automatically catches the exception
                // and converts it into 'ExceptionResult', then returns it.
                decimal tmp = 1 / val;

                return result.MakeSuccess(tmp);
            },
            (r) =>
            {
                Utils.WriteLine("", indent);
                Utils.WriteLine($"Failed reason is '\"{r.Message}\"'", indent);
                return result;
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

            // Restore the settings.
            Result.Configurations.AutomaticCatch = configAuto;
            Result.Configurations.UseMessagePropertyAsMessage = configUseMessage;
        }
    }
}
