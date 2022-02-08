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

        public string Title => "Inspect Share Child Graph";

        public int Order => 13;

        public string[] Description => new string[]
        {
            "Think of the situation that a result is needed by an other result for their success condition",
            "and is also needed by another result. This creates a graph which is sharing one child.",
            "This library can properly handle the case, but it's a little bit tricky when using depth-first search.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("This 'MakeNestedResultShare' method creates a sample result tree which is sharing a child.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "protected Result MakeNestedResultShare()",
                "{");
            Utils.WriteLineForCode(indent + 2,
                "/*",
                "* [4]",
                "*  |___ ___",
                "*  |   |   |",
                "*  |  [3] [2]",
                "*  |       |___",
                "*  |_______|   |",
                "*  |           |",
                "* [0]         [1]",
                "*",
                "*/",
                "",
                "Result result0 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={0}\"));",
                "Result result1 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={1}\"));",
                "Result result2 = Result.MakeAllSuccess(new List<Result> { result0, result1 }, new FailedReasonWithMessage($\"Id={2}\"));",
                "",
                "Result result3 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={3}\"));",
                "Result result4 = Result.MakeAllSuccess(new List<Result> { result0, result3, result2 }, new FailedReasonWithMessage($\"Id={4}\"));",
                "",
                "return result4;");
            Utils.WriteLineForCode("}", indent);


            Utils.WriteLine("", indent);
            Utils.WriteLine("Now, make the tree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result result = MakeNestedResultShare();", indent);

            Result result = MakeNestedResultShare();


            Utils.WriteLine("", indent);
            Utils.WriteLine("You can inspect the tree by depth-first search, but the shared child is inspected twice.", indent);

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
                "[4]",
                " |___ ___",
                " |   |   |",
                " |  [3] [2]",
                " |       |___",
                " |_______|   |",
                " |           |",
                "[0]         [1]",
                "   <= this result has two paths from the parents, so this is called twice.");
        }
    }
}
