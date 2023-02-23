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
        /// 取消所有修正，重置数值为未修正的状态
        /// </summary>
        private void ResetNumricArr()
        {
            if ( _numric_arr is null )
                _numric_arr = new Numric_Actor[(int)Cfg.Enum.Numric_Type.Max - 1];

            //#TODO-所有属性等于基础值，没有任何加成
        }

        /// <summary>
        /// 检查数值是否正确，正确返回true
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
        /// 所有的数值集合
        /// </summary>
        private Numric.Numric_Actor[] _numric_arr = null;

        #endregion
    }
}
