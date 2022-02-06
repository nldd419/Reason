using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.Reasons
{
    /// <summary>
    /// General purpose class for reasons that needs ID and arbitrary message.
    /// </summary>
    public class FailedReasonWithMessage : FailedReason
    {
        public FailedReasonWithMessage(string message)
        {
            this.message = message;
        }

        private readonly string message;
        public override string Message { get => message; }
    }
}
