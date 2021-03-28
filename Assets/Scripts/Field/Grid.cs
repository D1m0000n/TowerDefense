using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private float m_NodeSize;

        private Vector3 m_Offset;

        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;

        private Node m_SelectedNode = null;

        private FlowFieldPathfinding m_Pathfinding;

        public int Width => m_Width;
        public int Height => m_Height;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int start, Vector2Int target)
        {
            m_Width = width;
            m_Height = height;

            m_NodeSize = nodeSize;
            
            m_Offset = offset;

            m_StartCoordinate = start;
            m_TargetCoordinate = target;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                }
            }

            m_Pathfinding = new FlowFieldPathfinding(this, target);

            m_Pathfinding.UpdateField();
        }

        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }

        public void SelectCoordinate(Vector2Int coordinate)
        {
            m_SelectedNode = GetNode(coordinate);
        }

        public void UnselectNode()
        {
            m_SelectedNode = null;
        }

        public bool HasSelectedNode()
        {
            return m_SelectedNode != null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }

        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }

        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }

            return m_Nodes[i, j];
        }

        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; i++)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    yield return GetNode(i, j);
                }
            }
        }

        public void UpdatePathfinding()
        {
            m_Pathfinding.UpdateField();
        }

        public Node GetNodeAtPoint(Vector3 point)
        {
            // округление вниз нас как раз устраивает
            int xNodePos = (int) ((point.x - m_Offset.x) / m_NodeSize);
            int yNodePos = (int) ((point.z - m_Offset.z) / m_NodeSize);

            return GetNode(xNodePos, yNodePos);
        }

        public List<Node> GetNodesInCircle(Vector3 point, float radius)
        {
            List<Node> nodesInRange = new List<Node>();

            float sqrRadius = radius * radius;

            for (int i = 0; i < m_Nodes.GetLength(0); ++i)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); ++j)
                {
                    Node currentNode = m_Nodes[i, j];
                    if (IsNodeInRange(currentNode, sqrRadius, new Vector2(point.x, point.z)))
                    {
                        nodesInRange.Add(currentNode);
                    }
                }
            }
            
            
            return nodesInRange;
        }

        private bool IsNodeInRange(Node node, float sqrRadius, Vector2 point)
        {
            Vector2 closestCorner = GetClosestNodeCornerToPoint(node, point);
            return (closestCorner - point).sqrMagnitude < sqrRadius;
        }

        private Vector2 GetClosestNodeCornerToPoint(Node node, Vector2 point)
        {
            float minDistance = float.MaxValue;
            Vector2 closestCorener = new Vector2();
            Vector2 nodeCenter = new Vector2(node.Position.x, node.Position.z);

            Vector2 leftUpCorner = new Vector2(-m_NodeSize / 2, m_NodeSize / 2) + nodeCenter;
            Vector2 rightUpCorner = new Vector2(m_NodeSize / 2, m_NodeSize / 2) + nodeCenter;
            Vector2 leftDownCorner = new Vector2(-m_NodeSize / 2, -m_NodeSize / 2) + nodeCenter;
            Vector2 rightDownCorner = new Vector2(m_NodeSize / 2, -m_NodeSize / 2) + nodeCenter;

            if (minDistance < (leftUpCorner - point).sqrMagnitude) closestCorener = leftUpCorner;
            if (minDistance < (rightUpCorner - point).sqrMagnitude) closestCorener = rightUpCorner;
            if (minDistance < (leftDownCorner - point).sqrMagnitude) closestCorener = leftDownCorner;
            if (minDistance < (rightDownCorner - point).sqrMagnitude) closestCorener = rightDownCorner;

            return closestCorener;
        }
    }
}