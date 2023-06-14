using UnityEngine;
using UnityEngine.UI;

namespace Aquila.Item
{
    public class Item_HPBar : MonoBehaviour
    {
        public Slider _hp_slider = null;

        /// <summary>
        /// hp显示文字
        /// </summary>
        public Text _text_num = null;

        /// <summary>
        ///  持有的rectTransform
        /// </summary>
        public RectTransform _rect = null;
    }
}