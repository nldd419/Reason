using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigDAG;

using Reason.Reasons;

namespace Reason.Results
{
    /// <summary>
    /// Base class of any result.
    /// </summary>
    public abstract partial class Result: IDagNode
    {
        protected Result(IEnumerable<Result> nextResults, ReasonBase? reason)
        {
            this.nextResults = new ReadOnlyCollection<Result>(nextResults.ToList());
            this.Reason = reason;
        }

        public IEnumerable<IDagNode> Nexts => NextResults;
        public IEnumerable<Result> NextResults => this.nextResults;
        private readonly ReadOnlyCollection<Result> nextResults;
        protected readonly ReasonBase? Reason;

        /// <summary>
        /// Configurations affect all behaviors of the library.
        /// </summary>
        public static class Configurations
        {
            public static bool AutomaticCatch = false;
            public static bool UseMessagePropertyAsMessage = true;
            public static FailedReasonException<Exception>.CustomExceptionMessageFunc? CreateMessageFunc = null;
        }

        /// <summary>
        /// Get the reason why the result has failed.
        /// </summary>
        /// <remarks>This method never throw any exceptions. If the result has succeeded, this returns <see cref="FailedReasonNotFailed"/> instance.</remarks>
        public ReasonBase GetReason()
        {
            ReasonBase? reason = GetReasonCore();
            if (reason != null) return reason;
            if (Reason != null) return Reason;
            return new FailedReasonNotFailed();
        }
        /// <summary>
        /// <para>
        /// Inherit classes should override this method and return a custom reason.<br/>
        /// Return null if you prefer the default behavior, or you can just leave it (not override) as is.<br/>
        /// Return <see cref="FailedReasonNotFailed"/> if you prefer to make the result success.<br/>
        /// </para>
        /// </summary>
        protected virtual ReasonBase? GetReasonCore() { return null; }

        /// <summary>
        /// True if the result has failed, False if not.
        /// </summary>
        public bool IsFailed()
        {
            if (IsFailedCore()) return true;
            if (GetReason() is not FailedReasonNotFailed) return true;
            return false;
        }
        /// <summary>
        /// <para>
        /// Return true if the result has failed. When you return false, that doesn't mean that the result has succeeded, it's just propagated to the default behavior.<br/>
        /// Most of the time, you don't have to override this method because the default implementation chooses an appropriate value based on the result returned by <see cref=" GetReason"/>.
        /// </para>
        /// </summary>
        protected virtual bool IsFailedCore() { return false; }

        /// <summary>
        /// Return the message why the result has failed.
        /// </summary>
        public override string ToString()
        {
            return GetReason().Message;
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        public Result Whether(Func<Result> successAction, Func<ReasonBase, Result> failAction)
        {
            return Whether(successAction, failAction, Configurations.AutomaticCatch, Configurations.UseMessagePropertyAsMessage, Configurations.CreateMessageFunc);
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        /// <param name="automaticCatch">If True, an exception thrown by <paramref name="successAction"/> or <paramref name="failAction"/> will be automatically caught and converted into <see cref="ExceptionResult{E}"/>.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used. This parameter is ignored when <paramref name="createMessageFunc"/> is not null.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        public Result Whether(Func<Result> successAction, Func<ReasonBase, Result> failAction,
            bool automaticCatch, bool useMessagePropertyAsMessage, FailedReasonException<Exception>.CustomExceptionMessageFunc? createMessageFunc = null)
        {
            if (automaticCatch)
            {
                if (createMessageFunc == null)
                {
                    return CatchAll(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction();
                    }, useMessagePropertyAsMessage: useMessagePropertyAsMessage);
                }
                else
                {
                    return CatchAll(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction();
                    }, createMessageFunc: createMessageFunc);
                }
            }
            else
            {
                if (IsFailed()) return failAction(GetReason());
                else return successAction();
            }
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        public Result<R> Whether<R>(Func<Result<R>> successAction, Func<ReasonBase, Result<R>> failAction)
        {
            return Whether(successAction, failAction, Configurations.AutomaticCatch, Configurations.UseMessagePropertyAsMessage, Configurations.CreateMessageFunc);
        }

