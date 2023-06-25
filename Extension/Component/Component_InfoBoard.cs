using Aquila.ObjectPool;
using Aquila.Procedure;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object_DamageNumber = Aquila.ObjectPool.Object_DamageNumber;

namespace Aquila.Extension
{

    public class Component_InfoBoard : GameFrameworkComponent
    {
        /// <summary>
        /// 显示一个伤害数字实例
        /// </summary>
        public void ShowDamageNumber( string num, Vector3 worldPos )
        {
            var obj = GenObject<Object_DamageNumber>( typeof( Object_DamageNumber ).Name );
            if ( obj is null )
            {
                var go = Instantiate( _dmg_number_prefab );
                InitTransform( go.transform );
                var pool = GameEntry.ObjectPool.GetObjectPool<Object_DamageNumber>( typeof( Object_DamageNumber ).Name );
                pool.Register( Object_DamageNumber.Gen( go ), false );
                obj = pool.Spawn();
            }
            //init
            obj.Setup( obj.Target as GameObject );
            //set pos
            var rect_pos = WorldPos2BoardRectPos( worldPos, GameEntry.GlobalVar.MainCamera );
            obj.SetPos( rect_pos );
            obj.SetNumber( num, Color.red );
            //添加到队列中
            // _damage_number_queue.Enqueue(obj);
            _damage_number_spawn_dic.Add( obj.GetHashCode(), obj );
        }

        /// <summary>
        /// 获取一个hpbar，获取不到返回null
        /// </summary>
        public Object_HPBar GenHPBar()
        {
            Object_HPBar obj = GenObject<Object_HPBar>( typeof( Object_HPBar ).Name );
            if ( obj is null )//对象池里没对象，先创建
            {
                if ( _hp_bar_prefab == null )
                {
                    Log.Warning( "<color=red>Component_InfoBoard.GenHPBar()--->obj is null</color>" );
                    return null;
                }

                var go = Instantiate( _hp_bar_prefab ) as GameObject;
                InitTransform( go.transform );
                //spanw obj
                var pool = GameEntry.ObjectPool.GetObjectPool<Object_HPBar>( typeof( Object_HPBar ).Name );
                pool.Register( Object_HPBar.Gen( go ), false );
                obj = pool.Spawn();
            }
            obj.Setup( obj.Target as GameObject );
            return obj;
        }

        /// <summary>
        /// 初始化变换
        /// </summary>
        private void InitTransform( Transform tran )
        {
            tran.SetParent( _root );
            tran.localScale = Vector3.one;
        }

