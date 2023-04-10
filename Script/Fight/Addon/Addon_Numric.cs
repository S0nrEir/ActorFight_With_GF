using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Numric;
using Cfg.Enum;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// �����������
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
        /// ��ȡĳ�����ԵĻ���ֵ
        /// </summary>
        public (bool get_succ, float value) GetBaseValue( Actor_Attr type_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return (false, 0f);

            return (true, _numric_arr[int_type].BaseValue);
        }

        /// <summary>
        /// ����ĳ�����ԵĻ���ֵ�������޸ĺ��ֵ�ͳɹ����
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
        /// ��ȡĳ�����Ե���������ֵ
        /// </summary>
        public (bool get_succ, float value) GetCorrectionFinalValue( Actor_Attr type_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return (false, 0f);

            return (true, _numric_arr[int_type].CorrectionValue);
        }

        /// <summary>
        /// ����һ��װ�����͵���ֵ������
        /// </summary>
        public bool SetEquipModifier( Numric_Modify_Type_Enum type_, Numric_Modifier modifier_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return false;

            return _numric_arr[int_type].AddEquipModifier( modifier_ );
        }

        /// <summary>
        /// ����һ��buff���͵���ֵ������
        /// </summary>
        public bool SetBuffModifier( Actor_Attr type_, Numric_Modifier modifier_ )
        {
            var int_type = ( int ) type_;
            if ( OverLen( int_type ) )
                return false;

            return _numric_arr[int_type].AddBuffModifier( modifier_ );
        }

        /// <summary>
        /// ����һ��ְҵ��������ֵ������
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
        /// ȡ������������������ֵΪδ������״̬
        /// </summary>
        private void ResetNumricArr()
        {
            if ( _numric_arr is null )
                _numric_arr = new Numric_ActorBaseAttr[( int ) Cfg.Enum.Actor_Attr.Max - 1];

            var meta = GameEntry.DataTable.GetTable<Cfg.role.TB_RoleMeta>().Get( Actor.RoleMetaID );
            if ( meta is null )
            {
                Log.Warning( $"<color=yellow>meta is null,meta id = {Actor.RoleMetaID}</color>" );
                return;
            }
            SetBaseAttr( meta );
        }

        /// <summary>
        /// ���û�������
        /// </summary>
        private void SetBaseAttr(Cfg.role.RoleMeta meta)
        {
            //#todo����������ʱ��һ�������ã�����취��loop
            var proxy_module = GameEntry.Module.GetModule<Module_Proxy_Actor>();
            //max hp
            SetBaseValue( Actor_Attr.Max_HP, meta.HP );
            //curr hp
            SetBaseValue(Actor_Attr.Curr_HP,meta.HP );
            //str
            SetBaseValue(Actor_Attr.STR,meta.STR );
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
        /// �����ֵ�Ƿ���ȷ������ȷ����true
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


        #region override

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
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.NUMRIC_BaseAttr;

        public override void OnAdd()
        {

        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
        }
        #endregion

        #region fields

        /// <summary>
        /// ���е���ֵ����
        /// </summary>
        private Numric.Numric_ActorBaseAttr[] _numric_arr = null;

        #endregion
    }
}