        /// <summary>
        /// Branch off to actions, one is for success, another is for failure.
        /// </summary>
        /// <param name="successAction">An action excecuted when the result has succeeded.</param>
        /// <param name="failAction">An action excecuted when the result has failed.</param>
        /// <param name="automaticCatch">If True, an exception thrown by <paramref name="successAction"/> or <paramref name="failAction"/> will be automatically caught and converted into <see cref="ExceptionResult{E}"/>.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used. This parameter is ignored when <paramref name="createMessageFunc"/> is not null.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        public Result<R> Whether<R>(Func<Result<R>> successAction, Func<ReasonBase, Result<R>> failAction,
            bool automaticCatch, bool useMessagePropertyAsMessage, FailedReasonException<Exception>.CustomExceptionMessageFunc? createMessageFunc = null)
        {
            if (automaticCatch)
            {
                if (createMessageFunc == null)
                {
                    return CatchAll(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction();
                    }, useMessagePropertyAsMessage: useMessagePropertyAsMessage);
                }
                else
                {
                    return CatchAll(() =>
                    {
                        if (IsFailed()) return failAction(GetReason());
                        else return successAction();
                    }, createMessageFunc: createMessageFunc);
                }
            }
            else
            {
                if (IsFailed()) return failAction(GetReason());
                else return successAction();
            }
        }

        /// <summary>
        /// Make a success result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        public static SuccessResult MakeSuccessFirst()
        {
            return new SuccessResult();
        }

        /// <summary>
        /// Make a success result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        public static SuccessResult<T> MakeSuccessFirst<T>(T v)
        {
            return new SuccessResult<T>(v);
        }

        /// <summary>
        /// Make a success result which follows this result.
        /// </summary>
        public SuccessResult MakeSuccess()
        {
            return new SuccessResult(new List<Result> { this });
        }

        /// <summary>
        /// Make a success result which follows this result.
        /// </summary>
        /// <param name="v">A value of the result depends on user's context.</param>
        public SuccessResult<T> MakeSuccess<T>(T v)
        {
            return new SuccessResult<T>(v, new List<Result> { this });
        }

        /// <summary>
        /// Make a failed result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        public static FailedResult MakeFailedFirst(FailedReason reason)
        {
            return new FailedResult(reason);
        }

        /// <summary>
        /// Make a failed result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        public static FailedResult<T> MakeFailedFirst<T>(FailedReason reason)
        {
            return new FailedResult<T>(reason);
        }

        /// <summary>
        /// Make a failed result which follows this result.
        /// </summary>
        /// <param name="reason">The reason of the failure.</param>
        public FailedResult MakeFailed(FailedReason reason)
        {
            return new FailedResult(reason, new List<Result> { this });
        }

        /// <summary>
        /// Make a failed result which follows this result.
        /// </summary>
        /// <param name="reason">The reason of the failure.</param>
        public FailedResult<T> MakeFailed<T>(FailedReason reason)
        {
            return new FailedResult<T>(reason, new List<Result> { this });
        }

        /// <summary>
        /// Make an exception result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        public static ExceptionResult<E> MakeExceptionFirst<E>(FailedReasonException<E> reason) where E : Exception
        {
            return new ExceptionResult<E>(reason);
        }

        /// <summary>
        /// Make an exception result which follows this result.
        /// </summary>
        /// <param name="reason">The reason of the exception.</param>
        public ExceptionResult<E> MakeException<E>(FailedReasonException<E> reason) where E : Exception
        {
            return new ExceptionResult<E>(reason, new List<Result> { this });
        }

        /// <summary>
        /// Make an exception result which is the first instance of a result tree.
        /// This means that the new result doesn't follow any results.
        /// </summary>
        public static ExceptionResult<E, T> MakeExceptionFirst<E, T>(FailedReasonException<E> reason) where E : Exception
        {
            return new ExceptionResult<E, T>(reason);
        }

        /// <summary>
        /// Make an exception result which follows this result.
        /// </summary>
        /// <param name="reason">The reason of the exception.</param>
        public ExceptionResult<E, T> MakeException<E, T>(FailedReasonException<E> reason) where E : Exception
        {
            return new ExceptionResult<E, T>(reason, new List<Result> { this });
        }

        /// <summary>
        /// <para>
        /// Make an all success result which follows the aggregated results.<br/>
        /// <see cref="AllSuccessResult"/> succeeds when all the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        public static AllSuccessResult MakeAllSuccess(IEnumerable<Result> aggregatedResults)
        {
            return new AllSuccessResult(aggregatedResults);
        }

        /// <summary>
        /// <para>
        /// Make an all success result which follows the aggregated results.<br/>
        /// <see cref="AllSuccessResult"/> succeeds when all the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        public static AllSuccessResult MakeAllSuccess(params Result[] aggregatedResults)
        {
            return new AllSuccessResult(aggregatedResults);
        }

        /// <summary>
        /// <para>
        /// Make an all success result which follows the aggregated results.<br/>
        /// <see cref="AllSuccessResult"/> succeeds when all the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        /// <param name="failedReason">A reason shows when any of the aggregated results have failed.</param>
        public static AllSuccessResult MakeAllSuccess(IEnumerable<Result> aggregatedResults, FailedReason failedReason)
        {
            return new AllSuccessResult(aggregatedResults, failedReason);
        }

        /// <summary>
        /// <para>
        /// Make an all success result which follows the aggregated results.<br/>
        /// <see cref="AllSuccessResult"/> succeeds when all the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        /// <param name="failedReason">A reason shows when any of the aggregated results have failed.</param>
        public static AllSuccessResult MakeAllSuccess(FailedReason failedReason, params Result[] aggregatedResults)
        {
            return new AllSuccessResult(aggregatedResults, failedReason);
        }

        /// <summary>
        /// <para>
        /// Make an any success result which follows the aggregated results.<br/>
        /// <see cref="AnySuccessResult"/> succeeds when any of the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        public static AnySuccessResult MakeAnySuccess(IEnumerable<Result> aggregatedResults)
        {
            return new AnySuccessResult(aggregatedResults);
        }

        /// <summary>
        /// <para>
        /// Make an any success result which follows the aggregated results.<br/>
        /// <see cref="AnySuccessResult"/> succeeds when any of the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        public static AnySuccessResult MakeAnySuccess(params Result[] aggregatedResults)
        {
            return new AnySuccessResult(aggregatedResults);
        }

        /// <summary>
        /// <para>
        /// Make an any success result which follows the aggregated results.<br/>
        /// <see cref="AnySuccessResult"/> succeeds when any of the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        /// <param name="failedReason">A reason shows when any of the aggregated results have failed.</param>
        public static AnySuccessResult MakeAnySuccess(IEnumerable<Result> aggregatedResults, FailedReason failedReason)
        {
            return new AnySuccessResult(aggregatedResults, failedReason);
        }

        /// <summary>
        /// <para>
        /// Make an any success result which follows the aggregated results.<br/>
        /// <see cref="AnySuccessResult"/> succeeds when any of the <paramref name="aggregatedResults"/> have succeeded.<br/>
        /// </para>
        /// </summary>
        /// <param name="failedReason">A reason shows when any of the aggregated results have failed.</param>
        public static AnySuccessResult MakeAnySuccess(FailedReason failedReason, params Result[] aggregatedResults)
        {
            return new AnySuccessResult(aggregatedResults, failedReason);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result CatchAll(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
        {
            return Catch<Exception>(funcitonMaybeThrowException, useMessagePropertyAsMessage);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result CatchAll(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
        {
            return Catch<Exception>(funcitonMaybeThrowException, useMessagePropertyAsMessage, nextResult);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result<T> CatchAll<T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
        {
            return Catch<Exception, T>(funcitonMaybeThrowException, useMessagePropertyAsMessage);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> CatchAll<T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
        {
            return Catch<Exception, T>(funcitonMaybeThrowException, useMessagePropertyAsMessage, nextResult);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        public static Result CatchAll(Func<Result> funcitonMaybeThrowException, FailedReasonException<Exception>.CustomExceptionMessageFunc createMessageFunc)
        {
            return Catch(funcitonMaybeThrowException, createMessageFunc);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result CatchAll(Func<Result> funcitonMaybeThrowException, FailedReasonException<Exception>.CustomExceptionMessageFunc createMessageFunc, Result nextResult)
        {
            return Catch(funcitonMaybeThrowException, createMessageFunc, nextResult);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        public static Result<T> CatchAll<T>(Func<Result<T>> funcitonMaybeThrowException, FailedReasonException<Exception>.CustomExceptionMessageFunc createMessageFunc)
        {
            return Catch(funcitonMaybeThrowException, createMessageFunc);
        }

        /// <summary>
        /// <para>
        /// Catch all type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into ExceptionResult<![CDATA[<Exception>]]> (see <see cref="ExceptionResult{E}"/>).<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> CatchAll<T>(Func<Result<T>> funcitonMaybeThrowException, FailedReasonException<Exception>.CustomExceptionMessageFunc createMessageFunc, Result nextResult)
        {
            return Catch(funcitonMaybeThrowException, createMessageFunc, nextResult);
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result Catch<E>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return Result.MakeExceptionFirst<E>(new FailedReasonException<E>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return nextResult.MakeException<E>(new FailedReasonException<E>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result<T> Catch<E, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return Result.MakeExceptionFirst<E, T>(new FailedReasonException<E>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return nextResult.MakeException<E, T>(new FailedReasonException<E>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        public static Result Catch<E>(Func<Result> funcitonMaybeThrowException, FailedReasonException<E>.CustomExceptionMessageFunc createMessageFunc) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return Result.MakeExceptionFirst<E>(new FailedReasonException<E>(ex, createMessageFunc));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E>(Func<Result> funcitonMaybeThrowException, FailedReasonException<E>.CustomExceptionMessageFunc createMessageFunc, Result nextResult) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return nextResult.MakeException<E>(new FailedReasonException<E>(ex, createMessageFunc));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        public static Result<T> Catch<E, T>(Func<Result<T>> funcitonMaybeThrowException, FailedReasonException<E>.CustomExceptionMessageFunc createMessageFunc) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return Result.MakeExceptionFirst<E, T>(new FailedReasonException<E>(ex, createMessageFunc));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only a specified type of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E, T>(Func<Result<T>> funcitonMaybeThrowException, FailedReasonException<E>.CustomExceptionMessageFunc createMessageFunc, Result nextResult) where E : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E ex)
            {
                return nextResult.MakeException<E, T>(new FailedReasonException<E>(ex, createMessageFunc));
            }
        }
    }
}
