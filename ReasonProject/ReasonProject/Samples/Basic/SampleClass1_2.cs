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
    internal class SampleClass1_2 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Calculator";

        public string Title => "Division Failed";

        public void Exec(int indent)
        {
            Result ret;

            Utils.WriteLine("Calculating ( 1 / 0 )...", indent);

            ret = Div(1d, 0d, indent);

            if (ret.IsFailed()) Utils.WriteLine("Operation Failed!", indent);
            else Utils.WriteLine("Operation Succeeded!", indent);
        }
    }
}
