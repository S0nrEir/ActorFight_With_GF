using Aquila.Config;
using Aquila.Fight.Actor;
using GameFramework;
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
    public class EffectAddon : AddonBase
    {
        #region public

        /// <summary>
        /// 显示一个特效
        /// </summary>
        /// <param name="effectID">特效ID</param>
        /// <param name="assetPath">资源路径</param>
        /// <param name="duration">持续时间，0为一直显示</param>
        /// <param name="callBack">回调</param>
        public async void ShowBuffEffectAsync( int effectID, string assetPath, float duration , Action<EffectData, ActorEffect> callBack )
        {
            if ( string.IsNullOrEmpty( assetPath ) )
                return;

            var task = await AwaitableExtension.ShowEntity
                (
                    Aquila.GameEntry.Entity,
                    effectID,
                    typeof( ActorEffect ),
                    assetPath,
                    GameConfig.Entity.GROUP_ActorEffect,
                    GameConfig.Entity.Priority_Effect,
                    new ActorEffectData( effectID )
                );

            var actorEffect = task.Logic as ActorEffect;
            if ( actorEffect is null )
            {
                Log.Error( $"create actor effect faild--->id:{effectID}" );
                return;
            }

            var effectData = ReferencePool.Acquire<EffectData>();
            actorEffect.Setup( effectID, duration, Actor, duration <= 0 );
            Add( actorEffect.ID, actorEffect );
            callBack?.Invoke( effectData, actorEffect );
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
            _onShowSuccCallBack = null;
            _releasedEffectDic = null;
        }

        public override void OnAdd()
        {
            _onShowSuccCallBack = null;
            _releasedEffectDic = new Dictionary<int, ActorEffect>( 0x2 );
        }

        public override void Reset()
        {
            base.Reset();
            _onShowSuccCallBack = null;
            _releasedEffectDic?.Clear();
        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
        }
        #endregion

        #region fields

        /// <summary>
        /// 加载成功回调
        /// </summary>
        private Action<string, ActorEffect> _onShowSuccCallBack = null;

        /// <summary>
        /// 放出的特效集合
        /// </summary>
        private Dictionary<int, ActorEffect> _releasedEffectDic = null;

        #endregion
    }

    public class ActorEffectData : EntityData
    {
        public ActorEffectData( int entityID ) : base( entityID, typeof( ActorEffectData ).GetHashCode() )
        {
        }
    }

    /// <summary>
    /// actor特效数据信息
    /// </summary>
    public class EffectData : IReference
    {
        public string _assetPath = string.Empty;
        public float _duration = 0f;

        public void Clear()
        {
            _assetPath = string.Empty;
            _duration = 0f;
        }
    }
}
