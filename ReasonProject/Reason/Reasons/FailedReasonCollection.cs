using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.Reasons
{
    /// <summary>
    /// Class that has multiple failed reasons.
    /// </summary>
    public class FailedReasonCollection : FailedReason
    {
        public FailedReasonCollection(IEnumerable<FailedReason> failedReasons)
        {
            this.FailedReasons = new ReadOnlyCollection<FailedReason>(failedReasons.ToList());
        }

        public readonly ReadOnlyCollection<FailedReason> FailedReasons;

        public override string Message
        { 
            get
            {
                int count = FailedReasons.Count;
                StringBuilder sb = new StringBuilder();
                for(int i=0; i < count; i++)
                {
                    if(i > 0) sb.Append(Environment.NewLine);
                    sb.Append(FailedReasons[i].Message);
                }
                return sb.ToString();
            }
        }
    }
}
