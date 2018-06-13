using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;
using Unity.Collections;
using UnityEngine;

namespace MainContents.ECS
{
    /// <summary>
    /// 弾の移動処理
    /// </summary>
    public sealed class BulletMoveSystem : ComponentSystem
    {
        // 弾の移動に必要なデータ
        struct Group
        {
            public int Length;
            public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<BulletData> BulletData;
        }

        [Inject] Group _group;

        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < this._group.Length; i++)
            {
                var data = this._group.BulletData[i];

                float2 tmp = new float2(0f, 0f);
                float angle = math.radians(data.ShotAngle);
                tmp.x = math.cos(angle);
                tmp.y = math.sin(angle);

                var pos = this._group.Position[i];
                pos.Value += (tmp * data.ShotSpeed * deltaTime);

                this._group.Position[i] = pos;
            }
        }

    }
}