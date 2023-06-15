using Aquila.Extension;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// Actor基类
    /// </summary>
    public abstract partial class Actor_Base : EntityLogic
    {
        #region public methods
        /// <summary>
        /// 触发Actor事件
        /// </summary>
        public void Trigger( ActorEventEnum type, params object[] param )
        {
            _event_addon?.Trigger( type, param );
        }

        /// <summary>
        /// 尝试获取一个addon
        /// </summary>
        private bool TryGetAddon<T>( out T targetAddon ) where T : Addon_Base
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
        /// 注册事件到actor自身，OnShow时调用
        /// </summary>
        public void RegisterActorEvent( ActorEventEnum type, Action<int, object[]> action )
        {
            if ( !_event_addon.Register( type, action ) )
                throw new GameFrameworkException( "!_eventAddon.Register( type, action )" );
        }

        /// <summary>
        /// 注销事件到actor自身，回收时调用
        /// </summary>
        public void UnRegisterActorEvent( ActorEventEnum type )
        {
            if ( !_event_addon.UnRegister( type ) )
                throw new GameFrameworkException( "!_eventAddon.UnRegister( type )" );
        }
        #endregion

        //--------------------set--------------------
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
            if ( CachedTransform == null )
                return;

            CachedTransform.position = posToSet;
        }

        /// <summary>
        /// 基于欧拉角设置旋转
        /// </summary>
        public void SetRotation( Vector3 rotation )
        {
            if ( CachedTransform == null )
                return;

            CachedTransform.rotation = Quaternion.Euler( rotation );
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
                            Tools.Fight.TerrainPositionY( string.Empty, posToSet.x, posToSet.y ), //记得设置坐标加上layer
                            posToSet.y
                        )
                );
        }

        /// <summary>
        /// 自定义初始设置
        /// </summary>
        public virtual void Setup( int roleMetaID )
        {
            SetRoleMetaID( roleMetaID );
            Reset();
        }

        /// <summary>
        /// 设置角色表配置ID
        /// </summary>
        private void SetRoleMetaID( int roleMetaID )
        {
            RoleMetaID = roleMetaID;
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

        //--------------------override--------------------
        protected override void OnShow( object userData )
        {
            base.OnShow( userData );
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            SetWorldPosition( new Vector3( 999f, 999f, 999f ) );
            GameEntry.Module.GetModule<Module_ProxyActor>().UnRegister( ActorID );
            base.OnHide( isShutdown, userData );
        }

        /// <summary>
        /// dispose自己所有的addon
        /// </summary>
        protected override void OnRecycle()
        {
            //dispose all addon
            var iter = _addonDic.GetEnumerator();
            Addon_Base addon = null;
            while ( iter.MoveNext() )
            {
                addon = iter.Current.Value;
                addon.Dispose();
            }

            GameEntry.Module.GetModule<Module_ProxyActor>().UnRegister( ActorID );
            UnRegister();
            HostID = Component_GlobalVar.InvalidGUID;
            ExtensionRecycle();
            SetRoleMetaID( -1 );
            gameObject.tag = String.Empty;
            base.OnRecycle();
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
            OnInitActor( userData );
            AddAddon();
            var res = GameEntry.Module.GetModule<Module_ProxyActor>().Register( this, GetAllAddon() );
            if ( res.succ )
                InitAddons( res.instance );

            _allAddonInitDone = true;
        }

        /// <summary>
        /// 注册GF消息，在OnShow的时候调用,#todo_可能是无用函数，日后考虑删除
        /// </summary>
        protected virtual void Register()
        {
        }

        /// <summary>
        /// 注销GF消息，在回收的时候调用,#todo_可能是无用函数，日后考虑删除
        /// </summary>
        protected virtual void UnRegister()
        {
        }

        /// <summary>
        /// 重置自己和addon的数据为初始值
        /// </summary>
        public virtual void Reset()
        {
            if ( _addonDic != null )
            {
                var iter = _addonDic.GetEnumerator();
                while ( iter.MoveNext() )
                    iter.Current.Value?.Reset();
            }
        }


        /// <summary>
        /// 为自身添加一个Addon
        /// </summary>                                                 
        protected T AddAddon<T>() where T : Addon_Base, new()
        {
            if ( TryGetAddon<T>( out var addonToAdd ) )
            {
                Log.Debug( $"addon <color=white>{typeof( T ).ToString()}</color> has exist on this actor:{Name}" );
                return addonToAdd;
            }
            else
            {
                addonToAdd = new T();
                _addonDic.Add( typeof( T ).GetHashCode(), addonToAdd );

                addonToAdd.OnAdd();
                return addonToAdd;
            }
        }

        /// <summary>
        /// 获取自己的全部addon，没有返回一个空数组
        /// </summary>
        protected Addon_Base[] GetAllAddon()
        {
            if ( _addonDic is null || _addonDic.Count == 0 )
            {
                Log.Warning( "GetAllAddon--->_addonDic is null || _addonDic.Count == 0" );
                return new Addon_Base[0];
            }

            Addon_Base[] addons = new Addon_Base[_addonDic.Count];
            var idx = 0;
            foreach ( var kv in _addonDic )
                addons[idx++] = kv.Value;

            var iter = _addonDic.GetEnumerator();

            return addons;
        }

        /// <summary>
        /// 初始化自己的Addons
        /// </summary>
        protected virtual void InitAddons( Module_ProxyActor.ActorInstance instance )
        {
            var addons = GetAllAddon();
            foreach ( var addon in addons )
            {
                addon.Init( instance );
                addon.Reset();
            }
        }

        /// <summary>
        /// 添加addon
        /// </summary>
        protected virtual void AddAddon()
        {
            _event_addon = AddAddon<Addon_Event>();
        }

        /// <summary>
        /// Actor自定义数据的初始化
        /// </summary>
        protected virtual void OnInitActor( object user_data )
        {
        }

        protected Actor_Base()
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
        public int ActorID => Entity.Id;

        /// <summary>
        /// 角色表格配置ID
        /// </summary>
        public int RoleMetaID { get; private set; } = -1;

        /// <summary>
        /// 组件初始化标记
        /// </summary>
        public bool AddonInitFlag => _allAddonInitDone;

        /// <summary>
        /// 宿主ID
        /// </summary>
        public ulong HostID { get; private set; } = Component_GlobalVar.InvalidGUID;

        /// <summary>
        /// 组件初始化标记
        /// </summary>
        private bool _allAddonInitDone = false;

        /// <summary>
        /// 事件组件
        /// </summary>
        private Addon_Event _event_addon = null;
        protected Addon_Data _dataAddon = null;

        /// <summary>
        /// actor身上的组件保存，key为 type的hashCode
        /// </summary>
        private Dictionary<int, Addon_Base> _addonDic = new Dictionary<int, Addon_Base>();

        #endregion
    }


    //#region ActorInspector
    ///// <summary>
    ///// 用于记录actord信息的面板,抽时间写成inspector
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
