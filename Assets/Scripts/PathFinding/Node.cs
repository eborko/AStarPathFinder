using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PathFinding
{
    public class Node
    {
        public Cell Cell { get; set; }

        public Node Parent { get; set; }
        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int TotalCost { get; private set; }

        public Node LeftNode { get; set; }
        public Node TopNode { get; set; }
        public Node RightNode { get; set; }
        public Node BottomNode { get; set; }
        public Node TLNode { get; set; }
        public Node TRNode { get; set; }
        public Node BRNode { get; set; }
        public Node BLNode { get; set; }

        public Node(Cell cell)
        {
            Cell = cell;
            Cell.Owner = this;
        }

        public void UpdateTotalCost(Node target, bool isDiagonalPath)
        {
            #region Calculating G Cost
            if (isDiagonalPath)
            {
                GCost = Parent.GCost + 14;
            }
            else
            {
                GCost = Parent.GCost + 10;
            }
            #endregion

            #region Calculating Heuretic Cost
            int xDistanceFromTarget = Mathf.Abs(Cell.XPosition - target.Cell.XPosition);
            int yDistanceFromTarget = Mathf.Abs(Cell.YPosition - target.Cell.YPosition);

            HCost = (xDistanceFromTarget + yDistanceFromTarget) * 10;
            #endregion

            TotalCost = HCost + GCost;
        }

        internal void ResetCosts()
        {
            GCost= 0;
            HCost= 0;
            TotalCost = 0;
        }
    }
}

