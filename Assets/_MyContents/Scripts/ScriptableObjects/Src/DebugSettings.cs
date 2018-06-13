#if ENABLE_DEBUG

using System;
using UnityEngine;

using MainContents.ECS;

namespace MainContents.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DebugSettings", fileName = "DebugSettings")]
    public sealed class DebugSettings : ScriptableObject
    {
        // デバッグ用弾発射
        [Serializable]
        public struct SpawnBulletData
        {
            [SerializeField] public Vector2 CreatePosition;
            [SerializeField] public float Angle;
            [SerializeField] public float Speed;
            [SerializeField] public float Lifespan;
        }

        [Header("【Spawn Enemy】")]
        [SerializeField] public BarrageType CreateBarrageType;
        [SerializeField] public Vector2 CreatePosition;

        [Header("【Spawn Bullet】")]
        [SerializeField] public SpawnBulletData SpawnBulletDataInstance;
    }
}

#endif