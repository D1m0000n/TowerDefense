using System;
using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed = 10f;
        private int m_Damage = 5;
        private float m_DamageRadius = 5f;
        private bool m_DidHit = false;
        private EnemyData m_TargetEnemy;
        public void TickApproaching()
        {
            Vector3 direction = m_TargetEnemy.View.transform.position - transform.position + Vector3.up * 0.5f;
            transform.forward = direction.normalized;
            transform.Translate(direction.normalized * (m_Speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyView enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    m_DidHit = true;
                }
            }
        }

        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            List<Node> explosionNodes = Game.Player.Grid.GetNodesInCircle(transform.position, m_DamageRadius);
            foreach (Node node in explosionNodes)
            {
                foreach (EnemyData enemy in node.EnemyDatas)
                {
                    enemy.GetDamage(m_Damage);
                }
            }
            Destroy(gameObject);
        }

        public void SetTargetEnemy(EnemyData enemyData)
        {
            m_TargetEnemy = enemyData;
        }
    }
}