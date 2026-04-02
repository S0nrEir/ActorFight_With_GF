using System.Collections.Generic;
using Aquila.Event;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 技能组件
    /// </summary>
    public class Addon_Ability : Addon_Base
    {
        //----------------------pub----------------------
        /// <summary>
        /// 给与addon某个技能 / Give a ability to an addon
        /// </summary>
        public void GiveAbility(AbilityData data)
        {
            if (_specMap == null)
                _specMap = new Dictionary<int, AbilitySpecBase>();
            if (_specArr == null)
                _specArr = new AbilitySpecBase[0];

            var spec = AbilitySpecBase.Gen(data, _actorInstance);
            if (_specMap.ContainsKey(spec.AbilityId))
            {
                Tools.Logger.Warning($"<color=yellow>Addon_Ability.SetupWithAbilityData()--->duplicate ability id:{spec.AbilityId}</color>");
                return;
            }

            var newArr = new AbilitySpecBase[_specArr.Length + 1];
            _specArr.CopyTo(newArr, 0);
            newArr[_specArr.Length] = spec;
            _specArr = newArr;
            _specMap.Add(spec.AbilityId, spec);
            _initFlag = true;
        }

        /// <summary>
        /// 扣除技能消耗
        /// </summary>
        private void Deduct( int abilityID )
        {
            GetAbilitySpec( abilityID )?.Deduct();
        }

        /// <summary>
        /// 获取cd
        /// </summary>
        public (float remain, float duration) CoolDown( int abilityID )
        {
            var spec = GetAbilitySpec( abilityID );
            if ( spec is null )
            {
                Tools.Logger.Warning( $"<color=yellow>Addon_Ability.CoolDown()--->ability spec not found, abilityID:{abilityID}, actorID:{_actorInstance?.Actor?.ActorID}</color>" );
                return (0f, 0f);
            }

            return (spec.CoolDown._remain, spec.CoolDown._totalDuration);
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        public bool UseAbility( int abilityID,int triggerIndex, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            var spec = GetAbilitySpec( abilityID );
            if ( spec is null )
            {
                Tools.Logger.Warning( $"<color=yellow>Addon_Ability.UseAbility()--->ability spec not found, abilityID:{abilityID}, actorID:{_actorInstance?.Actor?.ActorID}</color>" );
                result._stateDescription = Tools.SetBitValue( result._stateDescription,
                    ( int ) AbilityHitResultTypeEnum.NONE_SPEC, true );
                return false;
            }
        
            return spec.UseAbility(triggerIndex, target, result );
        }

        public override void OnUpdate( float deltaTime, float realElapsed )
        {
            if ( !_initFlag )
                return;

            foreach ( var spec in _specArr )
                spec.OnUpdate( deltaTime );
        }

        /// <summary>
        /// 是否可使用技能，0=可以，1=cost不够，2=cd未准备好，3=无效
        /// </summary>
        public int CanUseAbility( int metaID )
        {
            var spec = GetAbilitySpec( metaID );
            if ( spec is null )
                return ( int ) CastRejectCode.AbilitySpecMissing;

            return spec.CanUseAbility();
        }

        //----------------------priv----------------------
        /// <summary>
        /// 获取指定的技能逻辑实例，获取不到返回空
        /// </summary>
        private AbilitySpecBase GetAbilitySpec( int metaID )
        {
            if ( _specMap is null || _specMap.Count == 0 )
            {
                Tools.Logger.Warning( $"<color=yellow>Addon_Ability.GetAbilitySpec()--->_specMap is null or empty, abilityID:{metaID}, actorID:{_actorInstance?.Actor?.ActorID}</color>" );
                return null;
            }

            if ( _specMap.TryGetValue( metaID, out var spec ) )
                return spec;

            Tools.Logger.Warning( $"<color=yellow>Addon_Ability.GetAbilitySpec()--->ability spec not found, abilityID:{metaID}, actorID:{_actorInstance?.Actor?.ActorID}, specCount:{_specMap.Count}</color>" );
            return null;
        }

        /// <summary>
        /// 当使用技能
        /// </summary>
        private void OnUseAbility( int addonType, object param )
        {
            if ( param is AddonParam_OnUseAbility temp )
                Deduct( temp._abilityID );
        }

#if UNITY_EDITOR
        /// <summary>
        /// <para>编辑器下初始化组件持有的技能和对应的spec</para>
        /// <para>Initialize the abilities and corresponding specs held by the component in editor mode</para>
        /// </summary>
        private bool InitSpec_Editor()
        {
            var abilities = GameEntry.AbilityPool.GetAbilities(_actorInstance.Actor.RoleMetaID);
            _specArr = new AbilitySpecBase[abilities.Length];
            _specMap = new Dictionary<int, AbilitySpecBase>( abilities.Length );
            for ( int i = 0; i < abilities.Length; i++ )
            {
                var spec = AbilitySpecBase.Gen( abilities[i], _actorInstance );
                _specArr[i] = spec;

                if ( _specMap.ContainsKey( spec.AbilityId ) )
                {
                    Tools.Logger.Warning( $"<color=yellow>Addon_Ability.InitSpec()--->duplicate ability id:{spec.AbilityId}, actorID:{_actorInstance?.Actor?.ActorID}</color>" );
                    continue;
                }

                _specMap.Add( spec.AbilityId, spec );
            }

            return true;
        }
#endif
        
        /// <summary>
        /// <para>初始化组件持有的技能和对应的spec</para>
        /// <para>Initialize the abilities and corresponding specs held by the component</para>
        /// </summary>
        private bool InitSpec()
        {
            var abilities = GameEntry.AbilityPool.GetAbilities(_actorInstance.Actor.RoleMetaID);
            if (abilities == null || abilities.Length == 0)
            {
                Tools.Logger.Warning("<color=yellow>Addon_Ability.InitSpec()--->no abilities found</color>");
                return false;
            }
            _specArr = new AbilitySpecBase[abilities.Length];
            _specMap = new Dictionary<int, AbilitySpecBase>( abilities.Length );
            for ( int i = 0; i < abilities.Length; i++ )
            {
                var spec = AbilitySpecBase.Gen( abilities[i], _actorInstance );
                _specArr[i] = spec;

                if ( _specMap.ContainsKey( spec.AbilityId ) )
                {
                    Tools.Logger.Warning( $"<color=yellow>Addon_Ability.InitSpec()--->duplicate ability id:{spec.AbilityId}, actorID:{_actorInstance?.Actor?.ActorID}</color>" );
                    continue;
                }

                _specMap.Add( spec.AbilityId, spec );
            }

            return true;
        }

        //--------------------override--------------------
        public override AddonTypeEnum AddonType => AddonTypeEnum.ABILITY;
        public override void OnAdd()
        {
        }

        public override void Init( Module_ProxyActor.ActorInstance instance )
        {
            base.Init( instance );
#if UNITY_EDITOR
            if ( !InitSpec_Editor() )
                return;
#else
            if ( !InitSpec() )
                return;
#endif
            _initFlag = true;
            instance.GetAddon<Addon_Event>().Register( ( int ) AddonEventTypeEnum.USE_ABILITY, ( int ) EventAddonPrioerityTypeEnum.ADDON_ABILITY, OnUseAbility );
        }

        public override void Dispose()
        {
            if ( _specArr is { Length: > 0 } )
            {
                foreach ( var spec in _specArr )
                    ReferencePool.Release( spec );
            }

            _specArr = null;
            _specMap = null;
            _initFlag = false;
            base.Dispose();
        }

        /// <summary>
        /// 持有的技能
        /// </summary>
        private AbilitySpecBase[] _specArr;

        /// <summary>
        /// 技能ID到逻辑实例的索引
        /// </summary>
        private Dictionary<int, AbilitySpecBase> _specMap;

        /// <summary>
        /// 初始化标记
        /// </summary>
        private bool _initFlag;
    }
}
