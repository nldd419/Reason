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
        public override string CategoryName => "Calculator";

        public string Title => "Division Success";

        public void Exec(int indent)
        {
            Result ret;

            Utils.WriteLine("Calculating ( 1 / 2 )...", indent);

            ret = Div(1d, 2d, indent);

            if (ret.IsFailed()) Utils.WriteLine("Operation Failed!", indent);
            else Utils.WriteLine("Operation Succeeded!", indent);
        }
    }
}
