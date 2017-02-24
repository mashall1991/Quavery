using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Framework.Unity.AI.AStarAlgorithm
{
    /// <summary>
    /// 用于存放参与A*计算的所有点
    /// 格子的模式
    /// </summary>
    public class AStarGrid
    {
        /// <summary>
        /// 运动的起始点
        /// </summary>
        public AStarGridNode StartNode { get; private set; }

        /// <summary>
        /// 运动的结束点
        /// </summary>
        public AStarGridNode EndNode { get; private set; }

        /// <summary>
        /// 网格的行数
        /// </summary>
        public int NumRows { get; private set; }

        /// <summary>
        /// 网格的列数
        /// </summary>
        public int NumCols { get; private set; }
        
        /// <summary>
        /// 参与A*计算的节点集合
        /// </summary>
        private ArrayList mNodes;

        // Methods
        public AStarGridNode GetNodeAtPosition(int x, int y)
        {
            if (x < 0)
            {
                x = 0;
            }
            else if (this.mNodes.Count <= x)
            {
                x = this.mNodes.Count - 1;
            }

            ArrayList list = (ArrayList)this.mNodes[x];
            if (y < 0)
            {
                y = 0;
            }
            else if (list.Count <= y)
            {
                y = list.Count - 1;
            }

            return (AStarGridNode)list[y];
        }

        public void Init(ArrayList myArray)
        {
            mNodes = myArray;
            NumCols = myArray.Count;
            NumRows = ((ArrayList)myArray[0]).Count;
        }

        public void Init(int cols, int rows)
        {
            mNodes = new ArrayList();

            NumCols = cols;
            NumRows = rows;

            for (int i = 0; i < NumCols; i++)
            {
                ArrayList list = new ArrayList();
                for (int j = 0; j < NumRows; j++)
                {
                    AStarGridNode node = new AStarGridNode(i, j, true);
                    list.Add(node);
                }

                mNodes.Add(list);
            }
        }

        public void SetEndNodePosition(int x, int y)
        {
            EndNode = GetNodeAtPosition(x, y);
        }

        public void SetStartNodePosition(int x, int y)
        {
            StartNode = GetNodeAtPosition(x, y);
        }
    }
}
