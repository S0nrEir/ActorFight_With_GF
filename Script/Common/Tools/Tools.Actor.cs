using Cfg.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Toolkit
{
    public static partial class Tools
    {
        /// <summary>
        /// Actor������
        /// </summary>
        public static class Actor
        {
            //#todoӦ��ΪActor_Attr��������һ��invalidö�٣�������ƥ��ʧ��ʱ����invalid��Ŀǰ���ص���max
            /// <summary>
            /// Actor_Base_Attr��Actor_Attrö�ٵ�ӳ�䣬û�з���Max�����Ҳ���ƥ��HP��MP
            /// </summary>
            public static Actor_Attr BaseAttr2NormalAttrEnum(Actor_Base_Attr type)
            {
                return Actor_Attr.Max;
            }
        }
    }
}