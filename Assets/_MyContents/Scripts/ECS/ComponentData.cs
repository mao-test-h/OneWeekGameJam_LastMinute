using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;

namespace MainContents.ECS
{
    /// <summary>
    /// 可動領域
    /// </summary>
    [System.Serializable]
    public struct RectData
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float xMin { get { return X; } }
        public float xMax { get { return X + Width; } }
        public float yMin { get { return Y; } }
        public float yMax { get { return Y + Height; } }
    }

    /// <summary>
    /// 弾幕共通設定
    /// </summary>
    [System.Serializable]
    public struct CommonBarrageSettings
    {
        /// <summary>
        /// 弾速
        /// </summary>
        public float ShotSpeed;

        /// <summary>
        /// 生成間隔
        /// </summary>
        public float CooldownTime;

        /// <summary>
        /// 生存時間
        /// </summary>
        public float Lifespan;
    }

    // ----------------------------------------------------
    #region // Player

    /// <summary>
    /// プレイヤー入力
    /// </summary>
    public struct PlayerInput : IComponentData
    {
        public int Fire;
        public float FireCooldown;
    }

    /// <summary>
    /// プレイヤーのライフ
    /// </summary>
    public struct PlayerLife : IComponentData
    {
        public float Value;
        public float Max;
    }

    /// <summary>
    /// プレイヤー設定
    /// </summary>
    [System.Serializable]
    public struct PlayerSettings : ISharedComponentData
    {
        /// <summary>
        /// ショット設定
        /// </summary>
        [System.Serializable]
        public struct ShootSettings
        {
            /// <summary>
            /// 弾速
            /// </summary>
            public float ShotSpeed;

            /// <summary>
            /// 生存時間
            /// </summary>
            public float Lifespan;

            /// <summary>
            /// 弾の発砲間隔
            /// </summary>
            public float FireCooldown;
        }

        /// <summary>
        /// ライフ設定
        /// </summary>
        [System.Serializable]
        public struct LifeSettings
        {
            /// <summary>
            /// 最大体力
            /// </summary>
            public float MaxLife;

            /// <summary>
            /// 回復量
            /// </summary>
            public float RecoveryAmount;

            /// <summary>
            /// ダメージ量
            /// </summary>
            public float DamageAmount;
        }

        /// <summary>
        /// 移動速度
        /// </summary>
        public float MoveSpeed;

        /// <summary>
        /// プレイヤー ショット設定
        /// </summary>
        public ShootSettings ShootSettingsInstance;

        /// <summary>
        /// ライフ設定
        /// </summary>
        public LifeSettings LifeSettingsInstance;

        /// <summary>
        /// 可動領域
        /// </summary>
        public RectData MovableAreaInstance;
    }

    /// <summary>
    /// 自機の色
    /// </summary>
    public struct PlayerColor : ISharedComponentData
    {
        /// <summary>
        /// 通常時
        /// </summary>
        public float4 NormalColor;
        /// <summary>
        /// ダメージを受けた時
        /// </summary>
        public float4 DamagedColor;
    }

    /// <summary>
    /// 自機のコリジョン設定
    /// </summary>
    [System.Serializable]
    public struct PlayerCollision : ISharedComponentData
    {
        /// <summary>
        /// 範囲
        /// </summary>
        public float Radius;
    }

    #endregion // Player

    // ----------------------------------------------------
    #region // Enemy Spawn System

    /// <summary>
    /// 敵生成情報 データ
    /// </summary>
    public struct EnemySpawnSystemData : IComponentData
    {
        /// <summary>
        /// 生成間隔(カウント用)
        /// </summary>
        public float CooldownTimeCounter;

        /// <summary>
        /// 生成数
        /// </summary>
        public int SpawnedEnemyCount;
    }

    /// <summary>
    /// 敵生成情報
    /// </summary>
    [System.Serializable]
    public struct EnemySpawnSystemSettings : ISharedComponentData
    {
        /// <summary>
        /// 生成間隔
        /// </summary>
        public float CooldownTime;

        /// <summary>
        /// 生成限界FPS
        /// </summary>
        public float LimitFps;

        /// <summary>
        /// 弾幕の種類数
        /// </summary>
        [UnityEngine.HideInInspector] public int MaxBarrageType;

        /// <summary>
        /// 可動領域
        /// </summary>
        public RectData SpawnArea;

        /// <summary>
        /// 可動領域をランダムに取得
        /// </summary>
        /// <returns></returns>
        public float2 RandomArea()
        {
            float x = UnityEngine.Random.Range(this.SpawnArea.xMin, this.SpawnArea.xMax);
            float y = UnityEngine.Random.Range(this.SpawnArea.yMin, this.SpawnArea.yMax);
            return new float2(x, y);
        }
    }

    #endregion // Enemy Spawn System

    // ----------------------------------------------------
    #region // Enemy

    public struct EnemyData : IComponentData
    {
        /// <summary>
        /// 生成間隔(カウント用)
        /// </summary>
        public float CooldownTimeCounter;
    }

    /// <summary>
    /// 敵機のコリジョン設定
    /// </summary>
    [System.Serializable]
    public struct EnemyCollision : ISharedComponentData
    {
        /// <summary>
        /// 範囲
        /// </summary>
        public float Radius;
    }

    #endregion // Enemy

    #region // Enemy(Barrage)

    /// <summary>
    /// 全方位弾
    /// </summary>
    [System.Serializable]
    public struct BarrageSettings_CircularBullet : ISharedComponentData
    {
        /// <summary>
        /// 生成数
        /// </summary>
        public int BulletCount;

        /// <summary>
        /// 弾幕共通設定
        /// </summary>
        public CommonBarrageSettings CommonBarrageSettings;
    }

    /// <summary>
    /// 自方向に単発
    /// </summary>
    [System.Serializable]
    public struct BarrageSettings_DirectionBullet : ISharedComponentData
    {
        /// <summary>
        /// 弾幕共通設定
        /// </summary>
        public CommonBarrageSettings CommonBarrageSettings;
    }

    #endregion // Enemy(Barrage)

    // ----------------------------------------------------
    #region // Bullet

    /// <summary>
    /// 弾のデータ
    /// </summary>
    public struct BulletData : IComponentData
    {
        /// <summary>
        /// 弾速
        /// </summary>
        public float ShotSpeed;

        /// <summary>
        /// 角度
        /// </summary>
        public float ShotAngle;

        /// <summary>
        /// 生存時間
        /// </summary>
        public float Lifespan;
    }

    public struct EnemyBullet : IComponentData { }
    public struct PlayerBullet : IComponentData { }

    /// <summary>
    /// 弾(自機・敵機共通)のコリジョン設定
    /// </summary>
    [System.Serializable]
    public struct BulletCollision : ISharedComponentData
    {
        /// <summary>
        /// 範囲
        /// </summary>
        public float Radius;
    }

    #endregion // Bullet
}