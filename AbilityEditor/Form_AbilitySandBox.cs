#if UNITY_EDITOR

using Aquila.Event;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// 技能沙盒界面类
    /// </summary>
    public class Form_AbilitySandBox : UIFormLogic
    {
        //override:
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            // 解析参数
            var param = Tools.UI.GetFormParam<AbilitySandBoxForm_Param>( userData );
            if ( param != null )
            {
                _playerID = param._playerID;
                _dummyID = param._dummyID;
                _abilityID = param._abilityID;
                ReferencePool.Release( param );
            }

            _actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            _proxyActor = GameEntry.Module.GetModule<Module_ProxyActor>();

            _abilityButton = Tools.GetComponent<Button>( gameObject, "AbilityButton/Button" );
            _abilityButton.onClick.AddListener( OnAbilityButtonClicked );
            
            _cdTxt = Tools.GetComponent<Text>( gameObject, "AbilityButton/Text" );
            _cdImg = Tools.GetComponent<Image>( gameObject, "AbilityButton/Image" );
            _abilityIdText = Tools.GetComponent<Text>( gameObject, "AbilityButton/AbilityIdText" );
            
            _abilityIdText.text = _abilityID.ToString();

            GameEntry.Event.Subscribe( EventArg_OnUseAblity.EventID, OnUseAbility );
            GameEntry.Event.Subscribe( EventArg_OnHitAbility.EventID, OnAbilityHit );
            GameEntry.Event.Subscribe( EventArg_WindUp.EventID, OnWindUp );
        }

        protected override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            base.OnUpdate( elapseSeconds, realElapseSeconds );
            RefreshAbilityCD();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            // 取消事件订阅
            GameEntry.Event.Unsubscribe( EventArg_OnUseAblity.EventID, OnUseAbility );
            GameEntry.Event.Unsubscribe( EventArg_OnHitAbility.EventID, OnAbilityHit );
            GameEntry.Event.Unsubscribe( EventArg_WindUp.EventID, OnWindUp );

            // 清理按钮监听器
            if ( _abilityButton != null )
                _abilityButton.onClick.RemoveAllListeners();

            // 重置成员变量
            _playerID = -1;
            _dummyID = -1;
            _abilityID = -1;
            _actorMgr = null;
            _proxyActor = null;
            _abilityButton = null;
            _cdTxt = null;
            _cdImg = null;
            _abilityIdText = null;

            base.OnClose( isShutdown, userData );
        }

        /// <summary>
        /// 技能按钮点击事件
        /// </summary>
        private void OnAbilityButtonClicked()
        {
            if ( _proxyActor == null || _playerID == -1 || _dummyID == -1 || _abilityID == -1 )
            {
                Log.Warning( "<color=yellow>Form_AbilitySandBox: 参数无效，无法释放技能</color>" );
                return;
            }

            // 释放技能：player 对 dummy 释放
            _proxyActor.Ability2SingleTarget( _playerID, _dummyID, _abilityID, GameEntry.GlobalVar.InvalidPosition );
        }

        /// <summary>
        /// 刷新技能CD显示
        /// </summary>
        private void RefreshAbilityCD()
        {
            if ( _actorMgr == null || _playerID == -1 || _abilityID == -1 )
                return;

            var cd = _actorMgr.GetCoolDown( _playerID, _abilityID );
            var percent = cd.duration > 0 ? cd.remain / cd.duration : 0f;

            // 显示或隐藏CD
            //Tools.SetActive( _cdImg.gameObject, percent > 0 );
            //Tools.SetActive( _cdTxt.gameObject, percent > 0 );

            if ( percent > 0 )
            {
                _cdImg.fillAmount = percent;
                _cdTxt.text = cd.remain.ToString( "F1" );
            }
        }

        /// <summary>
        /// 使用技能事件回调
        /// </summary>
        private void OnUseAbility( object sender, GameEventArgs arg )
        {
            if ( !(arg is EventArg_OnUseAblity) )
                return;

            var result = (arg as EventArg_OnUseAblity)._resultParam;
            if ( !result._succ )
            {
                Log.Info( $"<color=white>技能使用失败: {Tools.Fight.UsingAbilityFaildDescription_l10n( result._stateDescription )}</color>" );
            }
        }

        /// <summary>
        /// 技能命中事件回调
        /// </summary>
        private void OnAbilityHit( object sender, GameEventArgs arg )
        {
            // 可以在这里添加命中反馈逻辑
        }

        /// <summary>
        /// 开始读条事件回调
        /// </summary>
        private void OnWindUp( object sender, GameEventArgs arg )
        {
            // 可以在这里添加读条反馈逻辑
        }

        // 成员变量
        private int _playerID = -1;
        private int _dummyID = -1;
        private int _abilityID = -1;
        private Module_ActorMgr _actorMgr = null;
        private Module_ProxyActor _proxyActor = null;
        private Button _abilityButton = null;
        private Text _cdTxt = null;
        private Image _cdImg = null;
        private Text _abilityIdText = null;

        /// <summary>
        /// 沙盒界面参数类
        /// </summary>
        public class AbilitySandBoxForm_Param : IReference
        {
            public int _playerID = -1;        // player 的 ActorID
            public int _dummyID = -1;         // dummy 的 ActorID
            public int _abilityID = -1;       // 技能 ID

            public void Clear()
            {
                _playerID = -1;
                _dummyID = -1;
                _abilityID = -1;
            }
        }
    }

}

#endif