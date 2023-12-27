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
        /// 移除和此actor关联的actor
        /// </summary>
        public bool RemoveRelevane( int actorID )
        {
            return _relevanceActorSet.Remove( actorID );
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
        /// 获取actor身上的一个tag
        /// </summary>
        public bool ContainsTag(ushort tagToGet)
        {
            return _tagContainer.Contains(tagToGet);
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
            // Reset();
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
            gameObject.tag = tag;
        }

        //--------------------override--------------------
        protected override void OnShow( object userData )
        {
            var regSucc = GameEntry.Module.GetModule<Module_ProxyActor>().Register( this );
            if ( !regSucc.regSucc )
            {
                Log.Warning( $"<color=yellow>ActorBase.OnInit()--->!res.succ!</color>" );
                return;
            }

            _instance = regSucc.instance;
            OnInitActor( userData );
            AddAddon();
            InitAddons( regSucc.instance );

            foreach ( var addon in GetAllAddon() )
                GameEntry.Module.GetModule<Module_ProxyActor>().AddToAddonSystem( addon );

            _eventAddon.Ready();
            base.OnShow( userData );
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            SetWorldPosition( new Vector3( 999f, 999f, 999f ) );

            _tagContainer.Reset();
            _relevanceActorSet.Clear();
            _eventAddon.UnRegisterAll();

            ExtensionRecycle();
            SetRoleMetaID( -1 );

            //Module_ProxyActor注销和注册的逻辑请依赖entity的回调来调用（比如onHide，onShow，onInit，onRecycle等），
            //这样可以避免Module_ProxyActor主动清掉actor实例数据，然后entity访问不到的问题
            var unRegSucc = GameEntry.Module.GetModule<Module_ProxyActor>().UnRegister( ActorID );
            if (!unRegSucc)
                Log.Warning($"Actor_Base.OnHide()---->!unRegSucc,actor id:{ActorID}");
                
            base.OnHide( isShutdown, userData );
        }

        /// <summary>
        /// dispose自己所有的addon
        /// </summary>
        protected override void OnRecycle()
        {
            base.OnRecycle();
            _relevanceActorSet.Clear();
            _tagContainer = null;
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
            if ( _relevanceActorSet is null )
                _relevanceActorSet = new HashSet<int>();

            if ( _tagContainer is null )
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
            //将actor和addon关联
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
            GameEntry.Module.GetModule<Module_ProxyActor>().RemoveFromAddonSystem( addon );
        }

        /// <summary>
        /// Actor自定义数据的初始化
        /// </summary>
        protected virtual void OnInitActor( object userData )
        {
            if(userData is Actor_Base_EntityData data)
                Setup(data._roleMetaID);
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
        private HashSet<int> _relevanceActorSet;

        #endregion
    }

    public class Actor_Base_EntityData : EntityData
    {
        public Actor_Base_EntityData(int entityId, int typeId) : base(entityId, typeId)
        {
        }

        /// <summary>
        /// 角色meta表ID
        /// </summary>
        public int _roleMetaID = -1;
    }
}
