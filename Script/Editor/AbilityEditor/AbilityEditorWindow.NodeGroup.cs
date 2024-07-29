using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.Editor
{
    /// <summary>
    /// effect节点
    /// </summary>
    public class AbilityEditorEffectGroupNode : Node
    {
        //-----------pub-----------

        /// <summary>
        /// 写入测试数据
        /// </summary>
        public void WriteTestData()
        {
            TriggerTime = 2.0f;
        }
        
        public void Repaint()
        {
            MarkDirtyRepaint();
        }

        /// <summary>
        /// 设置节点的基本信息()
        /// </summary>
        public void SetBaseInfo(float triggerTime,int groupID)
        {
            TriggerTime = triggerTime;
            ID = groupID;
        }
        
        /// <summary>
        /// 获取该节点下的所有port
        /// </summary>
        public IReadOnlyCollection<AbilityViewPort> GetAllPorts()
        {
            if (_allPorts is null || _allPorts.Count == 0)
            {
                foreach (var port in inputContainer.Children())
                    _allPorts.Add(port as AbilityViewPort);

                foreach (var port in outputContainer.Children())
                    _allPorts.Add(port as AbilityViewPort);
            }
            
            return _allPorts.AsReadOnly();
        }

        /// <summary>
        /// 获取单个节点
        /// </summary>
        public static AbilityEditorEffectGroupNode Gen(string title,Port.Capacity inputCapacity,Port.Capacity outputCapacity,int id)
        {
            //两个端口，一个输入一个输出
            var node = new AbilityEditorEffectGroupNode();
            node.title = title;
            
            // var port = node.InstantiatePort(Orientation.Vertical, Direction.Output, outputCapacity, null);
            var port = AbilityViewPort.Create<AbilityViewEdge>(Orientation.Horizontal, Direction.Output, outputCapacity,typeof(Port));
            port.portName = "test";
            node.outputContainer.Add(port);
            
            //port = node.InstantiatePort(Orientation.Vertical, Direction.Input, inputCapacity, null);
            port = AbilityViewPort.Create<AbilityViewEdge>(Orientation.Horizontal,Direction.Input, inputCapacity,typeof(Port));
            node.inputContainer.Add(port);
            
            node.SetPosition(new Rect(100,100,100,100));
            node.RefreshPorts();
            node.SetBaseInfo(-1f ,id);
            node.RefreshExpandedState();
            return node;
        }

        /// <summary>
        /// 当前节点下的所有端口
        /// </summary>
        private List<AbilityViewPort> _allPorts = null;

        //-----------constructor-----------
        public AbilityEditorEffectGroupNode()
        {
            _allPorts = new List<AbilityViewPort>();
        }

        public virtual void OnCreate()
        {
            var button = new Button();
            button.text = "AddPort";
            button.clicked += OnAddPortClick;
            contentContainer.Add(button);
            
            contentContainer.Add(new Label($"TriggerTime:{TriggerTime}"));
            contentContainer.Add(new Label($"ID:{ID}"));
        }

        private void OnAddPortClick()
        {
            Debug.Log($"add port clicked!");
        }

        //-----------fields-----------

        /// <summary>
        /// 触发时间
        /// </summary>
        public float TriggerTime
        {
            get;
            private set;
        }

        public int ID
        {
            get;
            private set;
        }
    }
    
    /// <summary>
    /// 起始节点
    /// </summary>
    public class AbilityEditorNode_StartNode : AbilityEditorEffectGroupNode
    {
        //-----------pub-----------
        
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
            return node;
        }
    }

    public class AbilityEditorNode_EndNode : AbilityEditorEffectGroupNode
    {
        
    }
}
