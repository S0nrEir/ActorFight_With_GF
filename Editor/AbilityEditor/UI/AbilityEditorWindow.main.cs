using Aquila.AbilityEditor;
using Cfg.Enum;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static PlasticPipe.Server.MonitorStats;

namespace Editor.AbilityEditor
{
    public partial class AbilityEditorWindow : EditorWindow
    {
        [MenuItem( "Aquila/AbilityEditor/AbilityEditorWindow" )]
        public static void OpenAbilityEditorWindow()
        {
            AbilityEditorWindow wnd = GetWindow<AbilityEditorWindow>();
            wnd.titleContent = new GUIContent( "AbilityEditorWindow" );
            wnd.minSize = new Vector2( 1300, 300 );
            wnd.maxSize = new Vector2( 1300, 300 );
            wnd.Show();
        }

        public void CreateGUI()
        {
            if ( _abilityTreeAsset == null )
            {
                Debug.LogError( "AbilityEditorWindow.cs --> CreateGUI() --> abilityTreeAsset is null" );
                return;
            }

            _root = rootVisualElement;
            _abilityTreeAsset.CloneTree( _root );

            InitializeUIElements();
            RegisterDragAndDropCallbacks();
            RegisterTrackPanelContextMenu();
            _timelineTracks = new List<TimelineTrack>();

            #region nouse
            // var dragHintLabel = new Label("拖入 AbilityData 资源到此窗口")
            // {
            //     style =
            //     {
            //         fontSize = 16,
            //         unityTextAlign = TextAnchor.MiddleCenter,
            //         paddingTop = 20,
            //         paddingBottom = 20,
            //         color = Color.gray
            //     }
            // };
            // root.Add(dragHintLabel);

            // 显示当前加载的 AbilityData
            // var currentDataLabel = new Label("当前: 无")
            // {
            //     name = "currentDataLabel",
            //     style =
            //     {
            //         fontSize = 14,
            //         unityTextAlign = TextAnchor.MiddleCenter,
            //         paddingBottom = 10
            //     }
            // };
            // root.Add(currentDataLabel);
            #endregion
            
            // 初始化时间轴拖动线
            InitializeTimelineScrubber();
        }

        /// <summary>
        /// 初始化所有 UI 元素引用
        /// </summary>
        private void InitializeUIElements()
        {
            var abilityBaseInfoPanel = _root.Q<VisualElement>( "AbilityEditPanel" );
            if ( abilityBaseInfoPanel != null )
            {
                _abilityIDTextField = abilityBaseInfoPanel.Q<TextField>( "AbilityIDTxtField" );
                _abilityDescTextField = abilityBaseInfoPanel.Q<TextField>( "AbilityDescTxtField" );
                _costIDTextField = abilityBaseInfoPanel.Q<TextField>( "CostIDTxtField" );
                _coolDownIDTextField = abilityBaseInfoPanel.Q<TextField>( "CoolDownIDTxtField" );
                _timelineIDTextField = abilityBaseInfoPanel.Q<TextField>( "TimelineIDTxtField" );
                _targetTypeDropdown = abilityBaseInfoPanel.Q<DropdownField>( "TargetTypeDropdown" );
                _durationTextField = abilityBaseInfoPanel.Q<TextField>( "DurationTxtField" );
                _trackPanel = abilityBaseInfoPanel.Q<VisualElement>( "TrackPanel" );
                _timelineTrackPanel = abilityBaseInfoPanel.Q<VisualElement>( "TimelineTrackPanel" );

                var genTimelineTrackButton = abilityBaseInfoPanel.Q<Button>( "GenTimelineTrackBtn" );
                if ( genTimelineTrackButton != null )
                    genTimelineTrackButton.clicked += GenerateTimelineTracks;
            }
            var headerMenu = _root.Q<VisualElement>( "HeaderMenu" );
            if ( headerMenu != null )
            {
                var tempBtn = headerMenu.Q<Button>( "AddNewTrackBtn" );
                tempBtn.clicked += AddNewTrack;
            }

            // 验证控件是否成功获取，如果有任何一个为空则返回
            if ( _abilityIDTextField == null ||
                _abilityDescTextField == null ||
                _costIDTextField == null ||
                _coolDownIDTextField == null ||
                _timelineIDTextField == null ||
                _targetTypeDropdown == null ||
                _trackPanel == null )
            {
                Debug.LogError( "faild to get ui controls,can not init editor window." );
                return;
            }

            if ( _targetTypeDropdown != null )
            {
                List<string> enumChoices = new List<string>();
                Array enumValues = Enum.GetValues( typeof( AbilityTargetType ) );
                foreach ( AbilityTargetType enumValue in enumValues )
                    enumChoices.Add( enumValue.ToString() );

                _targetTypeDropdown.choices = enumChoices;
                if ( enumChoices.Count > 0 )
                    _targetTypeDropdown.value = enumChoices[0];
            }
        }

