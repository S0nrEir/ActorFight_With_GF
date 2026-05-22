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
