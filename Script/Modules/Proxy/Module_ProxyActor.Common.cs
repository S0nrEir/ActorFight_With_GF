//using Aquila.Extension;
//using Aquila.Fight.Addon;
//using Cfg.Enum;
//using System;
//using UnityEngine;

//namespace Aquila.Module
//{
//    /// <summary>
//    /// Actor的代理模块，此脚本处理通用接口和模块的主逻辑入口
//    /// </summary>
//    public partial class Module_ProxyActor : GameFrameworkModuleBase, IUpdate
//    {
//        //-----------------------pub-----------------------

//        //----------------------- override-----------------------
//        public override void Open( object param )
//        {
//            base.Open( param );
//            //MgrOpen();
//            SystemOpen();
//        }

//        public override void Close()
//        {
//            OnSystemClose();
//            //MgrClose();
//            base.Close();
//        }

//        public override void EnsureInit()
//        {
//            base.EnsureInit();
//            //MgrEnsureInit();
//            FightEnsureInit();
//            SystemEnsureInit();
//        }

//        public void OnUpdate( float elapsed, float realElapsed )
//        {
//            if ( !_open_flag )
//                return;

//            SystemUpdate( elapsed, realElapsed );
//        }
//    }
//}