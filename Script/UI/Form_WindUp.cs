using Aquial.UI;
using Aquila.Event;
using Aquila.Toolkit;
using GameFramework;
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
        /// <summary>
        /// 显示windup
        /// </summary>
        public static void ActiveWindUpForm(bool active,float totalTime)
        {
            if (active)
            {
                //有的话重新刷一下windup的时长
                if (GameEntry.BaseUI.HasUIForm((int)FormIdEnum.WindUpForm))
                {
                    var uiForm = GameEntry.BaseUI.GetUIForm((int)FormIdEnum.WindUpForm);
                    if (uiForm is null || uiForm.Logic is null)
                    {
                        Log.Warning($"<color=yellow>Form_Windup.ActiveWindUpForm()--->uiForm is null || uiForm.Logic is null</color>");
                        return;
                    }
                    
                    (uiForm.Logic as Form_WindUp).ReActive(totalTime);
                }
                //没有的话直接打开
                else
                {
                    var formParam = ReferencePool.Acquire<Form_WindUp.Form_WindUpParam>();
                    formParam._totalTime = totalTime;
                    GameEntry.UI.OpenForm(FormIdEnum.WindUpForm,formParam);
                }
            }
            else
            {
                GameEntry.UI.CloseForm(FormIdEnum.WindUpForm);
            }
        }
        
        //----------------pub

        public void ReActive(float totalTime)
        {
            _windUpItem.Stop();
            _windUpItem.GetReady(totalTime);
        }

        //----------------priv
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
        
        //----------------override

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            _windUpItem.Update(elapseSeconds);
        }

        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (_rootGO == null)
            {
                
                return;
            }

            var param = Tools.UI.GetFormParam<Form_WindUpParam>(userData);
            if (param is null)
                return;
            
            if(_windUpItem == null)
                InitWindUpItem(_rootGO);
            
            _windUpItem.GetReady(param._totalTime);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            _windUpItem.Clear();
            _windUpItem = null;
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

        /// <summary>
        /// 界面参数
        /// </summary>
        public class Form_WindUpParam : IReference
        {
            public float _totalTime = 0f;

            public void Clear() => _totalTime = 0f;
        }
    }
    
   
}
