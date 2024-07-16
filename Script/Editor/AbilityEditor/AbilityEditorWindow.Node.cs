using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Aquila.Editor
{
    /// <summary>
    /// 技能节点
    /// </summary>
    public class AbilityEditorNode : Node
    {
        /// <summary>
        /// 获取单个节点
        /// </summary>
        public static AbilityEditorNode Gen(string title,Port.Capacity inputCapacity,Port.Capacity outputCapacity)
        {
            //两个端口，一个输入一个输出
            var node = new AbilityEditorNode();
            node.title = title;
            
            // var port = node.InstantiatePort(Orientation.Vertical, Direction.Output, outputCapacity, null);
            var port = AbilityViewPort.Create<AbilityViewEdge>(Orientation.Horizontal, Direction.Output, outputCapacity,typeof(Port));
            port.portName = "test";
            node.outputContainer.Add(port);
            
            //port = node.InstantiatePort(Orientation.Vertical, Direction.Input, inputCapacity, null);
            port = AbilityViewPort.Create<AbilityViewEdge>(Orientation.Horizontal,Direction.Input, inputCapacity,typeof(Port));
            node.inputContainer.Add(port);
            
            node.SetPosition(new Rect(100,100,100,100));
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            node._guid = Guid.NewGuid();
            return node;
        }
        
        public Guid _guid;
    }
    
    /// <summary>
    /// 起始节点
    /// </summary>
    public class AbilityEditorNode_StartNode : AbilityEditorNode
    {
        /// <summary>
        /// 生成一个开始节点
        /// </summary>
        public static AbilityEditorNode_StartNode GenStartNode()
        {
            var node = new AbilityEditorNode_StartNode();
            node.title = "Start";
            var port = node.InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single,null);
            node.outputContainer.Add(port);
            node.SetPosition(new Rect(50,50,100,100));
            node.RefreshExpandedState();
            node.RefreshPorts();
            node.name = "Start";
            node._guid = Guid.NewGuid();
            return node;
        }
    }
}
