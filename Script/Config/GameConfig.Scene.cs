using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// �������
        /// </summary>
        public class Scene
        {
            /// <summary>
            /// �����Ĭ������ռ�����λ��//#todo���excel���������ַ�����Ϊ'-'�����
            /// </summary>
            /// </summary>
            public static Vector3 MAIN_CAMERA_DEFAULT_POSITION { get; } = new Vector3( -2.75f, 5.95f, -3.91f );
        }
    }
}