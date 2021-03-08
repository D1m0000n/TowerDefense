using System.Collections.Generic;
using Homework.HW3;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private FlowFieldPathfinding m_Pathfinding;
        
        public int Width => m_Width;

        public int Height => m_Height;


        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int target, Vector2Int start)
        {
            m_Width = width;
            m_Height = height;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                }
            }

            m_Pathfinding = new FlowFieldPathfinding(this, target, start);
            
            m_Pathfinding.UpdateField();
            m_Pathfinding.SetAllNodeStatus();
        }

        public Node GetNode(Vector2Int coonrdinate)
        {
            return GetNode(coonrdinate.x, coonrdinate.y);
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

        public IEnumerable<Node> EnumerateNodes()
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
            m_Pathfinding.SetAllNodeStatus();
        }

        public void TryOccupyNode(Vector2Int coordinate, bool occupy)
        {
            Node node = GetNode(coordinate);
            if (!occupy)
            {
                node.SetOccupationAvailability(OccupationAvailability.Undefined);
                node.IsOccupied = false;
                return;
            }
            bool canOccupy = m_Pathfinding.CanOccupy(coordinate);
            if (canOccupy)
            {
                node.SetOccupationAvailability(OccupationAvailability.CanNotOccupy);
                node.IsOccupied = true;
                return;
            }
            Debug.Log(coordinate + " can not be occupied");
        }
    }
}