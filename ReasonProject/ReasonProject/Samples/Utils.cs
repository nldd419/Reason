using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReasonProject.Samples
{
    internal static class Utils
    {
        /// <summary>
        /// Write line(s) with some indents. 
        /// </summary>
        public static void WriteLine(string line, int indent = 0)
        {
            Console.WriteLine(line.PadLeft(line.Length + indent).Replace(Environment.NewLine, Environment.NewLine.PadRight(Environment.NewLine.Length + indent)));
        }

        /// <summary>
        /// Write line(s) with some indents without linebreaks at the end. 
        /// </summary>
        public static void Write(string line, int indent = 0)
        {
            Console.Write(line.PadLeft(line.Length + indent).Replace(Environment.NewLine, Environment.NewLine.PadRight(Environment.NewLine.Length + indent)));
        }
    }
}
