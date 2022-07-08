using UnityEngine;

namespace Aquila
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static partial class Tools
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
        /// 为一个transform添加一个child gameObject
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="child">自节点</param>
        /// <returns>添加的child gameObject，失败返回null</returns>
        public static Transform AddChild( Transform parent )
        {
            if ( parent == null )
                return null;

            var go = new GameObject();
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.gameObject.layer;

            return go.transform;
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
