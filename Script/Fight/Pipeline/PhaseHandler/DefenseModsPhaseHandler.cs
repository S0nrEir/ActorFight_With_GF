using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 闃插尽淇锛氬簲鐢ㄥ彈鍑绘柟鐨勯槻寰″姏鍑忓厤銆佺櫨鍒嗘瘮鍑忎激 / Defense modifiers: applies defender damage reduction from defense and % reduction.
    /// </summary>
    internal sealed class DefenseModsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.DefenseMods;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("defense_mods_invalid_context");
                return;
            }

            context.DefenseModsIo.Input = context.FinalDelta;
            context.DefenseModsIo.Output = context.FinalDelta;
            context.DefenseReduction = context.DefenseModsIo.Input - context.DefenseModsIo.Output;
            result.SetContinue();
        }
    }
}