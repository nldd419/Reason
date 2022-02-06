using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.I18n.FailedMessages
{
    internal class FailedMessagesENG : IFailedMessages
    {
        public virtual string NotFailed => "Not failed.";
        public virtual string Unknown => "Unknown reason.";
        public virtual string ValueIsNull => "The value is null.";
    }
}
