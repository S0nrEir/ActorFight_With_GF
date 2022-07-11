namespace Aquila
{
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
