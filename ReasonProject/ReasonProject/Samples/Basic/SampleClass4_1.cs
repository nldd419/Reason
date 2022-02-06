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
    internal class SampleClass4_1 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Inspection";

        public string Title => "Depth-First Search";

        public void Exec(int indent)
        {
            Result ret;

            Utils.WriteLine("Creating an ordinary tree...", indent);

            ret = MakeNestedResult();

            Utils.WriteLine("Inspect by depth-first search.", indent);

            indent += 2;

            int callCount = 0;
            ResultInspector.InspectAll(ret, (result, depth, index, parent) => { Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent); },
                depthFirstSearch: true);
        }
    }
}
