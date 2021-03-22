using Enemy;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Assets/Enemy Asset", fileName = "Enemy Asset")]
    public class EnemyAsset : ScriptableObject
    {
        public int StartHealth;
        
        public EnemyView ViewPrefab;
    }
}