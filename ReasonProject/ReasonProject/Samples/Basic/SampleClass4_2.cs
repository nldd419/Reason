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
    internal class SampleClass4_2 : SampleCalcBase, ISample
    {
        public override string CategoryName => "Inspection";

        public string Title => "Inspect Result Tree By Breadth-First Search";

        public int Order => 10;

        public string[] Description => new string[]
        {
            "Similar to '10.Inspect Result Tree By Depth-First Search', you can also search a tree",
            "by breadth-first search.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("Make a sample tree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result result = MakeNestedResult();", indent);

            Result result = MakeNestedResult();


            Utils.WriteLine("", indent);
            Utils.WriteLine("You can inspect the tree by beadth-first search by passing false for the 'depthFirstSearch' parameter.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "int callCount = 0;",
                "ResultInspector.InspectAll(result,",
                "    (result, depth, index, parent) =>",
                "    {",
                "        Utils.WriteLine($\"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}\", indent);",
                "    },",
                "    depthFirstSearch: false);");

            Utils.WriteLine("", indent);
            int callCount = 0;
            ResultInspector.InspectAll(result,
                (result, depth, index, parent) =>
                {
                    Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent);
                },
                depthFirstSearch: false);

            Utils.WriteLine("", indent);
            Utils.WriteLine("I show the tree for a reference.", indent);
            Utils.WriteLine(indent,
                "[14]",
                " |_______________",
                " |               |",
                "[6]             [13]",
                " |_______        |_______",
                " |       |       |       |",
                "[2]     [5]     [9]     [12]",
                " |___    |___    |___    |____",
                " |   |   |   |   |   |   |    |",
                "[0] [1] [3] [4] [7] [8] [10] [11]");
        }
    }
}
