using Aquila.Event;
using Aquila.Module;
using Cfg.Fight;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 技能组件
    /// </summary>
    public partial class Addon_Ability : Addon_Base
    {
        //----------------------pub----------------------
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
            _cahchedAbility = GetAbilitySpec( abilityID );
            return (_cahchedAbility.CoolDown._remain, _cahchedAbility.CoolDown._totalDuration);
        }
        
        /// <summary>
        /// 使用技能
        /// </summary>
        public bool UseAbility( int abilityID,int triggerIndex, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            var spec = GetAbilitySpec( abilityID );
            if ( spec is null )
            {
                Log.Warning( "<color=yellow>Addon_Ability.UseAbility--->spec is null</color>" );
                result._stateDescription = Aquila.Toolkit.Tools.SetBitValue( result._stateDescription,
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
                return ( int ) AbilityUseResultTypeEnum.NONE_PARAM;

            return spec.CanUseAbility();
        }

        //----------------------priv----------------------
        /// <summary>
        /// 获取指定的技能逻辑实例，获取不到返回空
        /// </summary>
        private AbilitySpecBase GetAbilitySpec( int metaID )
        {
            if ( _specArr is null || _specArr.Length == 0 )
            {
                Log.Warning( " <color=yellow>is null || _spec_arr.Length == 0</color>" );
                return null;
            }

            foreach ( var tempSpec in _specArr )
            {
                if ( tempSpec.Meta.id == metaID )
                    return tempSpec;
            }

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

        /// <summary>
        /// 初始化组件持有的技能和对应的spec
        /// </summary>
        private bool InitSpec()
        {
            var roleMeta = GameEntry.LuBan.Tables.RoleMeta.Get( _actorInstance.Actor.RoleMetaID );
            if ( roleMeta is null )
            {
                Log.Warning( "Addon_Ability.Init()->role_meta is null" );
                return false;
            }
            var abilityIdSet = roleMeta.AbilityBaseID;
            _specArr = new AbilitySpecBase[abilityIdSet.Length];
            Table_AbilityBase abilityBaseMeta = null;
            var len = _specArr.Length;
            for ( var i = 0; i < len && i < abilityIdSet.Length; i++ )
            {
                abilityBaseMeta = GameEntry.LuBan.Tables.Ability.Get( abilityIdSet[i] );
                if ( abilityBaseMeta is null )
                {
                    Log.Warning( "Addon_Ability.Init()->ability_base_meta is null" );
                    return false;
                }
                _specArr[i] = AbilitySpecBase.Gen( abilityBaseMeta, _actorInstance );
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
            if ( !InitSpec() )
                return;

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
            // _meta      = null;
            _initFlag = false;
            base.Dispose();
        }
        
        /// <summary>
        /// 临时缓存变量
        /// </summary>
        private AbilitySpecBase _cahchedAbility = null;
        
        /// <summary>
        /// 持有的技能
        /// </summary>
        private AbilitySpecBase[] _specArr = null;

        /// <summary>
        /// 初始化标记
        /// </summary>
        private bool _initFlag = false;
    }
}
