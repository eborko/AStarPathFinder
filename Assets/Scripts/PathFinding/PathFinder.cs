using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Assets.Scripts.PathFinding
{
    public class PathFinder
    {
        private IList<Node> _closedList = new List<Node>();
        private IList<Node> _openList = new List<Node>();
        public IList<Node> Path { get; private set; }
        private bool _isFrstCall = true;

        public void CalculatePath(Node start, Node destination)
        {
            if (_isFrstCall)
            {
                ResetState();

                _openList.Add(start);

                _isFrstCall = false;
            }
            
            // Just to make it meaningful I give a new name
            Node currentNode = start;

            #region Adding parent to child nodes
            if (currentNode.LeftNode != null &&
                !_closedList.Contains(currentNode.LeftNode) &&
                currentNode.LeftNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.LeftNode, currentNode, destination, false);
            }

            if (currentNode.TopNode != null &&
                !_closedList.Contains(currentNode.TopNode) &&
                currentNode.TopNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.TopNode, currentNode, destination, false);
            }

            if (currentNode.RightNode != null &&
                !_closedList.Contains(currentNode.RightNode) &&
                currentNode.RightNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.RightNode, currentNode, destination, false);
            }

            if (currentNode.BottomNode != null &&
                !_closedList.Contains(currentNode.BottomNode) &&
                currentNode.BottomNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.BottomNode, currentNode, destination, false);
            }

            if (currentNode.TLNode != null &&
                currentNode.TopNode != null &&
                currentNode.LeftNode != null &&
                !_closedList.Contains(currentNode.TLNode) &&
                currentNode.LeftNode.Cell.IsWalkable &&
                currentNode.TopNode.Cell.IsWalkable &&
                currentNode.TLNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.TLNode, currentNode, destination, true);
            }

            if (currentNode.TRNode != null &&
                currentNode.TopNode != null &&
                currentNode.RightNode != null &&
                !_closedList.Contains(currentNode.TRNode) &&
                currentNode.RightNode.Cell.IsWalkable &&
                currentNode.TopNode.Cell.IsWalkable &&
                currentNode.TRNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.TRNode, currentNode, destination, true);
            }

            if (currentNode.BRNode != null &&
                currentNode.BottomNode != null &&
                currentNode.RightNode != null &&
                !_closedList.Contains(currentNode.BRNode) &&
                currentNode.RightNode.Cell.IsWalkable &&
                currentNode.BottomNode.Cell.IsWalkable &&
                currentNode.BRNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.BRNode, currentNode, destination, true);
            }

            if (currentNode.BLNode != null &&
                currentNode.BottomNode != null &&
                currentNode.LeftNode != null &&
                !_closedList.Contains(currentNode.BLNode) &&
                currentNode.LeftNode.Cell.IsWalkable &&
                currentNode.BottomNode.Cell.IsWalkable &&
                currentNode.BLNode.Cell.IsWalkable)
            {
                ApplyChildRules(currentNode.BLNode, currentNode, destination, true);
            }
            #endregion

            #region Find Node with lowest TotalScore
            // set minimum score to have maximum integer value
            int minimumScore = int.MaxValue;

            Node NodeWithLowestScore = null;
            foreach (Node node in _openList)
            {
                if (node.TotalCost < minimumScore)
                {
                    minimumScore = node.TotalCost;
                    NodeWithLowestScore = node;
                }
            }
            #endregion

            _openList.Remove(NodeWithLowestScore);
            _closedList.Add(NodeWithLowestScore);

            if (_closedList.Contains(destination) || _openList.Count == 0)
            {
                // If open list is not empty, we found path. Otherwise not.
                if (_openList.Count != 0)
                {
                    CreatePath(destination);
                }

                _isFrstCall = true;
            }
            else
            {
                CalculatePath(NodeWithLowestScore, destination);
            }
        }

        private void ResetState()
        {
            Path = new List<Node>();
            _openList = new List<Node>();
            _closedList = new List<Node>();

            for (int i = 0; i < PlayGround.Grid.XSize; i++)
            {
                for (int j = 0; j < PlayGround.Grid.YSize; j++)
                {
                    PlayGround.Grid[i, j].Parent = null;
                    PlayGround.Grid[i, j].ResetCosts();
                }
            }

            _isFrstCall = true;
        }

        /// <summary>
        /// Creates Path
        /// </summary>
        /// <param name="destination">Destination node.</param>
        private void CreatePath(Node destination)
        {
            Path = new List<Node>();
            Node currentNode = destination;
            Path.Add(currentNode);
            while (currentNode.Parent != null)
            {
                Path.Add(currentNode.Parent);
                currentNode = currentNode.Parent;
            }
        }

        /// <summary>
        /// Call for each child node
        /// </summary>
        /// <param name="child">A node to apply rules for</param>
        /// <param name="maybeParent">Node that pretend to become parent of child node.</param>
        /// <param name="destination">Destination node</param>
        /// <param name="isDiagonalChild">Descriptive position based on parent node</param>
        private void ApplyChildRules(Node child, Node maybeParent, Node destination, bool isDiagonalChild)
        {
            // Check if is a node alredy in the open list
            // If true, compare owned GCost by current GCost
            if (_openList.Contains(child))
            {
                int cost = (isDiagonalChild) ? 14 : 10;

                // If current cost is less than actual, change parent.
                if (child.GCost > maybeParent.GCost + cost)
                {
                    SetParentAndUpdateCost(child, maybeParent, destination, isDiagonalChild);
                }
            }
            else
            {
                SetParentAndUpdateCost(child, maybeParent, destination, isDiagonalChild);
                _openList.Add(child);
            }
        }

        /// <summary>
        /// Call when you need to change parent node
        /// </summary>
        /// <param name="child">Node to update</param>
        /// <param name="parent">Parent node</param>
        /// <param name="targetNode">Destination node</param>
        /// <param name="isDiagonalChild">Descriptive position based on parent node</param>
        private void SetParentAndUpdateCost(Node child, Node parent, Node targetNode, bool isDiagonalChild)
        {
            child.Parent = parent;
            child.UpdateTotalCost(targetNode, isDiagonalChild);
        }
    }
}
