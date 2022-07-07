using GameFramework;
using GameFramework.Event;
using Aquila.Fight.Addon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Fight.Addon.AddonBase;
using Aquila.Config;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// Actor基类
    /// </summary>
    public abstract class TActorBase : EntityLogic
    {
        #region public methods

        public void Trigger( ActorEventEnum type, params object[] param )
        {
            _eventAddon?.Trigger( type, param );
        }

        /// <summary>
        /// 尝试获取一个addon
        /// </summary>
        public bool TryGetAddon<T>( out T targetAddon ) where T : AddonBase
        {
            targetAddon = null;
            if ( _addonDic is null || _addonDic.Count == 0 )
                return false;

            if ( _addonDic.TryGetValue( typeof( T ).GetHashCode(), out var addon ) )
            {
                targetAddon = addon as T;
                return targetAddon != null;
            }
            return false;
        }

        /// <summary>
        /// 有效性检查，会检查所有的addon
        /// </summary>
        public bool Valid()
        {
            uint errCode = AddonValidErrorCodeEnum.NONE;
            uint tempCode = 0;
            string errString = string.Empty;
            foreach ( var iter in _addonDic )
            {
                tempCode = iter.Value.Valid();
                errCode |= tempCode;
                if ( errCode != AddonValidErrorCodeEnum.NONE )
                    throw new GameFrameworkException( AddonValidErrorCodeEnum.ErrCode2String( tempCode ) );
            }

            return errCode == AddonValidErrorCodeEnum.NONE;
        }


        /// <summary>
        /// 注册事件到actor自身，OnShow时调用
        /// </summary>
        public void RegisterActorEvent( ActorEventEnum type, Action<int, object[]> action )
        {
            //Debug.Log( $"<color=white>Actor{ActorID}--->RegisterActorEvent{type}</color>" );
            if ( !_eventAddon.Register( type, action ) )
                throw new GameFrameworkException( "!_eventAddon.Register( type, action )" );
        }

        /// <summary>
        /// 注销事件到actor自身，回收时调用
        /// </summary>
        public void UnRegisterActorEvent( ActorEventEnum type )
        {
            if ( !_eventAddon.UnRegister( type ) )
                throw new GameFrameworkException( "!_eventAddon.UnRegister( type )" );
        }

        /// <summary>
        /// 是否为我方阵营的actor，是返回true
        /// </summary>
        public bool IsMine()
        {
            //return HostID == GameFrameworkMode.GetModule<AccountModule>().Guid && HostID != GlobalVar.INVALID_GUID;
            return true;
        }

        #endregion

        #region set

        /// <summary>
        /// 设置layer
        /// </summary>
        public void SetLayer()
        {

        }

        /// <summary>
        /// 设置隐藏标记
        /// </summary>
        public void SetHideFlag( bool isHideImmidiate ) => HideImmidiate = isHideImmidiate;

        /// <summary>
        /// 设置ActorID
        /// </summary>
        public virtual void SetActorID( int id ) => ActorID = id;


        public void SetQuaternion( Quaternion rotationToSet )
        {
            CachedTransform.rotation = rotationToSet;
        }

        /// <summary>
        /// 设置在entityGroup下的本地坐标
        /// </summary>
        public void SetLocalPosition( Vector3 posToSet )
        {
            if ( CachedTransform == null )
                return;

            CachedTransform.localPosition = posToSet;
        }

        /// <summary>
        /// 设置世界坐标
        /// </summary>
        public void SetWorldPosition( Vector3 posToSet )
        {
            //Debug.Log( $"<color=orange>SetWorldPosition,actorID:{ActorID},pos to set:{posToSet}</color>" );
            if ( CachedTransform == null )
                return;

            CachedTransform.position = posToSet;
        }

        public void SetWorldPosition( Vector2 posToSet )
        {
            Debug.Log( $"<color=orange>SetWorldPosition,actorID:{ActorID}</color>" );
            if ( CachedTransform == null )
                return;

            SetWorldPosition
                ( 
                    new Vector3
                        ( 
                            posToSet.x, 
                            Utils.FightScene.TerrainPositionY( posToSet.x, posToSet.y ), 
                            posToSet.y 
                        ) 
                );
        }

        /// <summary>
        /// 设置tag
        /// </summary>
        public void SetTag( string tag )
        {
            if ( tag.Equals( gameObject.tag ) )
                return;

            Tools.SetTag( tag, gameObject, true );
        }

        /// <summary>
        /// 设置hostID
        /// </summary>
        public void SetHostID( ulong hostID ) => HostID = hostID;

        /// <summary>
        /// 设置阵营
        /// </summary>
        public void SetForceType( int type )
        {
            if ( type < ( int ) ForceTypeEnum.Zero || type >= ( int ) ForceTypeEnum.Maximun )
            {
                Log.Error( "type < ForceTypeEnum.Zero || type >= ForceTypeEnum.Maximun" );
                return;
            }

            ForceType = type;
        }

        /// <summary>
        /// 设置index
        /// </summary
        public void SetIndex( int idx )
        {
            idx = Index;

#if UNITY_EDITOR
            var inspector = gameObject.GetComponent<ActorInspector>();
            if ( inspector != null )
                inspector.SetIndex( idx );
#endif
        }

        public void Setup
            (
                string tag,
                int index,
                int actorID,
                ulong hostID,
                int forceType,
                int dataID
            )
        {
            Setup( tag, index, actorID, hostID, forceType );
            SetDataID( dataID );
            Reset();

            //↓↓↓ for test ↓↓↓//
#if UNITY_EDITOR
            var inspector = gameObject.GetOrAddComponent<ActorInspector>();
            inspector.Setup( this, dataID );
#endif
        }

        /// <summary>
        /// 设置actor的基本信息
        /// </summary>
        public void Setup( string tag, int index, int actorID, ulong hostID, int forceType = -1 )
        {

        }

        public void Setup( string tag, int index, int actorID, ulong hostID )
        {

        }

        public virtual void SetDataID( int roleBaseID )
        {
            if ( _dataAddon is null )
                return;

            var meta = TableManager.GetRoleBaseAttrByID( roleBaseID, 0 );
            if ( meta is null )
                throw new GameFrameworkException( "meta is null" );

            _dataAddon.SetObjectDataValue( DataAddonFieldTypeEnum.OBJ_META_ROLEBASE, meta );
            //ResetData();

#if UNITY_EDITOR
            var inspector = gameObject.GetComponent<ActorInspector>();
            if ( inspector != null )
                inspector.SetDataID( meta.Id );
#endif
        }

        #endregion


        #region override
        protected override void OnShow( object userData )
        {
            base.OnShow( userData );
            Register();
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            base.OnHide( isShutdown, userData );
            CachedTransform.position = new Vector3( 999f, 999f, 999f );
        }

        /// <summary>
        /// dispose自己所有的addon
        /// </summary>
        protected override void OnRecycle()
        {
            base.OnRecycle();
            UnRegister();

            //if (_addonDic != null)
            //{
            //    var iter = _addonDic.GetEnumerator();
            //    while (iter.MoveNext())
            //        iter.Current.Value?.Dispose();
            //}

            Index = -1;
            HostID = GlobalVar.INVALID_GUID;
            ForceType = -1;
            Area = -1;
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
            InitAddons();
            if ( gameObject.GetComponent<BoxCollider>() == null )
            {
                var collider = gameObject.AddComponent<BoxCollider>();
                collider.size = new Vector3( .8f, .8f, .8f );
                collider.isTrigger = true;
            }

            _allAddonInitDone = true;
        }

        /// <summary>
        /// 注册GF消息，在OnShow的时候调用
        /// </summary>
        protected virtual void Register()
        {
            GameEntry.Event.Subscribe( ActorDieEventArgs.EventId, OnOtherActorDie );
        }

        /// <summary>
        /// 注销GF消息，在回收的时候调用
        /// </summary>
        protected virtual void UnRegister()
        {
            GameEntry.Event.Unsubscribe( ActorDieEventArgs.EventId, OnOtherActorDie );
        }

        /// <summary>
        /// AbilityAction准备
        /// </summary>
        protected virtual bool OnPreAbilityAction( int abilityID )
        {
            return true;
        }

        /// <summary>
        /// AbilityAction之后
        /// </summary>
        protected virtual bool OnAfterAbilityAction()
        {
            return true;
        }

        /// <summary>
        /// 重置自己和addon的数据为初始值，而非清理，
        /// 建议的调用时机：初始化；回收时，
        /// </summary>
        public virtual void Reset()
        {
            SetHideFlag( false );
            ResetData();
            if ( _addonDic != null )
            {
                var iter = _addonDic.GetEnumerator();
                while ( iter.MoveNext() )
                    iter.Current.Value?.Reset();
            }

        }

        /// <summary>
        /// 重置数据,setDataID(),Reset时调用
        /// </summary> 
        protected virtual void ResetData()
        {

        }

        protected virtual void OnOtherActorDie( object sender, GameEventArgs e )
        {

        }

        /// <summary>
        /// 同步属性
        /// </summary>
        public virtual void SyncAttrByCache( ActorAttrCache cache )
        {
            if ( !TryGetAddon<DataAddon>( out var addon ) )
                return;

            if ( cache.HasMaxHP )
                addon.SetIntDataValue( DataAddonFieldTypeEnum.INT_MAX_HP, cache.MaxHP );//最大血量

            InfoBoardAddon boardAddon = null;
            if ( cache.HasCurHP )
            {
                var oldHp = addon.GetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, -1 );
                var newHP = cache.CurHP;
                addon.SetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, newHP );//当前血量
                var maxHP = addon.GetIntDataValue( DataAddonFieldTypeEnum.INT_MAX_HP, -1 );

                if ( TryGetAddon<InfoBoardAddon>( out boardAddon ) && maxHP != -1 )
                    ShowHP();

                void ShowHP()
                {
                    boardAddon.ChangeHPValue( ( float ) newHP / maxHP, newHP );
                    //Log.Info( $"<color=green>set currHp:</color>,value:{newHP},actorID:{ActorID}" );
                    //if ( boardAddon.HPBarInitFlag )
                    //{
                    //    boardAddon.ChangeHPValue( ( float ) newHP / maxHP, oldHp == -1 ? -1 : newHP );
                    //    Log.Info( $"<color=green>set currHp:</color>,value:{newHP},actorID:{ActorID}" );
                    //}
                    //else
                    //{
                    //    boardAddon.SetHPFlagDone();
                    //    boardAddon.ShowHPBarItem( true );
                    //    boardAddon.ChangeHPValue( ( float ) newHP / maxHP, -1 );
                    //}
                }
            }

            if ( cache.HasCurMP )
                addon.SetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_MP, cache.CurMP );//当前蓝

            if ( cache.HasMaxMP )
                addon.SetIntDataValue( DataAddonFieldTypeEnum.INT_MAX_MP, cache.MaxMP );//最大蓝

            if ( cache.HasShield )
            {
                addon.SetIntDataValue( DataAddonFieldTypeEnum.INT_SHIELD, cache.Shield );
                var shield = addon.GetIntDataValue( DataAddonFieldTypeEnum.INT_SHIELD, 0 );
                if ( boardAddon != null )
                    boardAddon.SetShieldValue( shield, addon.GetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, 0 ), addon.GetIntDataValue( DataAddonFieldTypeEnum.INT_MAX_HP, 0 ) );
            }

            //移速
            if ( cache.HasMoveSpeed )
            {
                //Debug.Log( "----------->move speed:" + cache.MoveSpeed );
                addon.SetIntDataValue( DataAddonFieldTypeEnum.INT_MOVE_SPEED, cache.MoveSpeed );
                var speed = 1f;
                if ( TryGetAddon<NavAddon>( out var navAddon ) )
                {
                    speed = addon.GetIntDataValue( DataAddonFieldTypeEnum.INT_MOVE_SPEED, 1 ) / 1000f;
                    navAddon.SetSpeed( speed );
                }

#if UNITY_EDITOR
                var inspector = gameObject.GetComponent<ActorInspector>();
                if ( inspector != null )
                    inspector.Attr_Speed = speed;
#endif
            }
        }

        public virtual void SyncAttrByPak( GC_SYN_OBJ_ATTR pak )
        {
            if ( pak is null )
                return;

            var cache = ReferencePool.Acquire<ActorAttrCache>();
            cache.Set( pak );
            SyncAttrByCache( cache );
        }

        #endregion

        /// <summary>
        /// 添加一个Addon到自身
        /// </summary>                                                 
        protected T AddAddon<T>() where T : AddonBase, new()
        {
            var addonToAdd = GetAddon<T>();
            if ( addonToAdd != null )
            {
                Log.Debug( $"addon <color=white>{typeof( T )}</color> has exist on this actor:{Name}" );
                return addonToAdd;
            }

            addonToAdd = new T();
            addonToAdd.Init( this, gameObject , CachedTransform );
            _addonDic.Add( typeof( T ).GetHashCode(), addonToAdd );

            addonToAdd.OnAdd();
            return addonToAdd;
        }

        /// <summary>
        /// 获取自身的addon，没有返回空
        /// </summary>
        [Obsolete( "推荐使用TryGetAddon<T>" )]
        public T GetAddon<T>() where T : AddonBase
        {
            if ( _addonDic is null )
                _addonDic = new Dictionary<int, AddonBase>();

            if ( _addonDic.Count == 0 )
                return null;

            _addonDic.TryGetValue( typeof( T ).GetHashCode(), out var addon );
            return addon as T;
        }

        /// <summary>
        /// 初始化自己的Addons
        /// </summary>
        protected virtual void InitAddons()
        {
            _eventAddon = AddAddon<EventAddon>();
            _dataAddon = AddAddon<DataAddon>();
        }

        protected TActorBase()
        {

        }

        #region fields

        /// <summary>
        /// actor类型
        /// </summary>
        public abstract ActorTypeEnum ActorType { get; }

        /// <summary>
        /// ActorID(ObjID)
        /// </summary>
        public int ActorID { get; private set; } = -1;

        /// <summary>
        /// 组件初始化标记
        /// </summary>
        public bool AddonInitFlag => _allAddonInitDone;

        /// <summary>
        /// 宿主ID
        /// </summary>
        public ulong HostID { get; private set; } = GlobalVar.INVALID_GUID;

        /// <summary>
        /// 阵营
        /// </summary>
        public int ForceType { get; private set; } = -1;

        /// <summary>
        /// 组件初始化标记
        /// </summary>
        private bool _allAddonInitDone = false;

        /// <summary>
        /// 事件组件
        /// </summary>
        private EventAddon _eventAddon = null;
        protected DataAddon _dataAddon = null;

        /// <summary>
        /// actor身上的组件保存，key为 type的hashCode
        /// </summary>
        private Dictionary<int, AddonBase> _addonDic = new Dictionary<int, AddonBase>();

        #endregion
    }


    //#region ActorInspector
    ///// <summary>
    ///// 用于记录actor信息，抽时间写成inspector
    ///// </summary>
    //internal class ActorInspector : MonoBehaviour
    //{
    //    public void Setup( TActorBase actor, int dataID )
    //    {
    //        if ( actor is null )
    //            return;

    //        Actor_Obj_ID = actor.ActorID.ToString();
    //        Entity_Group = actor.Entity.EntityGroup.Name;
    //        Index = actor.Index;
    //        Host_GUID = actor.HostID;
    //        Force_Type = actor.ForceType;
    //        Data_ID = dataID;
    //        Model_Path = actor.Entity.EntityAssetName;
    //        _tactor = actor;
    //        if ( _tactor.TryGetAddon<DataAddon>( out _dataAddon ) )
    //        {
    //            var meta = _dataAddon.GetObjectDataValue<Tab_RoleBaseAttr>(DataAddonFieldTypeEnum.OBJ_META_ROLEBASE);
    //            if ( meta != null )
    //                Configure_Speed = meta.MoveSpeed;
    //        }
    //    }

    //    private void Update()
    //    {
    //        curr_HP_Field = _dataAddon.GetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP ).ToString();
    //        max_HP_Field = _dataAddon.GetIntDataValue( DataAddonFieldTypeEnum.INT_MAX_HP ).ToString();
    //    }

    //    /// <summary>
    //    /// 设置状态
    //    /// </summary>
    //    public void SetState( string state ) => CURR_STATE = state;

    //    /// <summary>
    //    /// 设置dataID
    //    /// </summary>
    //    public void SetDataID( int dataID ) => Data_ID = dataID;
    //    public void SetIndex( int idx ) => Index = idx;
    //    public float Attr_Speed = -1;
    //    public float Configure_Speed = -1;
    //    public string Actor_Obj_ID = string.Empty;
    //    public string Entity_Group = string.Empty;
    //    public int Index = -1;
    //    public ulong Host_GUID = GlobeVar.INVALID_GUID;
    //    public int Force_Type = -1;
    //    public int Data_ID = -1;
    //    public string Model_Path = string.Empty;
    //    public string CURR_STATE = string.Empty;

    //    public string curr_HP_Field = string.Empty;
    //    public string max_HP_Field = string.Empty;

    //    private TActorBase _tactor = null;
    //    private DataAddon _dataAddon = null;
    //}
    //#endregion
}
