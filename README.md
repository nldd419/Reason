# Reason
C# library for handling errors

## I need the REASON why you failed it!
This library is an experiment for neat error handlings without exceptions.

## What is this libary and how to use?
This project has samples to execute, so I just paste some of the outputs.
```
Sample: 1.Get Return Value, Basic/Basic
  /*
   * Description:
   * Implementing logic such that all functions always returing a result value and a message makes
   * me feel better.
   * However, if you're sure that the method never throws any exception and is always success,
   * you can also simply return a primitive value.
   */
  
  This 'Div' calculates {first arg} / {second arg} and then return 'Result' object.
  
    Result<decimal> result = Div(1, 2, indent);
  
  Calculating ( 1 / 2 )...
  
  After that, you can retrieve the value using 'Result.Whether' method which takes some callback arguments.
  One is called when success, another is called when failure.
  
    result.Whether(
    (val) =>
    {
        Utils.Write($"The result is {val}.");
        return result; // You may delegate the result.
    },
    (failedReason) =>
    {
        Utils.WriteLine("Operation Failed!", indent);
        Utils.WriteLine($"The reason is '{failedReason.Message}'", indent);
        return result; // If you are not responsible for this failure, you may delegate the result as well.
    }); 
  
  The result is 0.5.
```

```
Sample: 6.Automatic Exception Catch, Basic/Exception
  /*
   * Description:
   * The other method for sending an error message to their caller is throwing an exception.
   * Aside from performance, it has some issues in my opinion.
   * First, you must enclose your statement in brackets of try-catch. It easily happens you to forget
   * to do that, and a compiler doesn't raise any compile error.
   * Another pattern is that writing try-catch at the root of your program and handling all exceptions there.
   * This is good most of the time. However, what if your program is constructing a viewmodel on a GUI application
   * and throwing an exception during initialization time?
   *   public void OnInitialize() {
   *     // Get some data from anywhere. Suppose this method causes an exception and forgot to handle.
   *     this.Data = GetSomething();
   *   }
   * Bacause there is a root logic to handle all exceptions, you may be willing to ignore dealing with this.
   * After the exception, the state of the viewmodel is unknown. If your root logic didn't properly finish
   * the page, your end-users may push a 'Submit' button and send the undefined state to your database.
   * 
   * My approach isn't a solution for forgetting something, but at least you're sure you don't throw any exception.
   * You can catch an exception automatically while executing 'Result.Whether' method.
   * That can be done by either specifying method parameters or setting global configurations.
   */
  
  Set global configurations.
  
    Result.Configurations.AutomaticCatch = true;
    Result.Configurations.UseMessagePropertyAsMessage = true;
  
  Create success result with a value of 0.
  
    Result<decimal> result = Result<decimal>.MakeSuccessFirst(0);
  
  Then call 'Result.Whether' method.
  
    result = result.Whether(
    (val) =>
    {
        Utils.WriteLine($"Calculating ( 1 / {val} )...", indent);
    
        // This causes throwing an exception, but it'ok because
        // 'Result.Whether' method automatically catches the exception
        // and converts it into 'ExceptionResult', then returns it.
        decimal tmp = 1 / val;
    
        return result.MakeSuccess(tmp);
    },
    (r) =>
    {
        Utils.WriteLine($"Failed reason is '"{r.Message}"'", indent);
        return result;
    });
  
  Calculating ( 1 / 0 )...
  
  See the result.
  
    if (result.IsFailed())
    {
        Utils.WriteLine("Operation Failed!", indent);
        Utils.WriteLine($"The reason is '{result.GetReason().Message}'", indent);
    }
  
  Operation Failed!
  The reason is 'Attempted to divide by zero.'
```

