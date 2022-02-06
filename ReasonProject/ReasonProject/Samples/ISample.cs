using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReasonProject.Samples
{
    internal interface ISample
    {
        string Category { get; }
        string Title { get; }
        void Exec(int indent);
    }
}
