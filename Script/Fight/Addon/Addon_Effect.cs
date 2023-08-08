using Aquila.Config;
using Aquila.Fight.Actor;
using System;
using System.Collections.Generic;
using Aquila.Module;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 特效组件
    /// </summary>
    public class Addon_FX : Addon_Base
    {
        //-----------------------public-----------------------

        /// <summary>
        /// 显示一个特效
        /// </summary>
        /// <param name="effectID">特效ID</param>
        /// <param name="assetPath">资源路径</param>
        /// <param name="duration">持续时间，0为一直显示</param>
        /// <param name="callBack">回调</param>
        public async void ShowEffectAsync( int fxID, string assetPath, float duration , Action<ActorEffectEntityData, ActorFX> callBack )
        {
            if ( string.IsNullOrEmpty( assetPath ) )
                return;

            var effectEntityData = new ActorEffectEntityData(fxID);
            effectEntityData._duration = duration;
            effectEntityData.ModelPath = assetPath;
            var task = await AwaitableExtensions.ShowEntity
                (
                    Aquila.GameEntry.Entity,
                    fxID,
                    typeof( ActorFX ),
                    assetPath,
                    GameConfig.Entity.GROUP_FX,
                    GameConfig.Entity.PRIORITY_ACTOR,
                    effectEntityData
                );

            var actorEffect = task.Logic as ActorFX;
            if ( actorEffect is null )
            {
                Log.Error( $"create actor effect faild--->id:{fxID}" );
                return;
            }

            actorEffect.Setup( fxID, duration, Actor, duration <= 0 );
            Add( actorEffect.ID, actorEffect );
            callBack?.Invoke( effectEntityData, actorEffect );
        }

        //----------------------- private-----------------------

        /// <summary>
        /// 添加到集合中
        /// </summary>
        private bool Add( int effectID, ActorFX effect )
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
        public bool Hide( ActorFX effect )
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
            ActorFX effect = null;
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

        private ActorFX Get( int effectID )
        {
            if ( _releasedEffectDic.TryGetValue( effectID, out var effect ) )
                return effect;

            return null;
        }

        //------------------------override------------------------
        public override AddonTypeEnum AddonType => AddonTypeEnum.EFFECT;

        public override void Init(Module_ProxyActor.ActorInstance instance)
        {
            base.Init(instance);
        }

        public override void Dispose()
        {
            base.Dispose();
            _releasedEffectDic = null;
        }

        public override void OnAdd()
        {
            _releasedEffectDic = new Dictionary<int, ActorFX>( 0x2 );
        }

        public override void Reset()
        {
            base.Reset();
            _releasedEffectDic?.Clear();
        }

        #region fields

        /// <summary>
        /// 放出的特效集合
        /// </summary>
        private Dictionary<int, ActorFX> _releasedEffectDic = null;

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
        /// 特效节点是否改成读取配置
        /// </summary>
        public string _effectPointName = "EffectPotin";

        /// <summary>
        /// 位置偏移
        /// </summary>
        public Vector3 _localPositionOffset = Vector3.zero;
    }
}
