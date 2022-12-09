using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PathFinding
{
    public class Grid
    {
        private Node[,] nodes;
        public Node this[int x, int y]
        {
            get
            {
                return nodes[x, y];
            }
            set 
            { 
                nodes[x, y] = value;
            }
        }

        public readonly int XSize;
        public readonly int YSize;

        public Grid(int xSize, int ySize)
        {
            XSize = xSize;
            YSize = ySize;

            nodes = new Node[XSize, YSize];
        }
    }
}
