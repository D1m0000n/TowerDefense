using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using Turret.Weapon.Projectile;
using UnityEngine;

namespace Turret.Weapon.Lazer
{
    public class TurretLazerWeapon : ITurretWeapon
    {
        private LineRenderer m_LineRenderer;
        private TurretView m_View;
        private TurretLazerWeaponAsset m_Asset;

        private float m_MaxDistance;
        private float m_Damage = 8f;

        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        private List<Node> m_NodesInRange;
        
        public TurretLazerWeapon(TurretLazerWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_MaxDistance = m_Asset.MaxDistance;
            m_LineRenderer = Object.Instantiate(asset.LineRendererPrefab, m_View.transform.position, Quaternion.identity);
            m_NodesInRange =
                Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.position, m_MaxDistance);
        }
        
        public void TickShoot()
        {
            TickWeapon();
            TickTower();
        }
        
        private void TickWeapon()
        {
            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_NodesInRange);
            if (m_ClosestEnemyData == null)
            {
                m_LineRenderer.gameObject.SetActive(false);
                return;
            }
            
            Vector3 originPos = m_View.ProjectileOrigin.position;

            m_LineRenderer.transform.position = originPos;
            m_LineRenderer.SetPosition(1, m_ClosestEnemyData.View.transform.position - originPos + Vector3.up * 0.5f);

            m_LineRenderer.gameObject.SetActive(true);
            TickTower();
            Shoot(m_ClosestEnemyData);
        }
        
        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }

        private void Shoot(EnemyData enemyData)
        {
            enemyData.GetDamage(m_Damage * Time.deltaTime);
        }
    }
}