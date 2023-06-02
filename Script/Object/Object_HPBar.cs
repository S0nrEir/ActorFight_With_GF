using Aquila.Item;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.ObjectPool
{
    public class Object_HPBar : Aquila_Object_Base
    {
        
        /// <summary>
        /// 获取持有的UI对象的RectTransform
        /// </summary>
        // public RectTransform Rect()
        // {
        //     return _hp_bar.RectTransform;
        // }

        /// <summary>
        /// 设置对象在屏幕空间中的位置
        /// </summary>
        public void SetScreenPos(Vector3 pos)
        {
            _hp_bar.RectTransform.position = pos; 
        }

        /// <summary>
        /// 设置数值
        /// </summary>
        public void SetValue(int curr, int max)
        {
            _hp_bar.SetValue(curr,max);
        }

        public override void Setup(GameObject go)
        {
            _hp_bar = Tools.GetComponent<Item_HPBar>(go.transform);
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
            obj.Initialize(go);
            return obj;
        }

        private Item_HPBar _hp_bar = null;
    }
}
