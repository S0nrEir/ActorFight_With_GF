
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.Editor
{
    public class AbilityView : GraphView
    {
        /// <summary>
        /// 创建一个新节点
        /// </summary>
        public bool CreateNormalNode(string title,Port.Capacity capacity = Port.Capacity.Single)
        {
            var node = AANode.Gen(title, capacity);
            _nodeList.AddLast(node);
            AddElement(node);
            return true;
        }

        /// <summary>
        /// 移除节点
        /// </summary>
        public bool RemoveNode(Guid guid)
        {
            return true;
        }

        /// <summary>
        /// 当前view节点数量
        /// </summary>
        public int NodeCount()
        {
            return graphElements.Count();
        }

        public AbilityView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            _nodeList = new LinkedList<AANode>();
            
            var startNode = AA_StartNode.GenStartNode();
            _nodeList.AddFirst(startNode);
            AddElement(startNode);
        }

        private LinkedList<AANode> _nodeList = null;
    }

    /// <summary>
    /// 技能节点
    /// </summary>
    public class AANode : Node
    {
        public Guid _guid;
        
        /// <summary>
        /// 获取单个节点
        /// </summary>
        public static AANode Gen(string title,Port.Capacity capacity)
        {
            //两个端口，一个输入一个输出
            var node = new AANode();
            // node.name = nodeName;
            node.title = title;
            
            var port = node.InstantiatePort(Orientation.Vertical, Direction.Output, capacity, null);
            node.outputContainer.Add(port);
            
            port = node.InstantiatePort(Orientation.Vertical, Direction.Input, capacity, null);
            node.inputContainer.Add(port);
            
            node.SetPosition(new Rect(100,100,100,100));
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            node._guid = Guid.NewGuid();
            return node;
        }
    }

    /// <summary>
    /// 起始节点
    /// </summary>
    public class AA_StartNode : AANode
    {
        /// <summary>
        /// 生成一个开始节点
        /// </summary>
        public static AA_StartNode GenStartNode()
        {
            var node = new AA_StartNode();
            node.title = "start";
            var port = node.InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single,null);
            node.outputContainer.Add(port);
            node.SetPosition(new Rect(100,100,100,100));
            node.RefreshExpandedState();
            node.RefreshPorts();
            node.name = "Start";
            node._guid = Guid.NewGuid();
            return node;
        }
    }

    /// <summary>
    /// 结束节点
    /// </summary>
    public class AA_EndNode : AANode
    {
        
    }
}
