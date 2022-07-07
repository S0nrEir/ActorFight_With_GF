using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// 名称转层级
        /// </summary>
        public static int NameToLayer( string name )
        {
            return 1 << LayerMask.NameToLayer( name );
        }

        /// <summary>
        /// 设置一个gameObject的tag
        /// </summary>
        public static void SetTag( string tag, GameObject GO, bool loopSet = false )
        {
            if ( GO == null )
                return;

            GO.tag = tag;
            var tran = GO.transform;
            var childCnt = tran.childCount;
            for ( var i = 0; i < childCnt; i++ )
                SetTag( tag, tran.GetChild( i ).gameObject, loopSet );
        }

        /// <summary>
        /// 设置一个gameObject的active
        /// </summary>
        public static void SetActive(GameObject go,bool active)
        {
            if ( go == null )
                return;

            if ( go.activeSelf != active )
                go.SetActive( active );
        }

        /// <summary>
        /// 获取某个Transform上指定子路径的组件，拿不到返回空
        /// </summary>
        public static T GetComponent<T>( Transform tran, string childPath ) where T : class
        {
            if ( string.IsNullOrEmpty( childPath ) )
                return null;

            return GetComponent<T>( tran.Find( childPath ) );
        }

        /// <summary>
        /// 获取某个Transform上的指定组件，拿不到返回空
        /// </summary>
        public static T GetComponent<T>( Transform tran ) where T : class
        {
            if ( tran == null )
                return null;

            return GetComponent<T>( tran.gameObject );
        }

        /// <summary>
        /// 获取某个gameObject上的指定组件，拿不到返回空
        /// </summary>
        public static T GetComponent<T>( GameObject go ) where T : class
        {
            if ( go == null )
                return null;

            return go.GetComponent<T>();
        }
    }



}
