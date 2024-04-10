using Aquila.Fight;
using Cfg.Common;
using Cfg.Enum;
using GameFramework;
using System;
using System.Collections.Generic;
using Aquila.Module;
using UnityGameFramework.Runtime;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// 技能工具类
        /// </summary>
        public static class Ability
        {
            /// <summary>
            /// 是否可以通过百分比计算应用修改属性的effect
            /// </summary>
            /// <returns></returns>
            public static bool CanApplyModifyAttrByEffect_ByPercentage(float baseValue,float conditionValue, float primitive)
            {
                primitive /= baseValue;
                return primitive >= conditionValue;
            }

            /// <summary>
            /// 是否可以按固定值计算应用修改属性的effect
            /// </summary>
            public static bool CanApplyModifyAttrEffect_ByFixed(float conditionValue,float valueToCheck)
            {
                return valueToCheck >= conditionValue;
            }
            
            /// <summary>
            /// 初始化effect生成器
            /// </summary>
            public static void InitEffectSpecGenerator()
            {
                if ( _generatorInitFlag )                 
                {
                    Log.Warning( $"<color=yellow>Tools.Ability.InitEffectSpecGenerator()--->_generatorInitFlag is true</color>" );
                    return;
                }

                var enumFields = typeof( EffectType ).GetFields();
                var len = enumFields.Length;
                _effectTypeDic = new Dictionary<int, Type>( len );
                object fieldInst = null;
                EffectType tempEnum = EffectType.Period_CoolDown;
                string typeName = string.Empty;
                //从1开始：system.int32跳过
                for ( var i = 1; i < len; i++ )
                {
                    var field = enumFields[i];
                    if ( !field.FieldType.IsEnum )
                        continue;

                    tempEnum = ( EffectType ) field.GetValue( fieldInst );
                    typeName = $"Aquila.Fight.EffectSpec_{field.Name}";
                    _effectTypeDic.Add( ( int ) tempEnum, Type.GetType( typeName ) );
                }

                _generatorInitFlag = true;
            }

            /// <summary>
            /// 根据配表类型获取对应的effect逻辑实例，拿不到返回null
            /// </summary>
            public static EffectSpec_Base CreateEffectSpecByReferencePool( Table_Effect meta,Module_ProxyActor.ActorInstance castor,Module_ProxyActor.ActorInstance target)
            {
                var type = ( int ) meta.Type;

                if (!_effectTypeDic.TryGetValue(type, out var effectType))
                {
                    Log.Warning( $"Tools.Ability.CreateEffectSpecByReferencePool()--->!_effectTypeDic.ContainsKey( ( int ) meta.Type ),key{meta.Type}" );
                    return null;
                }

                var effect = ReferencePool.Acquire(effectType) as EffectSpec_Base;
                if (effect is null)
                {
                    Log.Warning( $"Tools.Ability.CreateEffectSpecByReferencePool()--->effect is null" );
                    return null;
                }

                effect.Init(meta, castor, target);
                return effect;
                #region nouse

                //EffectSpec_Base effect = null;
                //switch ( meta.Type )
                //{
                //    case EffectType.Instant_PhyDamage:
                //        //return new EffectSpec_PhyDamage(meta);
                //        effect = ReferencePool.Acquire<EffectSpec_Instant_PhyDamage>();
                //        break;

                //    case EffectType.Period_FixedDamage:
                //        effect = ReferencePool.Acquire<EffectSpec_Period_FixedDamage>();
                //        break;

                //    case EffectType.Period_DerivingStack:
                //        effect = ReferencePool.Acquire<EffectSpec_Period_DerivingStack>();
                //        break;

                //    case EffectType.Instant_PercentageRemoveHealth:
                //        effect = ReferencePool.Acquire<EffectSpec_Instant_PercentageRemoveHealth>();
                //        break;

                //    case EffectType.Period_ActorTag:
                //        effect = ReferencePool.Acquire<EffectSpec_Period_ActorTag>();
                //        break;

                //    case EffectType.Instant_Summon:
                //        break;

                //    default:
                //        return null;
                //}

                //effect.Init( meta );
                //return effect;

                #endregion
            }

            /// <summary>
            /// 创建对应的effect逻辑实例，创建失败返回null
            /// </summary>
            public static T CreateEffectSpecByReferencePool<T>() where T : EffectSpec_Base
            {
                foreach (var type in _effectTypeDic.Values)
                {
                    if (type == typeof(T))
                    {
                        var effect = ReferencePool.Acquire(type);
                        return effect as T;
                    }
                }

                return null;
            }

            /// <summary>
            /// 创建对应的effect逻辑实例，创建失败返回null
            /// </summary>
            public static T CreateEffectSpecByReferencePool<T>(Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
                Module_ProxyActor.ActorInstance target = null) where T : EffectSpec_Base
            {
                return CreateEffectSpecByReferencePool(meta,castor,target) as T;
            }

            /// <summary>
            /// 初始化标记
            /// </summary>
            private static bool _generatorInitFlag = false;

            /// <summary>
            /// effect类型集合，存储所有effect的类型，方便生成
            /// </summary>
            private static Dictionary<int, Type> _effectTypeDic = null;

        }//end class Ability
    }//end class Tools
}
