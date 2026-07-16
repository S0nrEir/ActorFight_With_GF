using System.Collections.Generic;
using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Module
{

    /// <summary>
    /// actor的管理器，管理了所有actor的实例
    /// </summary>
    public class Module_ActorMgr : GameFrameworkModuleBase
    {
        #region pub



        /// <summary>
        /// 获取指定actor的对应基础属性
        /// </summary>
        public (bool succ, float value) GetActorBaseAttr( int actorID, /*Actor_Attr*/ actor_attribute type )
        {
            var instance = Get( actorID );
            if ( instance is null )
                return (false, 0f);

            return instance.GetAddon<Addon_BaseAttrNumric>().GetBaseValue( type );
        }

        /// <summary>
        /// 获取actor某个技能的冷却
        /// </summary>
        public (float remain, float duration) GetCoolDown( int actorID, int abilityID )
        {
            var instance = Get( actorID );
            if ( instance is null )
                return (1f, 1f);

            var addon = instance.GetAddon<Addon_Ability>();
            var cd = addon.CoolDown( abilityID );
            return (cd.remain, cd.duration);
        }

        /// <summary>
        /// 获取指定actor对应的修正属性
        /// </summary>
        public (bool succ, float value) GetCorrectionAttr( int actorID, /*Actor_Base_Attr*/ actor_attribute type )
        {
            var instance = Get( actorID );
            if ( instance is null )
                return (false, 0f);

            var correctionVal = instance.GetAddon<Addon_BaseAttrNumric>().GetCorrectionValue( type, 0 );
            return (correctionVal != 0, correctionVal);
        }

        /// <summary>
        /// 获取一个actor的位置，拿不到返回invlaidPosition
        /// </summary>
        public Vector3 GetPosition( int actorID )
        {
            var instance = Get( actorID );
            return instance is null ? GameEntry.GlobalVar.InvalidPosition : instance.Actor.CachedTransform.position;
        }

        /// <summary>
        /// 获取actor的根Transform组件
        /// </summary>
        public Transform GetActorTransform( int actorID )
        {
            return Get( actorID ).Actor.CachedTransform;
        }

        /// <summary>
        /// 将actor和addon关联，为一个actor实例添加一个addon
        /// </summary>
        public (bool succ, ActorInstance instance) AddAddon( Actor_Base actor, Addon_Base addon )
        {
            if ( actor is null )
            {
                Tools.Logger.Warning( "<color=yellow>actor is null.</color>" );
                return (false, null);
            }

            var instance = Get( actor.ActorID );
            //if ( !res.has )
            //    return (false, null);

            if ( instance is null )
                return (false, null);

            instance.AddAddon( addon );
            return (true, instance);
        }

        /// <summary>
        /// 注销单个Actor实例
        /// </summary>
        public bool UnRegister( int id )
        {
            if ( !Contains( id ) )
            {
                Tools.Logger.Warning( $"proxy doesnt have actor wich id = {id}" );
                return false;
            }

            if ( !_actorIDToInstance.TryGetValue( id, out var actorCase ) )
            {
                Tools.Logger.Warning( $"Module_ProxyActor.Mgr.UnRegister()--->faild to get actor instance,id:{id}" );
                return false;
            }

            //从组件系统中移除
            var addons = actorCase.AllAddons();
            foreach ( var addon in addons )
            {
                //当前的问题是，当前这帧先调用了dispose清掉了组件数据，然后才走到了MonoBehaviour的Update，这可能是由引擎层决定的调用顺序，导致组件系统访问了已经被清理的组件，在下一帧的时候组件系统才会清掉要移除的组件
                //解决办法是要么在这帧update调用前就把组件系统的对应组件清掉，要么保证脏标记在这帧update调用前被设置
                //当前选择了第一种办法，直接在组件系统里调用了addon.dispose，参见Module_ProxyActor.System.cs的issue

                //也有可能是当前这帧组件系统在跑update，然后调用了dispose，导致修改了正在update访问中的组件数据
                //设置releaseFlag为true的时候当前帧已经开始了，换句话说已经开始update了
                // addon.ReleasFlag = true;
                //RemoveFromAddonSystem( addon );
                GameEntry.Module.GetModule<Module_AddonSystem>().RemoveFromAddonSystem( addon );
                addon.Dispose();
            }

            ReferencePool.Release( actorCase );
            //return _proxyActorDic.Remove( id ) && _registered_id_set.Remove( id );
            return _actorIDToInstance.Remove( id );
        }

        /// <summary>
        /// 将一个actor添加到instance管理模块中
        /// </summary>
        public (bool regSucc, ActorInstance instance) Register( Actor_Base actor )
        {
            if ( actor is null )
            {
                Tools.Logger.Warning( "<color=yellow>Module_ProxyActor.Register()--->actor is null.</color>" );
                return (false, null);
            }

            if ( Contains( actor.ActorID ) )
            {
                Tools.Logger.Warning( $"<color=yellow>Module_ProxyActor.Register()--->proxy has contains actor,id={actor.ActorID}.</color>" );
                return (false, null); ;
            }

            var actorCase = ReferencePool.Acquire<ActorInstance>();
            actorCase.Setup( actor );
            _actorIDToInstance.Add( actor.ActorID, actorCase );

            return (true, actorCase);
        }
        #endregion

        /// <summary>
        /// 查找当前管理器中是否包含指定actor
        /// </summary>
        private bool Contains( int actorID )
        {
            return _actorIDToInstance.ContainsKey( actorID );
        }

        /// <summary>
        /// 获取指定的actor实例
        /// </summary>
        public ActorInstance Get( int id )
        {
            if ( !_actorIDToInstance.TryGetValue( id, out var actor_instance ) )
                Tools.Logger.Warning( $"faild to get actor id={id}" );

            return actor_instance;
        }

        #region override
        public override void EnsureInit()
        {
            base.EnsureInit();
            _actorIDToInstance = new Dictionary<int, ActorInstance>();
        }

        public override void Close()
        {
            base.Close();
            _actorIDToInstance.Clear();
            _actorIDToInstance = null;
        }

        #endregion

        /// <summary>
        /// actor实例集合
        /// </summary>
        private Dictionary<int, ActorInstance> _actorIDToInstance;
    }
}
