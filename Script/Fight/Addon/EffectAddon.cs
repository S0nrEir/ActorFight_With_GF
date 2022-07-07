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
    public class EffectAddon : AddonBase
    {
        #region public

        /// <summary>
        /// buff特效，永久显示直到buff消退或角色死亡
        /// </summary>
        public async void ShowBuffEffectAsync (int effectID ,Tab_Effect meta ,Action<Tab_Effect, ActorEffect> callBack)
        {
            if (meta is null || string.IsNullOrEmpty( meta.Path ))
                return;

            var task = await AwaitableExtension.ShowEntity
                (
                    MRG.GameEntry.Entity,
                    effectID,
                    typeof( ActorEffect ),
                    meta.Path,
                    GameConfig.Entity.GROUP_ActorEffect,
                    GameConfig.Entity.Priority_Effect,
                    new ActorEffectData( effectID )
                );

            var actorEffect = task.Logic as ActorEffect;
            if (actorEffect is null)
            {
                Log.Error( $"create actor effect faild--->id:{effectID}" );
                return;
            }

            actorEffect.Setup( effectID, meta.Duration, Actor, meta.Duration <= 0 );
            Add( actorEffect.ID, actorEffect );
            callBack?.Invoke( meta, actorEffect );
        }

        /// <summary>
        /// 加载特效，此乃异步操作
        /// </summary>
        public async void ShowAsync ( string assetPath, float duration ,Action<string,ActorEffect> callBack)
        {
            var effectID = ACTOR_ID_POOL.Gen();
            var task = await AwaitableExtension.ShowEntity
                (
                    MRG.GameEntry.Entity,
                    effectID,
                    typeof( ActorEffect ),
                    assetPath,
                    GameConfig.Entity.GROUP_ActorEffect,
                    GameConfig.Entity.Priority_Effect,
                    new ActorEffectData(effectID)
                );

            var actorEffect = task.Logic as ActorEffect;
            if (actorEffect is null)
            {
                Log.Error( $"create actor effect faild--->id:{effectID}" );
                return;
            }

            actorEffect.Setup( effectID, duration, Actor );
            Add( actorEffect.ID, actorEffect );
            callBack?.Invoke( assetPath, actorEffect );
        }

        #endregion

        #region private

        /// <summary>
        /// 添加到集合中
        /// </summary>
        private bool Add (int effectID,ActorEffect effect)
        {
            if (_releasedEffectDic.ContainsKey( effectID ))
                return false;

            _releasedEffectDic.Add( effectID, effect );
            return true;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Hide ( int effectID )
        {
            MRG.GameEntry.Entity.HideEntity( effectID );
            return Remove( effectID );
        }

        /// <summary>
        /// 隐藏
        /// </summary
        public bool Hide ( ActorEffect effect )
        {
            return effect != null && Hide( effect.ID );
        }

        /// <summary>
        /// 隐藏所有
        /// </summary>
        public bool HideAll ()
        {
            if (_releasedEffectDic.Count == 0)
                return false;

            var iter = _releasedEffectDic.GetEnumerator();
            ActorEffect effect = null;
            while (iter.MoveNext())
            {
                effect = iter.Current.Value;
                if (effect == null)
                    return false;

                if (!Hide( effect ))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 从集合中移除
        /// </summary>
        private bool Remove (int effectID)
        {
            if (_releasedEffectDic is null || _releasedEffectDic.Count == 0)
                return false;

            return _releasedEffectDic.Remove( effectID );
        }

        private ActorEffect Get(int effectID)
        {
            if (_releasedEffectDic.TryGetValue( effectID, out var effect ))
                return effect;

            return null;
        }

        #endregion

        #region override
        public override AddonTypeEnum AddonType => AddonTypeEnum.EFFECT;

        public override void Init ( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        public override void Dispose ()
        {
            base.Dispose();
            _onShowSuccCallBack = null;
            _releasedEffectDic = null;
        }

        public override void OnAdd ()
        {
            _onShowSuccCallBack = null;
            _releasedEffectDic = new Dictionary<int, ActorEffect>(0x2);
        }

        public override void Reset ()
        {
            base.Reset();
            _onShowSuccCallBack = null;
            _releasedEffectDic?.Clear();
        }

        public override void SetEnable ( bool enable )
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
        public ActorEffectData ( int entityID ) : base( entityID, typeof( ActorEffectData ).GetHashCode() )
        { 
        }
    }

}
