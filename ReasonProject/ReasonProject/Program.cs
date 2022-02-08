// See https://aka.ms/new-console-template for more information
using System.Reflection;

using ReasonProject;
using ReasonProject.Samples;
using Reason.Results;
using Reason.Utils;
using Reason.I18n;
using Reason.Reasons;

// This program introduce some usages of the library.

string[] cmdArgs = Environment.GetCommandLineArgs();
if(cmdArgs.Length > 2)
{
    if(cmdArgs[1] != "-o")
    {
        Console.WriteLine("You should pass '-o' for the second argument.");
        Environment.Exit(1);
    }

    if (string.IsNullOrWhiteSpace(cmdArgs[2]))
    {
        Console.WriteLine("You can't omit the output file path.");
        Environment.Exit(1);
    }

    bool force = false;
    if (cmdArgs.Length > 3 && cmdArgs[3] == "-f")
    {
        force = true;
    }

    try
    {
        Utils.TextWriter = new StreamWriter(new FileStream(cmdArgs[2], force ? FileMode.Append : FileMode.CreateNew, FileAccess.Write, FileShare.Read));
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex);
        Environment.Exit(1);
    }
}

Utils.WriteLine("Hello. This is 'Result' class samples.");
Utils.WriteLine("");

ShowHelp();

IEnumerable<Tuple<int, ISample>> samples = CreateSampleList().OrderBy(x => x.Category).ThenBy(x => x.Title).Select((x,i) => new Tuple<int, ISample>(i,x)).ToList();

while (EvalCommand(RequestInput(), samples)) ;

if (Utils.TextWriter != null) Utils.TextWriter.Close();

string RequestInput()
{
    Utils.Write("$ ");
    return Console.ReadLine() ?? "";
}

void ShowHelp()
{
    int commandIndent = 2;
    int descIndent = 4;

    Utils.WriteLine("Command List");

    Utils.WriteLine("h", commandIndent);
    Utils.WriteLine("help", commandIndent);
    Utils.WriteLine("Show this help.", descIndent);

    Utils.WriteLine("");
    Utils.WriteLine("ls [<Category>]", commandIndent);
    Utils.WriteLine("List samples. If <Category> is specified, only the samples belong the category is shown.", descIndent);

    Utils.WriteLine("");
    Utils.WriteLine("ls categories", commandIndent);
    Utils.WriteLine("ls c", commandIndent);
    Utils.WriteLine("List sample categories.", descIndent);

    Utils.WriteLine("");
    Utils.WriteLine("do all", commandIndent);
    Utils.WriteLine("do [<Category>]", commandIndent);
    Utils.WriteLine("do [<Index>]", commandIndent);
    Utils.WriteLine("Execute samples. If <Category> is specified, only the samples belong the category is executed.", descIndent);
    Utils.WriteLine("If <Index> is specified, only that sample is executed.", descIndent);

    Utils.WriteLine("");
    Utils.WriteLine("q", commandIndent);
    Utils.WriteLine(":q", commandIndent);
    Utils.WriteLine("quit", commandIndent);
    Utils.WriteLine("exit", commandIndent);
    Utils.WriteLine("Quit this program.", descIndent);
}

bool EvalCommand(string command, IEnumerable<Tuple<int, ISample>> samples)
{
    switch(command)
    {
        case string x when x == "q" || x == ":q" | x == "quit" || x == "exit":
            Utils.WriteLine("Bye.");
            return false;

        case string x when string.IsNullOrWhiteSpace(command):
            return true;

        case string x when x == "h" || x == "help":
            ShowHelp();
            return true;

        case string x when x == "ls categories" || x == "ls c":
            ShowCategories(samples);
            return true;

        case string x when x == "ls" || x.StartsWith("ls "):
            ListSamples(samples, x);
            return true;

        case string x when x == "do all":
            ExecAllSamples(samples);
            return true;

        case string x when x == "do" || x.StartsWith("do "):
            ExecSamples(samples, x);
            return true;

        default:
            Utils.WriteLine("Command Not Found.");
            return true;
    }
}

void ListSamples(IEnumerable<Tuple<int, ISample>> samples, string command)
{
    string parsedCommand = command.Substring(2).TrimStart();

    foreach (var s in samples)
    {
        if (!string.IsNullOrWhiteSpace(parsedCommand))
        {
            if (!s.Item2.Category.StartsWith(parsedCommand)) continue;
        }

        Utils.WriteLine($"{s.Item1}: \"{s.Item2.Title}, {s.Item2.Category}\"");
    }
}

void ShowCategories(IEnumerable<Tuple<int, ISample>> samples)
{
    int categoryIndent = 2;
    var categories = samples.Select(s => new { category = s.Item2.Category.Split('/'), sample = s.Item2 }).GroupBy(s => s.category[0]).OrderBy(s => s.Key);

    foreach (var c in categories)
    {
        Utils.WriteLine(c.Key);
        foreach (string name in c.Select(x => x.sample.Category).Distinct().OrderBy(x => x))
        {
            Utils.WriteLine($"{name}", categoryIndent);
        }
    }
}

void ExecAllSamples(IEnumerable<Tuple<int, ISample>> samples)
{
    int sampleIndent = 2;

    foreach (var s in samples)
    {
        Utils.WriteLine("");
        Utils.WriteLine($"Sample: {s.Item2.Title}, {s.Item2.Category}");
        Utils.Description(sampleIndent, s.Item2.Description);
        Utils.WriteLine("", sampleIndent);

        s.Item2.Exec(sampleIndent);
    }
}

void ExecSamples(IEnumerable<Tuple<int, ISample>> samples, string command)
{
    int sampleIndent = 2;

    string parsedCommand = command.Substring(2).TrimStart();

    if (string.IsNullOrWhiteSpace(parsedCommand))
    {
        Utils.WriteLine("'do' command must have an argument.");
        return;
    }
    
    int index = -1;
    if (int.TryParse(parsedCommand, out int idx))
    {
        if (idx < 0)
        {
            Utils.WriteLine("The index must be >= 0.");
            return;
        }

        index = idx;
    }

    foreach (var s in samples)
    {
        if (index < 0)
        {
            if (!string.IsNullOrWhiteSpace(parsedCommand))
            {
                if (!s.Item2.Category.StartsWith(parsedCommand)) continue;
            }
        }
        else
        {
            if (s.Item1 != index) continue;
        }

        Utils.WriteLine("");
        Utils.WriteLine($"Sample: {s.Item2.Title}, {s.Item2.Category}");
        Utils.Description(sampleIndent, s.Item2.Description);
        Utils.WriteLine("", sampleIndent);

        s.Item2.Exec(sampleIndent);
    }
}

IEnumerable<ISample> CreateSampleList()
{
    List<ISample> sampleList = new List<ISample>();
    string sampleNamespace = typeof(ISample).Namespace ?? "ReasonProject.Samples";

    Assembly assembly = Assembly.GetExecutingAssembly();
    foreach (Type type in assembly.GetTypes())
    {
        if (type.Namespace == null || !type.Namespace.StartsWith(sampleNamespace)) continue;
        if (!type.IsClass || !typeof(ISample).IsAssignableFrom(type)) continue;
        if (!type.Name.StartsWith("Sample")) continue;

        if (Activator.CreateInstance(type) is not ISample sample) continue;

        sampleList.Add(sample);
    }

    return sampleList;
}