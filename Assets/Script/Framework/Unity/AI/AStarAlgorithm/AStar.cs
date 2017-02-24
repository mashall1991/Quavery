using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Framework.Unity.AI.AStarAlgorithm
{
    /// <summary>
    /// 启发类型
    /// </summary>
    public enum Heuristic
    {
        /// <summary>
        /// 对角
        /// </summary>
        DIAGONAL,

        /// <summary>
        /// 曼哈顿
        /// </summary>
        MANHATTAN,

        /// <summary>
        /// 欧几里得
        /// </summary>
        EUCLIDIAN
    }

    /// <summary>
    /// 实现A*算法
    /// </summary>
    public class AStar
    {
        /// <summary>
        /// A*网格信息
        /// </summary>
        public AStarGrid Grid { get; private set; }

        /// <summary>
        /// 通过A*搜索找出的路径信息
        /// </summary>
        public ArrayList Path { get; private set; }

        /// <summary>
        /// open表
        /// 记住下一步还可以走哪些点
        /// </summary>
        private ArrayList mOpenTable;

        /// <summary>
        /// close表
        /// 记住哪些点已经走过了
        /// </summary>
        private ArrayList mClosedTable;

        /// <summary>
        /// 启发函数类型
        /// </summary>
        private Heuristic mHeuristicType;

        /// <summary>
        /// 对角线行进的成本
        /// </summary>
        private double mDiagonalCost;

        /// <summary>
        /// 直线行进的成本，路径图是以格子的方式实现的
        /// 要可以使用有向图的方式来实现
        /// </summary>
        private double mStraightCost;

        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="g">进行A*计算的网格信息</param>
        public void InitWithGrid(AStarGrid g)
        {
            Grid = g;

            // 就是根号2的值
            mDiagonalCost = 1.414;
            // 直线行进的成本，表示一个格子的成本为1
            mStraightCost = 1.0;

            // 启发函数类型
            mHeuristicType = Heuristic.MANHATTAN;

            mOpenTable = new ArrayList();
            mClosedTable = new ArrayList();
            Path = new ArrayList();
        }

        /// <summary>
        /// 对角线估价函数
        /// h(n) = (D2 * h_diagonal(n)) + D * (h_straight(n) - 2 * h_diagonal(n)))
        /// h_diagonal(n) = min(abs(n.x - goal.x), abs(n.y - goal.y))
        /// h_straight(n) = (abs(n.x - goal.x) + abs(n.y - goal.y))
        /// </summary>
        /// <param name="node">起始节点</param>
        /// <returns>对角线距离</returns>
        private double Diagonal(AStarGridNode node)
        {
            double distance_x = Math.Abs((float)(node.Position.x - Grid.EndNode.Position.x));
            double distance_y = Math.Abs((float)(node.Position.y - Grid.EndNode.Position.y));
            double h_diagonal = Math.Min(distance_x, distance_y);
            double h_straight = distance_x + distance_y;
            return ((mDiagonalCost * h_diagonal) + (mStraightCost * (h_straight - (2.0 * h_diagonal))));
        }

        /// <summary>
        /// 欧几里得估价函数
        /// h(n) = D * sqrt((n.x - goal.x)^2 + (n.y - goal.y)^2)
        /// </summary>
        /// <param name="node">起始节点</param>
        /// <returns>欧几里得距离</returns>
        private double Euclidian(AStarGridNode node)
        {
            double distance_x = node.Position.x - Grid.EndNode.Position.x;
            double distance_y = node.Position.y - Grid.EndNode.Position.y;
            return Math.Sqrt((distance_x * distance_x) + (distance_y * distance_y));
        }

        /// <summary>
        /// 曼哈顿估价函数
        /// H(n) = D * (abs(n.x – goal.x) + abs(n.y – goal.y))
        /// </summary>
        /// <param name="node">起始节点</param>
        /// <returns>曼哈顿距离</returns>
        private double Manhattan(AStarGridNode node)
        {
            return (mStraightCost * (Math.Abs((float)(node.Position.x - Grid.EndNode.Position.x)) + Math.Abs((float)(node.Position.y - Grid.EndNode.Position.y))));
        }

        public bool FindPath()
        {
            if ((Grid.StartNode == null) || (Grid.EndNode == null))
            {
                return false;
            }

            // 如果某点不可以走的话，直接返回
            if (!Grid.StartNode.Walkable || !Grid.EndNode.Walkable)
            {
                return false;
            }

            mOpenTable.Clear();
            mClosedTable.Clear();
            Path.Clear();

            // 初始化起始节点
            Grid.StartNode.G = 0.0;
            Grid.StartNode.H = GetHeruistic(Grid.StartNode);
            Grid.StartNode.F = Grid.StartNode.G + Grid.StartNode.H;
            return Search();
        }

        /// <summary>
        /// 估价函数H的计算方法
        /// </summary>
        /// <param name="node">A*节点</param>
        /// <returns>代价值</returns>
        private double GetHeruistic(AStarGridNode node)
        {
            switch (mHeuristicType)
            {
                case Heuristic.DIAGONAL:
                    return Diagonal(node);

                case Heuristic.MANHATTAN:
                    return Manhattan(node);

                case Heuristic.EUCLIDIAN:
                    return Euclidian(node);
            }

            // 默认为对角线
            return Diagonal(node);
        }

        private bool IsClosed(AStarGridNode node)
        {
            IEnumerator enumerator = this.mClosedTable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AStarGridNode current = (AStarGridNode)enumerator.Current;
                    if (current.Equals(node))
                    {
                        return true;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return false;
        }

        private bool IsOpen(AStarGridNode node)
        {
            IEnumerator enumerator = mOpenTable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AStarGridNode current = (AStarGridNode)enumerator.Current;
                    if (current.Equals(node))
                    {
                        return true;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return false;
        }

        private bool Search()
        {
            AStarGridNode startNode = Grid.StartNode;

            // 还没有遍历到最后一个节点
            while (false == startNode.Equals(Grid.EndNode))
            {
                int num = (int)Math.Max((float)0f, (float)(startNode.Position.x - 1f));
                int num2 = (int)Math.Min((float)(Grid.NumCols - 1), startNode.Position.x + 1f);
                int num3 = (int)Math.Max((float)0f, (float)(startNode.Position.y - 1f));
                int num4 = (int)Math.Min((float)(Grid.NumRows - 1), startNode.Position.y + 1f);

                for (int i = num; i <= num2; i++)
                {
                    for (int j = num3; j <= num4; j++)
                    {
                        AStarGridNode nodeAtPosition = Grid.GetNodeAtPosition(i, j);

                        // 剔除重复的节点
                        if (false == nodeAtPosition.Equals(startNode) && nodeAtPosition.Walkable)
                        {
                            double straightCost = mStraightCost;
                            if ((startNode.Position.x != nodeAtPosition.Position.x) && (startNode.Position.y != nodeAtPosition.Position.y))
                            {
                                straightCost = mDiagonalCost;
                            }

                            double num8 = startNode.G + (straightCost * nodeAtPosition.Cost);
                            double heruistic = GetHeruistic(nodeAtPosition);
                            double num10 = num8 + heruistic;

                            // 已经在open或者close表中
                            if (IsOpen(nodeAtPosition) || IsClosed(nodeAtPosition))
                            {
                                if (nodeAtPosition.F > num10)
                                {
                                    nodeAtPosition.F = num10;
                                    nodeAtPosition.G = num8;
                                    nodeAtPosition.H = heruistic;
                                    nodeAtPosition.ParentNode = startNode;
                                }
                            }
                            // 不在这些表中
                            else
                            {
                                nodeAtPosition.F = num10;
                                nodeAtPosition.G = num8;
                                nodeAtPosition.H = heruistic;
                                nodeAtPosition.ParentNode = startNode;

                                mOpenTable.Add(nodeAtPosition);
                            }
                        }
                    }
                }

                // 已经走过该节点
                mClosedTable.Add(startNode);
                // 检查是否没有需要走的节点了
                if (mOpenTable.Count == 0)
                {
                    return false;
                }

                // 排序open表提高效率
                mOpenTable.Sort(new AStarComparer());

                // 获取第一个节点然后从open表中删除
                startNode = (AStarGridNode)mOpenTable[0];
                mOpenTable.RemoveAt(0);
            }

            BuildPath();
            return true;
        }

        /// <summary>
        /// 构建路径信息
        /// </summary>
        private void BuildPath()
        {
            Path.Clear();

            for (AStarGridNode node = Grid.EndNode; false == node.Equals(Grid.StartNode); node = node.ParentNode)
            {
                Path.Add(node);
            }
        }

        // Nested Types
        public class AStarComparer : IComparer
        {
            // Methods
            int IComparer.Compare(object x, object y)
            {
                if (((AStarGridNode)x).F > ((AStarGridNode)y).F)
                {
                    return 1;
                }

                if (((AStarGridNode)x).F < ((AStarGridNode)y).F)
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}