        private void RegisterDragAndDropCallbacks()
        {
            _root.RegisterCallback<DragEnterEvent>( _ =>
            {
                if ( HasAbilityDataInDrag() )
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                else
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            } );

            _root.RegisterCallback<DragUpdatedEvent>( _ =>
            {
                if ( HasAbilityDataInDrag() )
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                else
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            } );

            _root.RegisterCallback<DragPerformEvent>( _ =>
            {
                DragAndDrop.AcceptDrag();
                foreach ( var obj in DragAndDrop.objectReferences )
                {
                    if ( obj is AbilityData abilityData )
                    {
                        _currentAbilityData = abilityData;
                        OnAbilityDataChanged( _currentAbilityData );
                        break;
                    }
                }
            } );
        }

        /// <summary>
        /// TrackPanel事件注册
        /// </summary>
        private void RegisterTrackPanelContextMenu()
        {
            if ( _trackPanel == null )
            {
                Debug.LogError( "RegisterTrackPanelContextMenu: _trackPanel is null!" );
                return;
            }
            _trackPanel.AddManipulator( new ContextualMenuManipulator( evt =>
            {
                //add track
                evt.menu.AppendAction( "Add Track", action => AddNewTrack() );

                //del track
                VisualElement trackToDelete = GetTrackUnderMouse( evt.mousePosition );
                if ( trackToDelete != null )
                    evt.menu.AppendAction( "Delete Track", action => DeleteTrackAtPosition( trackToDelete ) );
                else
                    evt.menu.AppendAction( "Delete Track", action => { }, DropdownMenuAction.Status.Disabled );

                //..
            } ) );
        }

        /// <summary>
        /// 初始化时间轴拖动线
        /// </summary>
        private void InitializeTimelineScrubber()
        {
            // 获取主面板引用
            _mainPanel = _root.Q<VisualElement>("AbilityEditPanel");

            // 主拖动线（时间轴区域内的拖动线）
            _timelineScrubber = new VisualElement
            {
                name = "TimelineScrubber",
                pickingMode = PickingMode.Ignore, // 不拦截鼠标事件，让事件穿透到下层
                style =
                {
                    position = Position.Absolute,
                    width = 2,
                    backgroundColor = new Color(1f, 0.5f, 0f, 1f), // 实心橙色
                    left = 50,
                    top = 0,
                    bottom = 0, // 使用 bottom 替代固定 height，自动填满容器高度
                    display = DisplayStyle.None,
                }
            };

            // 延伸线（从时间轴容器延伸到整个面板底部）
            _scrubberExtension = new VisualElement
            {
                name = "ScrubberExtension",
                pickingMode = PickingMode.Ignore, // 不拦截鼠标事件
                style =
                {
                    position = Position.Absolute,
                    width = 2,
                    backgroundColor = new Color(1f,  1f, 0f, 0.5f), // 半透明橙色
                    left = 50,
                    top = 0,
                    bottom = 0, // 自动延伸到容器底部
                    display = DisplayStyle.None,
                }
            };

            // 添加时间显示标签
            var timeLabel = new Label("0.0s")
            {
                name = "ScrubberTimeLabel",
                pickingMode = PickingMode.Ignore, // 标签也不拦截事件
                style =
                {
                    position = Position.Absolute,
                    fontSize = 10,
                    color = Color.white,
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f),
                    paddingLeft = 4,
                    paddingRight = 4,
                    paddingTop = 2,
                    paddingBottom = 2,
                    borderTopLeftRadius = 2,
                    borderTopRightRadius = 2,
                    borderBottomLeftRadius = 2,
                    borderBottomRightRadius = 2,
                    left = -20,
                    top = -22,
                    display = DisplayStyle.None
                }
            };

            _timelineScrubber.Add(timeLabel);
        }

