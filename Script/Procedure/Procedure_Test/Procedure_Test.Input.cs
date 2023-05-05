using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 测试流程
    /// </summary>
    public partial class Procedure_Test : ProcedureBase
    {
        //tip:命令模式：通过接口或者抽象类将输入和输入的逻辑解耦
        private void RunInput()
        {
            if ( _move_action is null )
                return;

            //这里读move action的值，在配置文件中，他的action type被配置为【Value】，表示状态在持续期间连续更改的输入，比如鼠标移动，手柄摇杆
            //Control type根据选择的内容不同在触发相关行为时返回对应的值，这里是Vector2，用V2表示上下左右的四个输入方向
            var value = _move_action.ReadValue<Vector2>();
            if ( value != Vector2.zero )
            {
                Debug.Log($"RunInput--->vector value:{value}");
            }
        }

        private void InputTest()
        {
            GameEntry.Resource.LoadAsset( @"Assets/Samples/Input System/1.3.0/Simple Demo/SimpleControls.inputactions", new LoadAssetCallbacks( OnLoadActionsSucc, OnLoadActionsFaild ) );
        }

        private void OnLoadActionsSucc( string assetName, object asset, float duration, object userData )
        {
            var action_asset = ( asset as InputActionAsset );
            if ( action_asset is null || action_asset.actionMaps.Count == 0)
            {
                Debug.LogError( $"action_asset is null || action_asset.actionMaps.Count == 0" );
                return;
            }

            // if ( action_asset.actionMaps.Count == 0 )
            // {
            //     Debug.LogError( $"action_asset.actionMaps.Count == 0,path={assetName}" );
            //     return;
            // }

            var map = action_asset.FindActionMap("gameplay",true);
            _move_action = map.FindAction( "move",true );
            _move_action.performed += OnMoveActionPerformed; 
            
            _fire_action = map.FindAction( "fire", true );
            _fire_action.performed += OnFireActionPerformed;

            _move_action.Enable();
            _fire_action.Enable();
            action_asset.Enable();
        }
        private InputAction _move_action = null;
        private InputAction _fire_action = null;

        /// <summary>
        /// 点击回调
        /// </summary>
        private void OnFireActionPerformed( InputAction.CallbackContext ctx )
        {
            if ( ctx.interaction is PressInteraction )
            {
                Debug.Log( "fire action performed!" );
            }
        }

        /// <summary>
        /// 移动回调
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
    }

}
