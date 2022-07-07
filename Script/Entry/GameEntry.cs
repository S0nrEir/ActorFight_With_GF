using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila
{
    public partial class GameEntry : MonoBehaviour
    {
        private /*IEnumerator*/ void Start()
        {
            //内置组件初始化
            InitBuiltinComponents();
            //自定义组件初始化
            InitCustomComponents();
            //GF扩展初始化
            UGFExtensions.Await.AwaitableExtension.SubscribeEvent();
            DontDestroyOnLoad( this );
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
