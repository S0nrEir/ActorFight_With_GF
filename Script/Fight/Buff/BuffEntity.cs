using GameFramework;
using GCGame.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace MRG.Fight.Buff
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
            EffectMeta = TableManager.GetEffectByID( effectMetaID, 0 );
            ImpactID = impactID;
            //if (EffectMeta is null)
            //    Log.Error( $"<color=red>EffectMeta is null</color>" );
        }

        public void Clear ()
        {
            EffectActorID = -1;
            ActorID = -1;
            EffectMeta = null;
            _remainTime = 0f;
            //GC_UPDATE_NEEDIMPACTINFO pak = null;
            //required int32  objId = 1;//objId
            //repeated int32  impactId = 2;//Buff Id
            //repeated int32  impactLogicId = 3;//Buff 逻辑ID
            //repeated int32  isForever = 4;//Buff 是否永久
            //repeated int32  remainTime = 5;//Buff 剩余时间
            //repeated int32  isAdd = 6;//添加或删除 0删除 1添加

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
        public Tab_Effect EffectMeta { get; private set; } = null;

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
