using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reason.Results;
using Reason.I18n;

namespace Reason.Utils
{
    public static class ResultInspector
    {
        /// <summary>
        /// Condition for filtering results to inspect.
        /// </summary>
        /// <param name="result">A result to inspect.</param>
        /// <param name="depth">A depth from the start result in the tree.</param>
        /// <param name="index">An index in the parent list. If the result has no parent, this returns -1.</param>
        /// <param name="parent">A parent of the result currently inspecting.</param>
        /// <returns>Return true if you want to inspect the result.</returns>
        public delegate bool ConditionFunc(Result result, ulong depth, int index, Result? parent);
        /// <summary>
        /// <para>
        /// Condition for pruning a subtree.<br/>
        /// The inspector won't inspect under the pruned subtree.<br/>
        /// </para>
        /// </summary>
        /// <param name="result">A result to inspect.</param>
        /// <param name="depth">A depth from the start result in the tree.</param>
        /// <param name="index">An index in the parent list. If the result has no parent, this returns -1.</param>
        /// <param name="parent">A parent of the result currently inspecting.</param>
        /// <returns>Return true if you want to prune the subtree.</returns>
        public delegate bool PruneConditionFunc(Result result, ulong depth, int index, Result? parent);
        /// <summary>
        /// A user-defined action of an inspection.
        /// </summary>
        /// <param name="result">A result to inspect.</param>
        /// <param name="depth">A depth from the start result in the tree.</param>
        /// <param name="index">An index in the parent list. If the result has no parent, this returns -1.</param>
        /// <param name="parent">A parent of the result currently inspecting.</param>
        public delegate void InspectAction(Result result, ulong depth, int index, Result? parent);

        /// <summary>
        /// Traverse all the results in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectAll(Result firstResult, InspectAction inspector, bool depthFirstSearch = true)
        {
            InspectWhere(firstResult, inspector, (r, d, i, p) => true, (r, d, i, p) => false, depthFirstSearch);
        }

        /// <summary>
        /// Traverse failed results in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectFailedOnly(Result firstResult, InspectAction inspector, bool depthFirstSearch = true)
        {
            InspectWhere(firstResult, inspector, (r, d, i, p) => r.IsFailed(), (r, d, i, p) => false, depthFirstSearch);
        }

        /// <summary>
        /// Traverse succeeded results in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectSucceededOnly(Result firstResult, InspectAction inspector, bool depthFirstSearch = true)
        {
            InspectWhere(firstResult, inspector, (r, d, i, p) => !r.IsFailed(), (r, d, i, p) => false, depthFirstSearch);
        }

        /// <summary>
        /// Traverse results which meets a condition in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="condition">A condition of a result you want to look into.</param>
        /// <param name="pruneCondition">A pruning condition of a result subtree.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectWhere(Result firstResult, InspectAction inspector, ConditionFunc condition, PruneConditionFunc pruneCondition, bool depthFirstSearch = true)
        {
            Inspect(firstResult, inspector, condition, pruneCondition, depthFirstSearch);
        }

        /// <summary>
        /// Traverse results which meets a condition in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="condition">A condition of a result you want to look into.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectWhere(Result firstResult, InspectAction inspector, ConditionFunc condition, bool depthFirstSearch = true)
        {
            Inspect(firstResult, inspector, condition, (r, d, i, p) => false, depthFirstSearch);
        }

