using Unity.Entities;
using Unity.Transforms2D;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MainContents.ECS
{
    /// <summary>
    /// 自機の被弾チェック
    /// </summary>
    public sealed class PlayerHitCheckSystem : ComponentSystem
    {
        // 敵の弾
        struct EnemyBulletGroup
        {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<EnemyBullet> Identify;
            [ReadOnly] public SharedComponentDataArray<BulletCollision> Collision;
        }

        // 自機情報
        struct PlayerGroup
        {
            public int Length;
            public ComponentDataArray<PlayerLife> Life;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerInput> Input;
            [ReadOnly] public SharedComponentDataArray<PlayerSettings> PlayerSettings;
            [ReadOnly] public SharedComponentDataArray<PlayerCollision> Collision;
        }

        [Inject] EnemyBulletGroup _enemyBulletGroup;
        [Inject] PlayerGroup _playerGroup;


        protected override void OnUpdate()
        {
            if (this._playerGroup.Length <= 0) { return; }

            var playerPos = this._playerGroup.Position[0].Value;
            var playerLife = this._playerGroup.Life[0];

            var bulletDamage = this._playerGroup.PlayerSettings[0].LifeSettingsInstance.DamageAmount;
            float pRadius = this._playerGroup.Collision[0].Radius;

            for (int i = 0; i < this._enemyBulletGroup.Length; ++i)
            {
                float2 bulletPos = this._enemyBulletGroup.Position[i].Value;
                float bRadius = this._enemyBulletGroup.Collision[i].Radius;

                if (math.pow((playerPos.x - bulletPos.x), 2) + math.pow((playerPos.y - bulletPos.y), 2)
                    <= math.pow((pRadius + bRadius), 2))
                {
                    PostUpdateCommands.DestroyEntity(this._enemyBulletGroup.Entities[i]);

                    playerLife.Value -= bulletDamage;
                    if (playerLife.Value <= 0)
                    {
                        playerLife.Value = 0;
                        PostUpdateCommands.DestroyEntity(this._playerGroup.Entities[0]);
                        break;
                    }

                }
            }

            this._playerGroup.Life[0] = playerLife;
        }
    }
}