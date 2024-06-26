using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.Toolkit;
using Cfg.Role;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGFExtensions.Await;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// Actor工厂类，创建和回收
    /// </summary>
    public class Module_Actor_Fac : GameFrameworkModuleBase
    {
        //--------------------public--------------------
        /// <summary>
        /// 异步创建一个actor，失败返回null task
        /// </summary>
        public async Task<Entity> ShowActorAsync( int entityID, int roleMetaID, object userData, string objectName )
        {
            var meta = GameEntry.LuBan.Tables.RoleMeta.Get( roleMetaID );
            if ( meta is null )
            {
                Log.Warning( $"<color=yellow>Module_ActorFac.ShowActorAsync()--->role meta is null, meta id:{roleMetaID}</color>" );
                return null;
            }

            //var actorType = Tools.Actor.RoleTypeEnum2SystemType( meta.RoleType );
            var entityInfo = Tools.Actor.GetActorEntityInfo( meta.RoleType );
            if ( !entityInfo.genSucc )
            {
                Log.Warning( $"<color=yellow>Module_ActorFac.ShowActorAsync()--->!entityInfo.genSucc,role type:{meta.RoleType}</color>" );
                return null;
            }

            var result = await AwaitableExtensions.ShowEntityAsync
                (
                    GameEntry.Entity,
                    entityID,
                    entityInfo.actorType,
                    @meta.AssetPath,
                    entityInfo.group,
                    entityInfo.priority,
                    userData
                );

            if ( !_actorGenCallBackDic.TryGetValue( entityInfo.actorType, out var onShowSucc ) )
            {
                Log.Warning( $"Module_ActorFac.ShowActorAsync()--->!_actorGenCallBackDic.ContainsKey( actorType, out var onShowSucc ),type:{entityInfo.actorType.ToString()}" );
                return null;
            }

            onShowSucc( entityID, roleMetaID, userData, result.Logic, meta );
            var baseActor = result.Logic as Actor_Base;
            SetName( objectName, baseActor );
            SetTag( "Actor", baseActor );

            return result;
        }

        public override void Open( object param )
        {
            base.Open( param );
        }
        public override void Close()
        {
            base.Close();
        }

        public override void EnsureInit()
        {
            _actorGenCallBackDic = new Dictionary<Type, Action<int, int, object, object, Table_RoleMeta>>()
            {
                { typeof(Actor_Hero),OnShowHeroSucc},
                { typeof(Actor_Orb),OnShowOrbSucc},
            };
        }

        /// <summary>
        /// hero类型actor生成
        /// </summary>
        private void OnShowHeroSucc( int entityID, int roleMetaID, object userData, object actor, Table_RoleMeta roleMeta )
        {
            var temp = actor as Actor_Base;
            temp.SetCoordAndPosition( 0, 0 );
        }

        /// <summary>
        /// orb类actor生成
        /// </summary>
        private void OnShowOrbSucc( int entityID, int roleMetaID, object userData, object actor, Table_RoleMeta meta )
        {
            var temp = actor as Actor_Orb;
            temp.SetWorldPosition( GameEntry.GlobalVar.InvalidPosition );
            Tools.SetActive( temp.gameObject, false );
            var orbData = userData as Actor_Orb_EntityData;
            if ( orbData is null )
            {
                Log.Warning( $"Module_ActorFac.OnShowTracingProjectileActorSucc()--->orbData is null" );
                return;
            }
            
            var module = GameEntry.Module.GetModule<Module_ProxyActor>();

            //todo:这里要检查一下状态，如果召唤者actor已经死了就从死亡位置发出，如果还活着就从武器挂点发出
            var position = module.GetPosition( orbData._callerID );
            //处理一下转向问题
            var targetTransform = module.GetActorTransform(orbData._targetActorID);
            if (targetTransform == null)
            {
                Log.Warning($"<color=yellow>Module_ActorFac.OnShowOrbSucc()--->targetTransform == null,target transform id:{orbData._targetActorID}</color>");
                return;
            }
            
            temp.SetWorldPosition( position );
            temp.transform.LookAt( targetTransform, UnityEngine.Vector3.up );
            temp.SetTargetTransformAndReady( targetTransform );
            Tools.SetActive( temp.gameObject, true );
        }

        /// <summary>
        /// 设置actor gameobject name
        /// </summary>
        private void SetName( string name, Actor_Base actor )
        {
            actor.name = $"{name}";
        }

        /// <summary>
        /// 设置actor gameobject tag
        /// </summary>
        private void SetTag( string tag, Actor_Base actor )
        {
            actor.tag = tag;
        }

        /// <summary>
        /// 设置acto gameobject layer
        /// </summary>
        private void SetLayer( string layer, Actor_Base actor )
        {
            Tools.SetLayer( layer, actor.gameObject, true );
        }

        /// <summary>
        /// 保存actor的生成回调函数合集
        /// </summary>
        private Dictionary<System.Type, Action<int, int, object, object, Table_RoleMeta>> _actorGenCallBackDic = null;

    }
}
