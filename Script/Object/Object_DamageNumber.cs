using System.Collections;
using System.Collections.Generic;
using Aquila.Item;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;

namespace  Aquila.ObjectPool
{
    public class Object_DamageNumber : Aquila_Object_Base
    {
        /// <summary>
        /// 按照既定方向移动
        /// </summary>
        public void Move(float delta_time)
        {
            _passed_time += delta_time;
            //在这里要算上速度
            _rect.position =  _rect.position + new Vector3(_damage_number._random_dir.x,_damage_number._random_dir.y,0f) * Speed;
        }
 
        /// <summary>
        /// 检查是否到了生存时间
        /// </summary>
        public bool TimesUp()
        {
            return _passed_time >= Duration;
        }

        /// <summary>
        /// 设置数值和显示颜色
        /// </summary>
        public void SetNumber(string number, Color color )
        {
            _damage_number._text.text = number;
            _damage_number._text.color = color;
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        public void SetPos(Vector3 rect_pos)
        {
            _rect.position = rect_pos;
        }

        public override void Setup(GameObject go)
        {
            base.Setup(go);
            _damage_number = Tools.GetComponent<Item_DamageNumber>(go.transform);
            _rect = Tools.GetComponent<RectTransform>(go.transform);
            var random = Random.insideUnitCircle;
            //数字只能往上飘，处理一下
            if (random.y < 0)
                random.y *= -1;

            _damage_number._random_dir = random.normalized;
        }

        protected override void Release(bool isShutdown)
        {
            _rect = null;
            _damage_number = null;
            base.Release(isShutdown);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }

        protected override void OnUnspawn()
        {
            _damage_number._text.text  = string.Empty;
            _damage_number._random_dir = Vector2.zero;
            _passed_time               = 0f;
            base.OnUnspawn();
        }

        public static Object_DamageNumber Gen(GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_DamageNumber>();
            obj.Initialize(go);
            return obj;
        }

        /// <summary>
        /// 持有的伤害数字对象
        /// </summary>
        private Item_DamageNumber _damage_number = null;

        /// <summary>
        /// 出生到目前为止的经过时间
        /// </summary>
        private float _passed_time = 0f;

        /// <summary>
        /// 统一3秒后销毁
        /// </summary>
        private const float Duration = 3f;

        /// <summary>
        /// 飘字速度
        /// </summary>
        private const float Speed = 0.05f;
        
        /// <summary>
        /// dmgNum实例的矩形变换组件
        /// </summary>
        private RectTransform _rect = null;
    }
   
}