
using UnityEngine;

namespace Assets.Scripts.Framework.Unity.AI.AStarAlgorithm
{
    /// <summary>
    /// 代表一个节点
    /// </summary>
    public class AStarGridNode
    {
        /// <summary>
        /// 代价值
        /// </summary>
        public double Cost { get; private set; }

        /// <summary>
        /// 估价函数
        /// F(X) = G(X) + H(X)
        /// 是从初始点经由节点n到目标点的估价函数
        /// </summary>
        public double F { get; set; }

        /// <summary>
        /// 从起始点到此位置的代价
        /// A*要求G(x)必须大于0，也就是说必须考虑此代价
        /// </summary>
        public double G { get; set; }

        /// <summary>
        /// 它就是启发函数，从此位置到目标位置的代价
        /// 在估计的时候必能大于它的实际代码
        /// </summary>
        public double H { get; set; }

        /// <summary>
        /// 用于记住返回路径，这个记住每一个点的父节点就好了
        /// </summary>
        public AStarGridNode ParentNode { get; set; }

        /// <summary>
        /// 该节点的位置信息
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// 此点有障碍物，不可走
        /// </summary>
        public bool Walkable { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">二维数组横向下标</param>
        /// <param name="y">二维数组纵向下标</param>
        /// <param name="isWalkable">是否可以行走，例如墙壁就不能行走</param>
        public AStarGridNode(int x, int y, bool isWalkable = true)
        {
            Position = new Vector2((float)x, (float)y);
            Walkable = isWalkable;
            Cost = 1.0;
        }

        /// <summary>
        /// 换算出地图上的真实坐标
        /// </summary>
        /// <returns>坐标值</returns>
        public Vector2 GetNodeRealPosition()
        {
            return new Vector2(-960f + (10f + (20f * Position.x)), 540f - (10f + (20f * Position.y)));
        }
    }
}
