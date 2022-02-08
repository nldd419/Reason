using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Reasons;
using Reason.I18n;
using Reason.Exceptions;

namespace Reason.Results
{
    /// <summary>
    /// Main class of concrete result.
    /// </summary>
    /// <typeparam name="T">Type of arbitrary result value when the result is success.</typeparam>
    public class Result<T> : Result
    {
        public Result(T? val, ReasonBase? reason, IEnumerable<Result> nextResults) : base(nextResults, reason)
        {
            this.val = val;
        }

        private readonly T? val;
        private bool allowGet = false;

        protected override ReasonBase? GetReasonCore()
        {
            if (val == null) return new FailedReasonValueIsNull();
            return null;
        }

        protected override bool IsFailedCore()
        {
            if (GetReason() is FailedReasonValueIsNull) return true;

            //For safety purpose when 'IsFailed' method is never called and 'Get' method always succeed on development. 
            allowGet = true;

            return false;
        }

        /// <summary>
        /// Get a value when the result has succeeded.<br/>
        /// </summary>
        /// <remarks>You must call <see cref="IsFailed"/> method before calling this.</remarks>
        /// <exception cref="ReasonCustomException">If you forgot to call <see cref="IsFailed"/> before calling this method, this exception will occur.</exception>
        public T Get()
        {
            if (!allowGet || val == null) throw new ReasonCustomException(I18nContext.Current.ExceptionMessages.DemandToCheckResult);

            return (T)val;
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        public Result Whether(Func<T, Result> successAction, Func<ReasonBase, Result> failAction)
        {
            return Whether(successAction, failAction, Configurations.AutomaticCatch, Configurations.UseMessagePropertyAsMessage, Configurations.CreateMessageFunc);
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        public Result Whether(Func<T, Result> successAction, Func<ReasonBase, Result> failAction,
            bool automaticCatch, bool useMessagePropertyAsMessage, FailedReasonException<Exception>.CustomExceptionMessageFunc? createMessageFunc = null)
        {
            if (automaticCatch)
            {
                if (createMessageFunc == null)
                {
                    return CatchAll(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction(Get());
                    }, useMessagePropertyAsMessage: useMessagePropertyAsMessage);
                }
                else
                {
                    return CatchAll(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction(Get());
                    }, createMessageFunc: createMessageFunc);
                }
            }
            else
            {
                if (IsFailed()) return failAction(GetReason());
                else return successAction(Get());
            }
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        public Result<R> Whether<R>(Func<T, Result<R>> successAction, Func<ReasonBase, Result<R>> failAction)
        {
            return Whether(successAction, failAction, Configurations.AutomaticCatch, Configurations.UseMessagePropertyAsMessage, Configurations.CreateMessageFunc);
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        public Result<R> Whether<R>(Func<T, Result<R>> successAction, Func<ReasonBase, Result<R>> failAction,
            bool automaticCatch, bool useMessagePropertyAsMessage, FailedReasonException<Exception>.CustomExceptionMessageFunc? createMessageFunc = null)
        {
            if (automaticCatch)
            {
                if (createMessageFunc == null)
                {
                    return CatchAll<R>(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction(Get());
                    }, useMessagePropertyAsMessage: useMessagePropertyAsMessage);
                }
                else
                {
                    return CatchAll<R>(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction(Get());
                    }, createMessageFunc: createMessageFunc);
                }
            }
            else
            {
                if (IsFailed()) return failAction(GetReason());
                else return successAction(Get());
            }
        }

        /// <summary>
        /// Make a success result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        /// <param name="v">A value of the result depends on user's context.</param>
        public static SuccessResult<T> MakeSuccessFirst(T v)
        {
            return Result.MakeSuccessFirst<T>(v);
        }

        /// <summary>
        /// Make a failed result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        /// <param name="reason">The reason of the failure.</param>
        public static new FailedResult<T> MakeFailedFirst(FailedReason reason)
        {
            return Result.MakeFailedFirst<T>(reason);
        }
    }
}
