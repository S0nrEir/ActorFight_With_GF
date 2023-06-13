using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aquila.Item
{
    /// <summary>
    /// 伤害数字
    /// </summary>
    public class Item_DamageNumber : MonoBehaviour
    {
        /// <summary>
        /// 飘字的随即方向
        /// </summary>
        public Vector2 _randomDir = Vector2.zero;
        
        /// <summary>
        /// 文本
        /// </summary>
        public Text _text = null;
    }

}