using Aquila.Config;
using Aquila.Extension;
using GameFramework;
using System.Collections.Generic;
using Cfg.Common;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// 输入模块，管理输入
    /// </summary>
    public partial class Module_Input : GameFrameworkModuleBase, IUpdate
    {
        
        private void OnFireActionPerformed( InputAction.CallbackContext ctx )
        {
            if ( ctx.interaction is not PressInteraction )
                return;
            
            //单对单，测试物理伤害
            //GameEntry.Module.GetModule<Module_ProxyActor>().Ability2SingleTarget( _actorID1, _actorID2, _testAbilityMetaID );

            //单对多，测试物理伤害
            //GameEntry.Module.GetModule<Module_ProxyActor>().Ability2MultiTarget( _actorID1, new int[]{_actorID2,_actorID3,_actorID4}, _testAbilityMetaID );

            //GameEntry.Module.GetModule<Module_ProxyActor>().Ability2SingleTarget( _actorID1, _actorID2, _testAbilityMetaID );
        }
        
        public override void Open(object param)
        {
            base.Open( param);
            GameEntry.Resource.LoadAsset( @"Assets/Samples/InputSystem/1_3_0/SimpleDemo/SimpleControls.inputactions", new LoadAssetCallbacks
                (
                    //succ callback
                    ( assetName, asset, duration, userData ) =>
                    {
                        var action_asset = ( asset as InputActionAsset );
                        if ( action_asset is null || action_asset.actionMaps.Count == 0 )
                        {
                            Debug.LogError( $"action_asset is null || action_asset.actionMaps.Count == 0" );
                            return;
                        }
                        var map = action_asset.FindActionMap( "gameplay", true );
                        _fireAction = map.FindAction( "fire", true );
                        _fireAction.performed += OnFireActionPerformed;
                        _fireAction.Enable();
                        action_asset.Enable();
                    },
                    //faild callback
                    ( assetName, status, errorMessage, userData ) =>
                    {
                    }
                )
            );
        }

        //impl
        public void OnUpdate( float elapsed,float realElapsed )
        {
        }
        
        private InputAction _fireAction = null;
    }

    /// <summary>
    /// 场景参数
    /// </summary>
    public class Fight_Param : IReference
    {
        public int x_width = 0;
        public int z_width = 0;
        public Table_Scripts _sceneScriptMeta = null;

        /// <summary>
        /// 检查字段有效性
        /// </summary>
        public bool FieldValid()
        {
            return z_width > 0 &&
                   x_width > 0;
        }

        public void Clear()
        {
            x_width = 0;
            z_width = 0;
            _sceneScriptMeta = null;
        }
    }
}
