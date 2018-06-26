#if !UNITY_EDITOR && UNITY_WEBGL
#define WEBGL_ONLY
#endif

using System;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using Unity.Mathematics;
using Unity.Collections;

using MainContents.ScriptableObjects;

namespace MainContents.ECS
{
    /// <summary>
    /// メインゲーム ECS 管理クラス
    /// </summary>
    public sealed class MainECS_Manager : MonoBehaviour
    {
        // ------------------------------
        #region // Defines

        /// <summary>
        /// MeshInstanceRendererに渡すデータ
        /// </summary>
        [Serializable]
        sealed class MeshInstanceRendererData
        {
            /// <summary>
            /// 表示スプライト
            /// </summary>
            [SerializeField] public Sprite Sprite;

            /// <summary>
            /// 表示マテリアル
            /// </summary>
            [SerializeField] public Material Material;
        }

        /// <summary>
        /// FPS計測
        /// </summary>
        public sealed class FPSCounter
        {
            const float FPSMeasurePeriod = 0.5f;

            public int CurrentFps { get; private set; }

            int _FpsAccumulator = 0;
            float _FpsNextPeriod = 0;

            public FPSCounter()
            {
                this._FpsNextPeriod = Time.realtimeSinceStartup + FPSMeasurePeriod;
            }

            public void Update()
            {
                ++this._FpsAccumulator;
                if (Time.realtimeSinceStartup > this._FpsNextPeriod)
                {
                    this.CurrentFps = (int)(this._FpsAccumulator / FPSMeasurePeriod);
                    this._FpsAccumulator = 0;
                    this._FpsNextPeriod += FPSMeasurePeriod;
                }
            }
        }

        #endregion // Defines

        // ------------------------------
        #region // Private Members(Editable)

        /// <summary>
        /// プレイヤーの表示データ
        /// </summary>
        [Header("【MeshInstanceRenderer Data】")]
        [SerializeField] MeshInstanceRendererData _playerRendererData;

        /// <summary>
        /// 自機弾の表示データ
        /// </summary>
        [SerializeField] MeshInstanceRendererData _playerBulletRendererData;

        /// <summary>
        /// 敵の表示データ
        /// </summary>
        [SerializeField] MeshInstanceRendererData _enemyRendererData;

        /// <summary>
        /// 敵弾の表示データ
        /// </summary>
        [SerializeField] MeshInstanceRendererData _enemyBulletRendererData;

        /// <summary>
        /// ゲーム設定
        /// </summary>
        [Header("【Settings】")]
        [SerializeField] GameSettings _gameSettings;

        /// <summary>
        /// タイトル用Canvas
        /// </summary>
        [Header("【UI】")]
        [SerializeField] Canvas _titleCanvas;

        /// <summary>
        /// リザルト用Canvas
        /// </summary>
        [SerializeField] Canvas _resultCanvas;

        /// <summary>
        /// リザルト用 結果表示テキスト
        /// </summary>
        [SerializeField] Text _survivalTimeText;

        #endregion // Private Members(Editable)

        // ------------------------------
        #region // Private Members

        /// <summary>
        /// EntityManager
        /// </summary>
        EntityManager _entityManager;

        /// <summary>
        /// Cameraの参照
        /// </summary>
        Camera _mainCamera;

        /// <summary>
        /// 生き延びた時間
        /// </summary>
        float _survivalTime = 0f;

        /// <summary>
        /// 自機のEntity
        /// </summary>
        Entity _playerEntity;

        #endregion // Private Members

        // ------------------------------
        #region // Private Members(Static)

        /// <summary>
        /// FPS計測
        /// </summary>
        static FPSCounter FPSCounterInstance;

        #endregion // Private Members(Static)

        // ------------------------------
        #region // Properties(Static)

        /// <summary>
        /// マウスのワールド座標
        /// </summary>
        public static Vector3 WorldMousePosision { get; private set; }

        /// <summary>
        /// fpsの取得
        /// </summary>
        public static int CurrentFps { get { return FPSCounterInstance.CurrentFps; } }

