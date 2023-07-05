using Aquila.Event;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Common;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 定期派发子Effect
    /// </summary>
    public class EffectSpec_Period_Deriving : EffectSpec_Base
    {
        public override void Apply( Module_ProxyActor.ActorInstance instance, AbilityResult_Hit result )
        {
            base.Apply( instance, result );
            var subEffect = Meta.DeriveEffects;
            Cfg.Common.Table_Effect meta = null;
            EffectSpec_Base effect = null;
            foreach ( var effectID in subEffect )
            {
                meta = GameEntry.LuBan.Tables.Effect.Get( effectID );
                if ( meta is null )
                {
                    Log.Warning( $"<color=yellow>EffectSpec_Period_Deriging.Apply()--->meta is null,id:{effectID}</color>" );
                    continue;
                }

                //
            }
        }

        public override void Init( Table_Effect meta )
        {
            base.Init( meta );
            _resetDurationWhenOverride = meta.ExtensionParam.IntParam_2 == 1;
        }

        public override void Clear()
        {
            _stackCount = 0;
            _resetDurationWhenOverride = false;
            base.Clear();
        }

        /// <summary>
        /// 叠加层数
        /// </summary>
        private int _stackCount = 0;

        /// <summary>
        /// 覆盖时重置持续时间
        /// </summary>
        private bool _resetDurationWhenOverride = false;
    }
}
