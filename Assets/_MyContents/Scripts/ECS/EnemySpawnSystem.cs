using Unity.Entities;
using Unity.Transforms2D;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

using MainContents.ScriptableObjects;

namespace MainContents.ECS
{
    /// <summary>
    /// 敵の生成処理
    /// </summary>
    public sealed class EnemySpawnSystem : ComponentSystem
    {
        struct Group
        {
            public int Length;
            public ComponentDataArray<EnemySpawnSystemData> Data;
            [ReadOnly] public SharedComponentDataArray<EnemySpawnSystemSettings> Settings;
        }

        [Inject] Group _group;

        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;
            var data = this._group.Data[0];
            var spawnSettings = this._group.Settings[0];
            data.CooldownTimeCounter -= deltaTime;

            if (data.CooldownTimeCounter <= 0f && MainECS_Manager.CurrentFps > spawnSettings.LimitFps)
            {
                data.CooldownTimeCounter = spawnSettings.CooldownTime;
                this.SpawnEnemy(ref data, ref spawnSettings);
            }
            this._group.Data[0] = data;
        }

        // 敵の生成
        void SpawnEnemy(ref EnemySpawnSystemData data, ref EnemySpawnSystemSettings spawnSettings)
        {
            ++data.SpawnedEnemyCount;

            var type = UnityEngine.Random.Range(0, spawnSettings.MaxBarrageType);
            var pos = spawnSettings.RandomArea();

            PostUpdateCommands.CreateEntity(MainECS_Manager.CommonEnemyArchetype);
            PostUpdateCommands.SetComponent(new Position2D { Value = pos });
            PostUpdateCommands.SetComponent(new EnemyData { });
            PostUpdateCommands.AddSharedComponent(MainECS_Manager.EnemyLook);
            PostUpdateCommands.AddSharedComponent(MainECS_Manager.EnemyCollision);

            switch ((BarrageType)type)
            {
                case BarrageType.CircularBullet:
                    {
                        PostUpdateCommands.AddSharedComponent(MainECS_Manager.BarrageSettings_CircularBullet);
                    }
                    break;
                case BarrageType.DirectionBullet:
                    {
                        PostUpdateCommands.AddSharedComponent(MainECS_Manager.BarrageSettings_DirectionBullet);
                    }
                    break;
            }
        }
    }
}