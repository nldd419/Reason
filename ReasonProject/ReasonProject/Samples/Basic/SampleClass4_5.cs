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
    internal class SampleClass4_5 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Inspection";

        public string Title => "Share Child Graph";

        public void Exec(int indent)
        {
            Result ret;

            Utils.WriteLine("Creating a tree in which two parents sharing a child...", indent);

            ret = MakeNestedResultShare();

            Utils.WriteLine("Inspect the tree by depth-first search. The shared child is inspected twice.", indent);

            indent += 2;

            int callCount = 0;
            ResultInspector.InspectAll(ret, (result, depth, index, parent) => { Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent); },
                depthFirstSearch: true);
        }
    }
}
