using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 鍛戒腑瑁佸畾锛氬熀浜庡懡涓巼涓庨棯閬跨巼杩涜闅忔満鍒ゅ畾锛孧iss 鏃惰烦杩囨暟鍊奸樁娈?/ Hit check: roll against hit rate vs evasion; skip value phases on miss.
    /// </summary>
    internal sealed class HitCheckPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.HitCheck;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("hit_check_invalid_context");
                return;
            }

            context.HitCheckIo.Input = context.FinalDelta;
            context.HitCheckIo.Output = context.FinalDelta;
            result.SetContinue();
        }
    }
}