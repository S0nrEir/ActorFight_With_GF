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
        public static AbilityEditorEffectGroupNode Gen(string title,Port.Capacity inputCapacity,Port.Capacity outputCapacity)
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

            _triggerTimeLabel = new Label($"TriggerTime:{TriggerTime}");
            // contentContainer.Add(_triggerTimeLabel);
            contentContainer.Add(_triggerTimeLabel);
            
            _idLabel = new Label($"ID:{ID}");
            // contentContainer.Add(new Label($"ID:{ID}"));
            contentContainer.Add(_idLabel);
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
            get
            {
                return _triggerTime;
            }
            set
            {
                _triggerTime = value;
                _triggerTimeLabel.text = $"TriggerTime:{_triggerTime}";
            }
        }

        private float _triggerTime = 0f;
        
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _idLabel.text = $"ID:{_id}";
            }
        }

        private int _id;
        
        private Label _triggerTimeLabel = null;
        private Label _idLabel = null;
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
