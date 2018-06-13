using System;
using UnityEngine;
using MainContents.ECS;

namespace MainContents.ScriptableObjects
{
    /// <summary>
    /// 弾幕の種類
    /// </summary>
    public enum BarrageType
    {
        DirectionBullet = 0,
        CircularBullet,
    }

    /// <summary>
    /// ゲーム設定
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/GameSettings", fileName = "GameSettings")]
    public sealed class GameSettings : ScriptableObject
    {
        /// <summary>
        /// 自機の色設定
        /// </summary>
        [Serializable]
        public struct PlayerColorSettings
        {
            public Color PlayerNormalColor;
            public Color PlayerDamagedColor;
        }

        /// <summary>
        /// コリジョン設定
        /// </summary>
        [System.Serializable]
        public struct CollisionSettings
        {
            /// <summary>
            /// 自機
            /// </summary>
            public float PlayerCollisionRadius;

            /// <summary>
            /// 敵機
            /// </summary>
            public float EnemyCollisionRadius;

            /// <summary>
            /// 弾(自機・敵機共通)
            /// </summary>
            public float BulletCollisionRadius;
        }



        /// <summary>
        /// オブジェクトのサイズ
        /// </summary>
        [Header("【Common Settings】")]
        [SerializeField] public float ObjectScale;

        [Header("【Collision Settings】")]
        [SerializeField] public PlayerCollision PlayerCollisionInstance;
        [SerializeField] public EnemyCollision EnemyCollisionInstance;
        [SerializeField] public BulletCollision BulletCollisionInstance;

        /// <summary>
        /// プレイヤー設定
        /// </summary>
        [Header("【Player Settings】")]
        [SerializeField] public PlayerSettings PlayerSettingsInstance;
        [SerializeField] public Vector2 PlayerCreatePosition;
        [SerializeField] public PlayerColorSettings PlayerColorSettingsInstance;

        /// <summary>
        /// 敵の生成間隔
        /// </summary>
        [Header("【Enemy Spawn System Settings】")]
        [SerializeField] public EnemySpawnSystemSettings EnemySpawnSystemSettingsInstance;

        /// <summary>
        /// 弾幕用データ
        /// </summary>
        [Header("【Barrage Data】")]
        [SerializeField] public BarrageSettings_DirectionBullet BarrageSettings_DirectionBulletInstance;
        [SerializeField] public BarrageSettings_CircularBullet BarrageSettings_CircularBulletInstance;
    }
}