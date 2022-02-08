using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReasonProject.Samples
{
    internal static class Utils
    {
        public const int CODE_INDENT = 2;
        public static TextWriter? TextWriter = null;

        /// <summary>
        /// Write line(s) with some indents. 
        /// </summary>
        public static void WriteLine(string line, int indent = 0)
        {
            WriteLineCore(line.PadLeft(line.Length + indent).Replace(Environment.NewLine, Environment.NewLine.PadRight(Environment.NewLine.Length + indent)));
        }

        /// <summary>
        /// Write line(s) with some indents. 
        /// </summary>
        public static void WriteLine(int indent = 0, params string[] lines)
        {
            string line = string.Join(Environment.NewLine, lines);
            WriteLineCore(line.PadLeft(line.Length + indent).Replace(Environment.NewLine, Environment.NewLine.PadRight(Environment.NewLine.Length + indent)));
        }

        /// <summary>
        /// Write line(s) with some indents. 
        /// </summary>
        public static void WriteLineForCode(string line, int indent = 0)
        {
            WriteLine(line, indent + CODE_INDENT);
        }

        /// <summary>
        /// Write line(s) with some indents. 
        /// </summary>
        public static void WriteLineForCode(int indent = 0, params string[] lines)
        {
            WriteLine(indent + CODE_INDENT, lines);
        }

        /// <summary>
        /// Write line(s) with some indents without linebreaks at the end. 
        /// </summary>
        public static void Write(string line, int indent = 0)
        {
            WriteCore(line.PadLeft(line.Length + indent).Replace(Environment.NewLine, Environment.NewLine.PadRight(Environment.NewLine.Length + indent)));
        }

        private static void WriteLineCore(string line)
        {
            Tee(line, true, TextWriter);
        }

        private static void WriteCore(string line)
        {
            Tee(line, false, TextWriter);
        }

        private static void Tee(string line, bool linebreak, TextWriter? tw)
        {
            if (linebreak)
            {
                Console.WriteLine(line);
            }
            else
            {
                Console.Write(line);
            }

            if(tw != null)
            {
                if (linebreak)
                {
                    tw.WriteLine(line);
                }
                else
                {
                    tw.Write(line);
                }
            }
        }

        public static void Description(params string[] lines)
        {
            Description(0, lines);
        }

        public static void Description(int indent, params string[] lines)
        {
            WriteLine("/*", indent);
            WriteLine(" * Description:", indent);

            foreach (string line in lines)
            {
                WriteLine(" * " + line, indent);
            }

            WriteLine(" */", indent);
        }
    }
}
