using Aquila.Event;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Aquila.UI
{
    /// <summary>
    /// 技能form
    /// </summary>
    public class Form_Ability : UIFormLogic
    {
        /// <summary>
        /// 技能命中
        /// </summary>
        private void OnAbilityHit( object sender, GameEventArgs arg )
        {
            
        }

        //---------------- event ----------------
        /// <summary>
        /// 开始读条
        /// </summary>
        private void OnWindUp(object sender, GameEventArgs arg)
        {
            if (!(arg is EventArg_WindUp))
                return;

            var param = arg as EventArg_WindUp;
            Form_WindUp.ActiveWindUpForm(param._isStart,param._totalTime);
        }
        
        /// <summary>
        /// 使用技能
        /// </summary
        private void OnUseAbility( object sender, GameEventArgs arg )
        {
            if (!(arg is EventArg_OnUseAblity))
                return;
            
            var result = (arg as EventArg_OnUseAblity)._resultParam;
            if ( !result._succ )
                Log.Info( $"<color=white>{Tools.Fight.UsingAbilityFaildDescription_l10n( result._stateDescription )}</color>" );
        }

        /// <summary>
        /// 测试button按下
        /// </summary>
        private void OnHostileTestIconItemClicked(int abilityID)
        {
            var selfID = _enemyActorIdArr[0];
            _actorProxy.Ability2SingleTarget( selfID, selfID, abilityID ,GameEntry.GlobalVar.InvalidPosition);
        }

        /// <summary>
        /// click事件
        /// </summary>
        private void OnIconItemClicked( int abilityID )
        {
            //todo:这里其实要检查技能类型和目标的，还没写完，就随便先写一个
            //对单个目标的技能测试
            //_abilityIdArr[2]:1002
            //_abilityIdArr[3]:1003
            //_abilityIdArr[4]:1004
            //_enemyActorIdArr[0]:1001
            _actorProxy.Ability2SingleTarget( _actorID, _enemyActorIdArr[0], abilityID ,GameEntry.GlobalVar.InvalidPosition);
        }
        
        /// <summary>
        /// 初始化缓存item
        /// </summary>
        private void InitAbilityIconItem()
        {
            _iconItemDic = new Dictionary<int, AbilityIconItem>();
            Tools.SetActive( _tempGameObejct, false );
            var len = _abilityIdArr.Length;
            GameObject generated = null;
            AbilityIconItem tempItem = null;
            for ( int i = 0; i < len; i++ )
            {
                var id = _abilityIdArr[i];
                generated = GameObject.Instantiate( _tempGameObejct, _grid.transform, true);
                generated.transform.localScale = Vector3.one;
                generated.transform.eulerAngles = Vector3.zero;
                Tools.SetActive( generated, true );

                tempItem = ReferencePool.Acquire<AbilityIconItem>();
                tempItem.Setup( generated, id, OnIconItemClicked );
                _iconItemDic.Add( id, tempItem );
            }
            
            //hostile；
            Tools.SetActive(_tempGameObejct,false);
            generated = GameObject.Instantiate(_hostileTestButton, transform, true);
            generated.transform.localScale = Vector3.one;
            generated.transform.eulerAngles = Vector3.zero;
            Tools.SetActive( generated, true );
            tempItem = ReferencePool.Acquire<AbilityIconItem>();
            //测试嗜血，敌人给自己上，玩家攻击
            tempItem.Setup(generated,1006, OnHostileTestIconItemClicked);
            _hostileAbilityIconItem = tempItem;
            tempItem = null;
        }


        /// <summary>
        /// 清掉item缓存
        /// </summary>
        private void ClearItemCacheDic()
        {
            if ( _iconItemDic is null || _iconItemDic.Count == 0 )
                return;

            AbilityIconItem temp = null;
            var iter = _iconItemDic.GetEnumerator();
            while ( iter.MoveNext() )
            {
                temp = iter.Current.Value;
                ReferencePool.Release( temp );
            }

            iter.Dispose();
            _iconItemDic.Clear();
            _iconItemDic = null;
            
            ReferencePool.Release(_hostileAbilityIconItem);
            _hostileAbilityIconItem = null;
        }

        /// <summary>
        /// 刷新技能图标CD
        /// </summary>
        private void RefreshAbilityIconCD()
        {
            (float remain, float duration) cd = (0f,0f);
            var percent = 0f;
            foreach ( var id in _abilityIdArr )
            {
                cd = _actorProxy.GetCoolDown( _actorID, id );
                percent = cd.remain / cd.duration;
                _iconItemDic[id].CD( percent, percent.ToString("n1") );
            }
            
            cd = _actorProxy.GetCoolDown( _enemyActorIdArr[0], 1006 );
            percent = cd.remain / cd.duration;
            _hostileAbilityIconItem.CD(percent, percent.ToString("n1"));
        }
        
        protected override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            RefreshAbilityIconCD();
        }
        
        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
        }

        protected override void OnOpen( object userData )
        {
            base.OnOpen( userData );
            var param = Tools.UI.GetFormParam<Form_AbilityParam>( userData );
            if ( param is null )
                return;

            _actorID         = param._mainActorID;
            _enemyActorIdArr = param._enemyActorID;
            _abilityIdArr    = param._abilityID;
            _actorProxy = GameEntry.Module.GetModule<Module_ProxyActor>();
            InitAbilityIconItem();
            _testBtn = Tools.GetComponent<Button>( gameObject, "ExitButton" );
            _testBtn.onClick.AddListener(() =>
            {
                
            });
            ReferencePool.Release( param );

            GameEntry.Event.Subscribe( EventArg_OnUseAblity.EventID , OnUseAbility );
            GameEntry.Event.Subscribe( EventArg_OnHitAbility.EventID, OnAbilityHit );
            GameEntry.Event.Subscribe( EventArg_WindUp.EventID      , OnWindUp);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            _abilityIdArr = null;
        }

        protected override void OnClose( bool isShutdown, object userData )
        {
            _actorID = -1;
            _actorProxy = null;
            ClearItemCacheDic();
            _testBtn.onClick.RemoveAllListeners();
            GameEntry.Event.Unsubscribe( EventArg_OnUseAblity.EventID, OnUseAbility );
            GameEntry.Event.Unsubscribe( EventArg_OnHitAbility.EventID, OnAbilityHit );
            GameEntry.Event.Unsubscribe( EventArg_WindUp.EventID, OnWindUp );
            base.OnClose( isShutdown, userData );
        }

        /// <summary>
        /// 用于敌人的测试abilityIconItem
        /// </summary>
        private AbilityIconItem _hostileAbilityIconItem = null;
        
        /// <summary>
        /// 用于敌人的测试button
        /// </summary>
        [SerializeField] private GameObject _hostileTestButton = null;
        
        /// <summary>
        /// 模板item
        /// </summary>
        [SerializeField] private GameObject _tempGameObejct = null;

        /// <summary>
        /// grid
        /// </summary>
        [SerializeField] private GridLayoutGroup _grid = null;

        /// <summary>
        /// actor模块
        /// </summary>
        private Module_ProxyActor _actorProxy = null;

        /// <summary>
        /// 要操作的actorID
        /// </summary>
        private int _actorID = -1;

        /// <summary>
        /// 敌人actorID
        /// </summary>
        private int[] _enemyActorIdArr = null;

        /// <summary>
        /// 技能ID
        /// </summary>
        private int[] _abilityIdArr = null;
        /// <summary>
        /// item缓存
        /// </summary>
        private Dictionary<int, AbilityIconItem> _iconItemDic = null;

        /// <summary>
        /// 测试按钮
        /// </summary>
        private Button _testBtn = null;
        
        /// <summary>
        /// 图标item
        /// </summary>
        private class AbilityIconItem : IReference
        {
            /// <summary>
            /// 设置CD
            /// </summary>
            public void CD( float percent, string text )
            {
                Tools.SetActive( _cd.gameObject, percent > 0 );
                Tools.SetActive( _text.gameObject, percent > 0 );
                //修改if逻辑
                if ( percent > 0 )
                {
                    _cd.fillAmount = percent;
                    _text.text = text;
                }
            }

            /// <summary>
            /// 初始化
            /// </summary>
            public void Setup( GameObject go, int abilityID, Action<int> _clickCallBack )
            {
                _root          = go;
                _abilityID     = abilityID;
                _cd            = Tools.GetComponent<Image>( go.transform, "cd" );
                _text          = Tools.GetComponent<Text>( go.transform , "Text" );
                _abilityIdText = Tools.GetComponent<Text>( go.transform , "AbilityIdText" );
                _image         = Tools.GetComponent<Image>( go          , "Image" );
                //_button                       = Tools.GetComponent<Button>( go, "Button" );
                _button       = Tools.GetComponent<Button>( _image.gameObject );
                clickCallBack = _clickCallBack;
                _button.onClick.AddListener( OnClicked );

                _abilityIdText.text = _abilityID.ToString();

            }

            /// <summary>
            /// 清理
            /// </summary>
            public void Clear()
            {
                clickCallBack = null;
                _button.onClick.RemoveAllListeners();
                _root          = null;
                _abilityID     = -1;
                _cd            = null;
                _text          = null;
                _abilityIdText = null;
                _button        = null;
                _image         = null;
            }

            private void OnClicked()
            {
                clickCallBack?.Invoke( _abilityID );
            }

            private Text _abilityIdText = null;
            private Text _text          = null;
            private Image _cd           = null;
            private Image _image        = null;
            private Button _button      = null;
            
            /// <summary>
            /// 技能ID
            /// </summary>
            private int _abilityID = -1;
            private Action<int> clickCallBack = null;

            /// <summary>
            /// 根节点
            /// </summary>
            private GameObject _root = null;
        }
        

        /// <summary>
        /// 界面参数
        /// </summary>
        public class Form_AbilityParam : IReference
        {
            public int _mainActorID = -1;

            public int[] _enemyActorID = null;

            /// <summary>
            /// {1000,1001,1002}
            /// </summary>
            public int[] _abilityID = null;

            public void Clear()
            {
                _mainActorID = -1;
                _enemyActorID = null;
                _abilityID = null;
            }
        }
    }
}
