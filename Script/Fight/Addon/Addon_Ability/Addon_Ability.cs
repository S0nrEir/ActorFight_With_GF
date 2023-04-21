using System.Collections;
using System.Collections.Generic;
using Aquila.Fight.Actor;
using Cfg.common;
using GameFramework;
using UnityEngine;

namespace  Aquila.Fight.Addon
{
    /// <summary>
    /// 技能组件
    /// </summary>
    public partial class Addon_Ability : AddonBase
    {
        //--------------------override--------------------
        public override AddonTypeEnum AddonType => AddonTypeEnum.ABILITY;
        public override void OnAdd()
        {
            //#todo从哪里初始化表和spec？
        }

        public override void SetEnable(bool enable)
        {
        }

        public override void Init(TActorBase actor, GameObject targetGameObject, Transform targetTransform)
        {
            base.Init(actor, targetGameObject, targetTransform);
            if (!(_spec is null))
            {
                
            }
            _spec = ReferencePool.Acquire<AbilitySpec>();
        }

        public override void Dispose()
        {
            base.Dispose();
            ReferencePool.Release(_spec);
            
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        /// <summary>
        /// 技能元数据
        /// </summary>
        private TB_AbilityBase _meta = null;
        
        /// <summary>
        /// 对应的技能逻辑
        /// </summary>
        private AbilitySpec _spec = null;
    }
}
