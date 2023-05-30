using System.Collections;
using System.Collections.Generic;
using Aquila.Fight.Actor;
using Aquila.Module;
using Cfg.common;
using Cfg.role;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
namespace  Aquila.Fight.Addon
{
    /// <summary>
    /// 技能组件
    /// </summary>
    public partial class Addon_Ability : Addon_Base
    {
        //----------------------pub----------------------
        /// <summary>
        /// 使用技能
        /// </summary>
        public bool UseAbility(int ability_id, Module_Proxy_Actor.ActorInstance target,ref AbilityHitResult result)
        {
            var spec = GetAbilitySpec(ability_id);
            if (spec is null)
            {
                Log.Warning("<color=yellow>Addon_Ability.UseAbility--->spec is null</color>");
                return false;
            }

            return spec.UseAbility(target,ref result);
        }

        public override void OnUpdate(float delta_time,float real_elapsed)
        {
            if(!_init_flag)
                return;
            
            foreach (var spec in _spec_arr)
                spec.OnUpdate(delta_time);
        }

        /// <summary>
        /// 是否可使用技能，可以返回true
        /// </summary>
        public bool CanUseAbility(int meta_id,ref AbilityHitResult result)
        {
            var spec = GetAbilitySpec(meta_id);
            if (spec is null)
                return false;

            return spec.CanUseAbility(ref result);
        }

        //----------------------priv----------------------
        /// <summary>
        /// 获取指定逻辑类型，获取不到返回空
        /// </summary>
        private AbilitySpecBase GetAbilitySpec(int meta_id)
        {
            if (_spec_arr is null || _spec_arr.Length == 0)
            {
                Log.Warning(" <color=yellow>is null || _spec_arr.Length == 0</color>");
                return null;
            }

            foreach (var temp_spec in _spec_arr)
            {
                if ( temp_spec.Meta.id == meta_id)
                    return temp_spec;
            }

            return null;
        }
        
        /// <summary>
        /// 初始化组件持有的技能和对应的spec
        /// </summary>
        private bool InitSpec()
        {
            var role_meta = GameEntry.DataTable.Tables.TB_RoleMeta.Get(Actor.RoleMetaID);
            if (role_meta is null)
            {
                Log.Warning("Addon_Ability.Init()->role_meta is null");
                return false;
            }
            
            var ids = role_meta.AbilityBaseID;
            _spec_arr = new AbilitySpecBase[ids.Length];
            AbilityBase ability_base_meta = null;
            var len = _spec_arr.Length;
            for (var i = 0; i < len && i < ids.Length; i++)
            {
                ability_base_meta = GameEntry.DataTable.Tables.TB_AbilityBase.Get(ids[i]);
                if (ability_base_meta is null)
                {
                    Log.Warning("Addon_Ability.Init()->ability_base_meta is null");
                    return false;
                }
                _spec_arr[i] = AbilitySpecBase.Gen(ability_base_meta,_actor_instance);
            }
            return true;
        }
        
        //--------------------override--------------------
        public override AddonTypeEnum AddonType => AddonTypeEnum.ABILITY;
        public override void OnAdd()
        {
            //#todo从哪里初始化表和spec？
        }

        public override void Init(TActorBase actor, GameObject targetGameObject, Transform targetTransform)
        {
            base.Init(actor, targetGameObject, targetTransform);
            if(!InitSpec())
                return;

            _init_flag = true;
        }
        public override void Dispose()
        {
            if (_spec_arr is { Length: > 0 })
            {
                foreach (var spec in _spec_arr)
                    ReferencePool.Release(spec);
            }

            _spec_arr  = null;
            // _meta      = null;
            _init_flag = false;
            base.Dispose();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        // /// <summary>
        // /// 技能元数据
        // /// </summary>
        // private TB_AbilityBase _meta = null;
        
        /// <summary>
        /// 持有的技能
        /// </summary>
        private AbilitySpecBase[] _spec_arr = null;

        /// <summary>
        /// 初始化标记
        /// </summary>
        private bool _init_flag = false;
    }
}
