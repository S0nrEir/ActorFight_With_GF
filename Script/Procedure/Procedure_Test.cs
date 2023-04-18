using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Aquila.Procedure
{
    /// <summary>
    /// ��������
    /// </summary>
    public class Procedure_Test : ProcedureBase
    {
        protected override void OnUpdate( IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds )
        {
            //base.OnUpdate( procedureOwner, elapseSeconds, realElapseSeconds );
            RunInput();
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            InputTest();
            //TimeWheelTest();
        }

        private void RunInput()
        {
            if ( _move_action is null )
                return;

            //�����move action��ֵ���������ļ��У�����action type������Ϊ��Value������ʾ״̬�ڳ����ڼ��������ĵ����룬��������ƶ����ֱ�ҡ��
            //Control type����ѡ������ݲ�ͬ�ڴ��������Ϊʱ���ض�Ӧ��ֵ��������Vector2����V2��ʾ�������ҵ��ĸ����뷽��
            var value = _move_action.ReadValue<Vector2>();
            if ( value != Vector2.zero )
            {
                ;
            }
        }

        private void InputTest()
        {
            GameEntry.Resource.LoadAsset( @"Assets/Samples/Input System/1.3.0/Simple Demo/SimpleControls.inputactions", new LoadAssetCallbacks( OnLoadActionsSucc, OnLoadActionsFaild ) );
        }

        private void OnLoadActionsSucc( string assetName, object asset, float duration, object userData )
        {
            var action_asset = ( asset as InputActionAsset );
            if ( action_asset is null )
            {
                Debug.LogError( $"faild to convert action asset,path={assetName}" );
                return;
            }

            if ( action_asset.actionMaps.Count == 0 )
            {
                Debug.LogError( $"action_asset.actionMaps.Count == 0,path={assetName}" );
                return;
            }

            var map = action_asset.FindActionMap("gameplay",true);
            _move_action = map.FindAction( "move",true );
            _move_action.started += OnMoveActionPerformed;

            _fire_action = map.FindAction( "fire", true );
            _fire_action.started += OnFireActionPerformed;

            _move_action.Enable();
            _fire_action.Enable();
            action_asset.Enable();
        }
        private InputAction _move_action = null;
        private InputAction _fire_action = null;

        /// <summary>
        /// ����ص�
        /// </summary>
        private void OnFireActionPerformed( InputAction.CallbackContext ctx )
        {
            if ( ctx.interaction is PressInteraction )
            {
                Debug.Log( "fire action performed!" );
            }
        }

        /// <summary>
        /// �ƶ��ص�
        /// </summary>
        private void OnMoveActionPerformed( InputAction.CallbackContext ctx )
        {
            if ( ctx.interaction is PressInteraction )
            {
                Debug.Log( "move action performed!" );
            }
        }

        private void OnLoadActionsFaild( string assetName, LoadResourceStatus status, string errorMessage, object userData )
        {
            Debug.LogError( $"load action faild , status = {status.ToString()}" );
        }

        private int counter = 0;
        private void TimeWheelTest()
        {
            GameEntry.TimeWheel.AddTask( Extension.TimeWheel_Task.GenRepeat
                   (
                       1f,
                       () =>
                       {
                           Debug.Log( $"testing...{counter++}" );
                       }
                   ) );
        }
    }

}
