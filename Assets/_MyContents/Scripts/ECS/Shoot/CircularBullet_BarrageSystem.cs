using Unity.Entities;
using Unity.Transforms2D;
using Unity.Collections;
using UnityEngine;

namespace MainContents.ECS
{
    public sealed class CircularBullet_BarrageSystem : ComponentSystem
    {
        struct Group
        {
            public int Length;
            public ComponentDataArray<EnemyData> EnemyData;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public SharedComponentDataArray<BarrageSettings_CircularBullet> Settings;
        }

        [Inject] Group _group;

        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < this._group.Length; i++)
            {
                var data = this._group.EnemyData[i];
                var pos = this._group.Position[i];
                var settings = this._group.Settings[i];
                data.CooldownTimeCounter -= deltaTime;
                if (data.CooldownTimeCounter <= 0f)
                {
                    data.CooldownTimeCounter = settings.CommonBarrageSettings.CooldownTime;
                    this.SpawnBullet(ref pos, ref settings);
                }
                this._group.EnemyData[i] = data;
            }
        }

        // 敵の生成
        void SpawnBullet(ref Position2D pos, ref BarrageSettings_CircularBullet settings)
        {
            for (int i = 0; i < settings.BulletCount; ++i)
            {
                PostUpdateCommands.CreateEntity(MainECS_Manager.EnemyBulletArchetype);
                PostUpdateCommands.SetComponent(pos);
                PostUpdateCommands.SetComponent(
                    new BulletData
                    {
                        ShotSpeed = settings.CommonBarrageSettings.ShotSpeed,
                        ShotAngle = (i / (float)settings.BulletCount) * 360f,
                        Lifespan = settings.CommonBarrageSettings.Lifespan
                    });
                PostUpdateCommands.AddSharedComponent(MainECS_Manager.EnemyBulletLook);
                PostUpdateCommands.AddSharedComponent(MainECS_Manager.BulletCollision);
            }
        }
    }
}