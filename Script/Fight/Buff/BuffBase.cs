using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Module;
using GameFramework;
using System.Collections.Generic;
namespace Aquila.Fight.Buff
{
    /// <summary>
    /// Buff基类
    /// </summary>
    public class BuffBase : IReference
    {
        public BuffBase ()
        { }

        public bool Setup ( int id )
        {
            return true;
        }

        public void ApplyCache ( TActorBase actor )
        {
            if (!_cache.TryGetValue( actor.ActorID, out var list ))
                return;

            foreach (var entity in list)
                CreateEffect( actor, entity );

            list.Clear();
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public void Cache ( int objID, BuffEntity entity )
        {
            if (!_cache.TryGetValue( objID, out var list ))
                list = new List<BuffEntity>();

            if (list.Contains( entity ))
                return;

            list.Add( entity );
            _cache.Add( objID, list );
        }

        public BuffEntity Get ( int objID )
        {
            if (!_entityDic.TryGetValue( objID, out var entity ))
                return null;

            return entity;
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool Remove ( int objID, out bool emptyEntity )
        {
            emptyEntity = true;
            _cache.Remove( objID );
            if (_entityDic is null || _entityDic.Count == 0)
                return false;

            if (!_entityDic.ContainsKey( objID ))
                return false;

            var entity = _entityDic[objID];
            var actor = GameFrameworkModule.GetModule<Module_Actor>().GetActor( objID);
            if (actor is null)
                return false;

            if (!actor.TryGetAddon<EffectAddon>( out var addon ))
                return false;

            addon.Hide( entity.EffectActorID );
            var removeSucc = _entityDic.Remove( objID );
            ReferencePool.Release( entity );
            entity = null;
            emptyEntity = _entityDic is null || _entityDic.Count == 0;
            return removeSucc;
        }

        /// <summary>
        /// 添加buffEntity实例
        /// </summary>
        public BuffEntity Add ( int objID, int impactID, out bool createSucc )
        {
            createSucc = false;
            //var impactMeta = TableManager.GetImpactByID( impactID, 0 );
            //if (impactMeta is null)
            //    return null;

            ////创建effectActor
            //var actor = GameEntry.Module.GetModule<ActorModule>().GetActor( objID,);

            //BuffEntity entity = GenEntityInfo( impactMeta, objID, impactID );
            ////能拿到actor，直接创建
            //if (actor != null)
            //{
            //    if (!actor.TryGetAddon<EffectAddon>( out var addon ))
            //        return null;

            //    createSucc = true;
            //    //show effect
            //    if (entity.EffectMeta != null)
            //    {
            //        addon.ShowEffectAsync
            //            (
            //                entity.EffectActorID,
            //                entity.EffectMeta,
            //                ( effectMeta, effect ) => Utils.Fight.SetEffectActorTran( effectMeta, effect )
            //            );
            //    }
            //}
            ////拿不到先缓存
            //else
            //{
            //    Cache( objID, entity );
            //}
            //_entityDic.Add( objID, entity );
            //return entity;
            //#todo增加配表buff
            return null;
        }

        /// <summary>
        /// 生成buffEntity信息
        /// </summary>
        private BuffEntity GenEntityInfo ( int objID, int impactID )
        {
            //var effectActorID = ACTOR_ID_POOL.Gen();
            //var entity = BuffEntity.Gen( objID, effectActorID, impactMeta.EffectId, impactID );
            //return entity;
            //#todo---effect和impact表
            return null;
        }

        /// <summary>
        /// 根据actor创建一个特效
        /// </summary>
        public void CreateEffect ( TActorBase actor, BuffEntity entity )
        {
            //if (!actor.TryGetAddon<EffectAddon>( out var addon ))
            //    return;

            //addon.ShowEffectAsync
            //    (
            //        entity.EffectActorID,
            //        entity.EffectMeta,
            //        ( effectMeta, effect ) => Tools.Fight.BindEffect( effectMeta, effect )
            //    );
        }

        public bool Contains ( int objID ) => _entityDic.ContainsKey( objID );

        public void Clear ()
        {
            //Meta = null;
            _entityDic?.Clear();
            _entityDic = null;
            _cache?.Clear();
            _cache = null;
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        //public BuffApplyTypeEnum ApplyType => BuffApplyTypeEnum.Invalid;

        /// <summary>
        /// 缓存
        /// </summary>
        private Dictionary<int, List<BuffEntity>> _cache = null;

        /// <summary>
        /// 表数据
        /// </summary>
        //public Tab_Impact Meta { get; private set; } = null;

        /// <summary>
        /// 实例列表，key = actorID,v=buffEntity
        /// </summary>
        private Dictionary<int, BuffEntity> _entityDic = null;
    }
}
