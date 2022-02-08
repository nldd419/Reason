using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using ReasonProject.Samples;
using Reason.Results;
using Reason.Utils;
using Reason.I18n;

namespace ReasonProject.Samples.Basic
{
    internal class SampleClass3 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Culture";

        public string Title => "Change Culture";

        public int Order => 8;

        public string[] Description => new string[]
        {
            "Changing the language of the library built-in messages is done by setting your culture.",
        };

        public void Exec(int indent)
        {
            // Save a current culture.
            CultureInfo defaultCulture = I18nContext.Current.Culture;


            Utils.WriteLine($"The current culture is {defaultCulture.DisplayName}.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLine("First, we do this with the current culture.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "Result<decimal> div = Div(1, 2, indent);",
                "Result result = Result.CatchAll(() =>",
                "{",
                "    // This causes throwing an exception that shows a built-in message of 'ReasonCustomException'.",
                "    decimal tmp = div.Get();",
                "    return Result.MakeSuccessFirst(tmp);",
                "}, useMessagePropertyAsMessage: true);");

            Utils.WriteLine("", indent);
            Result<decimal> div = Div(1, 2, indent);
            Result result = Result.CatchAll(() =>
            {
                // This causes throwing an exception that shows a built-in message of 'ReasonCustomException'.
                decimal tmp = div.Get();
                return Result.MakeSuccessFirst(tmp);
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
            Utils.WriteLine("Now, let's change the culture.", indent);

            Utils.WriteLine("", indent);
            Utils.Write("Please enter a culture name you want to try (e.g. us-EN, ja-JP): ", indent);
            string? cultureName = Console.ReadLine() ?? "";

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "CultureInfo cultureInfo = CultureInfo.CurrentCulture;",
                "try",
                "{",
                "    cultureInfo = new CultureInfo(cultureName, false);",
                "}",
                "catch(Exception)",
                "{",
                "    Utils.WriteLine($\"The culture wasn't found.\", indent);",
                "}",
                "",
                "Utils.WriteLine($\"{cultureInfo.DisplayName} has been selected.\", indent);");

            Utils.WriteLine("", indent);
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            try
            {
                cultureInfo = new CultureInfo(cultureName, false);
            }
            catch(Exception)
            {
                Utils.WriteLine($"The culture wasn't found.", indent);
            }

            Utils.WriteLine($"{cultureInfo.DisplayName} has been selected.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLine("Pass the culture to a static method 'I18nContext.ChangeContext'.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "// Change the context",
                "I18nContext.ChangeContext(cultureInfo);");

            // Change the context
            I18nContext.ChangeContext(cultureInfo);

            Utils.WriteLine("", indent);
            Utils.WriteLine("Then do the same thing again.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "div = Div(1, 2, indent);",
                "result = Result.CatchAll(() =>",
                "{",
                "    // This causes throwing an exception that shows a built-in message of 'ReasonCustomException'.",
                "    decimal tmp = div.Get();",
                "    return Result.MakeSuccessFirst(tmp);",
                "}, useMessagePropertyAsMessage: true);");

            Utils.WriteLine("", indent);
            div = Div(1, 2, indent);
            result = Result.CatchAll(() =>
            {
                // This causes throwing an exception that shows a built-in message of 'ReasonCustomException'.
                decimal tmp = div.Get();
                return Result.MakeSuccessFirst(tmp);
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


            // Restore the context
            I18nContext.ChangeContext(defaultCulture);
        }
    }
}
