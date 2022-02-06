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
    internal class ContextENG : I18nContext 
    {
        public ContextENG(CultureInfo culture) : base(culture)
        {
        }

        protected override IFailedMessages CreateFailedMessages() => new FailedMessagesENG();
        protected override IExceptionMessages CreateExceptionMessages() => new ExceptionMessagesENG();
    }
}
