// See https://aka.ms/new-console-template for more information
using System.Globalization;

using ReasonProject;
using Reason.Results;
using Reason.Utils;
using Reason.I18n;
using Reason.Reasons;

// This program introduce some usages of the library.

WriteLine("Result Class Samples:");

Result ret;
int sampleNo = 0;
int indent = 2;

{
    // Success case.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = ExecCalcClassDiv(1d, 2d, indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine(result.ToString(), indent); });

    sampleNo++;
}

// Change context
I18nContext.ChangeContext(new CultureInfo("en-US", false));

{
    // See the result of changing context.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = ExecCalcClassDiv(1d, 2d, indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine(result.ToString(), indent); });

    sampleNo++;
}

// Restore the context
I18nContext.ChangeContext(CultureInfo.CurrentCulture);

{
    // Failed case.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = ExecCalcClassDiv(1d, 0d, indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine(result.ToString(), indent); });

    sampleNo++;
}

{
    // Throwing exception case.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    Result ret0;

    Result ret1 = Result.CatchAll(() =>
    {
        double tmp = ExecCalcClassDivException(1d, 0d, indent);
        return Result<double>.MakeSuccessFirst(tmp);
    }, useMessagePropertyAsMessage: true);

    Result ret2 = Result.CatchAll(() =>
    {
        double tmp = ExecCalcClassDivException(1d, 0d, indent);
        return Result<double>.MakeSuccessFirst(tmp);
    }, useMessagePropertyAsMessage: false);

    ret0 = Result.MakeAllSuccess(new FailedReasonWithMessage(""), ret1, ret2);
    ret = ret0;

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    int ret_i = 0;
    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine($"ret{ret_i++}:{result}", indent); });

    sampleNo++;
}

{
    // Depth-first search.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = MakeNestedResult(indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    int callCount = 0;
    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine($"{callCount++}:{result}, depth={depth}", indent); });

    sampleNo++;
}

{
    // Breadth-first search.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = MakeNestedResult(indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    int callCount = 0;
    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine($"{callCount++}:{result}, depth={depth}", indent); }, depthFirstSearch: false);

    sampleNo++;
}

{
    // Depth-first search.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = MakeNestedResult(indent);
    // Inspect a subtree.
    ret = ret.NextResults.ElementAt(1);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    int callCount = 0;
    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine($"{callCount++}:{result}, depth={depth}", indent); });

    sampleNo++;
}

{
    // Depth-first search.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = MakeNestedResult(indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    // Pruning a subtree.
    int callCount = 0;
    ResultInspector.InspectWhere(ret, (result, depth, index, parent) => { WriteLine($"{callCount++}:{result}, depth={depth}", indent); },
        (result, depth, index, parent) => true,
        (result, depth, index, parent) => (depth == 1 && index == 1));

    sampleNo++;
}

{
    // Depth-first search.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = MakeNestedResult2(indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    int callCount = 0;
    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine($"{callCount++}:{result}, depth={depth}", indent); });

    sampleNo++;
}

{
    // Breadth-first search.
    WriteLine("");
    WriteLine($"Sample {sampleNo}:");

    ret = MakeNestedResult2(indent);

    if (ret.IsFailed()) WriteLine("Failed!", indent);
    else WriteLine("Succeeded!", indent);

    WriteLine("");
    WriteLine("Show All Results:", indent);

    int callCount = 0;
    ResultInspector.InspectAll(ret, (result, depth, index, parent) => { WriteLine($"{callCount++}:{result}, depth={depth}", indent); }, depthFirstSearch: false);

    sampleNo++;
}

/// <summary>
/// Write line(s) with some indents. 
/// </summary>
void WriteLine(string line, int indent = 0)
{
    Console.WriteLine(line.PadLeft(line.Length + indent).Replace(Environment.NewLine, Environment.NewLine.PadRight(Environment.NewLine.Length + indent)));
}

Result ExecCalcClassDiv(double numerator, double denominator, int indent)
{
    SampleCalcClass calc = new SampleCalcClass();
    
    Result<double> result = calc.Divide(numerator, denominator);

    Result ret = result.Whether((v) =>
    {
        WriteLine($"Result is {v}", indent);
        return Result.MakeSuccessFirst();
    },
    (r) =>
    {
        WriteLine($"Failed reason is {r.Message}", indent);
        return result;
    });

    return ret;
}

double ExecCalcClassDivException(double numerator, double denominator, int indent)
{
    SampleCalcClass calc = new SampleCalcClass();

    Result<double> result = calc.Divide(numerator, denominator);

    return result.Get();
}

Result MakeNestedResult(int indent)
{
    /*
     * [14]
     *  |_______________
     *  |               |
     * [6]             [13]
     *  |_______        |_______
     *  |       |       |       |
     * [2]     [5]     [9]     [12]
     *  |___    |___    |___    |____
     *  |   |   |   |   |   |   |    |
     * [0] [1] [3] [4] [7] [8] [10] [11]
     *  
     */

    Result result0 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={0}"));
    Result result1 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={1}"));
    Result result2 = Result.MakeAllSuccess(new List<Result> { result0, result1}, new FailedReasonWithMessage($"Id={2}"));

    Result result3 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={3}"));
    Result result4 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={4}"));
    Result result5 = Result.MakeAllSuccess(new List<Result> { result3, result4 }, new FailedReasonWithMessage($"Id={5}"));

    Result result6 = Result.MakeAllSuccess(new List<Result> { result2, result5 }, new FailedReasonWithMessage($"Id={6}"));

    Result result7 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={7}"));
    Result result8 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={8}"));
    Result result9 = Result.MakeAllSuccess(new List<Result> { result7, result8 }, new FailedReasonWithMessage($"Id={9}"));

    Result result10 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={10}"));
    Result result11 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={11}"));
    Result result12 = Result.MakeAllSuccess(new List<Result> { result10, result11 }, new FailedReasonWithMessage($"Id={12}"));

    Result result13 = Result.MakeAllSuccess(new List<Result> { result9, result12 }, new FailedReasonWithMessage($"Id={13}"));

    Result result14 = Result.MakeAllSuccess(new List<Result> { result6, result13 }, new FailedReasonWithMessage($"Id={14}"));

    return result14;
}

Result MakeNestedResult2(int indent)
{
    /*
     * [4]
     *  |___ ___
     *  |   |   |
     *  |  [3] [2]
     *  |_______|
     *  |       |
     * [0]     [1]
     *  
     */

    Result result0 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={0}"));
    Result result1 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={1}"));
    Result result2 = Result.MakeAllSuccess(new List<Result> { result0, result1 }, new FailedReasonWithMessage($"Id={2}"));

    Result result3 = Result.MakeFailedFirst(new FailedReasonWithMessage($"Id={3}"));
    Result result4 = Result.MakeAllSuccess(new List<Result> { result0, result3, result2 }, new FailedReasonWithMessage($"Id={4}"));

    return result4;
}