```
Sample: 9.Inspect Result Tree By Depth-First Search, Basic/Inspection
  /*
   * Description:
   * The most difficult issue which has me tearing my hair is errors raised in deep.
   * It abandons the reasons of errors occured in descendants' calls, and this is usually inevitable
   * when you have chosen to not use try-catch.
   * This library has the ability to let them hold all the results occured in descendants.
   */
  
  This 'MakeNestedResult' method creates a sample result tree.
  
    protected Result MakeNestedResult()
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
      Result result2 = Result.MakeAllSuccess(new List<Result> { result0, result1 }, new FailedReasonWithMessage($"Id={2}"));
      
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
  
  Now, make the tree.
  
    Result result = MakeNestedResult();
  
  You can inspect the tree by depth-first search with 'ResultInspector.InspectAll'.
  
    int callCount = 0;
    ResultInspector.InspectAll(result,
        (result, depth, index, parent) =>
        {
            Utils.WriteLine($"{callCount++}:{result}, depth={depth}, index={index}, parent={parent}", indent);
        },
        depthFirstSearch: true);
  
  0:Id=14, depth=0, index=-1, parent=
  1:Id=6, depth=1, index=0, parent=Id=14
  2:Id=2, depth=2, index=0, parent=Id=6
  3:Id=0, depth=3, index=0, parent=Id=2
  4:Id=1, depth=3, index=1, parent=Id=2
  5:Id=5, depth=2, index=1, parent=Id=6
  6:Id=3, depth=3, index=0, parent=Id=5
  7:Id=4, depth=3, index=1, parent=Id=5
  8:Id=13, depth=1, index=1, parent=Id=14
  9:Id=9, depth=2, index=0, parent=Id=13
  10:Id=7, depth=3, index=0, parent=Id=9
  11:Id=8, depth=3, index=1, parent=Id=9
  12:Id=12, depth=2, index=1, parent=Id=13
  13:Id=10, depth=3, index=0, parent=Id=12
  14:Id=11, depth=3, index=1, parent=Id=12
  
  I show the tree for a reference.
  [14]
   |_______________
   |               |
  [6]             [13]
   |_______        |_______
   |       |       |       |
  [2]     [5]     [9]     [12]
   |___    |___    |___    |____
   |   |   |   |   |   |   |    |
  [0] [1] [3] [4] [7] [8] [10] [11]
```

You can see full samples [here](docs/output_samples.txt) or by running the sample project.

## Contributions

### How to build
First, you should run `init-submodules.bat` at the root folder to initialize submodules, or you can do the init process manually by `git submodule init` command.  
Then, you can open the solution and run the project. 

### How to add samples
Inherit `ISample`, name the class 'SampleXXX', and put it under 'Samples/Basic' folder. If the category of the sample is not 'Basic', you should create a new folder under the 'Samples' folder and put the sample in it.
```
internal interface ISample
{
    string Category { get; }
    string Title { get; }
    string[] Description { get; }
    void Exec(int indent);
    int Order { get; }
}
```

### Internationalization
Inherit I18nContext, name the class 'ContextXXX' (XXX must be 'Three letter of ISO language name' ([link to MS Docs](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.cultureinfo.threeletterisolanguagename?view=net-6.0#system-globalization-cultureinfo-threeletterisolanguagename))), and throw it in 'I18n/Context' folder. The class should be return the instances which has messages for the language (or just leave it so as to use default ones).
```
internal class ContextJPN : I18nContext 
{
    public ContextJPN(CultureInfo culture) : base(culture)
    {
    }

    protected override IFailedMessages CreateFailedMessages() => new FailedMessagesJPN();
    protected override IExceptionMessages CreateExceptionMessages() => new ExceptionMessagesJPN();
}
```
You can create 'ExceptionMessages' and 'FailedMessages' with a similar naming rule of 'ContextXXX'. It needs to inherit english version of message class.
```
internal class FailedMessagesJPN : FailedMessagesENG
{
    public override string NotFailed => "失敗していません。";
    public override string Unknown => "不明な理由です。";
    public override string ValueIsNull => "値がnullです。";
}
```

### Git settings
I prefer the settings below.
- core.autocrlf=true
- core.safecrlf=true

### Rquirements
- .NET 6 (Visual Studio 2022)

### Naming Conventions
- Class => PascalCase
- Public fields or methods => PascalCase
- Private fields or parameters => camelCase  
  If the name of parameter conflicts with the one of private field, you can use 'this.xxx' to avoid the error.
- Constants => CONSTANT_CASE

I'm new to GitHub, so I may take some time to merge your Pull request.

## TODO
- Add tests (IMPORTANT!)
- Add more samples so that I can find issues of this approach.
- Add CI

## Others
Since currently the library version is 0.0.1, there is no built binary and you have to compile it by yourself.

Correcting my broken or unnatural english is all welcome.