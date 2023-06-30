using Aquila.UI;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        public static class UI
        {
            /// <summary>
            /// ��ȡ��Ӧ��UI�������ò������ؿ�
            /// </summary>
            public static T GetFormParam<T>( object userData ) where T : class
            {
                var param = userData as FormParam;
                return param._userData as T;
            }
        }

    }
}
