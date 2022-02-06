using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.Reasons
{
    /// <summary>
    /// Base class of providing reason.<br/>
    /// You can just inherit this class and override the <see cref="Message"/> property.
    /// </summary>
    public abstract class ReasonBase
    {
        public abstract string Message { get; }
    }
}
