using System;
using UnityEditor.Experimental.GraphView;

namespace Aquila.Editor
{
    
    public class AbilityViewPort : Port
    {
        protected AbilityViewPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }
    }
}