        #region // Archetype

        /// <summary>
        /// 自機のアーキタイプ
        /// </summary>
        public static EntityArchetype PlayerArchetype { get; private set; }

        /// <summary>
        /// 自機弾のアーキタイプ
        /// </summary>
        public static EntityArchetype PlayerBulletArchetype { get; private set; }

        /// <summary>
        /// 敵弾のアーキタイプ
        /// </summary>
        public static EntityArchetype EnemyBulletArchetype { get; private set; }

        /// <summary>
        /// 敵生成システムのアーキタイプ
        /// </summary>
        public static EntityArchetype EnemySpawnSystemArchetype { get; private set; }

        /// <summary>
        /// 敵共通のアーキタイプ
        /// </summary>
        public static EntityArchetype CommonEnemyArchetype { get; private set; }

        #endregion // Archetype

        #region // MeshInstanceRenderer

        /// <summary>
        /// 自機のMeshInstanceRenderer
        /// </summary>
        public static MeshInstanceRenderer PlayerLook { get; private set; }

        /// <summary>
        /// 自機弾のMeshInstanceRenderer
        /// </summary>
        public static MeshInstanceRenderer PlayerBulletLook { get; private set; }

        /// <summary>
        /// 敵のMeshInstanceRenderer
        /// </summary>
        public static MeshInstanceRenderer EnemyLook { get; private set; }

        /// <summary>
        /// 敵弾のMeshInstanceRenderer
        /// </summary>
        public static MeshInstanceRenderer EnemyBulletLook { get; private set; }

        #endregion // MeshInstanceRenderer

        #region // ISharedComponentData

        // 弾幕
        public static BarrageSettings_DirectionBullet BarrageSettings_DirectionBullet { get; private set; }
        public static BarrageSettings_CircularBullet BarrageSettings_CircularBullet { get; private set; }

        // コリジョン
        public static PlayerCollision PlayerCollision { get; private set; }
        public static EnemyCollision EnemyCollision { get; private set; }
        public static BulletCollision BulletCollision { get; private set; }

        #endregion // ISharedComponentData

        #endregion // Properties(Static)



        // ----------------------------------------------------
        #region // Unity Events

        /// <summary>
        /// UnityEvent : Start
        /// </summary>
        void Start()
        {
            this._entityManager = World.Active.GetOrCreateManager<EntityManager>();
            this._mainCamera = Camera.main;
            FPSCounterInstance = new FPSCounter();

            this._titleCanvas.enabled = true;
            this._resultCanvas.enabled = false;

            // Archetypes Settings
            PlayerArchetype = this._entityManager.CreateArchetype(
                typeof(Position2D),
                typeof(PlayerInput),
                typeof(PlayerLife),
                typeof(TransformMatrix));
            // ▼ SharedComponentData      ※SharedComponentDataはArchetypeに登録出来ないので使用される物をメモ
            // - MeshInstanceRenderer
            // - PlayerSettings
            // - PlayerCollision
            // - PlayerColor

            EnemyBulletArchetype = this._entityManager.CreateArchetype(
                typeof(Position2D),
                typeof(BulletData),
                typeof(EnemyBullet),
                typeof(TransformMatrix));
            // ▼ SharedComponentData
            // - MeshInstanceRenderer
            // - BulletCollision

            PlayerBulletArchetype = this._entityManager.CreateArchetype(
                typeof(Position2D),
                typeof(BulletData),
                typeof(PlayerBullet),
                typeof(TransformMatrix));
            // ▼ SharedComponentData
            // - MeshInstanceRenderer
            // - BulletCollision

            EnemySpawnSystemArchetype = this._entityManager.CreateArchetype(
                typeof(EnemySpawnSystemData));
            // ▼ SharedComponentData
            // - EnemySpawnSystemSettings

            CommonEnemyArchetype = this._entityManager.CreateArchetype(
                typeof(Position2D),
                typeof(EnemyData),
                typeof(TransformMatrix));
            // ▼ SharedComponentData
            // - MeshInstanceRenderer
            // - EnemyCollision
            // - [BarrageSettings_DirectionBullet] or [BarrageSettings_CircularBullet]
            //      →こちらについて、登録するSharedComponentDataによって敵が撃ってくる弾幕が変わる。どちらか片方しか登録されない想定。

            // MeshInstanceRenderers Settings
            PlayerLook = this.CreateMeshInstanceRenderer(this._playerRendererData);
            EnemyLook = this.CreateMeshInstanceRenderer(this._enemyRendererData);
            PlayerBulletLook = this.CreateMeshInstanceRenderer(this._playerBulletRendererData);
            EnemyBulletLook = this.CreateMeshInstanceRenderer(this._enemyBulletRendererData);

            // BarrageSettings
            BarrageSettings_DirectionBullet = this._gameSettings.BarrageSettings_DirectionBulletInstance;
            BarrageSettings_CircularBullet = this._gameSettings.BarrageSettings_CircularBulletInstance;

            // Collisions
            PlayerCollision = this._gameSettings.PlayerCollisionInstance;
            EnemyCollision = this._gameSettings.EnemyCollisionInstance;
            BulletCollision = this._gameSettings.BulletCollisionInstance;
        }

