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
    public class FailedReasonWithId<T> : FailedReason
    {
        public FailedReasonWithId(T id, string message)
        {
            this.Id = id;
            this.message = message;
        }

        public readonly T Id;
        private readonly string message;
        public override string Message { get => message; }
    }
}
