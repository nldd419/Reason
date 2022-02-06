using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reason.I18n.ExceptionMessages
{
    internal class ExceptionMessagesJPN : ExceptionMessagesENG
    {
        public override string DemandToCheckResult => "IsFailedメソッドを呼び出して、先に値が有効であることを確認してください。";
        public override string DepthOverflow => "結果ツリーを探索中にdepthがオーバーフローしました。";
    }
}
