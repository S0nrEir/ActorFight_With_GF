using Aquila.Event;
using Aquila.Fight;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    //简单说明：targetSingle将会让actor尝试调用【使用技能行为】，在通用的技能行为中，会让actor进入abilityState，进入abilityState后，有一
    //段timeline作为演出，并且持有一个时间表示技能前摇，前摇结束后，会调用本类的AffectAbility去应用携带的effect
    //一般来说这是正常的流程，如果你想直接激活effect，直接调用AffectAbility就好了
    //AffectAbility会调用技能组件应用携带的相关effect
    //state/behaviour:一整套技能流程
    //abilityAddon:直接使用单个技能
    
    /// <summary>
    /// Module_Proxy_Actor的部分类，用于处理actor proxy instance的战斗逻辑
    /// </summary>
    public partial class Module_ProxyActor
    {
        #region ability相关

        /// <summary>
        /// 应用一个effect的效果
        /// </summary>
        public void ApplyEffect(int castorID, int targetID, EffectSpec_Base effect)
        {
            ApplyEffect(Get(castorID),Get(targetID),effect);
        }

        /// <summary>
        /// 应用一个Effect的效果
        /// </summary>
        public void ApplyEffect ( ActorInstance castor, ActorInstance target, EffectSpec_Base effect )
        {
            var result = AbilityResult_Hit.Gen( castor.Actor.ActorID, target.Actor.ActorID );
            if ( castor is null || target is null )
            {
                Log.Warning( $"<color=yellow>Module_ProxyActor.Fight=====>ImplImpact()--->castor is null || target is null</color>" );
                //ReferencePool.Release( result );
                InvalidEffect( castor, target, effect, false );
                GameEntry.Event.Fire( this, EventArg_OnHitAbility.Create( result ) );
                return;
            }
            effect.Apply( castor, target, result );
            GameEntry.Event.Fire( this, EventArg_OnHitAbility.Create( result ) );
            if ( result._dealedDamage != 0 )
                GameEntry.InfoBoard.ShowDamageNumber( result._dealedDamage.ToString(), target.Actor.CachedTransform.position );

            ReferencePool.Release( result );
            TryRefreshActorHPUI( target );
            //DieIfEmptyHP( target );
            DieIfEmptyHPAndHide( target );
        }

        /// <summary>
        /// 应用一个effect的唤起效果
        /// </summary>
        public void ApplyAwakeEffect(int castorID, int targetID, EffectSpec_Base effect)
        {
            ApplyAwakeEffect(Get(castorID), Get(targetID), effect);
        }

        /// <summary>
        /// 应用一个effect的唤起效果
        /// </summary>
        public void ApplyAwakeEffect( ActorInstance castor, ActorInstance target, EffectSpec_Base effect )
        {
            if ( castor is null || target is null )
            {
                Log.Warning( "<color=yellow>Module_ProxyActor.Fight=====>ApplyEffect2Actor()--->!castorInstance.has </color>" );
                return;
            }
            var result = AbilityResult_Hit.Gen( castor.Actor.ActorID, target.Actor.ActorID );

            effect.OnEffectAwake( castor, target );
            if ( result._dealedDamage != 0 )
                GameEntry.InfoBoard.ShowDamageNumber( $"{( result._dealedDamage ).ToString()}", target.Actor.CachedTransform.position );

            GameEntry.Event.Fire( castor, EventArg_OnHitAbility.Create( result ) );
            ReferencePool.Release( result );
        }

        /// <summary>
        /// 生效技能，直接让一个effect命中，而无需使用
        /// </summary>
        public void AffectAbility( int castorID, int targetID, int abilityID, Vector3 targetPosition )
        {
            var result = AbilityResult_Hit.Gen( castorID, targetID );

            var castorInstance = TryGet( castorID );
            if ( !castorInstance.has )
            {
                Log.Warning( "<color=yellow>Module_ProxyActor.Fight=====>ApplyEffect2Actor()--->!castorInstance.has </color>" );
                return;
            }

            ActorInstance targetInstance = null;
            if ( targetID != -1 )
            {
                var temp = TryGet( targetID );
                if ( !temp.has )
                {
                    Log.Warning( "<color=yellow>Module_ProxyActor.Fight===>ApplyEffect2Actor()--->!targetInstance.has</color>" );
                    return;
                }
                targetInstance = temp.instance;
            }

            if ( targetPosition != GameEntry.GlobalVar.InvalidPosition )
            {
                result._targetPosition = targetPosition;
                result._stateDescription = Tools.SetBitValue( result._stateDescription, ( int ) AbilityHitResultTypeEnum.CONTAINS_POSITION, true );
            }
            else
            {
                result._stateDescription = Tools.SetBitValue( result._stateDescription, ( int ) AbilityHitResultTypeEnum.CONTAINS_POSITION, false );
            }

            var addon = castorInstance.instance.GetAddon<Addon_Ability>();
            if ( addon is null )
            {
                Log.Warning( "<color=yellow>Module_ProxyActor.Fight=====>ApplyEffect2Actor()--->!castorInstance.has</color>" );
                return;
            }

            addon.UseAbility( abilityID, targetInstance, result );
            if ( targetInstance != null && result._dealedDamage != 0 )
                GameEntry.InfoBoard.ShowDamageNumber( $"{( result._dealedDamage ).ToString()}", targetInstance.Actor.CachedTransform.position );

            GameEntry.Event.Fire( castorInstance, EventArg_OnHitAbility.Create( result ) );
            ReferencePool.Release( result );

            TryRefreshActorHPUI( castorInstance.instance );
            TryRefreshActorHPUI( targetInstance );
            DieIfEmptyHPAndHide( targetInstance );
        }

        /// <summary>
        /// 无效化一个effect
        /// </summary>
        public void InvalidEffect( int castorID, int targetID, EffectSpec_Base effect, bool callEnd = false )
        {
            InvalidEffect( Get( castorID ), Get( targetID ), effect, callEnd );
        }

        /// <summary>
        /// 无效化一个effect
        /// </summary>
        public void InvalidEffect( ActorInstance castor, ActorInstance target, EffectSpec_Base effect, bool callEnd = true )
        {
            if ( castor is null || target is null )
            {
                Log.Warning( $"<color=yellow>Module_ProxyActor.Fight=====>InvalidEffect()--->castor is null || target is null</color>" );
                return;
            }
            if ( callEnd )
                effect.OnEffectEnd( castor, target );

            ReferencePool.Release( effect );
        }

        /// <summary>
        /// 单对单释放技能，需要先尝试使用，然后让技能击中
        /// </summary>
        public void Ability2SingleTarget( int castorID, int targetID, int abilityMetaID, Vector3 position )
        {
            AbilityResult_Use result = ReferencePool.Acquire<AbilityResult_Use>();
            result._succ = false;
            result._stateDescription = 0;
            result._castorID = -1;
            if ( Get( castorID ) != null )
            {
                result._castorID = castorID;
            }
            else
            {
                result._stateDescription = 0;
                result._castorID = -1;
                result._stateDescription = Tools.SetBitValue( result._stateDescription,
                    ( int ) AbilityUseResultTypeEnum.NO_CASTOR, true );
                GameEntry.Event.Fire( this, EventArg_OnUseAblity.Create( result ) );
                ReferencePool.Release( result );
                return;
            }

            result._targetPosition = position;
            if ( Get( targetID ) != null )
            {
                result._targetIDArr = new int[] { targetID }; result._stateDescription = Tools.SetBitValue( result._stateDescription,
                    ( int ) AbilityUseResultTypeEnum.IS_TARGET_AS_POSITION, false );
            }
            else
            {
                if ( position != GameEntry.GlobalVar.InvalidPosition )
                {
                    result._stateDescription = Tools.SetBitValue( result._stateDescription,
                   ( int ) AbilityUseResultTypeEnum.IS_TARGET_AS_POSITION, true );
                }
                else
                {
                    //没有target也没有position
                    //todo:也有可能是无目标技能
                    result._stateDescription = Tools.SetBitValue( result._stateDescription,
                   ( int ) AbilityUseResultTypeEnum.NO_TARGET, true );
                    GameEntry.Event.Fire( this, EventArg_OnUseAblity.Create( result ) );
                    //ReferencePool.Release( result );
                    return;
                }
            }

            result._abilityID = abilityMetaID;
            var castorInstance = TryGet( castorID );
            if ( !castorInstance.has )
            {
                result._stateDescription =
                    Tools.SetBitValue( result._stateDescription, ( int ) AbilityUseResultTypeEnum.NO_CASTOR, true );
                // return result;
            }
            castorInstance.instance.GetAddon<Addon_Behaviour>()?.Exec( ActorBehaviourTypeEnum.ABILITY, result );
            GameEntry.Event.Fire( this, EventArg_OnUseAblity.Create( result ) );

            //is died
            var target = Get( targetID );
            //DieIfEmptyHP( target );
            DieIfEmptyHPAndHide( target );
        }
        #endregion

        /// <summary>
        /// 检查是否没血，是就进入死亡状态并且返回true，否则返回false
        /// </summary>
        private bool DieIfEmptyHP( ActorInstance instance )
        {
            var attrAddon = instance.GetAddon<Addon_BaseAttrNumric>();
            if ( attrAddon != null && attrAddon.GetCurrHPCorrection() <= 0 )
            {
                instance.GetAddon<Addon_Behaviour>()?.Exec( ActorBehaviourTypeEnum.DIE, null );
                GameEntry.Event.Fire( this, EventArg_OnActorDie.Create( instance.Actor.ActorID ) );
                return true;
            }

            instance.GetAddon<Addon_HP>()?.Refresh();
            return false;
        }

        /// <summary>
        /// 检查一个actor是否没血，是就让其死亡并且隐藏
        /// </summary>
        private void DieIfEmptyHPAndHide( ActorInstance instance )
        {
            if ( !DieIfEmptyHP( instance ) )
                return;

            var actor = instance.Actor;
            //todo:这里的流程是，actor死亡后隐藏，但不是直接调用entity组件的hide，这会导致entity直接被回收放回池里
            //actor的死亡仍然是业务逻辑的一部分，这里只是暂时用GameEntry.Entity.HideEntity来替代隐藏actor
            GameEntry.Entity.HideEntity( actor.ActorID );
        }

        /// <summary>
        /// 尝试刷新actor头顶的血条UI
        /// </summary>
        public void TryRefreshActorHPUI( ActorInstance instance )
        {
            instance.GetAddon<Addon_HP>()?.Refresh();
        }

        //----------------------- priv -----------------------

        /// <summary>
        /// 战斗部分模块初始化
        /// </summary>
        private void FightEnsureInit()
        {
            Tools.Ability.InitEffectSpecGenerator();
        }
    }
    
    //关于GAS的参考
    //AbilityAddon：AbilitySpec
    //AbilitySpec:技能使用逻辑，持有一些GameplayEffectSpec，技能逻辑，持有技能元数据；尝试使用技能；是否可使用（CD？消耗？）；技能前摇；使用技能；
    //GameplayEffectSpec：持有GameEffect
    //GamePlayEffect
    //GameEffect:表示持有一些tag用于描述该技能的类型或状态（比如“CD”类型）
    //TagAddon:保存人物tag

    //AbilitySpec：目前考虑的是由AbilityAddon持有，表示技能逻辑，持有技能元数据；尝试使用技能；是否可使用（CD？消耗？）；技能前摇；使用技能；
    //根据技能元数据创建一个对应类型的技能逻辑实例（比如投射物类型；召唤物；瞬发）

    //GameplayAbility:表示一个技能->ActiveAbility(),CancelAbility()等
    //技能也需要加上tag，并且有如下属性（酌情添加，也可以考虑使用TagContainer）
    //Ability Tags:GameplayAbility拥有的GameplayTag, 这只是用来描述GameplayAbility的GameplayTag.
    //Cancel Abilities with Tag:当该GameplayAbility激活时, 其他Ability Tags中拥有这些GameplayTag的GameplayAbility将会被取消.
    //Block Abilities with Tag:当该GameplayAbility激活时, 其他Ability Tags中拥有这些GameplayTag的GameplayAbility将会阻塞激活.
    //Activation Owned Tags:当该GameplayAbility激活时, 这些GameplayTag会交给该GameplayAbility的拥有者.
    //Activation Required Tags:该GameplayAbility只有在其拥有者拥有所有这些GameplayTag时才会激活.
    //Activation Blocked Tags:该GameplayAbility在其拥有者拥有任意这些标签时不能被激活.
    //Source Required Tags:该GameplayAbility只有在Source拥有所有这些GameplayTag时才会激活. Source GameplayTag只有在该GameplayAbility由Event触发时设置.
    //Source Blocked Tags:该GameplayAbility在Source拥有任意这些标签时不能被激活. Source GameplayTag只有在该GameplayAbility由Event触发时设置.
    //Target Required Tags:该GameplayAbility只有在Target拥有所有这些GameplayTag时才会激活. Target GameplayTag只有在该GameplayAbility由Event触发时设置.
    //Target Blocked Tags:该GameplayAbility在Target拥有任意这些标签时不能被激活. Target GameplayTag只有在该GameplayAbility由Event触发时设置.

    //GameplayEffect:表示一个effect，可以添加GameCue，并且持有一个Modifier，它是一个配置数据，不应该在运行时创建
    //GAS中的做法是，提供一个接口来给actor应用效果，比如effect.ApplyTo(actor);
    //但这样其实是把每个effect和actor关联到了一起，持续性的buff如果要用buffCompoent来做的话，需要保存对应的索引然后轮询才可以
    //在GAS中，GE还有一个GESpec用来处理他的逻辑，它可以在运行时创建

}