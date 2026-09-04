using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 防御修正：应用受击方的防御力减免、百分比减伤 / Defense modifiers: applies defender damage reduction from defense and % reduction.
    /// </summary>
    internal sealed class DefenseModsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.DefenseMods;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.DefenseModsIo.Input = context.FinalDelta;

            if (context.Request.SourceType == ResolveSourceType.TrueDamage)
            {
                context.DefenseModsIo.Output = context.DefenseModsIo.Input;
                context.FinalDelta = context.DefenseModsIo.Output;
                context.DefenseReduction = 0f;
                result.SetContinue();
                return;
            }

            var attrAddon = context.Request.Target.GetAddon<Addon_BaseAttrNumric>();
            if (attrAddon == null)
            {
                result.SetInterrupt("defense_mods_missing_addon");
                return;
            }

            var physicalDefense = attrAddon.GetCorrectionValue(actor_attribute.DEF, 0f);
            var magicDefense = attrAddon.GetCorrectionValue(actor_attribute.MDEF, 0f);
            var output = CombatDamageCalculator.ApplyDefense(
                context.DefenseModsIo.Input,
                physicalDefense,
                magicDefense,
                context.Request.SourceType);

            context.DefenseModsIo.Output = output;
            context.FinalDelta = output;
            context.DefenseReduction = context.DefenseModsIo.Input - output;
            result.SetContinue();
        }
    }
}
