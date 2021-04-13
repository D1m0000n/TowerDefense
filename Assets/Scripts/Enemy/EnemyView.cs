using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        
        private EnemyData m_Data;
        private IMovementAgent m_MovementAgent;
        private static readonly int Death = Animator.StringToHash("Death");

        public EnemyData Data => m_Data;
        public IMovementAgent MovementAgent => m_MovementAgent;

        public void AttachData(EnemyData data)
        {
            m_Data = data;
        }

        public void CreateMovementAgent(Grid grid)
        {
            if (m_Data.Asset.IsFlyingEnemy)
            {
                m_MovementAgent = new FlyingMovementAgent(m_Data.Asset.Speed, transform, grid, m_Data);
            }
            else
            {
                m_MovementAgent = new GridMovementAgent(m_Data.Asset.Speed, transform, grid, m_Data);
            }
        }

        public void AnimateDeath()
        {
            m_Animator.SetTrigger(Death);
        }
    }
}