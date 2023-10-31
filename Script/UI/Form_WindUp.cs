using System.Collections;
using System.Collections.Generic;
using Aquila.Event;
using Aquila.Toolkit;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Aquila.UI
{
    /// <summary>
    /// 蓄力 ui
    /// </summary>
    public class Form_WindUp : UIFormLogic
    {
        public static void ShowWindUpForm()
        {
            
            
        }
        
        //----------------priv
        
        //---------------- event ----------------
        /// <summary>
        /// 开始读条
        /// </summary>
        private void OnWindUp(object sender, GameEventArgs arg)
        {
            if (!(arg is EventArg_WindUp))
                return;

            var param = arg as EventArg_WindUp;
            if(param._isStart)
                _windUpItem.GetReady(param._totalTime);
            else
                _windUpItem.Stop();
        }
        
        /// <summary>
        /// 初始化读条的item
        /// </summary>
        private void InitWindUpItem(GameObject rootGo)
        {
            if (rootGo == null)
            {
                Log.Warning("<color=yellow>Form_Ability.InitWindUpItem--->rootGo is null</color>");
                return;
            }

            _windUpItem = new WindUpItem(rootGo);
            _windUpItem.Active();
        }
        
        /// <summary>
        /// 刷新吟唱item
        /// </summary>
        private void RefreshWindUpItem(float elpased)
        {
            _windUpItem.Update(elpased);
        }
        
        //----------------override

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            RefreshWindUpItem(elapseSeconds);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (_rootGO == null)
            {
                
                return;
            }

            InitWindUpItem(_rootGO);
            GameEntry.Event.Subscribe( EventArg_WindUp.EventID      , OnWindUp);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            _windUpItem.Clear();
            _windUpItem = null;
            GameEntry.Event.Unsubscribe( EventArg_WindUp.EventID, OnWindUp );
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        /// <summary>
        /// windup对象根节点
        /// </summary>
        [SerializeField] private GameObject _rootGO = null;

        /// <summary>
        /// windup item
        /// </summary>
        private WindUpItem _windUpItem = null;

        /// <summary>
        /// 读条进度
        /// </summary>
        private class WindUpItem
        {
            /// <summary>
            /// 激活
            /// </summary>
            public void Active()
            {
                Tools.SetActive(_root,true);
            }

            /// <summary>
            /// 刷帧跑读条
            /// </summary>
            public void Update(float deltaTime)
            {
                if(!ReadyFlag)
                    return;

                _passedTime += deltaTime;
                _slider.value = _passedTime / _totalTime;
                var remainTime = _totalTime - _passedTime;
                
                //finish
                if (_slider.value >= 1f)
                    Stop();
                else
                    _remainText.text = remainTime.ToString("n1");
            }

            /// <summary>
            /// 停止读条
            /// </summary>
            public void Stop()
            {
                Tools.SetActive(_slider.gameObject,false);
                Tools.SetActive(_remainText.gameObject,false);
                Tools.SetActive(_root,false);
                Reset();
            }

            /// <summary>
            /// 刷帧跑进度
            /// </summary>
            public void GetReady(float totalTime)
            {
                _totalTime    = totalTime;
                _passedTime   = 0f;
                _slider.value = 0f;
                _remainText.text = "0";
                Tools.SetActive(_root,true);
                Tools.SetActive(_slider.gameObject,true);
                Tools.SetActive(_remainText.gameObject,true);
                
                ReadyFlag = true;
            }
            
            public WindUpItem(GameObject root)
            {
                _root       = root;
                _slider     = Tools.GetComponent<Slider>(_root, "Slider");
                _remainText = Tools.GetComponent<Text>(_root  , "Remain");
                Reset();
            }
            
            /// <summary>
            /// 清掉数据
            /// </summary>
            public void Clear()
            {
                _root       = null;
                _slider     = null;
                _remainText = null;
            }

            /// <summary>
            /// 设置数据到初始状态
            /// </summary>
            private void Reset()
            {
                ReadyFlag   = false;
                _totalTime  = 0f;
                _passedTime = 0f;
                _slider.value = 0f;
            }

            /// <summary>
            /// 准备标记
            /// </summary>
            public bool ReadyFlag { get; private set; } = false;

            /// <summary>
            /// 读条
            /// </summary>
            private Slider _slider = null;
            
            /// <summary>
            /// 总时长
            /// </summary>
            private float _totalTime = 0f;
            
            /// <summary>
            /// 剩余时间文本
            /// </summary>
            private Text _remainText = null;

            /// <summary>
            /// 经过时间
            /// </summary>
            private float _passedTime = 0f;
            
            /// <summary>
            /// 跟对象
            /// </summary>
            private GameObject _root = null;
        }
    }
   
}
