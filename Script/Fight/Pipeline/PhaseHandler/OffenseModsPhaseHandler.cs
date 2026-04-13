using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 杩涙敾淇锛氬簲鐢ㄦ敾鍑绘柟鐨勭櫨鍒嗘瘮澧炰激銆佸睘鎬ц浆鍖栫瓑鍔犳垚 / Offense modifiers: applies attacker bonuses such as damage % buffs and attribute conversions.
    /// </summary>
    internal sealed class OffenseModsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.OffenseMods;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("offense_mods_invalid_context");
                return;
            }

            context.OffenseModsIo.Input = context.FinalDelta;
            context.OffenseModsIo.Output = context.FinalDelta;
            context.OffenseIncrease = context.OffenseModsIo.Output - context.OffenseModsIo.Input;
            result.SetContinue();
        }
    }
}