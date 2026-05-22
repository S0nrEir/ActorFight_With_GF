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
            _selector?.Tick();
        }

        private Object_AbilitySelectorBase _selector;
    }
}