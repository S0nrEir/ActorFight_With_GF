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
        public (bool get_succ, float value) GetBaseValue( Actor_Attr type )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return (false, 0f);

            return (true, _numricArr[intType].BaseValue);
        }

        /// <summary>
        /// 设置某项属性的基础值，返回修改后的值和成功标记
        /// </summary>
        public (bool setSucc, float valueAfterSet) SetBaseValue( Actor_Base_Attr type, float valueToSet )
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
            _hp.SetBaseVal(valueToSet);
            return (true, _hp.CorrectionValue);
        }

        /// <summary>
        /// 设置MP值
        /// </summary>
        public (bool setSucc, float valueAfterSet) SetCurrMP(float valueToSet)
        {
            _mp.SetBaseVal(valueToSet);
            return (true, _mp.CorrectionValue);
        }

        /// <summary>
        /// 获取当前hp的修正值
        /// </summary>
        public float GetCurrHPCorrection()
        {
            return _hp.CorrectionValue;
        }

        /// <summary>
        /// 获取当前mp的修正值
        /// </summary>
        public float GetCurrMPCorrection()
        {
            return _mp.CorrectionValue;
        }

        /// <summary>
        /// 获取某项属性的最终修正值
        /// </summary>
        public (bool getSucc, float value) GetCorrectionFinalValue( Actor_Attr type ,float default_value = 0f)
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return (false, default_value);

            return (true, _numricArr[intType].CorrectionValue);
        }

        /// <summary>
        /// 设置一个装备类型的数值修饰器
        /// </summary>
        public bool SetEquipModifier( Actor_Attr type, Numric_Modifier modifier )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return false;

            return _numricArr[intType].AddEquipModifier( modifier );
        }

        /// <summary>
        /// 设置一个buff类型的数值修饰器
        /// </summary>
        public bool SetBuffModifier( Actor_Attr type, Numric_Modifier modifier )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return false;

            return _numricArr[intType].AddBuffModifier( modifier );
        }

        /// <summary>
        /// 设置一个职业修正的数值修饰器
        /// </summary>
        public bool SetClassModifier( Actor_Attr type, Numric_Modifier modifier )
        {
            var intType = ( int ) type;
            if ( OverLen( intType ) )
                return false;

            return _numricArr[intType].AddClassModifier( modifier );
        }
        

        //----------------------priv----------------------
        /// <summary>
        /// 取消所有修正，重置数值为未修正的状态
        /// </summary>
        private void ResetNumricArr()
        {
            var meta = GameEntry.DataTable.Table<Cfg.Role.RoleMeta>().Get( Actor.RoleMetaID );
            if ( meta is null )
            {
                Log.Warning( $"<color=yellow>meta is null,meta id = {Actor.RoleMetaID}</color>" );
                return;
            }
            SetBaseAttr( meta );
        }

        /// <summary>
        /// 设置基础属性
        /// </summary>
        private void SetBaseAttr( Table_RoleMeta meta )
        {
            //现在是写死的，很蛋疼
            var proxy_module = GameEntry.Module.GetModule<Module_ProxyActor>();
            //max hp
            var res = SetBaseValue( Actor_Base_Attr.HP, meta.HP );
            SetCurrHP(res.valueAfterSet);
            //mp
            res = SetBaseValue( Actor_Base_Attr.MP, meta.MP );
            SetCurrMP(res.valueAfterSet);
            //str
            SetBaseValue( Actor_Base_Attr.STR, meta.STR );
            //def
            SetBaseValue( Actor_Base_Attr.DEF, meta.DEF );
            //agi
            SetBaseValue( Actor_Base_Attr.AGI, meta.AGI );
            //mvt
            SetBaseValue( Actor_Base_Attr.MVT, meta.MVT );
            //spw
            SetBaseValue( Actor_Base_Attr.SPW, meta.SPW );
            
            //curr mp
            // SetBaseValue( Actor_Attr.Curr_MP, meta.MP );
            // //curr hp
            // SetBaseValue( Actor_Attr.Curr_HP, meta.HP );
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
        
        //----------------------------override----------------------------
        public override string ToString()
        {
            return $"<color=green>curr hp:{GetCurrHPCorrection()},curr mp:{GetCurrMPCorrection()}</color>";
        }

        public override void Reset()
        {
            base.Reset();
            ResetNumricArr();
        }

        public override void Init( Actor_Base actor, GameObject target_go, Transform target_transform )
        {
            base.Init( actor, target_go, target_transform );
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
            ReferencePool.Release(_hp);
            ReferencePool.Release(_mp);
            _hp = null;
            _mp = null;
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.NUMRIC_BASEATTR;

        public override void OnAdd()
        {
            if ( _numricArr is null )
                _numricArr = new Numric_ActorBaseAttr[( int ) Cfg.Enum.Actor_Base_Attr.Max ];

            var len = _numricArr.Length;
            for ( var i = 0; i < len; i++ )
            {
                if ( _numricArr[i] is null )
                    _numricArr[i] = ReferencePool.Acquire<Numric_ActorBaseAttr>();
                else
                    Log.Warning( "Numric arr not not null on add!" );
            }

            _hp = ReferencePool.Acquire<Numric.Numric>();
            _mp = ReferencePool.Acquire<Numric.Numric>();
        }

        //----------------------------fields----------------------------
        /// <summary>
        /// 所有的数值集合
        /// </summary>
        private Numric.Numric_ActorBaseAttr[] _numricArr = null;

        /// <summary>
        /// 血量
        /// </summary>
        private Numric.Numric _hp = null;
        
        /// <summary>
        /// 魔法
        /// </summary>
        private Numric.Numric _mp = null;
    }
}
