using System.Collections.Generic;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Combat.Resolve
{
    public sealed class CombatResolver
    {
        public CombatResolver(ResolvePhaseProvider phaseProvider, PhaseRegistry phaseRegistry)
        {
            _phaseProvider = phaseProvider;
            _phaseRegistry = phaseRegistry;
        }

        public ResolveResultData Resolve(ResolveRequest request)
        {
            if (request == null)
            {
                return new ResolveResultData
                {
                    Success = false,
                    Interrupted = true,
                    Reason = "resolve_request_null",
                    ResolveTypeId = ResolvePhaseProvider.DefaultResolveTypeId,
                    SourceType = ResolveSourceType.Unknown,
                };
            }

            _phaseBuffer.Clear();
            _phaseProvider.TryGetPhases(request.ResolveTypeId, _phaseBuffer);

            var context = ReferencePool.Acquire<ResolveContext>();
            context.SetupResolveReq(request);
            context.ResetPhaseStates(_phaseBuffer);

            var result = ResolveInternal(request, context, _phaseBuffer);
            _phaseBuffer.Clear();
            return result;
        }

        public ResolveResultData Resolve(ResolveRequest request, ResolveContext context, List<ResolvePhaseDefinition> phaseDefinitions)
        {
            if (request == null)
            {
                return new ResolveResultData
                {
                    Success = false,
                    Interrupted = true,
                    Reason = "resolve_request_null",
                    ResolveTypeId = ResolvePhaseProvider.DefaultResolveTypeId,
                    SourceType = ResolveSourceType.Unknown,
                };
            }

            var runtimeContext = context;
            if (runtimeContext is null)
            {
                runtimeContext = ReferencePool.Acquire<ResolveContext>();
                runtimeContext.Setup(request);
            }

            var phases = phaseDefinitions;
            var usingInternalBuffer = false;
            if (phases == null)
            {
                usingInternalBuffer = true;
                _phaseBuffer.Clear();
                _phaseProvider.TryGetPhases(request.ResolveTypeId, _phaseBuffer);
                phases = _phaseBuffer;
            }

            var result = ResolveInternal(request, runtimeContext, phases);
            if (usingInternalBuffer)
                _phaseBuffer.Clear();

            return result;
        }

        private ResolveResultData ResolveInternal(ResolveRequest request, ResolveContext context, List<ResolvePhaseDefinition> phases)
        {
            var phaseResult = ReferencePool.Acquire<PhaseExecutionResult>();
            ResolveResultData resultData;

            if (phases == null || phases.Count <= 0)
            {
                context.MarkInterrupted("resolve_phase_empty");
                resultData = BuildResult(context, request);
                ReferencePool.Release(phaseResult);
                ReferencePool.Release(context);
                ReferencePool.Release(request);
                return resultData;
            }

            var index = 0;
            var stepCount = 0;
            while (index < phases.Count)
            {
                stepCount++;
                if (stepCount > MaxResolveStepCount)
                {
                    context.MarkInterrupted("resolve_loop_guard");
                    break;
                }

                var currentPhase = phases[index];
                context.LastPhase = currentPhase.Phase;

                if ((currentPhase.Policy & ResolvePhasePolicy.Skip) != 0)
                {
                    context.MarkSkipped(currentPhase.Phase);
                    index++;
                    continue;
                }

                if ((currentPhase.Policy & ResolvePhasePolicy.InterruptBeforeExecute) != 0)
                {
                    context.MarkInterrupted("resolve_interrupt_by_policy");
                    break;
                }

                if (!_phaseRegistry.TryGetHandler(currentPhase.Phase, out var handler) || handler == null)
                {
                    context.MarkInterrupted("resolve_handler_missing");
                    break;
                }

                phaseResult.SetContinue();
                handler.Execute(context, currentPhase, phaseResult);

                switch (phaseResult.SignalType)
                {
                    case ResolveFlowSignalType.Continue:
                        index++;
                        break;

                    case ResolveFlowSignalType.Skip:
                        context.MarkSkipped(currentPhase.Phase);
                        index++;
                        break;

                    case ResolveFlowSignalType.Interrupt:
                        context.MarkInterrupted(string.IsNullOrEmpty(phaseResult.Reason) ? "resolve_interrupted" : phaseResult.Reason);
                        index = phases.Count;
                        break;

                    case ResolveFlowSignalType.Abort:
                        context.MarkAborted(string.IsNullOrEmpty(phaseResult.Reason) ? "resolve_aborted" : phaseResult.Reason);
                        index = phases.Count;
                        break;

                    case ResolveFlowSignalType.JumpTo:
                        var jumpIndex = FindPhaseIndex(phaseResult.JumpToPhase, phases, index);
                        if (jumpIndex < 0)
                        {
                            context.MarkInterrupted("resolve_jump_target_not_found");
                            index = phases.Count;
                        }
                        else
                        {
                            index = jumpIndex;
                        }
                        break;

                    default:
                        index++;
                        break;
                }

                if (context.IsInterrupted || context.IsAborted)
                    break;
            }

            resultData = BuildResult(context, request);
            ReferencePool.Release(phaseResult);
            ReferencePool.Release(context);
            ReferencePool.Release(request);
            return resultData;
        }

        private static int FindPhaseIndex(ResolvePhaseType phase, List<ResolvePhaseDefinition> phases, int startIndex)
        {
            for (var i = startIndex; i < phases.Count; i++)
            {
                if (phases[i].Phase == phase)
                    return i;
            }

            return -1;
        }

        private static ResolveResultData BuildResult(ResolveContext context, ResolveRequest request)
        {
            var finalDelta = context != null ? context.FinalDelta : 0f;
            var totalIncrease = context != null ? context.OffenseIncrease + context.CritIncrease : 0f;
            var totalAbsorb = context != null ? context.ShieldAbsorb : 0f;
            var totalReduction = context != null ? context.DefenseReduction + context.BlockReduction + context.ShieldAbsorb : 0f;

            return new ResolveResultData
            {
                Success = context != null && !context.IsInterrupted && !context.IsAborted,
                Interrupted = context != null && context.IsInterrupted,
                Aborted = context != null && context.IsAborted,
                ResolveTypeId = request != null && request.ResolveTypeId > 0
                    ? request.ResolveTypeId
                    : ResolvePhaseProvider.DefaultResolveTypeId,
                SourceType = request != null ? request.SourceType : ResolveSourceType.Unknown,
                InputDelta = request != null ? request.InputDelta : 0f,
                FinalDelta = finalDelta,
                TotalIncrease = totalIncrease,
                TotalReduction = totalReduction,
                TotalAbsorb = totalAbsorb,
                AppliedDelta = context != null ? context.AppliedHpDelta : 0f,
                LastPhase = context != null ? context.LastPhase : ResolvePhaseType.Validity,
                Reason = context != null ? context.Reason : null,
            };
        }

        private const int MaxResolveStepCount = 64;
        private readonly ResolvePhaseProvider _phaseProvider;
        private readonly PhaseRegistry _phaseRegistry;
        private readonly List<ResolvePhaseDefinition> _phaseBuffer = new List<ResolvePhaseDefinition>(16);
    }
}