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
    public class FailedReasonException<E> : FailedReason where E : Exception
    {
        public delegate string CustomExceptionMessageFunc(E e);

        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public FailedReasonException(E e, bool useMessagePropertyAsMessage)
        {
            TheException = e;

            if (useMessagePropertyAsMessage)
            {
                this.message = e.Message;
            }
            else
            {
                this.message = e.ToString();
            }
        }

        /// <param name="createMessageFunc">A user-defined create message function.</param>
        public FailedReasonException(E e, CustomExceptionMessageFunc createMessageFunc)
        {
            TheException = e;
            this.message = createMessageFunc(e);
        }

        public readonly E TheException;

        private readonly string message;
        public override string Message { get => message; }
    }
}
