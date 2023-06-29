using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.UI
{
    public class Form_Test : UIFormLogic
    {
        protected override void OnOpen( object userData )
        {
            base.OnOpen( userData );
            Debug.Log( "11111" );
        }
    }

}
