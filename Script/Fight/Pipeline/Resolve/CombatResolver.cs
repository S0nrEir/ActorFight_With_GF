using System.Collections.Generic;
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
                    // SourceMeta = default,
                };
            }

            var context = ReferencePool.Acquire<ResolveContext>();
            var phaseResult = ReferencePool.Acquire<PhaseExecutionResult>();
            try
            {
                _phaseBuffer.Clear();
                _phaseProvider.TryGetPhases(request.ResolveTypeId, _phaseBuffer);
                if (_phaseBuffer.Count <= 0)
                {
                    context.Setup(request);
                    context.MarkInterrupted("resolve_phase_empty");
                    return BuildResult(context, request.ResolveTypeId);
                }

                context.Setup(request);

                var index = 0;
                var stepCount = 0;
                while (index < _phaseBuffer.Count)
                {
                    stepCount++;
                    if (stepCount > MaxResolveStepCount)
                    {
                        context.MarkInterrupted("resolve_loop_guard");
                        break;
                    }

                    var phaseDef = _phaseBuffer[index];
                    context.LastPhase = phaseDef.Phase;

                    if ((phaseDef.Policy & ResolvePhasePolicy.Skip) != 0)
                    {
                        context.MarkSkipped(phaseDef.Phase);
                        index++;
                        continue;
                    }

                    if ((phaseDef.Policy & ResolvePhasePolicy.InterruptBeforeExecute) != 0)
                    {
                        context.MarkInterrupted("resolve_interrupt_by_policy");
                        break;
                    }

                    if (!_phaseRegistry.TryGetHandler(phaseDef.Phase, out var handler) || handler == null)
                    {
                        context.MarkInterrupted("resolve_handler_missing");
                        break;
                    }

                    phaseResult.SetContinue();
                    handler.Execute(context, phaseDef, phaseResult);

                    switch (phaseResult.SignalType)
                    {
                        case ResolveFlowSignalType.Continue:
                            index++;
                            break;

                        case ResolveFlowSignalType.Skip:
                            context.MarkSkipped(phaseDef.Phase);
                            index++;
                            break;

                        case ResolveFlowSignalType.Interrupt:
                            context.MarkInterrupted(string.IsNullOrEmpty(phaseResult.Reason) ? "resolve_interrupted" : phaseResult.Reason);
                            index = _phaseBuffer.Count;
                            break;

                        case ResolveFlowSignalType.Abort:
                            context.MarkAborted(string.IsNullOrEmpty(phaseResult.Reason) ? "resolve_aborted" : phaseResult.Reason);
                            index = _phaseBuffer.Count;
                            break;

                        case ResolveFlowSignalType.JumpTo:
                            var jumpIndex = FindPhaseIndex(phaseResult.JumpToPhase);
                            if (jumpIndex < 0)
                            {
                                context.MarkInterrupted("resolve_jump_target_not_found");
                                index = _phaseBuffer.Count;
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

                return BuildResult(context, request.ResolveTypeId);
            }
            finally
            {
                _phaseBuffer.Clear();
                ReferencePool.Release(phaseResult);
                ReferencePool.Release(context);
                ReferencePool.Release(request);
            }
        }

        /// <summary>
        /// 找到当前结算队列中指定类型的结算类型下标
        /// </summary>
        private int FindPhaseIndex(ResolvePhaseType phase)
        {
            for (var i = 0; i < _phaseBuffer.Count; i++)
            {
                if (_phaseBuffer[i].Phase == phase)
                    return i;
            }

            return -1;
        }

        private static ResolveResultData BuildResult(ResolveContext context, int requestedResolveTypeId)
        {
            return new ResolveResultData
            {
                Success = !context.IsInterrupted && !context.IsAborted,
                Interrupted = context.IsInterrupted,
                Aborted = context.IsAborted,
                ResolveTypeId = requestedResolveTypeId > 0 ? requestedResolveTypeId : ResolvePhaseProvider.DefaultResolveTypeId,
                LastPhase = context.LastPhase,
                Reason = context.Reason,
            };
            
        }
        
        private const int MaxResolveStepCount = 64;
        private readonly ResolvePhaseProvider _phaseProvider;
        private readonly PhaseRegistry _phaseRegistry;
        private readonly List<ResolvePhaseDefinition> _phaseBuffer = new List<ResolvePhaseDefinition>(16);
    }
}