        /// <summary>
        /// The actual implementation of inspecting a result tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="condition">A condition of a result you want to look into.</param>
        /// <param name="pruneCondition">A pruning condition of a result subtree.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        private static void Inspect(Result firstResult, InspectAction inspector, ConditionFunc condition, PruneConditionFunc pruneCondition, bool depthFirstSearch)
        {
            InspectionParams iParams = new InspectionParams(firstResult);

            if (pruneCondition(iParams.Current.Result, iParams.Depth, iParams.Current.IndexAsChild, iParams.Current.Parent)) return;

            //For breadth-first search
            HashSet<Result> prunedResults = new HashSet<Result>();
            ulong currentDepth = iParams.Depth + 1;
            ulong prevMaxDepth = iParams.Depth.Max;

            bool finish = false;

            do
            {
                // Inspect it.
                if (condition(iParams.Current.Result, iParams.Depth, iParams.Current.IndexAsChild, iParams.Current.Parent)) inspector(iParams.Current.Result, iParams.Depth, iParams.Current.IndexAsChild, iParams.Current.Parent);

                if (depthFirstSearch)
                {
                    // Depth-first search.

                    // If the result has a first child, go inspect it unless it's not a pruning target.
                    bool goFindSibling = true;
                    if (iParams.Current.Result.NextResults.Any())
                    {
                        iParams.Push(new ResultInfo(iParams.Current.Result.NextResults.First(), iParams.Current.Result, 0));
                        goFindSibling = false;
                    }

                    // Deciding whether to prune a subtree or not. 
                    if (!goFindSibling && pruneCondition(iParams.Current.Result, iParams.Depth, iParams.Current.IndexAsChild, iParams.Current.Parent))
                    {
                        goFindSibling = true;
                        prunedResults.Add(iParams.Current.Result);
                    }

                    if (goFindSibling)
                    {
                        // Try to find the nearest sibling.
                        bool found = TryToFindNearestSibling(iParams, pruneCondition, prunedResults);
                        if (!found) finish = true;
                    }
                }
                else
                {
                    // Breadth-first search.

                    // Loop until a next result is found or there is no more deep node.
                    while (true)
                    {
                        bool goFindSibling = true;
                        if (iParams.Current.Parent == null)
                        {
                            // For the root node.
                            if (TryToGoInDeep(iParams, currentDepth, prunedResults))
                            {
                                goFindSibling = false;
                            }
                        }

                        // Deciding whether to prune a subtree or not. 
                        if (!goFindSibling && pruneCondition(iParams.Current.Result, iParams.Depth, iParams.Current.IndexAsChild, iParams.Current.Parent))
                        {
                            goFindSibling = true;
                            prunedResults.Add(iParams.Current.Result);
                        }

                        if (goFindSibling)
                        {
                            bool found;
                            // Loop until a next result is found or it reaches the last node.
                            while (true)
                            {
                                // Try to find the nearest sibling.
                                found = TryToFindNearestSibling(iParams, pruneCondition, prunedResults);

                                if (found)
                                {
                                    // Go in deep until the current depth.
                                    while (iParams.Depth < currentDepth)
                                    {
                                        if (iParams.Current.Result.NextResults.Any())
                                        {
                                            iParams.Push(new ResultInfo(iParams.Current.Result.NextResults.First(), iParams.Current.Result, 0));

                                            if (prunedResults.Contains(iParams.Current.Result))
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            found = false;
                                            break;
                                        }
                                    }

                                    if (found) break;
                                }
                                else { break; }
                            }

                            if (found) break;

                            // If the next result hasn't been found, check if the max depth has changed from the previous state.
                            if (iParams.Depth.Max == prevMaxDepth)
                            {
                                finish = true;
                                break;
                            }
                            else
                            {
                                prevMaxDepth = iParams.Depth.Max;
                                // Search +1 depth next time.
                                currentDepth++;
                                //Reset the current node to the root node.
                                iParams.ResetStuck(firstResult);
                            }
                        }
                        else
                        {
                            // A result has been found, and we were not going to search siblings.
                            break;
                        }
                    }
                }

            }while (!finish);
        }

