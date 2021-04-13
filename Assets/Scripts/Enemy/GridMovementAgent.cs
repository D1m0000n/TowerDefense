using Field;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class GridMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        private EnemyData m_Data;
        private Node prevNode = null;

        public GridMovementAgent(float speed, Transform transform, Grid grid, EnemyData data)
        {
            m_Speed = speed;
            m_Transform = transform;
            m_Data = data;

            Node node = Game.Player.Grid.GetNodeAtPoint(transform.position);
            node.EnemyDatas.Add(data);
            
            SetTargetNode(grid.GetStartNode());
        }
        
        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;

        public void Die()
        {
            Node node = Game.Player.Grid.GetNodeAtPoint(m_Transform.position);
            node.EnemyDatas.Remove(m_Data);
        }

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            
            float distance = (target - m_Transform.position).magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            if (prevNode == null)
            {
                prevNode = Game.Player.Grid.GetNodeAtPoint(m_Transform.position);
            }
            Vector3 dir = (target - m_Transform.position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
            Node curNode = Game.Player.Grid.GetNodeAtPoint(m_Transform.position);

            if (prevNode != curNode)
            {
                prevNode.EnemyDatas.Remove(m_Data);
                curNode.EnemyDatas.Add(m_Data);
            }

            prevNode = curNode;
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}