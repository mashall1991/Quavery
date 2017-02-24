
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Framework.Unity.AI.AStarAlgorithm
{
    public class AstarDebugBehaviour : MonoBehaviour
    {
        public AStar astar { get; set; }

        public float centerSize = 1f;
        public bool drawNodesCenter;
        public bool drawPathNodesCenter;

        // Methods
        private void OnDrawGizmos()
        {
            if (astar != null)
            {
                AStarGrid grid = astar.Grid;
                for (int i = 0; i < grid.NumCols; i++)
                {
                    for (int j = 0; j < grid.NumRows; j++)
                    {
                        AStarGridNode nodeAtPosition = grid.GetNodeAtPosition(i, j);
                        if (drawNodesCenter)
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawWireSphere((Vector3)nodeAtPosition.Position, centerSize);
                        }
                    }
                }

                ArrayList path = astar.Path;
                if (path != null)
                {
                    for (int k = 0; k < path.Count; k++)
                    {
                        AStarGridNode node2 = path[k] as AStarGridNode;
                        if ((node2 != null) && drawPathNodesCenter)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawWireSphere((Vector3)node2.Position, centerSize);
                        }
                    }
                }
            }
        }
    }
}
