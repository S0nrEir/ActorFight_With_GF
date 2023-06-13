using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Buff
{
    /// <summary>
    /// buff实例
    /// </summary>
    public class BuffEntity : IReference
    {
        /// <summary>
        /// 初始化设置
        /// </summary>
        public void Setup ( int actor_id, int effect_actor_id, int effectMetaID ,int impact_id)
        {
            Clear();
            ActorID = actor_id;
            EffectActorID = effect_actor_id;
            //EffectMeta = TableManager.GetEffectByID( effectMetaID, 0 );
            //EffectMeta = null;
            ImpactID = impact_id;
        }

        public void Clear ()
        {
            EffectActorID = -1;
            ActorID = -1;
            //EffectMeta = null;
            _remainTime = 0f;
        }

        /// <summary>
        /// actorID
        /// </summary>
        public int ActorID { get; private set; } = -1;

        /// <summary>
        /// 特效actorID
        /// </summary>
        public int EffectActorID { get; private set; } = -1;

        /// <summary>
        /// 特效
        /// </summary>
        //public Tab_Effect EffectMeta { get; private set; } = null;

        /// <summary>
        /// 所属的impactID
        /// </summary>
        public int ImpactID { get; private set; } = -1;

        /// <summary>
        /// 剩余时间
        /// </summary>
        private float _remainTime = 0f;

        public static BuffEntity Gen (int actor_id,int effect_actor_id,int effect_meta_id,int impact_id)
        {
            var entity = ReferencePool.Acquire<BuffEntity>();
            entity.Setup( actor_id, effect_actor_id, effect_meta_id ,impact_id);
            return entity;
        }
    }
}
