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

        public string Title => "10.Inspect Result Tree By Depth-First Search";

        public string[] Description => new string[]
        {
            "The most difficult issue which has me tearing my hair is errors raised in deep.",
            "It abandon the reasons of errors occured in descendants' calls, and this is usually inevitable",
            "when you have chosen to not use try-catch.",
            "This library has the ability to let them hold all the results occured in descendants.",
        };

        public void Exec(int indent)
        {
            Utils.WriteLine("This 'MakeNestedResult' method creates a sample result tree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode(indent,
                "protected Result MakeNestedResult()",
                "{");
            Utils.WriteLineForCode(indent+2,
                "/*",
                " * [14]",
                " *  |_______________",
                " *  |               |",
                " * [6]             [13]",
                " *  |_______        |_______",
                " *  |       |       |       |",
                " * [2]     [5]     [9]     [12]",
                " *  |___    |___    |___    |____",
                " *  |   |   |   |   |   |   |    |",
                " * [0] [1] [3] [4] [7] [8] [10] [11]",
                " *",
                " */",
                "",
                "Result result0 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={0}\"));",
                "Result result1 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={1}\"));",
                "Result result2 = Result.MakeAllSuccess(new List<Result> { result0, result1 }, new FailedReasonWithMessage($\"Id={2}\"));",
                "",
                "Result result3 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={3}\"));",
                "Result result4 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={4}\"));",
                "Result result5 = Result.MakeAllSuccess(new List<Result> { result3, result4 }, new FailedReasonWithMessage($\"Id={5}\"));",
                "",
                "Result result6 = Result.MakeAllSuccess(new List<Result> { result2, result5 }, new FailedReasonWithMessage($\"Id={6}\"));",
                "",
                "Result result7 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={7}\"));",
                "Result result8 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={8}\"));",
                "Result result9 = Result.MakeAllSuccess(new List<Result> { result7, result8 }, new FailedReasonWithMessage($\"Id={9}\"));",
                "",
                "Result result10 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={10}\"));",
                "Result result11 = Result.MakeFailedFirst(new FailedReasonWithMessage($\"Id={11}\"));",
                "Result result12 = Result.MakeAllSuccess(new List<Result> { result10, result11 }, new FailedReasonWithMessage($\"Id={12}\"));",
                "",
                "Result result13 = Result.MakeAllSuccess(new List<Result> { result9, result12 }, new FailedReasonWithMessage($\"Id={13}\"));",
                "",
                "Result result14 = Result.MakeAllSuccess(new List<Result> { result6, result13 }, new FailedReasonWithMessage($\"Id={14}\"));",
                "",
                "return result14;");
            Utils.WriteLineForCode("}", indent);


            Utils.WriteLine("", indent);
            Utils.WriteLine("Now, make the tree.", indent);

            Utils.WriteLine("", indent);
            Utils.WriteLineForCode("Result result = MakeNestedResult();", indent);

            Result result = MakeNestedResult();


            Utils.WriteLine("", indent);
            Utils.WriteLine("You can inspect the tree by depth-first search with 'ResultInspector.InspectAll'.", indent);

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
