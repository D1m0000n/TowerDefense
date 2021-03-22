using System;
using System.Collections.Generic;
using Field;
using UnityEngine;

namespace Homework.HW3
{
    public class FlowFieldPathfinding
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;

        public FlowFieldPathfinding(Grid grid, Vector2Int target, Vector2Int start)
        {
            m_Grid = grid;
            m_Target = target;
            m_Start = start;
        }

        public void UpdateField()
        {
            foreach (Node node in m_Grid.EnumerateNodes())
            {
                node.ResetWeight();
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);
            m_Grid.GetNode(m_Target).PathWeight = 0f;


            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    float weightToTarget = m_Grid.GetNode(current).PathWeight + neighbour.Weight;
                    Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour.Coordinate);
                    }
                }
            }
        }

        public void SetAllNodeStatus()
        {
            for (int i = 0; i < m_Grid.Height; i++)
            {
                for (int j = 0; j < m_Grid.Width; j++)
                {
                    m_Grid.GetNode(i, j).m_OccupationAvailability = OccupationAvailability.CanOccupy;
                }
            }

            Node startNode = m_Grid.GetNode(m_Start);
            Node targetNode = m_Grid.GetNode(m_Target);
            startNode.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
            targetNode.m_OccupationAvailability = OccupationAvailability.CanNotOccupy;

            Node currentNode = startNode.NextNode;
            while (currentNode != targetNode)
            {
                currentNode.m_OccupationAvailability = OccupationAvailability.Undefined;
                currentNode = currentNode.NextNode;
            }
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;

            Vector2Int leftDownCoordinate = coordinate + Vector2Int.down + Vector2Int.left;
            Vector2Int leftUpCoordinate = coordinate + Vector2Int.up + Vector2Int.left;
            Vector2Int rightDownCoordinate = coordinate + Vector2Int.down + Vector2Int.right;
            Vector2Int rightUpCoordinate = coordinate + Vector2Int.up + Vector2Int.right;

            bool isLeftNodeOccupied = leftCoordinate.x >=0 && m_Grid.GetNode(leftCoordinate).IsOccupied;
            bool isRightNodeOccupied = rightCoordinate.x < m_Grid.Width && m_Grid.GetNode(rightCoordinate).IsOccupied;
            bool isUpNodeOccupied = upCoordinate.y < m_Grid.Height && m_Grid.GetNode(upCoordinate).IsOccupied;
            bool isDownNodeOccupied = downCoordinate.y >= 0 && m_Grid.GetNode(downCoordinate).IsOccupied;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !isRightNodeOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !isLeftNodeOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !isUpNodeOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !isDownNodeOccupied;

            bool hasLeftDownNode = leftDownCoordinate.x >= 0 && leftDownCoordinate.y >= 0 &&
                                   !m_Grid.GetNode(leftDownCoordinate).IsOccupied && !isLeftNodeOccupied &&
                                   !isDownNodeOccupied;
            bool hasLeftUpNode = leftUpCoordinate.x >= 0 && leftUpCoordinate.y < m_Grid.Height &&
                                   !m_Grid.GetNode(leftUpCoordinate).IsOccupied && !isLeftNodeOccupied &&
                                   !isUpNodeOccupied;
            bool hasRightDownNode = rightDownCoordinate.x < m_Grid.Width && rightDownCoordinate.y >= 0 &&
                                   !m_Grid.GetNode(rightDownCoordinate).IsOccupied && !isRightNodeOccupied &&
                                   !isDownNodeOccupied;
            bool hasRightUpNode = rightUpCoordinate.x < m_Grid.Width && rightUpCoordinate.y < m_Grid.Width &&
                                    !m_Grid.GetNode(rightUpCoordinate).IsOccupied && !isRightNodeOccupied &&
                                    !isUpNodeOccupied;

            if (hasRightNode)
            {
                yield return new Connection(rightCoordinate, 1f);
            }

            if (hasLeftNode)
            {
                yield return new Connection(leftCoordinate, 1f);
            }

            if (hasUpNode)
            {
                yield return new Connection(upCoordinate, 1f);
            }

            if (hasDownNode)
            {
                yield return new Connection(downCoordinate, 1f);
            }

            if (hasLeftDownNode)
            {
                yield return new Connection(leftDownCoordinate, (float)Math.Sqrt(2f));
            }

            if (hasLeftUpNode)
            {
                yield return new Connection(leftUpCoordinate, (float) Math.Sqrt(2f));
            }

            if (hasRightDownNode)
            {
                yield return new Connection(rightDownCoordinate, (float) Math.Sqrt(2f));
            }

            if (hasRightUpNode)
            {
                yield return new Connection(rightUpCoordinate, (float) Math.Sqrt(2f));
            }
        }

        bool CheckTargetAccessibility(Node restrictedNode)
        {
            bool[,] used = new bool[m_Grid.Width, m_Grid.Height];
            for (int i = 0; i < m_Grid.Width; i++)
            {
                for (int j = 0; j < m_Grid.Height; j++)
                {
                    used[i, j] = false;
                }
            }
            
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);

            Node startNode = m_Grid.GetNode(m_Start);
            
            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                if (used[current.x, current.y])
                {
                    continue;
                }
                used[current.x, current.y] = true;

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);
                    if (neighbourNode == restrictedNode)
                    {
                        continue;
                    }
                    
                    if (neighbourNode == startNode)
                    {
                        return true;
                    }

                    if (!used[neighbour.Coordinate.x, neighbour.Coordinate.y] && !neighbourNode.IsOccupied)
                    {
                        queue.Enqueue(neighbour.Coordinate);
                    }
                }
            }

            return used[m_Start.x, m_Start.y];
        }
        public bool CanOccupy(Vector2Int coordinate)
        {
            Node node = m_Grid.GetNode(coordinate);
            if (node.m_OccupationAvailability == OccupationAvailability.CanOccupy)
            {
                return true;
            }

            if (node.m_OccupationAvailability == OccupationAvailability.CanNotOccupy)
            {
                return false;
            }

            return CheckTargetAccessibility(m_Grid.GetNode(coordinate));
        }
    }
}