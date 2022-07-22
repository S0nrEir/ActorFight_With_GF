using GameFramework;
using GameFramework.Event;
using Aquila.Fight.Addon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Fight.Addon.AddonBase;
using Aquila.Config;
using Aquila.Event;

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
                            Tools.Fight.TerrainPositionY(string.Empty, posToSet.x, posToSet.y ), //#todo设置坐标加上layer
                            posToSet.y 
                        ) 
                );
        }

        /// <summary>
        /// 设置hostID
        /// </summary>
        public void SetHostID( ulong hostID ) => HostID = hostID;

        public void Setup
            (
                string tag,
                int actor_id
            )
        {
            SetTag( tag );
            SetActorID( actor_id );
            Reset();
        }

        /// <summary>
        /// 设置actor的ID
        /// </summary>
        public void SetActorID( int actor_id )
        {
            ActorID = actor_id;
        }

        /// <summary>
        /// 设置actor的Tag
        /// </summary>
        private void SetTag( string tag )
        {
            if ( gameObject.tag.Equals( tag ) )
                return;

            gameObject.tag = tag;
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
            HostID = GlobalVar.INVALID_GUID;
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
        }

        /// <summary>
        /// 注销GF消息，在回收的时候调用
        /// </summary>
        protected virtual void UnRegister()
        {
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

        #endregion

        /// <summary>
        /// 添加一个Addon到自身
        /// </summary>                                                 
        protected T AddAddon<T>() where T : AddonBase, new()
        {
            //var addonToAdd = GetAddon<T>();
            if ( TryGetAddon<T>( out var addonToAdd ) )
            {
                Log.Debug( $"addon <color=white>{typeof( T )}</color> has exist on this actor:{Name}" );
                return addonToAdd;
            }
            else
            {
                addonToAdd = new T();
                addonToAdd.Init( this, gameObject, CachedTransform );
                _addonDic.Add( typeof( T ).GetHashCode(), addonToAdd );

                addonToAdd.OnAdd();
                return addonToAdd;
            }
        }

        /// <summary>
        /// 初始化自己的Addons
        /// </summary>
        protected virtual void InitAddons()
        {
            _eventAddon = AddAddon<EventAddon>();
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