        /// <summary>
        /// 注册时间轴拖动线的事件
        /// </summary>
        private void RegisterScrubberEvents(VisualElement timelineContainer)
        {
            if (_timelineScrubber == null || _scrubberExtension == null || timelineContainer == null)
            {
                Debug.LogError("RegisterScrubberEvents: Required elements are null!");
                return;
            }

            // 先从旧的父容器中移除拖动线（如果存在）
            if (_timelineScrubber.parent != null)
                _timelineScrubber.RemoveFromHierarchy();

            if (_scrubberExtension.parent != null)
                _scrubberExtension.RemoveFromHierarchy();

            // 添加主拖动线到时间轴容器（覆盖所有轨道）
            timelineContainer.Add(_timelineScrubber);

            // 添加延伸线到主面板（延伸到面板底部）
            if (_mainPanel != null)
                _mainPanel.Add(_scrubberExtension);
            else
                Debug.LogWarning("RegisterScrubberEvents: _mainPanel is null, scrubber extension not added!");

            var timeLabel = _timelineScrubber.Q<Label>();
            if (timeLabel == null)
            {
                Debug.LogError("RegisterScrubberEvents: timeLabel not found!");
                return;
            }

            // 设置可见性
            _timelineScrubber.style.display = DisplayStyle.Flex;
            _scrubberExtension.style.display = DisplayStyle.Flex;
            timeLabel.style.display = DisplayStyle.Flex;
            
            //避免重复注册
            timelineContainer.UnregisterCallback<MouseDownEvent>(OnTimelineMouseDown);
            timelineContainer.UnregisterCallback<MouseMoveEvent>(OnTimelineMouseMove);
            timelineContainer.UnregisterCallback<MouseUpEvent>(OnTimelineMouseUp);
            timelineContainer.UnregisterCallback<MouseLeaveEvent>(OnTimelineMouseLeave);

            timelineContainer.RegisterCallback<MouseDownEvent>(OnTimelineMouseDown);
            timelineContainer.RegisterCallback<MouseMoveEvent>(OnTimelineMouseMove);
            timelineContainer.RegisterCallback<MouseUpEvent>(OnTimelineMouseUp);
            timelineContainer.RegisterCallback<MouseLeaveEvent>(OnTimelineMouseLeave);

            Debug.Log("RegisterScrubberEvents: Timeline scrubber events registered successfully");
        }

