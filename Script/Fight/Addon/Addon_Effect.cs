using Aquila.Config;
using Aquila.Fight.Actor;
using System;
using System.Collections.Generic;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 特效组件
    /// </summary>
    public class Addon_Effect : AddonBase
    {
        #region public

        /// <summary>
        /// 显示一个特效
        /// </summary>
        /// <param name="effectID">特效ID</param>
        /// <param name="assetPath">资源路径</param>
        /// <param name="duration">持续时间，0为一直显示</param>
        /// <param name="callBack">回调</param>
        public async void ShowEffectAsync( int effectID, string assetPath, float duration , Action<ActorEffectEntityData, ActorEffect> callBack )
        {
            if ( string.IsNullOrEmpty( assetPath ) )
                return;

            var effectEntityData = new ActorEffectEntityData(effectID);
            effectEntityData._duration = duration;
            effectEntityData.ModelPath = assetPath;
            var task = await AwaitableExtensions.ShowEntity
                (
                    Aquila.GameEntry.Entity,
                    effectID,
                    typeof( ActorEffect ),
                    assetPath,
                    GameConfig.Entity.GROUP_ActorEffect,
                    GameConfig.Entity.Priority_Effect,
                    effectEntityData
                );

            var actorEffect = task.Logic as ActorEffect;
            if ( actorEffect is null )
            {
                Log.Error( $"create actor effect faild--->id:{effectID}" );
                return;
            }

            actorEffect.Setup( effectID, duration, Actor, duration <= 0 );
            Add( actorEffect.ID, actorEffect );
            callBack?.Invoke( effectEntityData, actorEffect );
        }

        #endregion

        #region private

        /// <summary>
        /// 添加到集合中
        /// </summary>
        private bool Add( int effectID, ActorEffect effect )
        {
            if ( _releasedEffectDic.ContainsKey( effectID ) )
                return false;

            _releasedEffectDic.Add( effectID, effect );
            return true;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Hide( int effectID )
        {
            Aquila.GameEntry.Entity.HideEntity( effectID );
            return Remove( effectID );
        }

        /// <summary>
        /// 隐藏
        /// </summary
        public bool Hide( ActorEffect effect )
        {
            return effect != null && Hide( effect.ID );
        }

        /// <summary>
        /// 隐藏所有
        /// </summary>
        public bool HideAll()
        {
            if ( _releasedEffectDic.Count == 0 )
                return false;

            var iter = _releasedEffectDic.GetEnumerator();
            ActorEffect effect = null;
            while ( iter.MoveNext() )
            {
                effect = iter.Current.Value;
                if ( effect == null )
                    return false;

                if ( !Hide( effect ) )
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 从集合中移除
        /// </summary>
        private bool Remove( int effectID )
        {
            if ( _releasedEffectDic is null || _releasedEffectDic.Count == 0 )
                return false;

            return _releasedEffectDic.Remove( effectID );
        }

        private ActorEffect Get( int effectID )
        {
            if ( _releasedEffectDic.TryGetValue( effectID, out var effect ) )
                return effect;

            return null;
        }

        #endregion

        #region override
        public override AddonTypeEnum AddonType => AddonTypeEnum.EFFECT;

        public override void Init( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        public override void Dispose()
        {
            base.Dispose();
            _releasedEffectDic = null;
        }

        public override void OnAdd()
        {
            _releasedEffectDic = new Dictionary<int, ActorEffect>( 0x2 );
        }

        public override void Reset()
        {
            base.Reset();
            _releasedEffectDic?.Clear();
        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
        }
        #endregion

        #region fields

        /// <summary>
        /// 放出的特效集合
        /// </summary>
        private Dictionary<int, ActorEffect> _releasedEffectDic = null;

        #endregion
    }

    /// <summary>
    /// 特效实体数据
    /// </summary>
    public class ActorEffectEntityData : EntityData
    {
        public ActorEffectEntityData( int entityID ) : base( entityID, typeof( ActorEffectEntityData ).GetHashCode() )
        {
        }

        /// <summary>
        /// 特效持续时间
        /// </summary>
        public float _duration = 0f;

        /// <summary>
        /// 特效节点#todo改成读取配置
        /// </summary>
        public string _effectPointName = "EffectPotin";

        /// <summary>
        /// 位置偏移
        /// </summary>
        public Vector3 _localPositionOffset = Vector3.zero;
    }
}
