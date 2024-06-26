using UnityGameFramework.Runtime;

namespace Aquila
{
    /// <summary>
    /// 内置组件游戏入口
    /// </summary>
    public partial class GameEntry
    {
        /// <summary>
        /// 获取游戏基础组件。
        /// </summary>
        public static BaseComponent Base
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置组件。
        /// </summary>
        public static ConfigComponent Config
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据结点组件。
        /// </summary>
        //public static DataNodeComponent DataNode
        //{
        //    get;
        //    private set;
        //}

        /// <summary>
        /// 获取内部数据表组件，内部表只给程序读取，也只给程序配置，不开放给外部
        /// </summary>
        public static DataTableComponent DataTable
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取调试组件。
        /// </summary>
        public static DebuggerComponent Debugger
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载组件。
        /// </summary>
        public static DownloadComponent Download
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体组件。
        /// </summary>
        public static EntityComponent Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取事件组件。
        /// </summary>
        public static EventComponent Event
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取有限状态机组件。
        /// </summary>
        public static FsmComponent Fsm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取本地化组件。
        /// </summary>
        public static LocalizationComponent Localization
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取网络组件。
        /// </summary>
        public static NetworkComponent Network
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对象池组件。
        /// </summary>
        public static ObjectPoolComponent ObjectPool
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取流程组件。
        /// </summary>
        public static ProcedureComponent Procedure
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源组件。
        /// </summary>
        public static ResourceComponent Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取场景组件。
        /// </summary>
        public static SceneComponent Scene
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置组件。
        /// </summary>
        public static SettingComponent Setting
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音组件。
        /// </summary>
        public static SoundComponent Sound
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面组件，内部UI组件
        /// </summary>
        public static UIComponent BaseUI
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取网络组件。
        /// </summary>
        public static WebRequestComponent WebRequest
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化内置组件
        /// </summary>
        private static void InitBuiltinComponents()
        {
            Base          = UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
            Config        = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent>();
            //DataNode    = UnityGameFramework.Runtime.GameEntry.GetComponent<DataNodeComponent>();
            DataTable     = UnityGameFramework.Runtime.GameEntry.GetComponent<DataTableComponent>();
            Debugger      = UnityGameFramework.Runtime.GameEntry.GetComponent<DebuggerComponent>();
            Download      = UnityGameFramework.Runtime.GameEntry.GetComponent<DownloadComponent>();
            Entity        = UnityGameFramework.Runtime.GameEntry.GetComponent<EntityComponent>();
            Event         = UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>();
            Fsm           = UnityGameFramework.Runtime.GameEntry.GetComponent<FsmComponent>();
            Localization  = UnityGameFramework.Runtime.GameEntry.GetComponent<LocalizationComponent>();
            Network       = UnityGameFramework.Runtime.GameEntry.GetComponent<NetworkComponent>();
            ObjectPool    = UnityGameFramework.Runtime.GameEntry.GetComponent<ObjectPoolComponent>();
            Procedure     = UnityGameFramework.Runtime.GameEntry.GetComponent<ProcedureComponent>();
            Resource      = UnityGameFramework.Runtime.GameEntry.GetComponent<ResourceComponent>();
            Scene         = UnityGameFramework.Runtime.GameEntry.GetComponent<SceneComponent>();
            Setting       = UnityGameFramework.Runtime.GameEntry.GetComponent<SettingComponent>();
            Sound         = UnityGameFramework.Runtime.GameEntry.GetComponent<SoundComponent>();
            BaseUI        = UnityGameFramework.Runtime.GameEntry.GetComponent<UIComponent>();
            WebRequest   = UnityGameFramework.Runtime.GameEntry.GetComponent<WebRequestComponent>();
        }

        /// <summary>
        /// 内置组件的一些默认设置
        /// </summary>
        private static void BuiltinComponentsSettting()
        {
        }
    }
}
