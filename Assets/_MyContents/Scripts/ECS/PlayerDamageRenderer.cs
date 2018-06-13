using Unity.Entities;
using Unity.Transforms2D;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace MainContents.ECS
{
    public class PlayerDamageRenderer : ComponentSystem
    {
        struct Group
        {
            public int Length;
            public ComponentDataArray<PlayerLife> Life;
            [ReadOnly] public SharedComponentDataArray<MeshInstanceRenderer> Renderer;
            [ReadOnly] public SharedComponentDataArray<PlayerColor> Colors;
        }

        [Inject] Group _group;

        protected override void OnUpdate()
        {
            if (this._group.Length <= 0) { return; }

            var colors = this._group.Colors[0];

            var value = this._group.Life[0].Value;
            var max = this._group.Life[0].Max;
            var ret = math.lerp(colors.DamagedColor, colors.NormalColor, value / max);
            MainECS_Manager.PlayerLook.material.color = new Color(ret.x, ret.y, ret.z, ret.w);
        }
    }

}