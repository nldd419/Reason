using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.I18n.ExceptionMessages
{
    internal class ExceptionMessagesENG : IExceptionMessages
    {
        public virtual string DemandToCheckResult => "You must check that the value is valid first by calling 'IsFailed' method.";
        public virtual string DepthOverflow => "The depth has overflowed when inspecting a result tree.";
    }
}
