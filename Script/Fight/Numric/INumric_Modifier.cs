using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Numric
{
    /// <summary>
    /// ��ֵ�޸������ͽӿ�
    /// </summary>
    public interface INumric_Modifier
    {

    }

    /// <summary>
    /// Numric��ֵ�޸ķ�ʽö��
    /// </summary>
    public enum Numric_Modify_Type_Enum
    {
        /// <summary>
        /// �ޣ���Ч
        /// </summary>
        None = 0,

        /// <summary>
        /// ��
        /// </summary>
        Add = 1,

        /// <summary>
        /// �ٷֱȣ���Ϊԭ�ȵİٷ�֮����
        /// </summary>
        Percent,
    }
}