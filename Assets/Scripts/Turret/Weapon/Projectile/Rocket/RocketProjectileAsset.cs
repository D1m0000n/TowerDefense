using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    [CreateAssetMenu(menuName = "Assets/Rocket Projectile Asset", fileName = "Rocket Projectile Asset")]
    public class RocketProjectileAsset : ProjectileAssetBase
    {
        [SerializeField] private RocketProjectile m_RocketPrefab;
        [SerializeField] public float Speed;
        [SerializeField] public float Damage;

        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            RocketProjectile rocket = Instantiate(m_RocketPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            rocket.SetTargetEnemy(enemyData);
            rocket.SetAsset(this);
            return rocket;
        }
    }
}