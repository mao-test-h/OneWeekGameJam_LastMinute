#if ENABLE_DEBUG && UNITY_EDITOR
using Unity.Entities;
using Unity.Transforms2D;
using Unity.Collections;
using UnityEngine;

using System.Collections.Generic;

using MainContents.ECS;
using MainContents.ScriptableObjects;

namespace MainContents.DebugUtility
{
    public sealed class DebugShowCollision : MonoBehaviour
    {
        [SerializeField] bool _isEnable = false;

        [SerializeField] GameSettings _gameSettings;

        [SerializeField] Color _playerColor;
        [SerializeField] Color _playerBulletColor;
        [SerializeField] Color _enemyColor;
        [SerializeField] Color _enemyBulletColor;

        public static List<Vector3> PlayerPositions = new List<Vector3>();
        public static List<Vector3> PlayerBulletPositions = new List<Vector3>();
        public static List<Vector3> EnemyPositions = new List<Vector3>();
        public static List<Vector3> EnemyBulletPositions = new List<Vector3>();

        void OnDrawGizmosSelected()
        {
            if (!this._isEnable) { return; }

            foreach (var pos in PlayerPositions)
            {
                Gizmos.color = this._playerColor;
                Gizmos.DrawSphere(pos, this._gameSettings.PlayerCollisionInstance.Radius);
            }

            foreach (var pos in PlayerBulletPositions)
            {
                Gizmos.color = this._playerBulletColor;
                Gizmos.DrawSphere(pos, this._gameSettings.BulletCollisionInstance.Radius);
            }

            foreach (var pos in EnemyPositions)
            {
                Gizmos.color = this._enemyColor;
                Gizmos.DrawSphere(pos, this._gameSettings.EnemyCollisionInstance.Radius);
            }

            foreach (var pos in EnemyBulletPositions)
            {
                Gizmos.color = this._enemyBulletColor;
                Gizmos.DrawSphere(pos, this._gameSettings.BulletCollisionInstance.Radius);
            }
        }
    }

    public class DebugShowCollisionSystem : ComponentSystem
    {
        // 自機情報
        struct PlayerGroup
        {
            public int Length;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerInput> Input;
            [ReadOnly] public SharedComponentDataArray<PlayerSettings> Settings;
        }

        // プレイヤーの弾
        struct PlayerBulletGroup
        {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerBullet> Identify;
        }

        // 敵情報
        struct EnemyGroup
        {
            public int Length;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<EnemyData> Data;
        }

        // 敵の弾
        struct EnemyBulletGroup
        {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<EnemyBullet> Identify;
        }


        [Inject] PlayerGroup _playerGroup;
        [Inject] PlayerBulletGroup _playerBulletGroup;

        [Inject] EnemyGroup _enemyGroup;
        [Inject] EnemyBulletGroup _enemyBulletGroup;



        protected override void OnUpdate()
        {
            DebugShowCollision.PlayerPositions.Clear();
            for (int i = 0; i < this._playerGroup.Length; ++i)
            {
                var pos = this._playerGroup.Position[i].Value;
                Vector3 ret = new Vector3(pos.x, 0, pos.y);
                DebugShowCollision.PlayerPositions.Add(ret);
            }

            DebugShowCollision.PlayerBulletPositions.Clear();
            for (int i = 0; i < this._playerBulletGroup.Length; ++i)
            {
                var pos = this._playerBulletGroup.Position[i].Value;
                Vector3 ret = new Vector3(pos.x, 0, pos.y);
                DebugShowCollision.PlayerBulletPositions.Add(ret);
            }

            DebugShowCollision.EnemyPositions.Clear();
            for (int i = 0; i < this._enemyGroup.Length; ++i)
            {
                var pos = this._enemyGroup.Position[i].Value;
                Vector3 ret = new Vector3(pos.x, 0, pos.y);
                DebugShowCollision.EnemyPositions.Add(ret);
            }

            DebugShowCollision.EnemyBulletPositions.Clear();
            for (int i = 0; i < this._enemyBulletGroup.Length; ++i)
            {
                var pos = this._enemyBulletGroup.Position[i].Value;
                Vector3 ret = new Vector3(pos.x, 0, pos.y);
                DebugShowCollision.EnemyBulletPositions.Add(ret);
            }
        }
    }
}
#endif