using Aquila.Extension;
using Aquila.Fight.Addon;
using Aquila.GameTag;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// Actor基类
    /// </summary>
    public abstract partial class Actor_Base : EntityLogic
    {
        #region public methods

        /// <summary>
        /// 添加和此actor关联的actor
        /// </summary>
        public bool AddRelevance( int actorID )
        {
            return _relevanceActorSet.Add( actorID );
        }

        /// <summary>
        /// 关联的actorID列表
        /// </summary>
        public HashSet<int> RelevanceActors
        {
            get => _relevanceActorSet;
        }

        /// <summary>
        /// 移除tag
        /// </summary>
        public void RemoveTag( ushort tagToRemove )
        {
            _tagContainer.Remove( tagToRemove );
        }

        /// <summary>
        /// 添加tag
        /// </summary>
        public void AddTag( ushort tagToAdd )
        {
            _tagContainer.Add( tagToAdd );
        }

        /// <summary>
        /// 触发addon事件
        /// </summary>
        public void Notify( int eventType, object param )
        {
            _eventAddon.Notify( eventType, param );
        }

        /// <summary>
        /// 尝试获取一个addon
        /// </summary>
        protected bool TryGetAddon<T>( out T targetAddon ) where T : Addon_Base
        {
            targetAddon = _instance.GetAddon<T>();
            return targetAddon != null;
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
            _eventAddon.Ready();
            foreach ( var addon in GetAllAddon() )
                GameEntry.Module.GetModule<Module_ProxyActor>().AddToAddonSystem( addon );

            base.OnShow( userData );
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            _tagContainer.Reset();
            _relevanceActorSet.Clear();
            SetWorldPosition( new Vector3( 999f, 999f, 999f ) );

            //Module_ProxyActor注销和注册的逻辑请依赖entity的回调来调用（比如onHide，onShow，onInit，onRecycle等），
            //这样可以避免Module_ProxyActor主动清掉actor实例数据，然后entity访问不到的问题
            foreach ( var addon in GetAllAddon() )
                GameEntry.Module.GetModule<Module_ProxyActor>().RemoveFromAddonSystem( addon );

            base.OnHide( isShutdown, userData );
        }

        /// <summary>
        /// dispose自己所有的addon
        /// </summary>
        protected override void OnRecycle()
        {
            //addon
            _eventAddon.UnRegisterAll();
            _eventAddon = null;
            _relevanceActorSet = null;
            HostID = Component_GlobalVar.InvalidGUID;
            ExtensionRecycle();
            SetRoleMetaID( -1 );
            gameObject.tag = String.Empty;
            _tagContainer = null;
            GameEntry.Module.GetModule<Module_ProxyActor>().UnRegister( ActorID );
            base.OnRecycle();
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
            var res = GameEntry.Module.GetModule<Module_ProxyActor>().Register( this );
            if ( !res.succ )
            {
                Log.Warning( $"<color=yellow>ActorBase.OnInit()--->!res.succ!</color>" );
                return;
            }

            _instance = res.instance;
            OnInitActor( userData );
            AddAddon();
            InitAddons( res.instance );

            _relevanceActorSet = new HashSet<int>();
            _allAddonInitDone = true;
            _tagContainer = new TagContainer( OnTagChange );
        }

        /// <summary>
        /// Tag改变
        /// </summary>
        protected virtual void OnTagChange( Int64 tag, Int64 changedTag, bool isADD )
        {
        }

        /// <summary>
        /// 重置自己和addon的数据为初始值
        /// </summary>
        public virtual void Reset()
        {
            var addons = _instance.AllAddons();
            foreach ( var addon in addons )
                addon.Reset();
        }


        /// <summary>
        /// 为自身添加一个Addon
        /// </summary>                                                 
        protected T AddAddon<T>() where T : Addon_Base, new()
        {
            var addonToAdd = new T();
            addonToAdd.OnAdd();
            GameEntry.Module.GetModule<Module_ProxyActor>().AddAddon( this, addonToAdd );
            return addonToAdd;
        }

        /// <summary>
        /// 获取自己的全部addon，没有返回一个空数组
        /// </summary>
        protected Addon_Base[] GetAllAddon()
        {
            return _instance.AllAddons();
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
            _eventAddon = AddAddon<Addon_Event>();
        }

        /// <summary>
        /// 移除addon
        /// </summary>
        protected void RemoveAddon( Addon_Base addon )
        {
            addon.OnRemove();
            GameEntry.Module.GetModule<Module_ProxyActor>().RemoveFromAddonSystem( addon);
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
        public abstract RoleType ActorType { get; }

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
        protected Addon_Event _eventAddon = null;

        /// <summary>
        /// 数据组件
        /// </summary>
        protected Addon_Data _dataAddon = null;

        /// <summary>
        /// actor实例
        /// </summary>
        private ActorInstance _instance = null;

        /// <summary>
        /// tag管理器
        /// </summary>
        protected TagContainer _tagContainer = null;

        /// <summary>
        /// 关联actor集合
        /// </summary>
        private HashSet<int> _relevanceActorSet = null;

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
