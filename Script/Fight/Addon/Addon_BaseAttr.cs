using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Numric;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

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
    /// -----------------------+----------------------+----------------------+----------------
    /// zzz_Numric_Modifier_1 |zzz_Numric_Modifier_1  |zzz_Numric_Modifier_1  |zzz_Numric_Modifier_1
    /// zzz_Numric_Modifier_2 |zzz_Numric_Modifier_2  |zzz_Numric_Modifier_2  |zzz_Numric_Modifier_2
    /// zzz_Numric_Modifier_3 |zzz_Numric_Modifier_3  |zzz_Numric_Modifier_3  |zzz_Numric_Modifier_3
    /// ----------------------+----------------------+----------------------+----------------
    /// </summary>
    public class Addon_BaseAttrNumric : AddonBase
    {
        #region pub

        /// <summary>
        /// 获取某项属性的基础值
        /// </summary>
        public (bool get_succ, float value) GetBaseValue( Actor_Attr type_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return (false, 0f);

            return (true, _numric_arr[int_type].BaseValue);
        }

        /// <summary>
        /// 设置某项属性的基础值，返回修改后的值和成功标记
        /// </summary>
        public (bool set_succ, float value_after_set) SetBaseValue( Actor_Attr type_, float value_to_set )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return (false, _numric_arr[int_type].BaseValue);

            _numric_arr[int_type].SetBaseVal( value_to_set );
            return (true, _numric_arr[int_type].BaseValue);
        }

        /// <summary>
        /// 获取某项属性的最终修正值
        /// </summary>
        public (bool get_succ, float value) GetCorrectionFinalValue( Actor_Attr type_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return (false, 0f);

            return (true, _numric_arr[int_type].CorrectionValue);
        }

        /// <summary>
        /// 设置一个装备类型的数值修饰器
        /// </summary>
        public bool SetEquipModifier( Numric_Modify_Type_Enum type_, Numric_Modifier modifier_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return false;

            return _numric_arr[int_type].AddEquipModifier( modifier_ );
        }

        /// <summary>
        /// 设置一个buff类型的数值修饰器
        /// </summary>
        public bool SetBuffModifier( Actor_Attr type_, Numric_Modifier modifier_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return false;

            return _numric_arr[int_type].AddBuffModifier( modifier_ );
        }

        /// <summary>
        /// 设置一个职业修正的数值修饰器
        /// </summary>
        public bool SetClassModifier( Actor_Attr type_, Numric_Modifier modifier_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return false;

            return _numric_arr[int_type].AddClassModifier( modifier_ );
        }

        #endregion

        #region priv

        /// <summary>
        /// 取消所有修正，重置数值为未修正的状态
        /// </summary>
        private void ResetNumricArr()
        {
            var meta = GameEntry.DataTable.Table<Cfg.role.TB_RoleMeta>().Get( Actor.RoleMetaID );
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
        private void SetBaseAttr( Cfg.role.RoleMeta meta )
        {
            //#todo设置属性暂时是一个个设置，想个办法走loop
            var proxy_module = GameEntry.Module.GetModule<Module_Proxy_Actor>();
            //max hp
            SetBaseValue( Actor_Attr.Max_HP, meta.HP );
            //curr hp
            SetBaseValue( Actor_Attr.Curr_HP, meta.HP );
            //str
            SetBaseValue( Actor_Attr.STR, meta.STR );
            //def
            SetBaseValue( Actor_Attr.DEF, meta.DEF );
            //agi
            SetBaseValue( Actor_Attr.AGI, meta.AGI );
            //mvt
            SetBaseValue( Actor_Attr.MVT, meta.MVT );
            //spw
            SetBaseValue( Actor_Attr.SPW, meta.SPW );
            //mp
            SetBaseValue( Actor_Attr.Max_MP, meta.MP );
            //curr mp
            SetBaseValue( Actor_Attr.Curr_MP, meta.MP );
        }

        /// <summary>
        /// 检查数值是否正确，不正确返回true
        /// </summary>
        private bool OverLen( int int_type )
        {
            if ( _numric_arr is null || int_type >= _numric_arr.Length )
            {
                Log.Warning( $"attr int type {int_type.ToString()} is over len:{_numric_arr.Length}" );
                return true;
            }
            return false;
        }

        #endregion


        //----------------------------override----------------------------

        public override void Reset()
        {
            base.Reset();
            ResetNumricArr();
        }

        public override void Init( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        public override void Dispose()
        {
            base.Dispose();
            if ( _numric_arr != null )
            {
                var len = _numric_arr.Length;
                for ( var i = 0; i < len; i++ )
                {
                    //_numric_arr[i].Clear();
                    ReferencePool.Release( _numric_arr[i] );
                    _numric_arr[i] = null;
                }
                _numric_arr = null;
            }
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.NUMRIC_BASEATTR;

        public override void OnAdd()
        {
            if ( _numric_arr is null )
                _numric_arr = new Numric_ActorBaseAttr[( int ) Cfg.Enum.Actor_Attr.Max ];

            var len = _numric_arr.Length;
            for ( var i = 0; i < len; i++ )
            {
                if ( _numric_arr[i] is null )
                    _numric_arr[i] = ReferencePool.Acquire<Numric_ActorBaseAttr>();
                else
                    Log.Warning( "Numric arr not not null on add!" );
            }
        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
        }

        //----------------------------fields----------------------------
        /// <summary>
        /// 所有的数值集合
        /// </summary>
        private Numric.Numric_ActorBaseAttr[] _numric_arr = null;
    }
}
