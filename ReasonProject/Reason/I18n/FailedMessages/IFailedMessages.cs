using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.I18n.FailedMessages
{
    public interface IFailedMessages
    {
        string NotFailed { get; }
        string Unknown { get; }
        string ValueIsNull { get; }
    }
}
