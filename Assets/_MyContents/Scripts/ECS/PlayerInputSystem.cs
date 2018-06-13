using Unity.Entities;
using Unity.Transforms2D;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MainContents.ECS
{
    public sealed class PlayerInputSystem : ComponentSystem
    {
        struct Group
        {
            public int Length;
            public ComponentDataArray<PlayerInput> Input;
            public ComponentDataArray<PlayerLife> Life;
            [ReadOnly] public SharedComponentDataArray<PlayerSettings> PlayerSettings;
        }

        [Inject] Group _group;

        protected override void OnUpdate()
        {
            if (this._group.Length <= 0) { return; }

            float deltaTime = Time.deltaTime;

            var inputData = _group.Input[0];
            PlayerInput ret = new PlayerInput();
            ret.FireCooldown = math.max(0f, inputData.FireCooldown - deltaTime);
            ret.Fire = (ret.FireCooldown <= 0f && Input.GetButton("Fire1")) ? 1 : 0;
            _group.Input[0] = ret;

            var playerLife = this._group.Life[0];
            var lifeSettings = this._group.PlayerSettings[0].LifeSettingsInstance;
            if (Input.GetButtonDown("Jump"))
            {
                playerLife.Value += lifeSettings.RecoveryAmount;
                if (playerLife.Value >= playerLife.Max) { playerLife.Value = playerLife.Max; }
            }
            this._group.Life[0] = playerLife;
        }
    }

}