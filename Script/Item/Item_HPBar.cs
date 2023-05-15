using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Aquila.Item
{
    public class Item_HPBar : MonoBehaviour
    {
        public float Value()
        {
            return _hp_slider.value;
        }

        /// <summary>
        /// 设置slider的信息
        /// </summary>
        public void SetValue(int curr,int max)
        {
            if(_hp_slider == null)
            {
                Log.Info("<color=yellow>_hp_slider == null</color>");
                return;
            }
            
            _hp_slider.value = (float)curr / max;
            // _max_num_text.text = max.ToString();
            // _curr_num_text.text = curr.ToString();
            _text_num.text = $"{curr}/{max}";
        }

        public void Init()
        {
            
        }
        
        [SerializeField] private Slider _hp_slider = null;
        // [SerializeField] private Text _max_num_text;
        // [SerializeField] private Text _curr_num_text;

        /// <summary>
        /// hp显示文字
        /// </summary>
        [SerializeField] private Text _text_num = null;
    }
}