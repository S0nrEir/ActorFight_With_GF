using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs.AnimationBlendTree;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.Editor
{
    
    public class NodeClickManipulator : Manipulator
    {

        public NodeClickManipulator()
        {
        }

        public NodeClickManipulator(Action<UnityEditor.Experimental.GraphView.Node,MouseDownEvent> onClick)
        {
            _onClick = onClick;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }

        /// <summary>
        /// 处理node的按下事件
        /// </summary>
        private void OnMouseDown(MouseDownEvent evt)
        {
            _onClick?.Invoke(target as UnityEditor.Experimental.GraphView.Node,evt);
        }

        /// <summary>
        /// click回调
        /// </summary>
        public Action<UnityEditor.Experimental.GraphView.Node,MouseDownEvent> _onClick = null;
    }
}