        /// <summary>
        /// UnityEvent : Update
        /// </summary>
        void Update()
        {
            if (!this._entityManager.Exists(this._playerEntity)
                && !this._titleCanvas.enabled
                && !this._resultCanvas.enabled)
            {
                this.Result();
            }
            FPSCounterInstance.Update();
        }

        /// <summary>
        /// UnityEvent : LateUpdate
        /// </summary>
        void LateUpdate()
        {
            // FIXME: 本当はPlayerInputSystem内でInput.GetAxisよりキーイベントを取得して自機を動かしていたが、
            // Unityのバグ?なのか、WebGLビルド時のみキーイベントが消えずに押しっぱなし?になる謎の不具合を確認したので
            // 仕方なく一時的にマウス操作にする。。。
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            WorldMousePosision = this._mainCamera.ScreenToWorldPoint(mousePos);
        }

        #endregion // Unity Events

        // ----------------------------------------------------
        #region // Public Functions

        /// <summary>
        /// ゲーム開始
        /// </summary>
        public void GameStart()
        {
            this._survivalTime = Time.realtimeSinceStartup;
            this._titleCanvas.enabled = false;
            this._resultCanvas.enabled = false;

            var allEntity = this._entityManager.GetAllEntities(Allocator.Temp);
            foreach (var entity in allEntity)
            {
                this._entityManager.DestroyEntity(entity);
            }
            allEntity.Dispose();

            // Create EnemySpawnSystem
            {
                var enemySpawnEntity = this._entityManager.CreateEntity(EnemySpawnSystemArchetype);
                this._entityManager.SetComponentData(enemySpawnEntity, new EnemySpawnSystemData());
                // 弾幕の種類数のみ動的に取得する都合上、インスタンスをコピーして手動で渡していく
                var settings = this._gameSettings.EnemySpawnSystemSettingsInstance;
                this._entityManager.AddSharedComponentData(
                    enemySpawnEntity,
                    new EnemySpawnSystemSettings
                    {
                        CooldownTime = settings.CooldownTime,
                        LimitFps = settings.LimitFps,
                        SpawnArea = settings.SpawnArea,
                        MaxBarrageType = System.Enum.GetNames(typeof(BarrageType)).Length
                    });
            }

            // Create Player
            {
                var playerSettings = this._gameSettings.PlayerSettingsInstance;
                this._playerEntity = this._entityManager.CreateEntity(PlayerArchetype);
                this._entityManager.SetComponentData(this._playerEntity, new Position2D { Value = this._gameSettings.PlayerCreatePosition });
                this._entityManager.SetComponentData(this._playerEntity, new PlayerInput());
                this._entityManager.SetComponentData(this._playerEntity, new TransformMatrix());
                this._entityManager.SetComponentData(this._playerEntity,
                    new PlayerLife
                    {
                        Value = playerSettings.LifeSettingsInstance.MaxLife,
                        Max = playerSettings.LifeSettingsInstance.MaxLife
                    });

                this._entityManager.AddSharedComponentData(this._playerEntity, PlayerLook);
                this._entityManager.AddSharedComponentData(this._playerEntity, this._gameSettings.PlayerSettingsInstance);
                this._entityManager.AddSharedComponentData(this._playerEntity, PlayerCollision);
                var colorSettings = this._gameSettings.PlayerColorSettingsInstance;
                var nColor = colorSettings.PlayerNormalColor;
                var dColor = colorSettings.PlayerDamagedColor;
                this._entityManager.AddSharedComponentData(this._playerEntity,
                    new PlayerColor
                    {
                        NormalColor = new float4(nColor.r, nColor.g, nColor.b, nColor.a),
                        DamagedColor = new float4(dColor.r, dColor.g, dColor.b, dColor.a)
                    });
            }
        }

