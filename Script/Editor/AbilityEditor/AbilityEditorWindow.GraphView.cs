using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.Editor
{
    public class AbilityView : GraphView
    {
        //-----------pub-----------
        
        /// <summary>
        /// 创建一个一进一出的节点
        /// </summary>
        public bool CreateOneInOneOut(string title,Port.Capacity capacity = Port.Capacity.Single)
        {
            var node = AbilityEditorNode.Gen(title, capacity,capacity);
            node.AddManipulator(new NodeClickManipulator(OnNodeClick));
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
        
        //-----------override-----------
        /// <summary>
        /// 端口连接筛选
        /// </summary>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            // return ports.ToList();
            var compatiblePorts = new List<Port>();
            foreach (var port in ports)
            {
                //要连接的起始端口不能自己连自己
                if(startPort.node == port.node)
                    continue;
                
                //端口方向不能一样
                if(startPort.direction == port.direction)
                    continue;
                
                if (startPort.portType == typeof(Port))
                    continue;
                
                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }

        //-----------constructor-----------
        public AbilityView(EditorWindow window)
        {
            SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            _nodeList = new LinkedList<AbilityEditorNode>();
            
            var startNode = AbilityEditorNode_StartNode.GenStartNode();
            _nodeList.AddFirst(startNode);
            AddElement(startNode);

            _window = window as AbilityEditorWindow;
            if(_window is null)
                Debug.LogError("AbilityEditorWindow.GraphView.cs: window is null");
        }
        
        //-----------event-----------
        /// <summary>
        /// on node click
        /// </summary>
        private void OnNodeClick(Node node,MouseDownEvent evt)
        {
            Debug.Log("node clicked");
            SelectedNode = node as AbilityEditorNode;
            if(_window != null)
                _window.RefreshNodePanel(SelectedNode);
        }
        
        //-----------fields-----------
        /// <summary>
        /// 当前view的node集合
        /// </summary>
        private LinkedList<AbilityEditorNode> _nodeList = null;
        
        /// <summary>
        /// 当前node选中的node
        /// </summary>
        public AbilityEditorNode SelectedNode
        {
            get;
            private set;
        }

        /// <summary>
        /// 所属窗体
        /// </summary>
        private AbilityEditorWindow _window = null;
    }
}
