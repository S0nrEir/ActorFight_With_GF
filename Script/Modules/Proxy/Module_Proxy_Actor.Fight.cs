using Aquila.Fight;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace  Aquila.Module
{
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


    //一个冷却的例子：
    //var cdSpec   = this.Owner.MakeOutgoingSpec(this.Ability.Cooldown);
    //this.Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
    //技能数据(this)的持有者(asc)获取对应类型的effectSpec实例
    //然后将该实例应用于owner

    /// <summary>
    /// Module_Proxy_Actor的部分类，用于处理actor proxy instance的战斗逻辑
    /// </summary>
    public partial class Module_Proxy_Actor
    {
        /// <summary>
        /// 单对单释放技能
        /// </summary>
        public AbilityResult AbilityToSingleTarget(int castor_id,int target_id,int ability_meta_id)
        {
            
            //obtain ability result
            var result = default(AbilityResult);
            result.Init();
            result._castor_actor_id = castor_id;
            result._target_actor_id = target_id;
            
            //检查类型走不同的流程
            // if (castor is null || target is null)
            // {
            //     Log.Warning("<color=yellow>castor is null || target is null</color>");
            //     result.SetState(AbilityResultDescTypeEnum.INVALID);
            //     return result;
            // }
            
            //拿技能组件
            var castor_instance = TryGet(castor_id);
            Addon_Ability ability_addon = null; 
            if (!castor_instance.has)
            {
                // result.SetState(AbilityResultDescTypeEnum.CANT_USE);
                return result;
            }

            //检查释放条件
            ability_addon = castor_instance.instance.GetAddon<Addon_Ability>();
            if (!ability_addon.CanUseAbility(ability_meta_id,ref result))
            {
                // result.SetState(AbilityResultDescTypeEnum.CANT_USE);
                return result;
            }

            var target_instance = TryGet(target_id);
            if (!target_instance.has)
            {
                // result.SetState(AbilityResultDescTypeEnum.CANT_USE);
                return result;
            }

            ability_addon.UseAbility(ability_meta_id, target_instance.instance, ref result);
            
            //#todo:使用玩技能后玩家面板如何表现，考虑在这里更新，或者effect的实现里更新？（我觉得在这里更新比较好 by boxing）
            return result;
        }
    }
}