using Aquila.Config;
using Aquila.Module;
using UGFExtensions.Await;
using UnityEngine;

namespace Aquila
{
    /// <summary>
    /// 游戏业务入口
    /// </summary>
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
            //初始化相机
            GlobalVar.GetMainCamera();
            
            DontDestroyOnLoad( this );
        }

        void Update()
        {
            GameFrameworkModule.Update();
        }

        private void FixedUpdate()
        {
            GameFrameworkModule.FixedUpdate();
        }
    }

}
