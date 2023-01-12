using GameFramework;
using System;
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
        public void Update()
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
    /// 脱离于框架的涉及具体业务的模块基类
    /// </summary>
    public abstract class GameFrameworkModuleBase
    {
        public virtual void Start(object param)
        {
            _open_flag = true;
        }

        public virtual void End()
        {
            _open_flag = false;
        }

        /// <summary>
        /// 关闭当前模块
        /// </summary>
        public virtual void OnClose()
        {
            //clear all sub module
            if ( Contains_Sub_Module && _sub_module_dic != null && _sub_module_dic.Count != 0 )
            {
                var iter = _sub_module_dic.GetEnumerator();
                GameFrameworkModuleBase sub_module = null;
                while ( iter.MoveNext() )
                {
                    sub_module = iter.Current.Value;
                    sub_module.OnClose();
                }
                _sub_module_dic.Clear();
            }//end if
            _sub_module_dic = null;
        }

        /// <summary>
        /// 模块内部数据的主动初始化
        /// </summary>
        public virtual void EnsureInit()
        {
            if ( Contains_Sub_Module )
                _sub_module_dic = new Dictionary<int, GameFrameworkModuleBase>();
        }

        /// <summary>
        /// 添加子模块
        /// </summary>
        protected T AddSubModule<T>() where T : GameFrameworkModuleBase
        {
            if ( !SubModuleValid() )
                return null;

            if ( ContainsSubModule<T>() )
            {
                Log.Warning( $"ContainsSubModule type:{typeof( T ).Name}" );
                return null;
            }

            var module = CreateSubModule<T>();
            module.EnsureInit();
            _sub_module_dic.Add( typeof( T ).GetHashCode(), module );
            return module as T;
        }

        /// <summary>
        /// 获取某个子模块
        /// </summary>
        protected bool TryGetSubModule<T>(out GameFrameworkModuleBase temp) where T : GameFrameworkModuleBase
        {
            temp = null;

            if ( !SubModuleValid() )
                return false;

            if ( !ContainsSubModule<T>() )
                return false;

            var code = typeof( T ).GetHashCode();
            var succ = _sub_module_dic.TryGetValue( code, out temp );
            return succ;
        }

        /// <summary>
        /// 是否包含某个子模块
        /// </summary>
        private bool ContainsSubModule<T>() where T : GameFrameworkModuleBase
        {
            return _sub_module_dic.ContainsKey( typeof( T ).GetHashCode() );
        }

        private bool SubModuleValid()
        {
            if ( !Contains_Sub_Module )
                throw new GameFrameworkException( $"{GetType().Name} not contains sub module" );

            return true;
        }

        /// <summary>
        /// 创建一个子module
        /// </summary>
        private static GameFrameworkModuleBase CreateSubModule<T>() where T : GameFrameworkModuleBase
        {
            return (GameFrameworkModuleBase) Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 开启标记
        /// </summary>
        protected bool _open_flag = false;

        /// <summary>
        /// 是否包含子模块
        /// </summary>
        protected virtual bool Contains_Sub_Module => false;

        /// <summary>
        /// 子模块目录
        /// </summary>
        private Dictionary<int, GameFrameworkModuleBase> _sub_module_dic = null;

    }
}
