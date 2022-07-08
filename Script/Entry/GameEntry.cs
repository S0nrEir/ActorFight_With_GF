using UGFExtensions.Await;
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
            //扩展初始化
            AwaitableExtensions.SubscribeEvent();
            DontDestroyOnLoad( this );
        }

        void Update()
        {

        }
    }

}
