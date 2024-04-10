using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Numric;
using Cfg.Enum;
using Cfg.Role;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

//对于当前生命，当前魔法这类变更没有来源依据（比如modifier）属性，无需使用修正值，只需要baseValue即可
namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 基础属性组件
    /// ----------------------+-----------------------+-----------------------+----------------
    /// Numric_1              | Numric_2              | Numric_3              | Numric_4
    /// ----------------------+-----------------------+-----------------------+----------------
    /// xxx_Numric_Modifier_1 |xxx_Numric_Modifier_1  |xxx_Numric_Modifier_1  |xxx_Numric_Modifier_1
    /// xxx_Numric_Modifier_2 |xxx_Numric_Modifier_2  |xxx_Numric_Modifier_2  |xxx_Numric_Modifier_2
    /// xxx_Numric_Modifier_3 |xxx_Numric_Modifier_3  |xxx_Numric_Modifier_3  |xxx_Numric_Modifier_3
    /// ----------------------+-----------------------+-----------------------+----------------
    /// yyy_Numric_Modifier_1 |yyy_Numric_Modifier_1  |yyy_Numric_Modifier_1  |yyy_Numric_Modifier_1
    /// yyy_Numric_Modifier_2 |yyy_Numric_Modifier_2  |yyy_Numric_Modifier_2  |yyy_Numric_Modifier_2
    /// yyy_Numric_Modifier_3 |yyy_Numric_Modifier_3  |yyy_Numric_Modifier_3  |yyy_Numric_Modifier_3
    /// ----------------------+-----------------------+-----------------------+----------------------
    /// zzz_Numric_Modifier_1 |zzz_Numric_Modifier_1  |zzz_Numric_Modifier_1  |zzz_Numric_Modifier_1
    /// zzz_Numric_Modifier_2 |zzz_Numric_Modifier_2  |zzz_Numric_Modifier_2  |zzz_Numric_Modifier_2
    /// zzz_Numric_Modifier_3 |zzz_Numric_Modifier_3  |zzz_Numric_Modifier_3  |zzz_Numric_Modifier_3
    /// ----------------------+-----------------------+-----------------------+---------------------
    /// </summary>
    public class Addon_BaseAttrNumric : Addon_Base
    {
        //-----------------------pub-----------------------
        /// <summary>
        /// 获取某项属性的基础值
        /// </summary>
        public (bool getSucc, float value) GetBaseValue( actor_attribute type )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return (false, 0f);

            return (true, _numricArr[intType].BaseValue);
        }

        /// <summary>
        /// 设置某项属性的基础值，返回修改后的值和成功标记
        /// </summary>
        public (bool setSucc, float valueAfterSet) SetBaseValue( actor_attribute type, float valueToSet )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return (false, _numricArr[intType].BaseValue);

            _numricArr[intType].SetBaseVal( valueToSet );
            return (true, _numricArr[intType].BaseValue);
        }
        
        /// <summary>
        /// 设置HP值
        /// </summary>
        public (bool setSucc, float valueAfterSet) SetCurrHP(float valueToSet)
        {
            _numricArr[(int)actor_attribute.Curr_HP].SetBaseVal(valueToSet);
            return (true, _numricArr[(int)actor_attribute.Curr_HP].CorrectionValue);
        }

        /// <summary>
        /// 设置MP值
        /// </summary>
        public (bool setSucc, float valueAfterSet) SetCurrMP(float valueToSet)
        {
            _numricArr[(int)actor_attribute.Curr_MP].SetBaseVal(valueToSet);
            return (true, _numricArr[(int)actor_attribute.Curr_MP].CorrectionValue);
        }

        ///// <summary>
        ///// 获取hp上限的修正值
        ///// </summary>
        //public float GetCurrHPMaxCorrection()
        //{
        //    var res = GetCorrectionFinalValue( Actor_Base_Attr.HP, 0f );
        //    return res.value;
        //}

        /// <summary>
        /// 获取当前hp的修正值
        /// </summary>
        public float GetCurrHPCorrection()
        {
            // return _currHP.CorrectionValue;
            return _numricArr[(int)actor_attribute.Curr_HP].CorrectionValue;
        }

        /// <summary>
        /// 获取当前mp的修正值
        /// </summary>
        public float GetCurrMPCorrection()
        {
            // return _currMP.CorrectionValue;
            return _numricArr[(int)actor_attribute.Max_MP].CorrectionValue;
        }

        /// <summary>
        /// 获取某项属性的最终修正值
        /// </summary>
        // public (bool getSucc, float value) GetCorrectionValue( actor_attribute type ,float default_value = 0f)
        // {
        //     var intType = ( int ) type;
        //     if ( OverLen( intType ) )
        //         return (false, default_value);
        //
        //     return (true, _numricArr[intType].CorrectionValue);
        // }
        
        /// <summary>
        /// 获取某项属性的最终修正值
        /// </summary>
        public float GetCorrectionValue(actor_attribute type, float defaultValue)
        {
            var intType = ( int ) type;
            // if ( OverLen( intType ) )
            //     return defaultValue;

            return _numricArr[intType].CorrectionValue;
        }

        /// <summary>
        /// 设置一个装备类型的数值修饰器
        /// </summary>
        public bool SetEquipModifier( actor_attribute type, Numric_Modifier modifier )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return false;

            return _numricArr[intType].AddEquipModifier( modifier );
        }

        /// <summary> 设置一个buff类型的数值修饰器 </summary>
        public bool SetEffectModifier( actor_attribute type, Numric_Modifier modifier )
        {
            var intType = ( int ) type;
            // if ( OverLen( intType ) )
            //     return false;

            return _numricArr[intType].AddBuffModifier( modifier );
        }

        /// <summary> 移除属性修饰器 </summary>
        public bool RemoveEffectModifier(actor_attribute type, Numric_Modifier modifier)
        {
            var intType = (int)type;
            if (OverLen(intType))
                return false;

            return _numricArr[intType].RemoveBuffModifier(modifier);
        }

        /// <summary>
        /// 设置一个职业修正的数值修饰器
        /// </summary>
        public bool SetClassModifier( actor_attribute type, Numric_Modifier modifier )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return false;                   

            return _numricArr[intType].AddClassModifier( modifier );
        }
        

        //----------------------priv----------------------
        /// <summary>
        /// 设置基础属性
        /// </summary>
        private void SetBaseAttr( Table_RoleMeta meta )
        {
            //现在是写死的，很蛋疼
            var proxy_module = GameEntry.Module.GetModule<Module_ProxyActor>();
            
            //max hp & curr hp
            var res = SetBaseValue( actor_attribute.Curr_HP, meta.base_attr_value.max_hp );
            SetBaseValue(actor_attribute.Max_HP, meta.hp_factor * meta.base_attr_value.max_hp);
            SetCurrHP(res.valueAfterSet);
            //max mp & curr mp
            res = SetBaseValue( actor_attribute.Curr_MP, meta.base_attr_value.max_mp );
            SetBaseValue(actor_attribute.Max_MP, meta.mp_factor * meta.base_attr_value.max_mp);
            SetCurrMP(res.valueAfterSet);
            //str，系数*值
            SetBaseValue( actor_attribute.STR, meta.str_factor * meta.base_attr_value.str );
            //def
            SetBaseValue( actor_attribute.DEF, meta.def_factor * meta.base_attr_value.def );
            //agi
            SetBaseValue( actor_attribute.AGI, meta.agi_factor * meta.base_attr_value.agi );
            //spd
            SetBaseValue( actor_attribute.SPD, meta.spd_factor * meta.base_attr_value.spd );
            //mvt
            SetBaseValue( actor_attribute.MVT, meta.mvt_factor * meta.base_attr_value.mvt );
            //spw
            SetBaseValue( actor_attribute.SPW, meta.spw_factor * meta.base_attr_value.spw );
            //atk
            SetBaseValue( actor_attribute.ATK, meta.atk_factor * meta.base_attr_value.atk );
        }

        /// <summary>
        /// 检查数值是否正确，不正确返回true
        /// </summary>
        private bool OverLen( int intType )
        {
            if ( _numricArr is null || intType >= _numricArr.Length )
            {
                Log.Warning( $"attr int type {intType.ToString()} is over len:{_numricArr.Length}" );
                return true;
            }
            return false;
        }

        /// <summary>
        /// 属性改变
        /// </summary>
        private void OnAttrChange(int type,object param)
        {
            var eventInfo = param as EffectSpec_OnHitted_Trigger_ModifyAttrParam;
            if (eventInfo is null)
                return;
            
            Log.Info($"dirty value:{eventInfo._dirtyCorrectionValue},new:{GetCorrectionValue(eventInfo._changedAttr,0f)}");
        }

        //----------------------------override----------------------------
        public override string ToString()
        {
            return $"<color=green>curr hp:{GetCurrHPCorrection()},curr mp:{GetCurrMPCorrection()}</color>";
        }

        public override void Reset()
        {
            base.Reset();
            //修改所有数值为为修正的状态
            var meta = GameEntry.LuBan.Table<Cfg.Role.RoleMeta>().Get( _actorInstance.Actor.RoleMetaID );
            if ( meta is null )
            {
                Log.Warning( $"<color=yellow>meta is null,meta id = {_actorInstance.Actor.RoleMetaID}</color>" );
                return;
            }
            SetBaseAttr( meta );
        }

        public override void Init(Module_ProxyActor.ActorInstance instance)
        {
            base.Init(instance);
            instance.GetAddon<Addon_Event>().Register((int)AddonEventTypeEnum.ON_ATTR_CHANGE,(int)AddonType ,OnAttrChange);
            
        }

        public override void Dispose()
        {
            base.Dispose();
            if ( _numricArr != null )
            {
                var len = _numricArr.Length;
                for ( var i = 0; i < len; i++ )
                {
                    //_numric_arr[i].Clear();
                    ReferencePool.Release( _numricArr[i] );
                    _numricArr[i] = null;
                }
                _numricArr = null;
            }
            // ReferencePool.Release(_currHP);
            // ReferencePool.Release(_currMP);
            // _currHP = null;
            // _currMP = null;
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.NUMRIC_BASEATTR;

        public override void OnAdd()
        {
            if ( _numricArr is null )
                _numricArr = new Numric_ActorBaseAttr[( int ) Cfg.Enum.actor_attribute.Max];

            var len = _numricArr.Length;
            for ( var i = 0; i < len; i++ )
            {
                if ( _numricArr[i] is null )
                {
                    _numricArr[i] = ReferencePool.Acquire<Numric_ActorBaseAttr>();
                    _numricArr[i].EnsureInit();
                }
                else
                {
                    Log.Warning( "Numric arr not not null on add!" );
                }
            }

            // _currHP = ReferencePool.Acquire<Numric.Numric>();
            // _currHP.EnsureInit();
            // _currMP = ReferencePool.Acquire<Numric.Numric>();
            // _currMP.EnsureInit();
        }

        //----------------------------fields----------------------------
        /// <summary>
        /// 所有的数值集合
        /// </summary>
        private Numric.Numric_ActorBaseAttr[] _numricArr = null;

        /// <summary>
        /// 血量
        /// </summary>
        // private Numric.Numric _currHP = null;
        
        /// <summary>
        /// 魔法
        /// </summary>
        // private Numric.Numric _currMP = null;
    }
}
