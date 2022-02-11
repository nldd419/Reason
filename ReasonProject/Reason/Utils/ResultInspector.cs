using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigDAG;

using Reason.Results;
using Reason.I18n;

namespace Reason.Utils
{
    public static class ResultInspector
    {
        /// <summary>
        /// Traverse all the results in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectAll(Result firstResult, DagInspector.InspectAction inspector, bool depthFirstSearch = true)
        {
            DagInspector.InspectAll(firstResult, inspector, depthFirstSearch);
        }

        /// <summary>
        /// Traverse failed results in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectFailedOnly(Result firstResult, DagInspector.InspectAction inspector, bool depthFirstSearch = true)
        {
            DagInspector.InspectWhere(firstResult, inspector, (r, d, i, p) => ((Result)r).IsFailed(), (r, d, i, p) => false, depthFirstSearch);
        }

        /// <summary>
        /// Traverse succeeded results in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectSucceededOnly(Result firstResult, DagInspector.InspectAction inspector, bool depthFirstSearch = true)
        {
            DagInspector.InspectWhere(firstResult, inspector, (r, d, i, p) => !((Result)r).IsFailed(), (r, d, i, p) => false, depthFirstSearch);
        }

        /// <summary>
        /// Traverse results which meets a condition in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="condition">A condition of a result you want to look into.</param>
        /// <param name="pruneCondition">A pruning condition of a result subtree.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectWhere(Result firstResult, DagInspector.InspectAction inspector, DagInspector.ConditionFunc condition, DagInspector.PruneConditionFunc pruneCondition, bool depthFirstSearch = true)
        {
            DagInspector.InspectWhere(firstResult, inspector, condition, pruneCondition, depthFirstSearch);
        }

        /// <summary>
        /// Traverse results which meets a condition in the tree.
        /// </summary>
        /// <param name="firstResult">A start point you want to look into.</param>
        /// <param name="inspector">An action applys to a result.</param>
        /// <param name="condition">A condition of a result you want to look into.</param>
        /// <param name="depthFirstSearch">True if you use a depth-first search method for tree traversing, otherwise a breadth-first search method is used.</param>
        public static void InspectWhere(Result firstResult, DagInspector.InspectAction inspector, DagInspector.ConditionFunc condition, bool depthFirstSearch = true)
        {
            DagInspector.InspectWhere(firstResult, inspector, condition, (r, d, i, p) => false, depthFirstSearch);
        }

    }
}
