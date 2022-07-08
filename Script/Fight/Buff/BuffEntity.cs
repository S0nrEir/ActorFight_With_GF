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
        public void Setup ( int actorID, int effectActorID, int effectMetaID ,int impactID)
        {
            Clear();
            ActorID = actorID;
            EffectActorID = effectActorID;
            //#todo分离Effect表数据
            //EffectMeta = TableManager.GetEffectByID( effectMetaID, 0 );
            //EffectMeta = null;
            ImpactID = impactID;
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

        public static BuffEntity Gen (int actorID,int effectActorID,int effectMetaID,int impactID)
        {
            var entity = ReferencePool.Acquire<BuffEntity>();
            entity.Setup( actorID, effectActorID, effectMetaID ,impactID);
            return entity;
        }
    }
}
