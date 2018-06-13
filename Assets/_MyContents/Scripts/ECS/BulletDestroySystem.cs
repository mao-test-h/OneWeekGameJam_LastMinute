using Unity.Entities;
using UnityEngine;

namespace MainContents.ECS
{
    /// <summary>
    /// 弾の破棄処理
    /// </summary>
    public sealed class BulletDestroySystem : ComponentSystem
    {
        struct Group
        {
            public int Length;
            public EntityArray Entities;
            public ComponentDataArray<BulletData> BulletData;
        }

        [Inject] Group _group;

        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < this._group.Length; i++)
            {
                var data = this._group.BulletData[i];
                data.Lifespan -= deltaTime;
                if (data.Lifespan <= 0f)
                {
                    PostUpdateCommands.DestroyEntity(_group.Entities[i]);
                    continue;
                }
                this._group.BulletData[i] = data;
            }
        }

    }
}