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

        public string Title => "13.Prune Subtree";

        public string[] Description => new string[]
        {
            "The result trees are immutable. Thus you can't remove a subtree from them.",
            "However, you can tell the 'ResultInspector' that which subtree you don't want to inspect.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("Make a sample tree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result result = MakeNestedResult();", indent);

            Result result = MakeNestedResult();

            Utils.WriteLine("", indent);
            Utils.WriteLine("Inspect the tree with specifying so that the second subtree is pruned while the inspection.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "int callCount = 0;",
                "ResultInspector.InspectWhere(result,",
                "    (result, depth, index, parent) =>",
                "    {",
                "        Utils.WriteLine($\"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}\", indent);",
                "    },",
                "    (result, depth, index, parent) => true,",
                "    (result, depth, index, parent) => (depth == 1 && index == 1), // This line is the one that specify the condition of pruning.",
                "    depthFirstSearch: true);");

            Utils.WriteLine("", indent);
            int callCount = 0;
            ResultInspector.InspectWhere(result,
                (result, depth, index, parent) =>
                {
                    Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent);
                },
                (result, depth, index, parent) => true,
                (result, depth, index, parent) => (depth == 1 && index == 1), // This line is the one that specify the condition of pruning.
                depthFirstSearch: true);

            Utils.WriteLine("", indent);
            Utils.WriteLine("I show the tree for a reference.", indent);
            Utils.WriteLine(indent,
                "[14]",
                " |_______________",
                " |               |",
                "[6]             [13] <= all the node under here has been pruned.",
                " |_______        |_______",
                " |       |       |       |",
                "[2]     [5]     [9]     [12]",
                " |___    |___    |___    |____",
                " |   |   |   |   |   |   |    |",
                "[0] [1] [3] [4] [7] [8] [10] [11]");
        }
    }
}
