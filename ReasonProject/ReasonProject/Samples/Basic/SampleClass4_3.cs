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

        public string Title => "12.Get Subtree";

        public string[] Description => new string[]
        {
            "Getting subtrees is easy because the result has their next results.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("Make a sample tree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result result = MakeNestedResult();", indent);

            Result result = MakeNestedResult();

            Utils.WriteLine("", indent);
            Utils.WriteLine("Get the second subtree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("result = result.NextResults.ElementAt(1);", indent);

            result = result.NextResults.ElementAt(1);

            Utils.WriteLine("", indent);
            Utils.WriteLine("Inspect the subtree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "int callCount = 0;",
                "ResultInspector.InspectAll(result,",
                "    (result, depth, index, parent) =>",
                "    {",
                "        Utils.WriteLine($\"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}\", indent);",
                "    },",
                "    depthFirstSearch: true);");

            Utils.WriteLine("", indent);
            int callCount = 0;
            ResultInspector.InspectAll(result,
                (result, depth, index, parent) =>
                {
                    Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent);
                },
                depthFirstSearch: true);

            Utils.WriteLine("", indent);
            Utils.WriteLine("I show the tree for a reference.", indent);
            Utils.WriteLine(indent,
                "[14]",
                " |_______________",
                " |               |",
                "[6]             [13] <= here we have got by 'result = result.NextResults.ElementAt(1);'.",
                " |_______        |_______",
                " |       |       |       |",
                "[2]     [5]     [9]     [12]",
                " |___    |___    |___    |____",
                " |   |   |   |   |   |   |    |",
                "[0] [1] [3] [4] [7] [8] [10] [11]");
        }
    }
}
