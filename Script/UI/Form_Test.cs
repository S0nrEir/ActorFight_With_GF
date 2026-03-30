using Aquila.Toolkit;
using UnityGameFramework.Runtime;

namespace Aquila.UI
{
    public class Form_Test : UIFormLogic
    {
        protected override void OnOpen( object userData )
        {
            base.OnOpen( userData );
            Tools.Logger.Info( "11111" );
        }
    }

}