        /// <summary>
        /// リザルト
        /// </summary>
        public void Result()
        {
            this._titleCanvas.enabled = false;
            this._resultCanvas.enabled = true;
            this._survivalTime = (float)Math.Round(Time.realtimeSinceStartup - this._survivalTime, 2);
            var builder = new StringBuilder();
            builder.Append("Survival Time : ").Append(this._survivalTime);
            this._survivalTimeText.text = builder.ToString();
        }

        /// <summary>
        /// Tweetボタン
        /// <summary>
        public void Tweet()
        {
#if ENABLE_UNITY_ROOM
            var builder = new StringBuilder();
            builder.Append("ギリギリ弾幕STG").Append("\n");
            builder.AppendFormat("あなたは{0}秒生き残れました", this._survivalTime).Append("\n");
            Debug.Log(builder.ToString());
#if WEBGL_ONLY
            naichilab.UnityRoomTweet.Tweet("last_minute_barrage_stg", builder.ToString(), "unityroom", "unity1week");
#endif
#endif
        }

        /// <summary>
        /// ランキングボタン
        /// </summary>
        public void Ranking()
        {
#if ENABLE_UNITY_ROOM
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(this._survivalTime);
#endif
        }

        #endregion // Public Functions

        // ----------------------------------------------------
        #region // Private Functions

        /// <summary>
        /// 渡した表示データからMeshInstanceRendererの生成
        /// </summary>
        /// <param name="data">表示データ</param>
        /// <returns>生成したMeshInstanceRenderer</returns>
        MeshInstanceRenderer CreateMeshInstanceRenderer(MeshInstanceRendererData data)
        {
            // Sprite to Mesh
            var mesh = new Mesh();

            // Unity.Mathematicsが行優先な都合上、Z-Upみたいな挙動になるので都合を合わせるためにMeshの頂点レベルで回しておく
            var vertices = Array.ConvertAll(data.Sprite.vertices, _ => (Vector3)_).ToList();
            Matrix4x4 mat = Matrix4x4.TRS(
                new Vector3(0f, 0f, 0f),
                Quaternion.Euler(90f, 0f, 0f),
                Vector3.one * this._gameSettings.ObjectScale);
            for (int i = 0; i < vertices.Count; ++i)
            {
                vertices[i] = mat.MultiplyPoint3x4(vertices[i]);
            }
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, data.Sprite.uv.ToList());
            mesh.SetTriangles(Array.ConvertAll(data.Sprite.triangles, _ => (int)_), 0);

            var matInst = new Material(data.Material);

            // 渡すマテリアルはGPU Instancingに対応させる必要がある
            var meshInstanceRenderer = new MeshInstanceRenderer();
            meshInstanceRenderer.mesh = mesh;
            meshInstanceRenderer.material = matInst;
            return meshInstanceRenderer;
        }

        #endregion // Private Functions
    }
}