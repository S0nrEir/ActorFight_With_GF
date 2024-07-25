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

        public bool Save()
        {
            AbilityEditorEffectGroupNode node;
            AbilityViewPort port;
            AbilityEditorEffectGroupNode startNode = null;
            foreach (var tempNode in nodes)
            {
                node = tempNode as AbilityEditorEffectGroupNode;
                if (node is null)
                {
                    Debug.LogError($"faild to cast node to AbilityEditorEffectGroupNode");
                    return false;
                }

                // foreach (var input in node.inputContainer.Children())
                // {
                //     port = input as AbilityViewPort;
                //     if (port is null)
                //     {
                //         Debug.LogError($"faild to cast port to AbilityViewPort");
                //         return false;
                //     }
                //
                //     if (!port.connected)
                //     {
                //         Debug.LogError($"faild to connect port,port name:{port.portName},port:{port.name}");
                //         return false;
                //     }
                // }
                //
                // foreach (var output in node.outputContainer.Children())
                // {
                //     port = output as AbilityViewPort;
                //     if (port is null)
                //     {
                //         Debug.LogError($"faild to cast port to AbilityViewPort");
                //         return false;
                //     }
                //
                //     if (!port.connected)
                //     {
                //         Debug.LogError($"faild to connect port,port name:{port.portName},port:{port.name}");
                //         return false;
                //     }
                // }
                
                //没输入有输出，是起始节点
                // if (node.inputContainer.Children().Count() == 0 && node.outputContainer.Children().Count() > 0)
                // {
                //     startNode = node;
                // }

            }//end foreach
            
            // if (startNode is null)
            // {
            //     Debug.LogError($"faild to find start node");
            //     return false;
            // }
            
            return true;
        }
        
        /// <summary>
        /// 创建一个一进一出的节点
        /// </summary>
        public AbilityEditorEffectGroupNode CreateOneInOneOut(string title,int groupID,Port.Capacity capacity = Port.Capacity.Single)
        {
            var node = AbilityEditorEffectGroupNode.Gen(title,capacity,capacity,groupID);
            node.AddManipulator(new NodeClickManipulator(OnNodeClick));
            node.OnCreate();
            //_nodeList.AddLast(node);
            AddElement(node);
            return node;
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
            //_nodeList = new LinkedList<AbilityEditorEffectGroupNode>();
            
            // var startNode = AbilityEditorNode_StartNode.GenStartNode();
            // _nodeList.AddFirst(startNode);
            // AddElement(startNode);

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
            SelectedNode = node as AbilityEditorEffectGroupNode;
            if(_window != null)
                _window.RefreshNodePanel(SelectedNode);
            
        }
        
        //-----------fields-----------
        /// <summary>
        /// 当前view的node集合
        /// </summary>
        // private LinkedList<AbilityEditorEffectGroupNode> _nodeList = null;
        
        /// <summary>
        /// 当前node选中的node
        /// </summary>
        public AbilityEditorEffectGroupNode SelectedNode
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
