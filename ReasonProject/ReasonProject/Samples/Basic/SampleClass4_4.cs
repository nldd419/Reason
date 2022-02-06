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
    internal class SampleClass4_4 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Inspection";

        public string Title => "Prune Subtree";

        public void Exec(int indent)
        {
            Result ret;

            Utils.WriteLine("Creating an ordinary tree...", indent);

            ret = MakeNestedResult();

            Utils.WriteLine("Inspect the tree by depth-first search. The second subtree is pruned while the inspection.", indent);

            indent += 2;

            int callCount = 0;
            ResultInspector.InspectWhere(ret, (result, depth, index, parent) => { Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent); },
                (result, depth, index, parent) => true,
                (result, depth, index, parent) => (depth == 1 && index == 1),
                depthFirstSearch: true);
        }
    }
}
