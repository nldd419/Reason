using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.I18n.FailedMessages
{
    internal class FailedMessagesJPN : FailedMessagesENG
    {
        public override string NotFailed => "失敗していません。";
        public override string Unknown => "不明な理由です。";
        public override string ValueIsNull => "値がnullです。";
    }
}
