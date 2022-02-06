using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.Results
{
    public interface IResultTree
    {
        IEnumerable<Result> NextResults { get; }
    }
}
