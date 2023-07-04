using Aquila.UI;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        public static class UI
        {
            /// <summary>
            /// 获取对应的UI参数，拿不到返回空
            /// </summary>
            public static T GetFormParam<T>( object userData ) where T : class
            {
                var param = userData as FormParam;
                return param._userData as T;
            }
        }

    }
}
