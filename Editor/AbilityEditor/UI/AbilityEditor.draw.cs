using Aquila.AbilityEditor;
using Cfg.Enum;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// AbilityEditorWindow 的 UI 绘制部分
    /// 包含所有与 UI 可视化、样式、布局相关的方法
    /// </summary>
    public partial class AbilityEditorWindow
    {
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
                    genTimelineTrackButton.clicked += DrawTimelineTracks;
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

            // 设置 TimelineTrackPanel 样式，禁用竖向滚动
            if ( _timelineTrackPanel != null )
            {
                _timelineTrackPanel.style.overflow = Overflow.Hidden;
                _timelineTrackPanel.style.flexGrow = 1;
                _timelineTrackPanel.style.flexShrink = 1;
            }
        }

        /// <summary>
        /// 创建并绘制单个轨道的可视化元素
        /// </summary>
        private void DrawTrackElement( TimelineTrack track )
        {
            var trackElement = VisualElementFactory.GenTrack( track.TrackColor );
            var trackLabel = VisualElementFactory.GenTrackNameLabel( track.Name );
            trackElement.Add( trackLabel );
            trackElement.userData = track;
            trackElement.RegisterCallback<MouseDownEvent>( evt =>
            {
                if ( evt.button == 0 )
                    HighlightTrackSelection( trackElement );

            } );
            _trackPanel.Add( trackElement );
        }

        /// <summary>
        /// 清除当前轨道的选中状态
        /// </summary>
        private void ClearTrackSelection( VisualElement trackElement )
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

        /// <summary>
        /// 高亮显示选中的轨道
        /// </summary>
        private void HighlightTrackSelection( VisualElement trackElement )
        {
            ClearTrackSelection( _selectedTrackElement );
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
        /// 注册时间轴面板的鼠标滚轮缩放控制
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
                        DrawTimelineTracks();
                        Debug.Log( $"Zoom changed: {_currentZoom:F2}x ({_currentZoom * 100:F0}%)" );
                    }
                }
            } );
        }

        /// <summary>
        /// 注册横向滚动控制（鼠标滚轮横向滚动）
        /// </summary>
        private void RegisterHorizontalScrollControl( ScrollView scrollView )
        {
            scrollView.RegisterCallback<WheelEvent>( evt =>
            {
                // 如果没有按 Ctrl 键，则执行横向滚动
                if ( !evt.ctrlKey )
                {
                    evt.StopPropagation();

                    // 获取当前的横向滚动偏移
                    float scrollOffset = scrollView.horizontalScroller.value;

                    // 滚轮增量转换为滚动距离（增大系数使滚动更流畅）
                    float scrollDelta = evt.delta.y * 20f;

                    // 计算新的滚动位置并限制在有效范围内
                    float newScrollValue = scrollOffset + scrollDelta;
                    float minValue = scrollView.horizontalScroller.lowValue;
                    float maxValue = scrollView.horizontalScroller.highValue;

                    // 限制滚动范围，确保不会超出内容边界
                    scrollView.horizontalScroller.value = Mathf.Clamp( newScrollValue, minValue, maxValue );
                }
            } );
        }

        /// <summary>
        /// 注册鼠标拖动滚动控制（中键或 Shift + 左键拖动）
        /// </summary>
        private void RegisterDragScrollControl( ScrollView scrollView )
        {
            bool isDragging = false;
            float dragStartScrollPos = 0f;
            Vector2 dragStartMousePos = Vector2.zero;

            scrollView.RegisterCallback<MouseDownEvent>( evt =>
            {
                // 鼠标中键或 Shift + 左键开始拖动
                if ( evt.button == 2 || ( evt.button == 0 && evt.shiftKey ) )
                {
                    isDragging = true;
                    dragStartScrollPos = scrollView.horizontalScroller.value;
                    dragStartMousePos = evt.mousePosition;
                    evt.StopPropagation();
                }
            } );

            scrollView.RegisterCallback<MouseMoveEvent>( evt =>
            {
                if ( isDragging )
                {
                    // 计算鼠标移动距离
                    float deltaX = evt.mousePosition.x - dragStartMousePos.x;

                    // 更新滚动位置（反向移动）
                    scrollView.horizontalScroller.value = dragStartScrollPos - deltaX;
                    evt.StopPropagation();
                }
            } );

            scrollView.RegisterCallback<MouseUpEvent>( evt =>
            {
                if ( ( evt.button == 2 || evt.button == 0 ) && isDragging )
                {
                    isDragging = false;
                    evt.StopPropagation();
                }
            } );

            scrollView.RegisterCallback<MouseLeaveEvent>( evt =>
            {
                isDragging = false;
            } );
        }

        /// <summary>
        /// 绘制时间轴轨道
        /// </summary>
        private void DrawTimelineTracks()
        {
            if ( _timelineTrackPanel == null )
            {
                Debug.LogError( "DrawTimelineTracks: _timelineTrackPanel is null!" );
                return;
            }

            if ( !float.TryParse( _durationTextField.value, out float duration ) || duration <= 0 )
            {
                ShowNotification( new GUIContent( "DrawTimelineTracks: Invalid duration value" ) );
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

            // 创建横向滚动视图，支持横向拖动和滚轮滚动
            var scrollView = new ScrollView( ScrollViewMode.Horizontal )
            {
                style =
                {
                    flexGrow = 1,
                    flexShrink = 1,
                    // 禁用竖向滚动，只保留横向滚动
                    overflow = Overflow.Hidden
                }
            };

            // 隐藏竖向滚动条（ScrollView 只需要横向滚动）
            scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

            // 注册缩放控制（Ctrl + 滚轮）
            //RegisterTimelineZoomControl( scrollView );

            // 注册普通滚轮事件（不按 Ctrl 时横向滚动）
            RegisterHorizontalScrollControl( scrollView );

            // 注册鼠标拖动滚动（按住鼠标中键或 Shift + 左键拖动）
            //RegisterDragScrollControl( scrollView );

            var timelineContainer = new VisualElement
            {
                name = "TimelineContainer",
                style =
                {
                    flexDirection = FlexDirection.Column,
                    width = totalWidth + 50,
                    minWidth = totalWidth + 50, // 确保最小宽度
                    position = Position.Relative, // 重要：需要相对定位来放置拖动线
                    overflow = Overflow.Visible // 确保所有子元素可见
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
                    width = totalWidth, // 明确设置宽度，确保刻度在整个时间轴范围内可见
                    minWidth = totalWidth, // 确保最小宽度
                    position = Position.Relative,
                    overflow = Overflow.Visible // 确保绝对定位的子元素可见
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
                        marginBottom = 2,
                        width = totalWidth + 50, // 确保轨道行宽度与容器一致
                        minWidth = totalWidth + 50
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
        /// 将 AbilityData 数据更新到 UI 控件
        /// </summary>
        private void UpdateUIFromData( AbilityData data )
        {
            _abilityIDTextField.value = data.Id.ToString();
            _abilityDescTextField.value = data.Desc ?? string.Empty;
            _costIDTextField.value = data.CostEffectID.ToString();
            _coolDownIDTextField.value = data.CoolDownEffectID.ToString();
            _timelineIDTextField.value = data.TimelineID.ToString();
            _targetTypeDropdown.value = data.TargetType.ToString();
        }

        /// <summary>
        /// 创建时间轴拖动线（用于显示时间位置）
        /// </summary>
        private void CreateTimelineScrubber()
        {
            // 创建拖动线（延伸到 TimelineTrackPanel 容器内）
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
            if (_timelineScrubber == null || timelineContainer == null)
            {
                Debug.LogError("RegisterScrubberEvents: Required elements are null!");
                return;
            }

            // 保存 timelineContainer 的引用，用于后续计算偏移
            _timelineContainer = timelineContainer;

            // 先从旧的父容器中移除拖动线（如果存在）
            if (_timelineScrubber.parent != null)
                _timelineScrubber.RemoveFromHierarchy();

            // 将拖动线添加到 timelineContainer（延伸到 TimelineTrackPanel 容器内）
            timelineContainer.Add(_timelineScrubber);

            var timeLabel = _timelineScrubber.Q<Label>();
            if (timeLabel == null)
            {
                Debug.LogError("RegisterScrubberEvents: timeLabel not found!");
                return;
            }

            // 设置可见性
            _timelineScrubber.style.display = DisplayStyle.Flex;
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

        /// <summary>
        /// 更新时间轴拖动线位置
        /// </summary>
        private void UpdateScrubberPosition(float mouseX)
        {
            if (_timelineScrubber == null || _timelineContainer == null)
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

            // 计算在时间轴上对齐后的位置（相对于 timelineContainer）
            // timelineContainer 内部有 50px 的左边距用于轨道名称
            float alignedX = _scrubberTime * _pixelsPerSecond * _currentZoom;
            _timelineScrubber.style.left = alignedX + 50;
            timeLabel.text = $"{_scrubberTime:F2}s";
        }

        /// <summary>
        /// 更新时间轴拖动线高度（当轨道数量改变时调用）
        /// </summary>
        private void UpdateScrubberHeight()
        {
            // 由于使用了 bottom = 0，拖动线会自动填满 mainPanel 的高度
            // 此方法保留用于未来可能需要的自定义高度调整
        }
    }
}
