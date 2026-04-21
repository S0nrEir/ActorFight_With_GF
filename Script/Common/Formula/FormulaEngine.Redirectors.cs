using System.Collections.Generic;
using Aquila.Combat.Resolve;
using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Formula
{
    public sealed partial class FormulaEngine
    {
        /// <summary>
        /// 注册内置标识符重定向器：将 castor/target 与各属性的访问函数绑定。
        /// </summary>
        private static void RegisterBuiltinIdentifierRedirectors(FormulaEngine engine)
        {
            var redirectors = BuildBuiltinRedirectors();
            foreach (var kvp in redirectors)
                engine.RegisterIdentifierRedirector(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// 构建内置标识符重定向器映射表。
        /// 每条 castor.xxx / target.xxx 显式写死，不拼字符串。
        /// </summary>
        private static Dictionary<string, FormulaIdentifierRedirector> BuildBuiltinRedirectors()
        {
            var redirectors = new Dictionary<string, FormulaIdentifierRedirector>(StringComparer.OrdinalIgnoreCase);

            // ---- castor 属性 ----
            redirectors["castor.hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Curr_HP, 0f);
                return true;
            };

            redirectors["castor.max_hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Max_HP, 0f);
                return true;
            };

            redirectors["castor.mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Curr_MP, 0f);
                return true;
            };

            redirectors["castor.max_mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Max_MP, 0f);
                return true;
            };

            redirectors["castor.atk"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.ATK, 0f);
                return true;
            };

            redirectors["castor.def"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.DEF, 0f);
                return true;
            };

            redirectors["castor.spd"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.SPD, 0f);
                return true;
            };

            redirectors["castor.mvt"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.MVT, 0f);
                return true;
            };

            redirectors["castor.str"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.STR, 0f);
                return true;
            };

            redirectors["castor.agi"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.AGI, 0f);
                return true;
            };

            redirectors["castor.spw"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.SPW, 0f);
                return true;
            };

            // ---- target 属性 ----
            redirectors["target.hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Curr_HP, 0f);
                return true;
            };

            redirectors["target.max_hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Max_HP, 0f);
                return true;
            };

            redirectors["target.mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Curr_MP, 0f);
                return true;
            };

            redirectors["target.max_mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.Max_MP, 0f);
                return true;
            };

            redirectors["target.atk"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.ATK, 0f);
                return true;
            };

            redirectors["target.def"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.DEF, 0f);
                return true;
            };

            redirectors["target.spd"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.SPD, 0f);
                return true;
            };

            redirectors["target.mvt"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.MVT, 0f);
                return true;
            };

            redirectors["target.str"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.STR, 0f);
                return true;
            };

            redirectors["target.agi"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.AGI, 0f);
                return true;
            };

            redirectors["target.spw"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;

                value = attr.GetCorrectionValue(actor_attribute.SPW, 0f);
                return true;
            };

            // ---- efct 参数 ----
            redirectors["efct.i1"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetIntParam1();
                return true;
            };

            redirectors["efct.i2"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetIntParam2();
                return true;
            };

            redirectors["efct.i3"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetIntParam3();
                return true;
            };

            redirectors["efct.i4"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetIntParam4();
                return true;
            };

            redirectors["efct.f1"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetFloatParam1();
                return true;
            };

            redirectors["efct.f2"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetFloatParam2();
                return true;
            };

            redirectors["efct.f3"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetFloatParam3();
                return true;
            };

            redirectors["efct.f4"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx is null)
                    return false;

                value = resolveCtx.Request.EffectData.GetFloatParam4();
                return true;
            };

            return redirectors;
        }
    }
}
