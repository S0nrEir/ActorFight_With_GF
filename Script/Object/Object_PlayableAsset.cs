using GameFramework;
using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.ObjectPool
{
    public class Object_PlayableAsset : ObjectBase
    {
        /// <summary>
        /// 创建一个Object_PlayableAsset缓存对象
        /// </summary>
        public static Object_PlayableAsset Create(string name,PlayableAsset asset)
        {
            var obj = ReferencePool.Acquire<Object_PlayableAsset>();
            obj.Initialize( name,asset );
            return obj;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }

        protected override void OnUnspawn()
        {
            base.OnUnspawn();
        }

        protected override void Release( bool isShutdown )
        {
        }
    }

}
