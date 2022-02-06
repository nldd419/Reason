using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.I18n;

namespace Reason.Reasons
{
    /// <summary>
    /// Class represents the failed reason is unknown.
    /// </summary>
    public class FailedReasonUnknown : FailedReason
    {
        public override string Message { get => I18nContext.Current.FailedMessages.Unknown; }
    }
}
