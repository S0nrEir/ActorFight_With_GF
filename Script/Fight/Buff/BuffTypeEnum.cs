using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRG.Fight.Buff
{

    /// <summary>
    /// buff作用类型
    /// </summary>
    public enum BuffApplyTypeEnum
    {
        Invalid = -1,

        /// <summary>
        /// actor
        /// </summary>
        Actor = 0,

        /// <summary>
        /// 用户
        /// </summary>
        User = 1,
    }
}