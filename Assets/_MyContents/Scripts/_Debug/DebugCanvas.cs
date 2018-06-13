using UnityEngine;
using UnityEngine.UI;

using Unity.Entities;
using Unity.Transforms2D;
using Unity.Mathematics;

using MainContents.ECS;
using MainContents.ScriptableObjects;

namespace MainContents.DebugUtility
{
    public sealed class DebugCanvas : MonoBehaviour
    {
#if !ENABLE_DEBUG
        void Start() { Destroy(this.gameObject); }
#else

        [SerializeField] GameSettings _gameSettings;
        [SerializeField] DebugSettings _debugSettings;

        [SerializeField] Text _TextFpsCount;

        EntityManager _entityManager;

        void Start()
        {
            this._entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        void Update()
        {
            this._TextFpsCount.text = ECS.MainECS_Manager.CurrentFps.ToString();
        }


        public void OnDebugCreateEnemy(int type)
        {
            var pos = this._debugSettings.CreatePosition;
            this.DebugCreateEnemyInternal(type, pos);
        }

        public void OnDebugCreateEnemyRandom()
        {
            var settings = this._gameSettings.EnemySpawnSystemSettingsInstance;
            var type = UnityEngine.Random.Range(0, settings.MaxBarrageType);
            var pos = settings.RandomArea();
            this.DebugCreateEnemyInternal(type, pos);
        }

        void DebugCreateEnemyInternal(int type, float2 pos)
        {
            var entity = this._entityManager.CreateEntity(MainECS_Manager.CommonEnemyArchetype);

            switch ((BarrageType)type)
            {
                case BarrageType.CircularBullet:
                    {
                        this._entityManager.SetComponentData(entity, new Position2D { Value = pos });
                        this._entityManager.SetComponentData(entity, new EnemyData { });
                        this._entityManager.AddSharedComponentData(entity, MainECS_Manager.EnemyLook);
                        this._entityManager.AddSharedComponentData(entity, MainECS_Manager.EnemyCollision);
                        this._entityManager.AddSharedComponentData(entity, MainECS_Manager.BarrageSettings_CircularBullet);
                    }
                    break;
                case BarrageType.DirectionBullet:
                    {
                        this._entityManager.SetComponentData(entity, new Position2D { Value = pos });
                        this._entityManager.SetComponentData(entity, new EnemyData { });
                        this._entityManager.AddSharedComponentData(entity, MainECS_Manager.EnemyLook);
                        this._entityManager.AddSharedComponentData(entity, MainECS_Manager.EnemyCollision);
                        this._entityManager.AddSharedComponentData(entity, MainECS_Manager.BarrageSettings_DirectionBullet);
                    }
                    break;
            }
        }

        public void OnDebugCreateBullet()
        {
            var entity = this._entityManager.CreateEntity(MainECS_Manager.EnemyBulletArchetype);

            var data = this._debugSettings.SpawnBulletDataInstance;
            this._entityManager.SetComponentData(
                entity, new Position2D { Value = data.CreatePosition });

            this._entityManager.SetComponentData(
                entity, new BulletData
                {
                    ShotSpeed = data.Speed,
                    ShotAngle = data.Angle,
                    Lifespan = data.Lifespan
                });
            this._entityManager.AddSharedComponentData(entity, MainECS_Manager.EnemyBulletLook);
            this._entityManager.AddSharedComponentData(entity, MainECS_Manager.BulletCollision);
        }

#endif
    }
}