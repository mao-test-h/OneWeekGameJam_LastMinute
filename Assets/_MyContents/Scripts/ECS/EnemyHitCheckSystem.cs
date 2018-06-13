using Unity.Entities;
using Unity.Transforms2D;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MainContents.ECS
{
    /// <summary>
    /// 敵機の被弾チェック
    /// </summary>
    public sealed class EnemyHitCheckSystem : ComponentSystem
    {
        // 敵情報
        struct EnemyGroup
        {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<EnemyData> Data;
            [ReadOnly] public SharedComponentDataArray<EnemyCollision> Collision;
        }

        // プレイヤーの弾
        struct PlayerBulletGroup
        {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerBullet> Identify;
            [ReadOnly] public SharedComponentDataArray<BulletCollision> Collision;
        }

        [Inject] EnemyGroup _enemyGroup;
        [Inject] PlayerBulletGroup _playerBulletGroup;

        protected override void OnUpdate()
        {
            for (int i = 0; i < this._enemyGroup.Length; ++i)
            {
                var enemyPos = this._enemyGroup.Position[i].Value;
                float eRadius = this._enemyGroup.Collision[i].Radius;
                for (int j = 0; j < this._playerBulletGroup.Length; ++j)
                {
                    var bulletPos = this._playerBulletGroup.Position[j].Value;
                    float bRadius = this._playerBulletGroup.Collision[j].Radius;
                    if (math.pow((enemyPos.x - bulletPos.x), 2) + math.pow((enemyPos.y - bulletPos.y), 2)
                        <= math.pow((eRadius + bRadius), 2))
                    {
                        PostUpdateCommands.DestroyEntity(this._enemyGroup.Entities[i]);
                        PostUpdateCommands.DestroyEntity(this._playerBulletGroup.Entities[j]);
                    }
                }
            }
        }
    }
}