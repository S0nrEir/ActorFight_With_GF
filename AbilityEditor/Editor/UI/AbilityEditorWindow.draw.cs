using Aquila.AbilityEditor;
using Cfg.Enum;
using Editor.AbilityEditor.Config;
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
            var tempPanel = _root.Q<VisualElement>( "BaseArea_1" );
            if ( tempPanel != null )
            {
                _abilityIDTextField   = tempPanel.Q<TextField>( "AbilityIDTxtField" );
                _abilityDescTextField = tempPanel.Q<TextField>( "AbilityDescTxtField" );
                _costIDTextField      = tempPanel.Q<TextField>( "CostIDTxtField" );
                _coolDownIDTextField  = tempPanel.Q<TextField>( "CoolDownIDTxtField" );
            }
            
            tempPanel = _root.Q<VisualElement>( "BaseArea_2" );
            if (tempPanel != null)
            {
                _timelineIDTextField       = tempPanel.Q<TextField>( "TimelineIDTxtField" );
                _timelineAssetPathTxtField = tempPanel.Q<TextField>( "TimelineAssetPathTxtField");
                _targetTypeDropdown        = tempPanel.Q<DropdownField>( "TargetTypeDropdown" );
                _durationTextField         = tempPanel.Q<TextField>( "DurationTxtField" );
            }

            tempPanel = _root.Q<VisualElement>( "BaseArea_3" );
            if (tempPanel != null)
            {
                var tempBtn = tempPanel.Q<Button>( "GenTimelineTrackBtn" );
                if ( tempBtn != null )
                    tempBtn.clicked += DrawTimelineTrackItems;

                // tempBtn = tempPanel.Q<Button>( "SaveBtn" );
                // if ( tempBtn != null )
                //     tempBtn.clicked += OnSaveButtonClicked;

                tempBtn = tempPanel.Q<Button>( "GenConfigBtn" );
                if ( tempBtn != null )
                    tempBtn.clicked += OnClickGenConfigBtn;
            }

            tempPanel = _root.Q<VisualElement>( "HeaderMenu" );
            if ( tempPanel != null )
            {
                var tempBtn = tempPanel.Q<Button>( "AddNewTrackBtn" );
                tempBtn.clicked += AddNewTrack;
            }
            
            tempPanel = _root.Q<VisualElement>( "AbilityEditPanel" );
            if ( tempPanel != null )
            {
                _trackPanel         = tempPanel.Q<VisualElement>( "TrackPanel" );
                _timelineTrackPanel = tempPanel.Q<VisualElement>( "TimelineTrackPanel" );
            } 
            
            if ( _abilityIDTextField == null ||
                _abilityDescTextField == null ||
                _costIDTextField == null ||
                _coolDownIDTextField == null ||
                _timelineIDTextField == null ||
                _timelineAssetPathTxtField == null ||
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

            // 获取并缓存 ScrollView 和 TimelineContainer
            _timelineScrollView = _root.Q<ScrollView>( "TimelineScrollView" );
            _timelineContainer = _root.Q<VisualElement>( "TimelineContainer" );

            if ( _timelineScrollView != null )
            {
                // 设置滚动条样式
                _timelineScrollView.horizontalScroller.style.height = 16;
                // 注册滚动控制事件
                RegisterHorizontalScrollControl( _timelineScrollView );
            }
            else
            {
                Debug.LogError( "InitializeUIElements: TimelineScrollView not found in UXML!" );
            }

            if ( _timelineContainer == null )
                Debug.LogError( "InitializeUIElements: TimelineContainer not found in UXML!" );
        }

        /// <summary>
        /// 创建并绘制单个轨道的VisualElement
        /// </summary>
        private void DrawTrackItemElement( TimelineTrackItem track )
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

            var trackData = _selectedTrackElement.userData as TimelineTrackItem;
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
            _selectedTrackItem = null;
        }

        /// <summary>
        /// 高亮显示选中的轨道
        /// </summary>
        private void HighlightTrackSelection( VisualElement trackElement )
        {
            ClearTrackSelection( _selectedTrackElement );
            var track = trackElement.userData as TimelineTrackItem;
            _selectedTrackElement = trackElement;
            _selectedTrackItem = track;

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

            // 绑定到Inspector
            ShowTrackInInspector(track, trackElement);

            Debug.Log( $"Selected track: {_selectedTrackItem.Name}" );
        }

        /// <summary>
        /// 在Inspector中显示Track信息
        /// </summary>
        private void ShowTrackInInspector(TimelineTrackItem track, VisualElement trackElement)
        {
            if (track == null)
                return;

            if (_trackInspectorProxy == null)
                _trackInspectorProxy = ScriptableObject.CreateInstance<Aquila.AbilityEditor.TrackInspectorProxy>();

            _trackInspectorProxy.BindTrackData(track, trackElement, OnTrackNameChanged);
            Selection.activeObject = _trackInspectorProxy;
        }

        /// <summary>
        /// Track名称变更回调
        /// </summary>
        private void OnTrackNameChanged(TimelineTrackItem track, string newName)
        {
            if (_selectedTrackElement == null)
                return;

            // 更新TrackPanel中的Label
            var trackLabel = _selectedTrackElement.Q<Label>();
            if (trackLabel != null)
                trackLabel.text = newName;

            // 如果时间轴已生成，重新绘制以更新Timeline中的名称
            if (_timelineContainer != null && _timelineContainer.childCount > 0)
            {
                if (_durationTextField != null &&
                    float.TryParse(_durationTextField.value, out float duration) &&
                    duration > 0)
                {
                    DrawTimelineTrackItems();
                }
            }

            Debug.Log($"Track name changed to: {newName}");
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
                        DrawTimelineTrackItems();
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
                    //evt.PreventDefault();

                    // 获取当前的横向滚动偏移（使用 scrollOffset 而不是 scroller.value）
                    float currentScrollX = scrollView.scrollOffset.x;

                    // 滚轮增量转换为滚动距离（增大系数使滚动更流畅）
                    float scrollDelta = evt.delta.y * 20f;

                    // 计算新的滚动位置
                    float newScrollX = currentScrollX + scrollDelta;

                    // 限制滚动范围，确保不会超出内容边界
                    float minValue = scrollView.horizontalScroller.lowValue;
                    float maxValue = scrollView.horizontalScroller.highValue;
                    newScrollX = Mathf.Clamp( newScrollX, minValue, maxValue );

                    // 使用 scrollOffset 设置新的滚动位置（与拖动滚动条的方式一致）
                    scrollView.scrollOffset = new Vector2( newScrollX, scrollView.scrollOffset.y );
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

        // 点击生成配置
        private void OnClickGenConfigBtn()
        {
            Debug.Log("[AbilityEditorWindow] 开始生成配置...");
            var config = AbilityConfigGenerator.Generate(this);
            AbilityConfigAccessor.SetConfig(config);
            AbilityDataExporter.ExportToAsset(config, _timelineTrackItems);
            ShowNotification(new GUIContent($"✓ 配置已生成并保存 (ID: {config.AbilityID})"));

            Debug.Log($"[AbilityEditorWindow] 配置生成完成:\n{AbilityConfigAccessor.ToString()}");
        }

        /// <summary>
        /// 绘制时间轴轨道
        /// </summary>
        private void DrawTimelineTrackItems()
        {
            if ( _timelineScrollView == null || _timelineContainer == null )
            {
                Debug.LogError( "DrawTimelineTrackItems: _timelineScrollView or _timelineContainer is null!" );
                return;
            }

            if ( !float.TryParse( _durationTextField.value, out float duration ) || duration <= 0 )
            {
                ShowNotification( new GUIContent( "DrawTimelineTrackItems: Invalid duration value" ) );
                return;
            }

            // 清空 TimelineContainer 的内容
            _timelineContainer.Clear();

            const float scaleInterval = 0.1f;
            float pixelsPerSecond = 100f * _currentZoom;
            _pixelsPerSecond = pixelsPerSecond; // 更新像素每秒比例
            _timelineDuration = duration; // 更新时间轴时长
            int totalScales = Mathf.CeilToInt( duration / scaleInterval );
            float totalWidth = duration * pixelsPerSecond;
            const float trackHeight = 40f;
            const float scaleHeight = 30f;

            // 更新 TimelineContainer 的宽度样式
            _timelineContainer.style.width = totalWidth + 50;
            _timelineContainer.style.minWidth = totalWidth + 50;

            // 使用VisualElementFactory创建刻度容器
            var scaleContainer = VisualElementFactory.GenScaleContainer(totalWidth, scaleHeight);

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

            _timelineContainer.Add( scaleContainer );
            foreach ( var track in _timelineTrackItems )
            {
                if ( !track.IsEnabled )
                    continue;

                // 使用VisualElementFactory创建轨道行
                var trackRow = VisualElementFactory.GenTrackRow(totalWidth, trackHeight);

                // 使用VisualElementFactory创建轨道名称标签
                //var trackNameLabel = VisualElementFactory.GenTrackNameLabelForTimeline(track.Name);

                var trackTimeline = VisualElementFactory.GenTrackTimeline( track.TrackColor, totalWidth, trackHeight );
                trackTimeline.userData = track;

                //trackRow.Add( trackNameLabel );
                trackRow.Add( trackTimeline );
                _timelineContainer.Add( trackRow );

                RegisterTrackItemToClipManager( track, trackTimeline );
            }

            // 更新clip管理器的timeline参数
            UpdateClipManagerTimelineParams();

            // 刷新所有clips UI（重新创建clips，因为timeline被清空了）
            RefreshAllClipsUI();

            // 注册拖动线事件并更新其高度
            RegisterScrubberEvents( _timelineContainer );
            UpdateScrubberHeight();

            Debug.Log( $"Generated timeline track items: Duration={duration}s, Tracks={_timelineTrackItems.Count}, TotalWidth={totalWidth}px, Zoom={_currentZoom:F2}" );
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
            _timelineAssetPathTxtField.value = data.TimelineAssetPath ?? string.Empty;
            _targetTypeDropdown.value = data.TargetType.ToString();
        }

        /// <summary>
        /// 创建时间轴拖动线（用于显示时间位置）
        /// </summary>
        private void CreateTimelineScrubber()
        {
            // 使用VisualElementFactory创建拖动线
            _timelineScrubber = VisualElementFactory.GenTimelineScrubber();

            // 使用VisualElementFactory创建时间显示标签
            var timeLabel = VisualElementFactory.GenScrubberTimeLabel();

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
