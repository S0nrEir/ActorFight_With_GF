using Aquila.Fight;
using Cfg.Common;
using Cfg.Enum;
using GameFramework;
using System;
using System.Collections.Generic;
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
            /// 根据配表类型生成对应的effect逻辑实例，拿不到返回null
            /// </summary>
            public static EffectSpec_Base CreateEffectSpecByReferencePool( Table_Effect meta )
            {
                EffectSpec_Base effect = null;
                var type = ( int ) meta.Type;
                if ( !_effectTypeDic.ContainsKey( type ) )
                {
                    Log.Warning( $"Tools.Ability.CreateEffectSpecByReferencePool()--->!_effectTypeDic.ContainsKey( ( int ) meta.Type ),key{meta.Type}" );
                    return null;
                }


                effect = ReferencePool.Acquire( _effectTypeDic[type] ) as EffectSpec_Base;
                if ( effect is null )
                {
                    Log.Warning( $"Tools.Ability.CreateEffectSpecByReferencePool()--->effect is null" );
                    return null;
                }
                effect.Init( meta );
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
