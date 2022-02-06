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

        public string Title => "Division Throwing Exception And Show Stacktrace";

        public void Exec(int indent)
        {
            Result ret;

            Utils.WriteLine("You can show the stack trace.", indent);
            Utils.WriteLine("Calculating ( 1 / 2 )...", indent);

            ret = Result.CatchAll(() =>
            {
                double tmp = DivThrowingException(1d, 2d);
                return Result<double>.MakeSuccessFirst(tmp);
            }, useMessagePropertyAsMessage: false);

            Utils.WriteLine(ret.GetReason().Message, indent);

            if (ret.IsFailed()) Utils.WriteLine("Operation Failed!", indent);
            else Utils.WriteLine("Operation Succeeded!", indent);
        }
    }
}
