using System.Collections;
using System.Collections.Generic;
using Aquila.Config;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Aquila.ObjectPool
{
    public class Object_HPBar : Aquila_Object_Base
    {   
        /// <summary>
        /// 设置数值
        /// </summary>
        public void SetValue(int curr, int max)
        {
            _hp_bar.SetValue(curr,max);
        }

        public void Setup(GameObject go)
        {
            _hp_bar = Tools.GetComponent<HPBarItem>(go.transform);
            if (_hp_bar == null)
                Log.Warning("<color=yellow>_hp_bar == null</color>");
        }

        /// <summary>
        /// 获取值
        /// </summary>
        public float Value()
        {
            return _hp_bar.Value();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }

        protected override void OnUnspawn()
        {
            base.OnUnspawn();
        }

        protected override void Release(bool isShutdown)
        {
            _hp_bar = null;
            base.Release(isShutdown);
        }

        /// <summary>
        /// 生成一个Object_HPBar对象
        /// </summary>
        public static Object_HPBar Gen(GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_HPBar>();
            obj.Initialize(GameConfig.ObjectPool.OBJECT_POOL_HP_BAR_NAME,go);
            obj.Setup(go);
            return obj;
        }

        private HPBarItem _hp_bar = null;
    }

    public class HPBarItem : MonoBehaviour
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
            _max_num_text.text = max.ToString();
            _curr_num_text.text = curr.ToString();
        }

        public void Init()
        {
            
        }
        
        [SerializeField] private Slider _hp_slider = null;
        [SerializeField] private Text _max_num_text;
        [SerializeField] private Text _curr_num_text;
    }
}
