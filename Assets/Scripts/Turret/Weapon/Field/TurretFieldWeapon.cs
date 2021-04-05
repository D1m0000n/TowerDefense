using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using Turret.Weapon.Lazer;
using UnityEngine;

namespace Turret.Weapon.Field
{
    public class TurretFieldWeapon : ITurretWeapon
    {
        private TurretView m_View;
        private TurretFieldWeaponAsset m_Asset;

        private float m_MaxDistance;
        private float m_Damage = 8f;
        private GameObject m_FieldSphere;

        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        private List<Node> m_NodesInRange;
        
        public TurretFieldWeapon(TurretFieldWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_MaxDistance = m_Asset.MaxDistance;
            m_NodesInRange =
                Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
            m_FieldSphere = m_Asset.FieldSphere;
            Vector3 scale = new Vector3(m_MaxDistance, m_MaxDistance, m_MaxDistance);
            m_FieldSphere.transform.localScale = scale;
            m_FieldSphere.transform.position = m_View.ProjectileOrigin.position;
            m_FieldSphere.SetActive(true);
        }
        
        public void TickShoot()
        {
            TickWeapon();
        }
        
        private void TickWeapon()
        {
            foreach (Node node in m_NodesInRange)
            {
                foreach (EnemyData enemy in node.EnemyDatas)
                {
                    enemy.GetDamage(m_Damage * Time.deltaTime);
                }
            }
        }
    }
}