using Aquila.AbilityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
            wnd.Show();
        }

        public void CreateGUI()
        {
            _abilityTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>( Misc.UXML_FILE_PATH );
            if ( _abilityTreeAsset is null )
            {
                Debug.LogError( $"AbilityEditorWindow.cs ---> CreateGUI() ---> _abilityTreeAsset is null" );
                return;
            }

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
            _timelineTrackItems = new List<TimelineTrackItem>();
            InitializeClipManager();

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
            CreateTimelineScrubber();
        }


        private void RegisterDragAndDropCallbacks()
        {
            _root.RegisterCallback<DragEnterEvent>( _ =>
            {
                DragAndDrop.visualMode = HasAbilityDataInDrag() ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
            } );

            _root.RegisterCallback<DragUpdatedEvent>( _ =>
            {
                DragAndDrop.visualMode = HasAbilityDataInDrag() ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
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

            var trackToDelete = trackElementToDelete.userData as TimelineTrackItem;
            if ( trackToDelete != null && _timelineTrackItems != null && _timelineTrackItems.Contains( trackToDelete ) )
                _timelineTrackItems.Remove( trackToDelete );

            if ( _trackPanel != null )
                _trackPanel.Remove( trackElementToDelete );

            if ( trackElementToDelete == _selectedTrackElement )
            {
                _selectedTrackElement = null;
                _selectedTrackItem = null;
            }

            Debug.Log( $"DeleteTrackAtPosition: deleted track '{trackToDelete?.Name ?? "<unknown>"}'" );

            // 检查 duration 是否有效
            if ( _durationTextField != null &&
                float.TryParse( _durationTextField.value, out float duration ) &&
                duration > 0 )
            {
                Debug.Log( "DeleteTrackAtPosition: Regenerating timeline track items..." );
                DrawTimelineTrackItems();
            }
        }

        /// <summary>
        /// 添加新轨道
        /// </summary>
        private void AddNewTrack()
        {
            var newTrackItem = new TimelineTrackItem
                (
                    $"Track_{_timelineTrackItems.Count + 1}",
                    Misc.GetTrackColor( _timelineTrackItems.Count ),
                    true
                );
            _timelineTrackItems.Add( newTrackItem );
            DrawTrackItemElement( newTrackItem );

            // 如果时间轴已生成，重新生成以更新显示
            if (_timelineTrackPanel != null && _timelineTrackPanel.childCount > 0)
            {
                if (_durationTextField != null &&
                    float.TryParse(_durationTextField.value, out float duration) &&
                    duration > 0)
                {
                    Debug.Log("AddNewTrack: Regenerating timeline track items...");
                    DrawTimelineTrackItems();
                }
            }
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

            UpdateUIFromData( data );
            LoadTracksFromAbilityData( data );
        }

        /// <summary>
        /// 保存编辑器状态到 AbilityData
        /// </summary>
        private bool SaveToAbilityData()
        {
            if (_currentAbilityData == null)
            {
                Debug.LogError("Cannot save: No AbilityData loaded");
                return false;
            }

            try
            {
                // 同步基本信息字段
                if (_abilityIDTextField != null && int.TryParse(_abilityIDTextField.value, out int abilityID))
                    _currentAbilityData.Id = abilityID;

                if (_abilityDescTextField != null)
                    _currentAbilityData.Desc = _abilityDescTextField.value;

                if (_costIDTextField != null && int.TryParse(_costIDTextField.value, out int costID))
                    _currentAbilityData.CostEffectID = costID;

                if (_coolDownIDTextField != null && int.TryParse(_coolDownIDTextField.value, out int coolDownID))
                    _currentAbilityData.CoolDownEffectID = coolDownID;

                if (_timelineIDTextField != null && int.TryParse(_timelineIDTextField.value, out int timelineID))
                    _currentAbilityData.TimelineID = timelineID;

                if (_timelineAssetPathTxtField != null)
                    _currentAbilityData.TimelineAssetPath = _timelineAssetPathTxtField.value;

                if (_targetTypeDropdown != null && System.Enum.TryParse<Cfg.Enum.AbilityTargetType>(_targetTypeDropdown.value, out var targetType))
                    _currentAbilityData.TargetType = targetType;

                // 同步 Timeline 时长
                _currentAbilityData.TimelineDuration = _timelineDuration;

                // 创建 SerializedTrackData 列表
                var tracks = new List<SerializedTrackData>();
                if (_timelineTrackItems != null)
                {
                    foreach (var trackItem in _timelineTrackItems)
                    {
                        if (trackItem != null)
                        {
                            var serializedTrack = new SerializedTrackData(trackItem);
                            tracks.Add(serializedTrack);
                        }
                    }
                }

                // 设置 Tracks
                _currentAbilityData.SetTracks(tracks);

                // 验证数据
                if (!_currentAbilityData.Validate(out string error))
                    Debug.LogWarning($"Validation warning: {error}. Saved anyway.");

                // 标记为脏并保存
                UnityEditor.EditorUtility.SetDirty(_currentAbilityData);
                UnityEditor.AssetDatabase.SaveAssets();

                Debug.Log($"Saved AbilityData: {_currentAbilityData.name} ({tracks.Count} tracks)");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save AbilityData: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 从 AbilityData 加载 Tracks 到编辑器
        /// </summary>
        private void LoadTracksFromAbilityData(AbilityData data)
        {
            if (data == null)
                return;

            // 检查是否有 Tracks 数据
            if (data.Tracks == null || data.Tracks.Count == 0)
            {
                Debug.Log($"AbilityData '{data.name}' has no tracks. Timeline is empty.");
                return;
            }

            // 清空当前 Timeline 状态
            ClearAllTracks();

            // 恢复 Timeline 时长
            _timelineDuration = data.TimelineDuration;
            if (_durationTextField != null)
            {
                _durationTextField.value = _timelineDuration.ToString("F2");
            }

            // 恢复 Tracks
            foreach (var serializedTrack in data.Tracks)
            {
                if (serializedTrack == null)
                    continue;

                try
                {
                    // 转换为运行时对象
                    var trackItem = serializedTrack.ToTrackItem();

                    // 添加到列表
                    if (_timelineTrackItems == null)
                        _timelineTrackItems = new List<TimelineTrackItem>();

                    _timelineTrackItems.Add(trackItem);

                    // 创建 Track UI 元素
                    DrawTrackItemElement(trackItem);

                    Debug.Log($"Loaded track: {trackItem.Name} with {trackItem.Clips.Count} clips");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to load track '{serializedTrack.TrackName}': {ex.Message}");
                }
            }

            // 如果有有效的时长，重新生成时间轴
            if (_durationTextField != null &&
                float.TryParse(_durationTextField.value, out float duration) &&
                duration > 0)
            {
                Debug.Log($"LoadTracksFromAbilityData: Regenerating timeline with {data.Tracks.Count} tracks...");
                DrawTimelineTrackItems();
            }

            Debug.Log($"Loaded {data.Tracks.Count} tracks from AbilityData");
        }

        /// <summary>
        /// 清空所有 Tracks
        /// </summary>
        private void ClearAllTracks()
        {
            // 清空数据列表
            if (_timelineTrackItems != null)
            {
                _timelineTrackItems.Clear();
            }

            // 清空 TrackPanel UI（左侧轨道列表）
            if (_trackPanel != null)
            {
                // 移除所有子元素（除了标题 Label）
                var children = _trackPanel.Children().ToList();
                foreach (var child in children)
                {
                    // 保留 "Tracks" 标题 Label
                    if (child is Label label && label.text == "Tracks")
                        continue;

                    _trackPanel.Remove(child);
                }
            }

            // 清空 TimelineTrackPanel UI（右侧时间轴）
            if (_timelineContainer != null)
            {
                _timelineContainer.Clear();
            }

            // 清空选中状态
            _selectedTrackElement = null;
            _selectedTrackItem = null;

            Debug.Log("ClearAllTracks: Cleared all tracks and UI elements");
        }

        /// <summary>
        /// Save 按钮点击事件处理
        /// </summary>
        private void OnSaveButtonClicked()
        {
            if (SaveToAbilityData())
                ShowNotification(new GUIContent("✓ Saved to AbilityData"));
            else
                ShowNotification(new GUIContent("✗ Save failed. Check console."));
        }

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
            if (_timelineScrubber == null)
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
        private VisualElement _timelineScrubber; // 时间轴拖动线（延伸到 TimelineTrackPanel 容器内）
        private ScrollView _timelineScrollView; // 时间轴滚动视图（从 UXML 中获取）
        private VisualElement _timelineContainer; // 时间轴容器（从 UXML 中获取）
        private bool _isDraggingScrubber = false;
        private float _scrubberTime = 0f; // 当前时间轴位置（秒）
        private float _pixelsPerSecond = 100f; // 像素每秒比例
        private float _timelineDuration = 5f; // 时间轴总时长
        private const float _scaleInterval = 0.1f; // 刻度间隔（秒）

        //ui controls
        private TextField _abilityIDTextField;
        private TextField _abilityDescTextField;
        private TextField _costIDTextField;
        private TextField _coolDownIDTextField;
        private TextField _timelineIDTextField;
        private TextField _timelineAssetPathTxtField;
        private DropdownField _targetTypeDropdown;
        private TextField _durationTextField;
        private VisualTreeAsset _abilityTreeAsset;

        //ability datad
        private AbilityData _currentAbilityData;

        //ui elements
        private VisualElement _root;
        private VisualElement _trackPanel;
        private List<TimelineTrackItem> _timelineTrackItems;
        private VisualElement _selectedTrackElement;
        private TimelineTrackItem _selectedTrackItem;
        private VisualElement _timelineTrackPanel;
    }
}
