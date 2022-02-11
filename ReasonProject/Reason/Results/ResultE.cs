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
    // A file of class Result is split because defining Catch methods is too long.

    /// <summary>
    /// Base class of any result.
    /// </summary>
    public abstract partial class Result: IDagNode
    {
        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result Catch<E1, E2>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result<T> Catch<E1, E2, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        public static Result Catch<E1, E2>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        public static Result<T> Catch<E1, E2, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result Catch<E1, E2, E3>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2, E3>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result<T> Catch<E1, E2, E3, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3, T>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, E3, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3, T>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        public static Result Catch<E1, E2, E3>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2, E3>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        public static Result<T> Catch<E1, E2, E3, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3, T>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, E3, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3, T>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result Catch<E1, E2, E3, E4>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2, E3, E4>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result<T> Catch<E1, E2, E3, E4, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3, T>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4, T>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, E3, E4, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3, T>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4, T>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        public static Result Catch<E1, E2, E3, E4>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2, E3, E4>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        public static Result<T> Catch<E1, E2, E3, E4, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3, T>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4, T>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, E3, E4, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3, T>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4, T>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result Catch<E1, E2, E3, E4, E5>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
            catch (E5 ex)
            {
                return Result.MakeExceptionFirst<E5>(new FailedReasonException<E5>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2, E3, E4, E5>(Func<Result> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
            catch (E5 ex)
            {
                return nextResult.MakeException<E5>(new FailedReasonException<E5>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        public static Result<T> Catch<E1, E2, E3, E4, E5, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3, T>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4, T>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
            catch (E5 ex)
            {
                return Result.MakeExceptionFirst<E5, T>(new FailedReasonException<E5>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="useMessagePropertyAsMessage">If True, <see cref="Exception.Message"/> is used as a message, otherwise <see cref="Exception.ToString"/> is used.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, E3, E4, E5, T>(Func<Result<T>> funcitonMaybeThrowException, bool useMessagePropertyAsMessage, Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, useMessagePropertyAsMessage));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, useMessagePropertyAsMessage));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3, T>(new FailedReasonException<E3>(ex, useMessagePropertyAsMessage));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4, T>(new FailedReasonException<E4>(ex, useMessagePropertyAsMessage));
            }
            catch (E5 ex)
            {
                return nextResult.MakeException<E5, T>(new FailedReasonException<E5>(ex, useMessagePropertyAsMessage));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        /// <param name="createMessageFunc5">A user-defined create message function.</param>
        public static Result Catch<E1, E2, E3, E4, E5>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4,
            FailedReasonException<E5>.CustomExceptionMessageFunc createMessageFunc5)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
            catch (E5 ex)
            {
                return Result.MakeExceptionFirst<E5>(new FailedReasonException<E5>(ex, createMessageFunc5));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        /// <param name="createMessageFunc5">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result Catch<E1, E2, E3, E4, E5>(Func<Result> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4,
            FailedReasonException<E5>.CustomExceptionMessageFunc createMessageFunc5,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
            catch (E5 ex)
            {
                return nextResult.MakeException<E5>(new FailedReasonException<E5>(ex, createMessageFunc5));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        /// <param name="createMessageFunc5">A user-defined create message function.</param>
        public static Result<T> Catch<E1, E2, E3, E4, E5, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4,
            FailedReasonException<E5>.CustomExceptionMessageFunc createMessageFunc5)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return Result.MakeExceptionFirst<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return Result.MakeExceptionFirst<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return Result.MakeExceptionFirst<E3, T>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return Result.MakeExceptionFirst<E4, T>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
            catch (E5 ex)
            {
                return Result.MakeExceptionFirst<E5, T>(new FailedReasonException<E5>(ex, createMessageFunc5));
            }
        }

        /// <summary>
        /// <para>
        /// Catch only specified types of exception thrown by <paramref name="funcitonMaybeThrowException"/> and convert it into <see cref="ExceptionResult{E}"/>.<br/>
        /// If <paramref name="funcitonMaybeThrowException"/> didn't throw any exception, the returning <see cref="Result"/> is the return value of <paramref name="funcitonMaybeThrowException"/>.<br/>
        /// </para>
        /// </summary>
        /// <param name="funcitonMaybeThrowException">A function that may throw an exception.</param>
        /// <param name="createMessageFunc1">A user-defined create message function.</param>
        /// <param name="createMessageFunc2">A user-defined create message function.</param>
        /// <param name="createMessageFunc3">A user-defined create message function.</param>
        /// <param name="createMessageFunc4">A user-defined create message function.</param>
        /// <param name="createMessageFunc5">A user-defined create message function.</param>
        /// <param name="nextResult">A result object which the new exception result follows when the function results in throwing an exception.</param>
        public static Result<T> Catch<E1, E2, E3, E4, E5, T>(Func<Result<T>> funcitonMaybeThrowException,
            FailedReasonException<E1>.CustomExceptionMessageFunc createMessageFunc1,
            FailedReasonException<E2>.CustomExceptionMessageFunc createMessageFunc2,
            FailedReasonException<E3>.CustomExceptionMessageFunc createMessageFunc3,
            FailedReasonException<E4>.CustomExceptionMessageFunc createMessageFunc4,
            FailedReasonException<E5>.CustomExceptionMessageFunc createMessageFunc5,
            Result nextResult)
            where E1 : Exception
            where E2 : Exception
            where E3 : Exception
            where E4 : Exception
            where E5 : Exception
        {
            try
            {
                return funcitonMaybeThrowException();
            }
            catch (E1 ex)
            {
                return nextResult.MakeException<E1, T>(new FailedReasonException<E1>(ex, createMessageFunc1));
            }
            catch (E2 ex)
            {
                return nextResult.MakeException<E2, T>(new FailedReasonException<E2>(ex, createMessageFunc2));
            }
            catch (E3 ex)
            {
                return nextResult.MakeException<E3, T>(new FailedReasonException<E3>(ex, createMessageFunc3));
            }
            catch (E4 ex)
            {
                return nextResult.MakeException<E4, T>(new FailedReasonException<E4>(ex, createMessageFunc4));
            }
            catch (E5 ex)
            {
                return nextResult.MakeException<E5, T>(new FailedReasonException<E5>(ex, createMessageFunc5));
            }
        }

        // I gave up.
    }
}
