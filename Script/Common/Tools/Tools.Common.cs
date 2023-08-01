using GameFramework;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Aquila.Toolkit
{
    /// <summary>
    /// 工具类
    /// </summary>
    public partial class Tools
    {
        /// <summary>
        /// 获取两点间的距离平方根
        /// </summary>
        public static float DistanceSQR( Vector3 me, Vector3 target )
        {
            return ( target - me ).sqrMagnitude;
        }

        /// <summary>
        /// 从一个timeline track中获取对应类型的clip的asset，如果track有多个clip，则只返回第一个，拿不到返回空
        /// </summary>
        public static T GetFirstClipAssetFromTrack<T>( TrackAsset track ) where T : PlayableAsset
        {
            if ( track is null )
                return null;

            var clips = track.GetClips();
            foreach ( var clip in clips )
            {
                if ( clip.asset is T )
                    return clip.asset as T;
            }
            return null;
        }

        /// <summary>
        /// 获取timeline中指定类型的track，拿不到返回null
        /// </summary>
        public static T GetTrackFromTimeline<T>( TimelineAsset timeline ) where T : TrackAsset
        {
            var tracks = timeline.GetOutputTracks();
            if ( tracks is null )
                return null;

            foreach ( var track in tracks )
            {
                if ( track is T )
                    return track as T;
            }

            return null;
        }

        public static void SetParent( Transform child, Transform parent )
        {
            if ( child.parent == parent )
                return;

            child.SetParent( parent );
        }

        /// <summary>
        /// 名称转层级
        /// </summary>
        public static int NameToLayer( string name )
        {
            return 1 << LayerMask.NameToLayer( name );
        }

        /// <summary>
        /// 设置一个物体的层级
        /// </summary>
        public static void SetLayer( string layer, GameObject go, bool loop_set = false )
        {
            if ( go == null )
                return;

            go.layer = LayerMask.NameToLayer( layer );
            var tran = go.transform;
            if ( loop_set )
            {
                var child_count = tran.childCount;
                for ( int i = 0; i < child_count; i++ )
                    SetLayer( layer, tran.GetChild( i ).gameObject, loop_set );
            }
        }

        /// <summary>
        /// 设置一个gameObject的tag
        /// </summary>
        public static void SetTag( string tag, GameObject GO, bool loop_set = false )
        {
            if ( GO == null )
                return;

            GO.tag = tag;
            var tran = GO.transform;
            if ( loop_set )
            {
                var childCnt = tran.childCount;
                for ( var i = 0; i < childCnt; i++ )
                    SetTag( tag, tran.GetChild( i ).gameObject, loop_set );
            }
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
        public static void SetActive( GameObject go, bool active )
        {
            if ( go == null )
                return;

            if ( go.activeSelf != active )
                go.SetActive( active );
        }


        /// <summary>
        /// 获取某个GameObject上指定子路径的组件，拿不到返回空
        /// </summary>
        public static T GetComponent<T>( GameObject go, string childPath ) where T : class
        {
            if ( go == null )
                return null;

            return GetComponent<T>( go.transform.Find( childPath ) );
        }

        /// <summary>
        /// 获取某个Transform上指定子路径的组件，拿不到返回空
        /// </summary>
        public static T GetComponent<T>( Transform tran, string child_path ) where T : class
        {
            if ( string.IsNullOrEmpty( child_path ) )
                return null;

            return GetComponent<T>( tran.Find( child_path ) );
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

        /// <summary>
        /// 给一个GO上添加Component
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="go">要添加到的gameObject</param>
        /// <param name="comp">添加的组件</param>
        /// <returns>成功返回true</returns>
        public static bool TryAddComponent<T>( GameObject go, out T comp ) where T : Component
        {
            comp = null;
            if ( go == null )
                return false;

            comp = go.AddComponent<T>();
            return true;
        }


        /// <summary>
        /// 返回64位int数据中是否包含指定位数
        /// </summary>
        public static bool GetBitValue_i64( Int64 value, ushort index )
        {
            if ( index > 63 )
                throw new GameFrameworkException( "index > 63!" );

            var val = 1 << index;
            return ( value & val ) == val;
        }

        /// <summary>
        /// 设定64位int数据中某一位的值
        /// </summary>
        public static Int64 SetBitValue_i64( Int64 value, int index, bool bit_value )
        {
            //if ( index > 63 )
            //    throw new GameFrameworkException( "index > 63!" );

            var val = 1 << index;
            return ( bit_value ? ( value | val ) : ( value & ~val ) );
        }

        /// <summary>
        /// 返回Int数据中某一位是否为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <returns>true表示该位为1，false表示该位为0</returns>
        public static bool GetBitValue( int value, ushort index )
        {
            if ( index > 31 )
                throw new ArgumentOutOfRangeException( "index" ); //索引出错

            var val = 1 << index;
            return ( value & val ) == val;
        }

        /// <summary>
        /// 设定Int数据中某一位的值
        /// </summary>
        /// <param name="value">位设定前的值</param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <param name="bitValue">true设该位为1,false设为0</param>
        /// <returns>返回位设定后的值</returns>
        public static int SetBitValue( int value, ushort index, bool bit_value )
        {
            if ( index > 31 )
                throw new ArgumentOutOfRangeException( "index" ); //索引出错

            var val = 1 << index;
            return bit_value ? ( value | val ) : ( value & ~val );
        }

        /// <summary>
        /// 直接对一个int值进行或操作，返回操作后的值
        /// </summary>
        public static int OrBitValue( int orig_value, int attenmp_value )
        {
            return orig_value | attenmp_value;
        }
    }
}
