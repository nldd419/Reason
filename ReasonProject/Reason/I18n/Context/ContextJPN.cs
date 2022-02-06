using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.I18n;
using Reason.I18n.FailedMessages;
using Reason.I18n.ExceptionMessages;

namespace Reason.I18n.Context
{
    internal class ContextJPN : I18nContext 
    {
        public ContextJPN(CultureInfo culture) : base(culture)
        {
        }

        protected override IFailedMessages CreateFailedMessages() => new FailedMessagesJPN();
        protected override IExceptionMessages CreateExceptionMessages() => new ExceptionMessagesJPN();
    }
}
