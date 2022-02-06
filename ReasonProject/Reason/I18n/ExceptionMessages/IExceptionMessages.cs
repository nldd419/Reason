using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.I18n.ExceptionMessages
{
    public interface IExceptionMessages
    {
        string DemandToCheckResult { get; }
        string DepthOverflow { get; }
    }
}
