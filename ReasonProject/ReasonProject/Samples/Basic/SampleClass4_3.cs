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
    internal class SampleClass4_3 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Inspection";

        public string Title => "Get Subtree";

        public void Exec(int indent)
        {
            Result ret;

            Utils.WriteLine("Creating an ordinary tree...", indent);

            ret = MakeNestedResult();

            Utils.WriteLine("Get the second subtree.", indent);

            ret = ret.NextResults.ElementAt(1);

            Utils.WriteLine("Inspect the subtree by depth-first search.", indent);

            indent += 2;

            int callCount = 0;
            ResultInspector.InspectAll(ret, (result, depth, index, parent) => { Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent); },
                depthFirstSearch: true);
        }
    }
}
