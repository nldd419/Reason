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

        public void Exec(int indent)
        {
            Result ret;

            CultureInfo defaultCulture = I18nContext.Current.Culture;

            Utils.WriteLine($"The current culture is {defaultCulture.DisplayName}", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLine("Calculating ( 1 / 2 )...", indent);

            ret = Result.CatchAll(() =>
            {
                double tmp = DivThrowingException(1d, 2d);
                return Result<double>.MakeSuccessFirst(tmp);
            }, useMessagePropertyAsMessage: true);

            Utils.WriteLine(ret.GetReason().Message, indent);

            if (ret.IsFailed()) Utils.WriteLine("Operation Failed!", indent);
            else Utils.WriteLine("Operation Succeeded!", indent);

            Utils.WriteLine("", indent);
            Utils.Write("Please enter a culture name you want to try (e.g. us-EN, ja-JP): ", indent);
            string? cultureName = Console.ReadLine() ?? "";

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            try
            {
                cultureInfo = new CultureInfo(cultureName, false);
            }
            catch(Exception)
            {
                Utils.WriteLine($"The culture wasn't found.", indent);
            }

            Utils.WriteLine($"{cultureInfo.DisplayName} is selected.", indent);

            // Change the context
            I18nContext.ChangeContext(cultureInfo);

            Utils.WriteLine("", indent);
            Utils.WriteLine("Calculating ( 1 / 2 )...", indent);

            ret = Result.CatchAll(() =>
            {
                double tmp = DivThrowingException(1d, 2d);
                return Result<double>.MakeSuccessFirst(tmp);
            }, useMessagePropertyAsMessage: true);

            Utils.WriteLine(ret.GetReason().Message, indent);

            if (ret.IsFailed()) Utils.WriteLine("Operation Failed!", indent);
            else Utils.WriteLine("Operation Succeeded!", indent);

            // Restore the context
            I18nContext.ChangeContext(defaultCulture);
        }
    }
}