        /// <summary>
        /// 获取一个指定类型的对象池对象，拿不到返回null
        /// </summary>
        private T GenObject<T>( string pool_name ) where T : Object_Base
        {
            // var type_name = nameof(T);
            var pool = GameEntry.ObjectPool.GetObjectPool<T>( pool_name );
            if ( pool != null )
                return pool.Spawn() as T;

            Log.Warning( "<color=yellow>Component_InfoBoard.GenObject--->pool == null</color>" );
            return null;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public bool UnSpawn<T>( string pool_name, object obj ) where T : Object_Base
        {
            var pool = GameEntry.ObjectPool.GetObjectPool<T>( pool_name );
            if ( pool is null )
            {
                Log.Warning( "<color=yellow>Component_InfoBoard.UnSpawn--->pool == null</color>" );
                return false;
            }
            pool.Unspawn( obj );
            return true;
        }

        /// <summary>
        /// 世界坐标转信息板的矩形空间坐标
        /// </summary>
        public Vector2 WorldPos2BoardRectPos( Vector3 world_pos, Camera world_camera )
        {
            var screen_pos = RectTransformUtility.WorldToScreenPoint( world_camera, world_pos );
            RectTransformUtility.ScreenPointToWorldPointInRectangle( Rect, screen_pos, Camera,
                out var board_pos );
            return board_pos;
        }

        //-----------------------priv-----------------------
        public void Preload()
        {
            if ( _init_flag )
            {
                return;
            }

            //创建hpbar对象池和资源
            var hp_pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_HPBar>( typeof( Object_HPBar ).Name, 0xf );
            hp_pool.ExpireTime = 360f;
            GameEntry.Resource.LoadAsset
                (
                    @"Assets/Res/Prefab/UI/Item/HPBar.prefab",
                    new LoadAssetCallbacks( ( assetName, asset, duration, userData ) =>
                         {
                             _hp_bar_prefab = asset as GameObject;
                             if ( _hp_bar_prefab == null )
                             {
                                 Log.Warning( "<color=yellow>HPBar.prefab convert faild</color>" );
                                 return;
                             }

                             if ( GameEntry.Procedure.GetProcedure<Procedure_Prelaod>() is Procedure_Prelaod procedure )
                             {
                                 //#todo:主动通知流程加载完成，因为GF只有异步加载,暂时没时间加同步，先这样做了
                                 procedure.NotifyFlag( Procedure_Prelaod._infoboardHPBarLoadFinish );
                             }
                         },
                        LoadAssetFaildCallBack
                ) );

            //damage number对象池和资源
            var dm_pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_DamageNumber>( typeof( Object_DamageNumber ).Name );
            dm_pool.ExpireTime = hp_pool.ExpireTime;

            GameEntry.Resource.LoadAsset
                (
                    @"Assets/Res/Prefab/UI/Item/DamageNumber.prefab",
                    new LoadAssetCallbacks(
                        ( assetName, asset, duration, userData ) =>
                        {
                            _dmg_number_prefab = asset as GameObject;
                            if ( _dmg_number_prefab == null )
                            {
                                Log.Warning( "<color=yellow>DamageNumber.prefab convert faild</color>" );
                            }

                            if ( GameEntry.Procedure.CurrentProcedure is Procedure_Prelaod procedure )
                            {
                                procedure.NotifyFlag( Procedure_Prelaod._infoboardDmgNumberLoadFinish );
                            }
                        },
                        LoadAssetFaildCallBack
                    )
                );

            _init_flag = true;
        }

        /// <summary>
        /// 处理正在显示中的DamageNumber
        /// </summary>
        private void UpdateSpawningDamageNumber( float delta_time )
        {
            Object_DamageNumber temp = null;
            if ( _damage_number_spawn_dic.Count != 0 )
            {
                var iter = _damage_number_spawn_dic.GetEnumerator();
                while ( iter.MoveNext() )
                {
                    temp = iter.Current.Value;
                    if ( temp.TimesUp() )
                    {
                        //remove
                        _damage_number_unspawn_set.Add( iter.Current.Key );
                        continue;
                    }
                    else
                    {
                        temp.Move( delta_time );
                    }
                }
            }

            if ( _damage_number_unspawn_set.Count != 0 )
            {
                foreach ( var key in _damage_number_unspawn_set )
                {
                    if ( _damage_number_spawn_dic.TryGetValue( key, out temp ) )
                    {
                        _damage_number_spawn_dic.Remove( key );
                        UnSpawn<Object_DamageNumber>( typeof( Object_DamageNumber ).Name, temp.Target );
                    }
                }
                _damage_number_unspawn_set.Clear();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _init_flag = false;
            _damage_number_spawn_dic = new Dictionary<int, Object_DamageNumber>( 0xf );
            _damage_number_unspawn_set = new HashSet<int>( 0xf );
            // _damage_number_queue = new Queue<Object_DamageNumber>(0xf);
            // _damage_number_unspawn_queue = new Queue<Object_DamageNumber>(0xf);
        }

        /// <summary>
        /// 刷帧，主要处理伤害数字
        /// </summary>
        private void Update()
        {
            UpdateSpawningDamageNumber( Time.deltaTime );
        }

        private void LoadAssetFaildCallBack( string assetName, LoadResourceStatus status, string errorMessage, object userData )
        {
            Log.Warning( $"$<color=yellow>load asset {assetName} faild,status:{status.ToString()}</color>" );
        }

        //-----------------------fields-----------------------
        public RectTransform Rect
        {
            get
            {
                if ( _rect == null )
                    _rect = _root.GetComponent<RectTransform>();

                return _rect;
            }
        }
        private RectTransform _rect = null;

        /// <summary>
        /// 渲染信息板的画布
        /// </summary>
        public Canvas Canvas => _canvas;

        /// <summary>
        /// 渲染信息板用的相机
        /// </summary>
        public Camera Camera => _camera;

        /// <summary>
        /// 初始化标记
        /// </summary>
        private bool _init_flag = false;

        /// <summary>
        /// 根节点
        /// </summary>
        [SerializeField] private Transform _root = null;

        /// <summary>
        /// 画布
        /// </summary>
        private Canvas _canvas = null;

        /// <summary>
        /// hpbar预设
        /// </summary>
        private GameObject _hp_bar_prefab = null;

        /// <summary>
        /// 伤害数字预设
        /// </summary>
        private GameObject _dmg_number_prefab = null;

        /// <summary>
        /// 显示用的相机
        /// </summary>
        [SerializeField] private Camera _camera = null;

        /// <summary>
        /// 管理已经放出的DamageNumber实例
        /// </summary>
        private Dictionary<int, Object_DamageNumber> _damage_number_spawn_dic = null;

        /// <summary>
        /// 准备释放的DamageNumber集合管理
        /// </summary>
        private HashSet<int> _damage_number_unspawn_set = null;

        // /// <summary>
        // /// 保存并且处理伤害数字实例的队列
        // /// </summary>
        // private Queue<Object_DamageNumber> _damage_number_queue = null;
        //
        // /// <summary>
        // /// 伤害数字实例的回收队列
        // /// </summary>
        // private Queue<Object_DamageNumber> _damage_number_unspawn_queue = null;
    }
}
