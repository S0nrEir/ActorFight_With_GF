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

        public void ApplyCache ( Actor_Base actor )
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

        public BuffEntity Get ( int obj_id )
        {
            if (!_entityDic.TryGetValue( obj_id, out var entity ))
                return null;

            return entity;
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool Remove ( int obj_id, out bool empty_entity )
        {
            empty_entity = true;
            _cache.Remove( obj_id );
            if (_entityDic is null || _entityDic.Count == 0)
                return false;

            if (!_entityDic.ContainsKey( obj_id ))
                return false;

            var entity = _entityDic[obj_id];
            //var actor = GameEntry.Module.GetModule<Module_Actor_Factory>().GetActor<HeroActor>( objID);
            //if (actor is null)
            //    return false;

            //if (!actor.TryGetAddon<Addon_Effect>( out var addon ))
            //    return false;

            //addon.Hide( entity.EffectActorID );
            var removeSucc = _entityDic.Remove( obj_id );
            ReferencePool.Release( entity );
            entity = null;
            empty_entity = _entityDic is null || _entityDic.Count == 0;
            return removeSucc;
        }

        /// <summary>
        /// 添加buffEntity实例
        /// </summary>
        public BuffEntity Add ( int obj_id, int impact_id, out bool createSucc )
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
            return null;
        }

        /// <summary>
        /// 生成buffEntity信息
        /// </summary>
        private BuffEntity GenEntityInfo ( int obj_id, int impact_id )
        {
            //var effectActorID = ACTOR_ID_POOL.Gen();
            //var entity = BuffEntity.Gen( objID, effectActorID, impactMeta.EffectId, impactID );
            //return entity;
            return null;
        }

        /// <summary>
        /// 根据actor创建一个特效
        /// </summary>
        public void CreateEffect ( Actor_Base actor, BuffEntity entity )
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
