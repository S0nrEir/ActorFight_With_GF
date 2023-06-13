using Aquila.Config;
using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Toolkit;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 特效
    /// </summary>
    public class ActorFX : EntityLogic
    {

        #region public
        public void Setup ( int id, float survivalTime, Actor_Base parentActor = null, bool isForever = false )
        {
            ID = id;
            SurvivalTime = survivalTime;
            TimesUpFlag = false;
            PassedTime = 0f;
            _actor = parentActor;
            IsForever = isForever;

            gameObject.name = $"Actor_Effect_{ID}";

// #if UNITY_EDITOR
//             _inspector = gameObject.GetComponent<ActorEffectInspector>();
//             if (_inspector == null)
//                 _inspector = gameObject.AddComponent<ActorEffectInspector>();
//
//             _inspector.Setup( ID, survivalTime, _actor != null ? _actor.ActorID : Component_GlobalVar.InvalidID, Entity.EntityAssetName );
// #endif
        }

        #endregion

        //-----------------------private----------------------- 

        /// <summary>
        /// 特效存活时间到
        /// </summary>
        private void OnTimesUp ()
        {
            Log.Info( $"<color=white>OnTimesUp--->{Entity.EntityAssetName}</color>" );
            _actor.Trigger( ActorEventEnum.EFFECT_TIMES_UP, this );
            TimesUpFlag = true;
        }
        

        //----------------------- override-----------------------
        protected override void OnUpdate ( float elapseSeconds, float realElapseSeconds )
        {
            base.OnUpdate( elapseSeconds, realElapseSeconds );

            PassedTime += elapseSeconds;
            if (PassedTime >= SurvivalTime)
            {
                if (IsForever)
                    PassedTime = 0f;
                else
                    OnTimesUp();
            }

// #if UNITY_EDITOR
//             if (_inspector != null)
//                 _inspector.SetPassedTime( PassedTime, TimesUpFlag );
// #endif
        }

        protected override void OnShow ( object userData )
        {
            base.OnShow( userData );
            Tools.SetActive( gameObject, true );
        }

        protected override void OnRecycle ()
        {
            base.OnRecycle();
            TimesUpFlag = false;
            PassedTime = 0f;
        }

        private void OnDestroy()
        {
            Log.Info( $"<color=yellow>ActorEffect--->OnDestroy()</color>" );
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            base.OnHide( isShutdown, userData );
            //#这里是对entity的额外处理
            var helperNode = GameEntry.Entity.GetEntityGroup( GameConfig.Entity.GROUP_ACTOR_FX ).Helper as EntityGroupHelperBase;
            if ( helperNode != null ) 
                CachedTransform.SetParent( helperNode.transform );
        }

        /// <summary>
        /// 特效ID
        /// </summary>
        public int ID { get; private set; } = -1;

        /// <summary>
        /// 时间标记
        /// </summary>
        public bool TimesUpFlag = false;

        /// <summary>
        /// 经过时间
        /// </summary>
        public float PassedTime { get; private set; } = -1f;

        /// <summary>
        /// 存活时间
        /// </summary>
        public float SurvivalTime { get; private set; } = -1f;

        /// <summary>
        /// 被持有的actor
        /// </summary>
        public Actor_Base Actor => _actor;

        /// <summary>
        /// 被持有的actor
        /// </summary>
        private Actor_Base _actor = null;

        /// <summary>
        /// 永久显示
        /// </summary>
        public bool IsForever = false;

        // private ActorEffectInspector _inspector = null;
    }

    /// <summary>
    /// 信息调试脚本
    /// </summary>
    // internal class ActorEffectInspector : MonoBehaviour
    // {
    //     public void SetPassedTime ( float n, bool flag )
    //     {
    //         PassedTime = n;
    //         TimesUpFlag = flag;
    //     }
    //
    //     public void Setup ( int id, float sur_time, int actor_id, string path )
    //     {
    //         ID = id;
    //         SurvivalTime = sur_time;
    //         AssetPath = path;
    //         ActorID = actor_id.ToString();
    //     }
    //
    //     public int ID = -1;
    //
    //     public bool TimesUpFlag = false;
    //
    //     /// <summary>
    //     /// 经过时间
    //     /// </summary>
    //     public float PassedTime = -1f;
    //
    //     /// <summary>
    //     /// 存活时间
    //     /// </summary>
    //     public float SurvivalTime = -1f;
    //
    //     /// <summary>
    //     /// 资源路径
    //     /// </summary>
    //     public string AssetPath = string.Empty;
    //
    //     /// <summary>
    //     /// 被持有的actor
    //     /// </summary>
    //     public string ActorID = string.Empty;
    //
    // }
}