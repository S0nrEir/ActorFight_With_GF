using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// 模块扩展组件
    /// </summary>
    public class Component_Module : GameFrameworkComponent
    {

        /// <returns></returns>
        public T GetModule<T>() where T : GameFrameworkModuleBase, new()
        {
            return ( T ) GetModule( typeof( T ) );
        }

        /// <summary>
        /// 渲染帧
        /// </summary>
        public static void Update()
        {
            for ( int i = 0; i < _allUpdates.Count; i++ )
                _allUpdates[i].OnUpdate( Time.deltaTime );
        }

        /// <summary>
        /// 固定帧
        /// </summary>
        public void FixedUpdate()
        {
            foreach ( var item in _allFixedUpdates )
                item.OnFixedUpdate();
        }

        /// <summary>
        /// 关闭游戏的所有模块
        /// </summary>
        public void ShutDown()
        {
            foreach ( var item in _allGameModules.Values )
                item.OnClose();

            _allUpdates.Clear();
            _allFixedUpdates.Clear();
            _allGameModules.Clear();
        }

        /// <summary>
        /// 关闭指定类型的GameModule
        /// </summary>
        public void ShutDown( Type[] moduleTypes )
        {
            var hashCode = 0;
            GameFrameworkModuleBase module = null;
            foreach ( var type in moduleTypes )
            {
                hashCode = type.GetHashCode();
                if ( _allGameModules.TryGetValue( hashCode, out module ) )
                    module.OnClose();
            }
        }


        #region 内部函数

        /// <summary>
        /// 获取模块
        /// </summary>
        public GameFrameworkModuleBase GetModule( Type type )
        {
            int hashCode = type.GetHashCode();
            GameFrameworkModuleBase module = null;
            if ( _allGameModules.TryGetValue( hashCode, out module ) )
                return module;

            module = CreateModule( type );
            module.EnsureInit();
            return module;
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        private GameFrameworkModuleBase CreateModule( Type type )
        {
            int hashCode = type.GetHashCode();
            GameFrameworkModuleBase module = ( GameFrameworkModuleBase ) Activator.CreateInstance( type );
            _allGameModules[hashCode] = module;
            //整理含IUpdate的模块
            var update = module as IUpdate;
            if ( update != null )
                _allUpdates.Add( update );
            //整理含IFixed的模块
            var fixedUpdate = module as IFixedUpdate;
            if ( fixedUpdate != null )
                _allFixedUpdates.Add( fixedUpdate );
            return module;
        }

        #endregion

        #region 属性
        //所有的子模块
        private static readonly Dictionary<int, GameFrameworkModuleBase> _allGameModules = new Dictionary<int, GameFrameworkModuleBase>();
        //所有渲染帧函数
        private static List<IUpdate> _allUpdates = new List<IUpdate>();
        //所有的固定帧函数
        private static List<IFixedUpdate> _allFixedUpdates = new List<IFixedUpdate>();
        #endregion



    }

    /// <summary>
    /// 模块基类
    /// </summary>
    public abstract class GameFrameworkModuleBase
    {
        /// <summary>
        /// 关闭当前模块
        /// </summary>
        public abstract void OnClose();

        /// <summary>
        /// 模块内部数据的主动初始化
        /// </summary>
        public virtual void EnsureInit()
        {
        }
    }
}
