using Field;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        private EnemyData m_Data;

        public FlyingMovementAgent(float speed, Transform transform, Grid grid, EnemyData data)
        {
            m_Speed = speed;
            m_Transform = transform;
            m_Data = data;
            
            Node node = Game.Player.Grid.GetNodeAtPoint(transform.position);
            node.EnemyDatas.Add(data);
            
            SetTargetNode(grid.GetTargetNode());
        }
        
        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            Vector3 direction = target - m_Transform.position;
            direction.y = 0;

            if (direction.magnitude < TOLERANCE)
            {
                m_TargetNode = null;
                return;
            }

            Vector3 dir = direction.normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}