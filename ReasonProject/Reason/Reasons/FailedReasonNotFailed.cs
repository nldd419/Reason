using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.I18n;

namespace Reason.Reasons
{
    /// <summary>
    /// Class represents that the result has not failed.
    /// </summary>
    public class FailedReasonNotFailed : ReasonBase
    {
        public override string Message { get => I18nContext.Current.FailedMessages.NotFailed; }
    }
}
