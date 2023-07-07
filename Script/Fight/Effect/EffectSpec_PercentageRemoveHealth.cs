using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;

namespace Aquila.Fight
{
    /// <summary>
    /// 百分比移除生命值
    /// </summary>
    public class EffectSpec_RemoveHealth : EffectSpec_Base//#todo___添加custome接口
    {
        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            base.Apply( castor, target, result );
            var addon = target.GetAddon<Addon_BaseAttrNumric>();
            if ( addon is null )
            {

            }
            //#todo___添加持有的tag，可以考虑放到父类
        }
    }
}