        // 提取事件处理方法，便于注销
        private void OnTimelineMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0) // 左键点击
            {
                _isDraggingScrubber = true;
                // 使用 localMousePosition.x 减去轨道名称区域宽度 (50px)
                float adjustedX = evt.localMousePosition.x - 50;
                UpdateScrubberPosition(adjustedX);
                evt.StopPropagation();
            }
        }

        private void OnTimelineMouseMove(MouseMoveEvent evt)
        {
            if (_isDraggingScrubber)
            {
                // 使用 localMousePosition.x 减去轨道名称区域宽度 (50px)
                float adjustedX = evt.localMousePosition.x - 50;
                UpdateScrubberPosition(adjustedX);
                evt.StopPropagation();
            }
        }

        private void OnTimelineMouseUp(MouseUpEvent evt)
        {
            if (_isDraggingScrubber && evt.button == 0)
            {
                _isDraggingScrubber = false;
                evt.StopPropagation();
            }
        }

        private void OnTimelineMouseLeave(MouseLeaveEvent evt)
        {
            _isDraggingScrubber = false;
        }

        /// <summary>
        /// 更新时间轴拖动线位置
        /// </summary>
        private void UpdateScrubberPosition(float mouseX)
        {
            if (_timelineScrubber == null || _scrubberExtension == null)
                return;

            var timeLabel = _timelineScrubber.Q<Label>();
            if (timeLabel == null)
                return;

            float effectiveWidth = _timelineDuration * _pixelsPerSecond * _currentZoom;
            float clampedX = Mathf.Clamp(mouseX, 0, effectiveWidth);
            float rawTime = clampedX / (_pixelsPerSecond * _currentZoom);
            int scaleSteps = Mathf.RoundToInt(rawTime / _scaleInterval);
            _scrubberTime = scaleSteps * _scaleInterval;
            _scrubberTime = Mathf.Clamp(_scrubberTime, 0f, _timelineDuration);
            float alignedX = _scrubberTime * _pixelsPerSecond * _currentZoom;
            _timelineScrubber.style.left = alignedX + 50;
            _scrubberExtension.style.left = alignedX + 50;
            timeLabel.text = $"{_scrubberTime:F2}s";

        }

        /// <summary>
        /// 更新时间轴拖动线高度（当轨道数量改变时调用）
        /// </summary>
        private void UpdateScrubberHeight()
        {
            // 由于使用了 bottom = 0，拖动线会自动填满容器高度
            // 此方法保留用于未来可能需要的自定义高度调整
            // 主拖动线会自动填满 timelineContainer 的高度
            // 延伸线会自动填满 mainPanel 的高度
        }
        private VisualElement GetTrackUnderMouse( Vector2 mousePosition )
        {
            if ( _trackPanel == null || _root?.panel == null )
                return null;

            try
            {
                var picked = _trackPanel.panel.Pick( mousePosition );
                var candidate = picked;
                while ( candidate != null && candidate != _root )
                {
                    if ( candidate.parent == _trackPanel )
                        return candidate;

                    candidate = candidate.parent;
                }
            }
            catch(Exception exp)
            {
                Debug.LogError($"{exp.ToString()}");
                return null;
            }
            return null;
        }

        /// <summary>
        /// 删除指定位置的轨道
        /// </summary>
        private void DeleteTrackAtPosition( VisualElement trackElementToDelete )
        {
            if ( trackElementToDelete == null )
            {
                Debug.LogWarning( "DeleteTrackAtPosition: no track under mouse position." );
                return;
            }

            var trackToDelete = trackElementToDelete.userData as TimelineTrack;
            if ( trackToDelete != null && _timelineTracks != null && _timelineTracks.Contains( trackToDelete ) )
                _timelineTracks.Remove( trackToDelete );

            if ( _trackPanel != null )
                _trackPanel.Remove( trackElementToDelete );

            if ( trackElementToDelete == _selectedTrackElement )
            {
                _selectedTrackElement = null;
                _selectedTrack = null;
            }

            Debug.Log( $"DeleteTrackAtPosition: deleted track '{trackToDelete?.Name ?? "<unknown>"}'" );

            // 检查 duration 是否有效
            if ( _durationTextField != null &&
                float.TryParse( _durationTextField.value, out float duration ) &&
                duration > 0 )
            {
                Debug.Log( "DeleteTrackAtPosition: Regenerating timeline tracks..." );
                GenerateTimelineTracks();
            }
        }

        /// <summary>
        /// 添加新轨道
        /// </summary>
        private void AddNewTrack()
        {
            var newTrack = new TimelineTrack( $"Track_{_timelineTracks.Count + 1}", Color.gray, true );
            _timelineTracks.Add( newTrack );
            CreateTrackVisualElement( newTrack );

            Debug.Log($"AddNewTrack: Added '{newTrack.Name}'");

            // 如果时间轴已生成，重新生成以更新显示
            if (_timelineTrackPanel != null && _timelineTrackPanel.childCount > 0)
            {
                // 检查 duration 是否有效
                if (_durationTextField != null &&
                    float.TryParse(_durationTextField.value, out float duration) &&
                    duration > 0)
                {
                    Debug.Log("AddNewTrack: Regenerating timeline tracks...");
                    GenerateTimelineTracks();
                }
            }
        }

        private void CreateTrackVisualElement( TimelineTrack track )
        {
            var trackElement = VisualElementFactory.GenTrack( track.TrackColor );
            var trackLabel = VisualElementFactory.GenTrackNameLabel( track.Name );
            trackElement.Add( trackLabel );
            trackElement.userData = track;
            trackElement.RegisterCallback<MouseDownEvent>( evt =>
            {
                if ( evt.button == 0 )
                    SelectTrack( trackElement );

            } );
            _trackPanel.Add( trackElement );
        }

        private void UnSelectTrack( VisualElement trackElement )
        {
            if ( _selectedTrackElement == null)
                return;

            var trackData = _selectedTrackElement.userData as TimelineTrack;
            if ( trackData == null )
                return;

            _selectedTrackElement.style.borderLeftWidth = 0;
            _selectedTrackElement.style.borderRightWidth = 0;
            _selectedTrackElement.style.borderTopWidth = 0;
            _selectedTrackElement.style.borderBottomWidth = 0;
            _selectedTrackElement.style.borderLeftColor = Color.clear;
            _selectedTrackElement.style.borderRightColor = Color.clear;
            _selectedTrackElement.style.borderTopColor = Color.clear;
            _selectedTrackElement.style.borderBottomColor = Color.clear;
            trackElement.style.backgroundColor = trackData.TrackColor;
            _selectedTrackElement = null;
            _selectedTrack = null;
            Debug.Log( $"Unselected track: {trackData.Name}" );
        }

        private void SelectTrack( VisualElement trackElement )
        {
            UnSelectTrack( _selectedTrackElement );
            var track = trackElement.userData as TimelineTrack;
            _selectedTrackElement = trackElement;
            _selectedTrack = track;

            _selectedTrackElement.style.borderLeftWidth   = 1;
            _selectedTrackElement.style.borderRightWidth  = 1;
            _selectedTrackElement.style.borderTopWidth    = 1;
            _selectedTrackElement.style.borderBottomWidth = 1;
            _selectedTrackElement.style.borderLeftColor   = Color.white;
            _selectedTrackElement.style.borderRightColor  = Color.white;
            _selectedTrackElement.style.borderTopColor    = Color.white;
            _selectedTrackElement.style.borderBottomColor = Color.white;

            Color highlightColor = track.TrackColor * 1.3f;
            highlightColor.a = 1f;
            _selectedTrackElement.style.backgroundColor = highlightColor;
            Debug.Log( $"Selected track: {_selectedTrack.Name}" );
        }

        /// <summary>
        /// 注册时间轴面板的鼠标滚轮事件
        /// </summary>
        private void RegisterTimelineZoomControl( VisualElement scrollView )
        {
            scrollView.RegisterCallback<WheelEvent>( evt =>
            {
                if ( evt.ctrlKey )
                {
                    evt.StopPropagation();

                    float zoomDelta = -evt.delta.y * 0.001f;
                    float newZoom = Mathf.Clamp( _currentZoom + zoomDelta, 0.2f, 2.0f );

                    if ( Mathf.Abs( newZoom - _currentZoom ) > 0.01f )
                    {
                        _currentZoom = newZoom;
                        GenerateTimelineTracks();
                        Debug.Log( $"Zoom changed: {_currentZoom:F2}x ({_currentZoom * 100:F0}%)" );
                    }
                }
            } );
        }

        /// <summary>
        /// 生成时间轴轨道
        /// </summary>
        private void GenerateTimelineTracks()
        {
            if ( _timelineTrackPanel == null )
            {
                Debug.LogError( "GenerateTimelineTracks: _timelineTrackPanel is null!" );
                return;
            }

            if ( !float.TryParse( _durationTextField.value, out float duration ) || duration <= 0 )
            {
                ShowNotification( new GUIContent( "GenerateTimelineTracks: Invalid duration value" ) );
                return;
            }

            _timelineTrackPanel.Clear();
            const float scaleInterval = 0.1f;
            float pixelsPerSecond = 100f * _currentZoom;
            _pixelsPerSecond = pixelsPerSecond; // 更新像素每秒比例
            _timelineDuration = duration; // 更新时间轴时长
            int totalScales = Mathf.CeilToInt( duration / scaleInterval );
            float totalWidth = duration * pixelsPerSecond;
            const float trackHeight = 40f;
            const float scaleHeight = 30f;

            var scrollView = new ScrollView( ScrollViewMode.Horizontal ) { style = { flexGrow = 1 } };
            RegisterTimelineZoomControl( scrollView );
            
            var timelineContainer = new VisualElement
            {
                name = "TimelineContainer",
                style =
                {
                    flexDirection = FlexDirection.Column,
                    width = totalWidth + 50,
                    position = Position.Relative // 重要：需要相对定位来放置拖动线
                }
            };

            var scaleContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    height = scaleHeight,
                    marginBottom = 5,
                    marginLeft = 50,
                    position = Position.Relative
                }
            };

            for ( int i = 0; i <= totalScales; i++ )
            {
                float timeValue = i * scaleInterval;
                bool isMajorScale = i % 10 == 0;
                float leftPos = i * ( pixelsPerSecond * scaleInterval );

                var scaleMark = VisualElementFactory.GenScaleMark( leftPos, isMajorScale );
                scaleContainer.Add( scaleMark );

                if ( isMajorScale )
                {
                    var scaleLabel = VisualElementFactory.GenScaleLabel( timeValue.ToString( "F1" ) + "s", leftPos - 15 );
                    scaleContainer.Add( scaleLabel );
                }
            }

            timelineContainer.Add( scaleContainer );
            foreach ( var track in _timelineTracks )
            {
                if ( !track.IsEnabled )
                    continue;

                var trackRow = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        height = trackHeight,
                        marginBottom = 2
                    }
                };

                var trackNameLabel = new Label( track.Name )
                {
                    style =
                    {
                        width = 50,
                        unityTextAlign = TextAnchor.MiddleLeft,
                        fontSize = 11,
                        color = Color.white,
                        paddingLeft = 5
                    }
                };

                var trackTimeline = VisualElementFactory.GenTrackTimeline( track.TrackColor, totalWidth, trackHeight );
                trackTimeline.userData = track;

                trackRow.Add( trackNameLabel );
                trackRow.Add( trackTimeline );
                timelineContainer.Add( trackRow );
            }

            scrollView.Add( timelineContainer );
            _timelineTrackPanel.Add( scrollView );

            // 注册拖动线事件并更新其高度
            RegisterScrubberEvents( timelineContainer );
            UpdateScrubberHeight();

            Debug.Log( $"Generated timeline tracks: Duration={duration}s, Tracks={_timelineTracks.Count}, TotalWidth={totalWidth}px, Zoom={_currentZoom:F2}" );
        }

        /// <summary>
        /// 检查拖入的对象中是否包含 AbilityData
        /// </summary>
        private bool HasAbilityDataInDrag()
        {
            foreach ( var obj in DragAndDrop.objectReferences )
            {
                if ( obj is AbilityData )
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 当 AbilityData 改变时调用
        /// </summary>
        private void OnAbilityDataChanged( AbilityData data )
        {
            if ( data is null )
                return;

            Debug.Log( $"load ability data : {data.name} (ID: {data.Id})" );

            // 更新 UI 显示
            var label = _root.Q<Label>( "currentDataLabel" );
            if ( label != null )
            {
                label.text = $"当前: {data.name}";
                label.style.color = Color.white;
            }

            PopulateUIWithData( data );
        }

        /// <summary>
        /// 将 AbilityData 填充到 UI 控件
        /// </summary>
        private void PopulateUIWithData( AbilityData data )
        {
            _abilityIDTextField.value = data.Id.ToString();
            _abilityDescTextField.value = data.Desc ?? string.Empty;
            _costIDTextField.value = data.CostEffectID.ToString();
            _coolDownIDTextField.value = data.CoolDownEffectID.ToString();
            _timelineIDTextField.value = data.TimelineID.ToString();
            _targetTypeDropdown.value = data.TargetType.ToString();
        }

        //ui controls
        private TextField _abilityIDTextField;
        private TextField _abilityDescTextField;
        private TextField _costIDTextField;
        private TextField _coolDownIDTextField;
        private TextField _timelineIDTextField;
        private DropdownField _targetTypeDropdown;
        private TextField _durationTextField;
        [SerializeField] private VisualTreeAsset _abilityTreeAsset;

        //ability datad
        private AbilityData _currentAbilityData;

        //ui elements
        private VisualElement _root;
        private VisualElement _trackPanel;
        private List<TimelineTrack> _timelineTracks;
        private VisualElement _selectedTrackElement;
        private TimelineTrack _selectedTrack;
        private VisualElement _timelineTrackPanel;

        /// <summary>
        /// 获取当前时间轴位置（秒）
        /// </summary>
        /// <returns>当前时间轴位置</returns>
        public float GetCurrentScrubberTime()
        {
            return _scrubberTime;
        }
        
        /// <summary>
        /// 设置时间轴位置（会自动对齐到刻度）
        /// </summary>
        /// <param name="time">要设置的时间（秒）</param>
        public void SetScrubberTime(float time)
        {
            if (_timelineScrubber == null || _scrubberExtension == null)
                return;
                
            // 对齐到最近的刻度
            int scaleSteps = Mathf.RoundToInt(time / _scaleInterval);
            _scrubberTime = Mathf.Clamp(scaleSteps * _scaleInterval, 0f, _timelineDuration);
            
            // 更新位置
            float alignedX = _scrubberTime * _pixelsPerSecond * _currentZoom;
            UpdateScrubberPosition(alignedX);
        }

        private float _currentZoom = 1.0f;
        
        // Timeline Scrubber Line properties
        private VisualElement _timelineScrubber;
        private VisualElement _scrubberExtension; // 延伸到面板底部的部分
        private bool _isDraggingScrubber = false;
        private float _scrubberTime = 0f; // 当前时间轴位置（秒）
        private float _pixelsPerSecond = 100f; // 像素每秒比例
        private float _timelineDuration = 5f; // 时间轴总时长
        private const float _scaleInterval = 0.1f; // 刻度间隔（秒）
        private VisualElement _mainPanel; // 主面板引用
    }
}
