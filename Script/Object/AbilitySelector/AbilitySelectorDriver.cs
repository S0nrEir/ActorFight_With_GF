using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.ObjectPool
{
    public class AbilitySelectorDriver : MonoBehaviour
    {
        public void Setup(Object_AbilitySelectorBase selector)
        {
            _selector = selector;
        }

        //#todo:引入输入系统后改一下这里，不要在刷帧检查输入了
        private void Update()
        {
            if (_selector == null)
                return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetMouseButtonDown(1))
            {
                _selector.CancelSelection();
                return;
            }

            if (UnityEngine.Input.GetMouseButtonDown(0))
                _selector.ConfirmSelection();
        }

        private Object_AbilitySelectorBase _selector;
    }
}