        /// <summary>
        /// <para>
        /// Try to find the nearest sibling.<br/>
        /// Note that the properties in the first parameter (iParams) and the third one (prunedResults) would be modified.<br/>
        /// </para>
        /// </summary>
        /// <returns>True if it found the nearest sibling.</returns>
        private static bool TryToFindNearestSibling(InspectionParams iParams, PruneConditionFunc pruneCondition, HashSet<Result> prunedResults)
        {
            /*
             * This method tries to find the nearest sibling.
             * The nearest sibling in this context means that finding a result in the order below.
             * 1. A next index in the list which the parent has.
             * 2. If you couldn't find it, move up to the parent and try 1. again. 
             * 
             * If a start position is at [0], then the order is shown in the figure below. 
             * 
             *           [x]
             *         ___|___
             *        |       |
             *      [x]      [3]
             *      _|_      _|_
             *     |   |    |   |
             *    [x] [2]  [x] [x]
             *    _|_
             *   |   |
             *  [0] [1]
             * 
             */

            // Try to find the nearest sibling.
            bool found = false;
            while (true)
            {
                while (true)
                {
                    if (iParams.Current.Parent == null) break;

                    int nextIndex = iParams.Current.IndexAsChild + 1;
                    if (nextIndex < iParams.Current.Parent.NextResults.Count())
                    {
                        // The nearest sibling is found.
                        found = true;
                        Result nextResult = iParams.Current.Parent.NextResults.ElementAt(nextIndex);
                        Result parent = iParams.Current.Parent;
                        iParams.Pop();
                        iParams.Push(new ResultInfo(nextResult, parent, nextIndex));
                        break;
                    }
                    else
                    {
                        // Move one level to the upper node.
                        iParams.Pop();
                    }
                }

                if (!found)
                {
                    break;
                }
                else
                {
                    if (pruneCondition(iParams.Current.Result, iParams.Depth, iParams.Current.IndexAsChild, iParams.Current.Parent))
                    {
                        // If the result is a pruning target, try again.
                        prunedResults.Add(iParams.Current.Result);
                        found = false;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// <para>
        /// Try to go in deep until specified depth.<br/>
        /// Note that the properties in the first parameter (iParams) would be modified.<br/>
        /// </para>
        /// </summary>
        /// <returns>True if it has succeeded to go in deep.</returns>
        private static bool TryToGoInDeep(InspectionParams iParams, ulong currentDepth, HashSet<Result> prunedResults)
        {
            bool found = true;

            // Go in deep until the current depth.
            while (iParams.Depth < currentDepth)
            {
                if (iParams.Current.Result.NextResults.Any())
                {
                    iParams.Push(new ResultInfo(iParams.Current.Result.NextResults.First(), iParams.Current.Result, 0));

                    if (prunedResults.Contains(iParams.Current.Result))
                    {
                        found = false;
                        break;
                    }
                }
                else
                {
                    found = false;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// A class holding parameters used by inspection logics. (e.g. depth, state, etc.)
        /// </summary>
        private class InspectionParams
        {
            public InspectionParams(Result startResult)
            {
                this.resultStack.Push(new ResultInfo(startResult));
                this.Depth = new DepthStruct(0);
            }

            public ResultInfo Current { get => resultStack.Peek(); }
            private Stack<ResultInfo> resultStack = new Stack<ResultInfo>();

            public DepthStruct Depth;

            /// <summary>
            /// Push a result info which is the next inspecting target to the stack.
            /// </summary>
            public void Push(ResultInfo resultInfo)
            {
                Depth++;
                resultStack.Push(resultInfo);
            }

            /// <summary>
            /// Pop a previous result info.
            /// </summary>
            public ResultInfo Pop()
            {
                Depth--;
                return resultStack.Pop();
            }

            /// <summary>
            /// Reset a stuck with a new result. This method doesn't affect a <see cref="DepthStruct.Max"/>.
            /// </summary>
            /// <param name="startResultInfo"></param>
            public void ResetStuck(Result startResult)
            {
                resultStack.Clear();
                resultStack.Push(new ResultInfo(startResult));
                Depth.ResetToZero();
            }

            /// <summary>
            /// Depth structure handling special operations.
            /// </summary>
            public struct DepthStruct
            {
                public DepthStruct(ulong depth)
                {
                    this.depth = depth;
                    this.Max = depth;
                }

                private DepthStruct(ulong depth, ulong maxDepth)
                {
                    this.depth = depth;
                    this.Max = maxDepth;
                }

                public ulong Value
                {
                    get { return depth; }
                }
                private ulong depth;
                public ulong Max { get; private set; }

                public static DepthStruct operator ++(DepthStruct me)
                {
                    if (me.depth == ulong.MaxValue) throw new OverflowException(I18nContext.Current.ExceptionMessages.DepthOverflow);

                    ulong newDepth = me.depth + 1;
                    ulong newMax = me.Max > newDepth ? me.Max : newDepth;

                    return new DepthStruct(newDepth, newMax);
                }

                public static DepthStruct operator --(DepthStruct me)
                {
                    if (me.depth == 0) throw new OverflowException(I18nContext.Current.ExceptionMessages.DepthOverflow);

                    ulong newDepth = me.depth - 1;
                    ulong newMax = me.Max;

                    return new DepthStruct(newDepth, newMax);
                }

                public static implicit operator ulong(DepthStruct me)
                {
                    return me.depth;
                }

                /// <summary>
                /// This method doesn't affect a <see cref="Max"/>.
                /// </summary>
                public void ResetToZero()
                {
                    depth = 0;
                }
            }
        }

        /// <summary>
        /// Infomation about a result which is being inspected.
        /// </summary>
        public class ResultInfo
        {
            public ResultInfo(Result result)
            {
                this.Result = result;
                this.Parent = null;
            }

            public ResultInfo(Result result, Result parent, int indexAsChild)
            {
                this.Result = result;
                this.Parent = parent;
                // The index can't be automatically determined, because the parent possibly has a multiple same child and that kind of graph is allowed.
                this.indexAsChild = indexAsChild;
            }

            public Result Result;
            public Result? Parent { get; set; }
            public int IndexAsChild
            {
                get
                {
                    if (Parent == null) return -1;
                    return indexAsChild;
                }
                set
                {
                    indexAsChild = value;
                }
            }
            private int indexAsChild = -1;
        }
    }
}
