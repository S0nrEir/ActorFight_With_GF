using Aquila.Fight.Actor;
using Aquila.Numric;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{

    public class Addon_Numric : AddonBase
    {
        #region pub



        #endregion

        #region priv

        /// <summary>
        /// ȡ������������������ֵΪδ������״̬
        /// </summary>
        private void ResetNumricArr()
        {
            if ( _numric_arr is null )
                _numric_arr = new Numric_Actor[(int)Cfg.Enum.Numric_Type.Max - 1];

            //#TODO-�������Ե��ڻ���ֵ��û���κμӳ�
        }

        /// <summary>
        /// �����ֵ�Ƿ���ȷ����ȷ����true
        /// </summary>
        private bool OverLen( int int_type )
        {
            if ( _numric_arr is null || int_type >= _numric_arr.Length )
            {
                Log.Warning( "Addon_Data.Base.cs--->int_type >= _numric_arr.Length" );
                return false;
            }
            return true;
        }

        #endregion


        #region override

        public override void Reset()
        {
            ResetNumricArr();
            base.Reset();
        }

        public override void Init( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.NUMRIC;

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
        private Numric.Numric_Actor[] _numric_arr = null;

        #endregion
    }
}
