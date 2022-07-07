using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// actor的Event组件，只保留于Actor内部，event也只能通知到自己的Actor，这样做是为了让actor，state，addon互相解耦
    /// </summary>
    public class EventAddon : AddonBase
    {
        #region public

        /// <summary>
        /// 发送一个event
        /// </summary>
        public void Trigger( ActorEventEnum type, object[] param )
        {
            //Debug.Log( $"<color=green>Actor{Actor.ActorID}--->Trigger:{type}</color>" );
            var intType = ( int ) type;
            if ( !_eventDic.TryGetValue( intType, out var action ) )
                return;

            action?.Invoke( intType, param );
        }

        /// <summary>
        /// 添加
        /// </summary>
        public bool Register( int intType, Action<int, object[]> action )
        {
            //Debug.Log( $"<color=white>Actor{Actor.ActorID}--->RegisterActorEvent{( (ActorEventEnum)intType ).ToString()}</color>" );

            //不需要listener，因为listener就是actor自己
            if ( _eventDic.ContainsKey( intType ) )
                return false;

            _eventDic.Add( intType, action );
            return true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        public bool Register( ActorEventEnum eventType, Action<int, object[]> action )
        {
            //Debug.Log( $"<color=white>Actor{Actor.ActorID}--->RegisterActorEvent{eventType}</color>" );
            return Register( ( int ) eventType, action );
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool UnRegister( ActorEventEnum eventType )
        {
            //Debug.Log( $"<color=green>Actor{Actor.ActorID}--->UnRegisterActorEvent{eventType}</color>" );
            return UnRegister( ( int ) eventType );
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool UnRegister( int intType )
        {
            return _eventDic.Remove( intType );
        }
        #endregion

        public override AddonTypeEnum AddonType => AddonTypeEnum.EVENT;

        public override void OnAdd()
        {
            _eventDic = new Dictionary<int, Action<int, object[]>>();
        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
        }

        public override void Reset()
        {
            base.Reset();
            //_eventDic?.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventDic?.Clear();
            _eventDic = null;
        }

        public override void Init( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        public override uint Valid()
        {
            return _eventDic is null ? base.Valid() : AddonValidErrorCodeEnum.NONE_EVENT;
        }

        /// <summary>
        /// 事件集,K=eventID,V=(eventID,param)
        /// </summary>
        private Dictionary<int, Action<int, object[]>> _eventDic;
    }

    /// <summary>
    /// ActorEvent枚举
    /// </summary>
    public enum ActorEventEnum
    {
        /// <summary>
        /// 导航组件到达目标点
        /// </summary>
        NAV_ARRIVE_TARGET = 0,

        /// <summary>
        /// colliderTrigger触发并且次数到达上限
        /// </summary>
        COLLIDER_TRIGGER_COUNT_LMT,

        /// <summary>
        /// ColliderTrigger触发
        /// </summary>
        COLLIDER_TRIGGER_HIT,

        /// <summary>
        /// 移动到了最后的路点
        /// </summary>
        MOVE_TO_FINAL_POINT,

        /// <summary>
        /// 技能完成
        /// </summary>
        ABILITY_FINISH,

        /// <summary>
        /// 特效时间到
        /// </summary>
        EFFECT_TIMES_UP,
    }

}