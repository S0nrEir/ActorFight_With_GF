using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Timeline Clip的UI表示和交互处理类
    /// 负责clip的绘制、拖动、边界调整等交互
    /// </summary>
    public class TimelineClipUI
    {
        // 拖动状态
        private enum DragMode
        {
            None,
            Move,
            ResizeLeft,
            ResizeRight
        }

        private DragMode _currentDragMode = DragMode.None;
        private float _dragStartMouseX;
        private float _dragStartTime;
        private float _dragStartEndTime;
        private Vector2 _dragStartMousePosition;

        // Timeline参数
        private float _pixelsPerSecond;
        private float _timelineStartTime;
        private float _timelineEndTime;
        private float _zoom;

        // 事件回调
        public event Action<TimelineClipUI> OnClipSelected;
        public event Action<TimelineClipUI> OnClipModified;
        public event Action<TimelineClipUI> OnClipDeleted;

        public TimelineClipData ClipData => _clipData;
        public VisualElement ClipElement => _clipElement;

        public TimelineClipUI(TimelineClipData clipData)
        {
            _clipData = clipData ?? throw new ArgumentNullException(nameof(clipData));
        }

        /// <summary>
        /// 刷新clip的显示（当数据改变时调用）
        /// </summary>
        public void Refresh()
        {
            if ( _clipElement != null )
            {
                _clipElement.style.backgroundColor = _clipData.ClipColor;
                UpdateVisualTransform();
            }
        }

        /// <summary>
        /// 销毁clip的UI元素
        /// </summary>
        public void Destroy()
        {
            // 确保释放鼠标捕获（如果有）
            if ( _clipElement != null && _currentDragMode != DragMode.None )
            {
                _clipElement.ReleaseMouse();
                _currentDragMode = DragMode.None;
            }

            if ( _clipElement != null )
            {
                _clipElement.RemoveFromHierarchy();
                _clipElement = null;
            }

            _clipLabel = null;
            _leftHandle = null;
            _rightHandle = null;
            _startTimeLabel = null;
            _endTimeLabel = null;
        }

        #region UI Creation

        /// <summary>
        /// 创建clip的可视化元素
        /// </summary>
        public VisualElement CreateVisualElement(float pixelsPerSecond, float zoom, float timelineStartTime, float timelineEndTime)
        {
            _pixelsPerSecond = pixelsPerSecond;
            _zoom = zoom;
            _timelineStartTime = timelineStartTime;
            _timelineEndTime = timelineEndTime;

            // 判断是否为即时clip
            bool isInstant = _clipData.IsInstantClip;
            // 使用VisualElementFactory创建clip容器
            _clipElement = VisualElementFactory.GenClipContainer(_clipData.ClipId, _clipData.ClipColor, isInstant);
            _clipElement.userData = this;

            _clipLabel = VisualElementFactory.GenClipLabel(_clipData.ClipName, isInstant);
            // 只为非即时clip创建左右手柄
            if (!isInstant)
            {
                _leftHandle = VisualElementFactory.GenLeftHandle();
                _rightHandle = VisualElementFactory.GenRightHandle();
            }

            // 创建时间标签
            if (isInstant)
            {
                _startTimeLabel = VisualElementFactory.GenInstantClipTimeLabel();
            }
            else
            {
                // 持续时间clip显示开始和结束时间标签
                _startTimeLabel = VisualElementFactory.GenDurationClipStartTimeLabel();
                _endTimeLabel = VisualElementFactory.GenDurationClipEndTimeLabel();
            }
            
            _clipElement.Add(_clipLabel);
            if (_leftHandle != null)
                _clipElement.Add(_leftHandle);
            
            if (_rightHandle != null)
                _clipElement.Add(_rightHandle);
            
            if (_startTimeLabel != null)
                _clipElement.Add(_startTimeLabel);
            
            if (_endTimeLabel != null)
                _clipElement.Add(_endTimeLabel);

            RegisterEvents();
            UpdateVisualTransform();

            return _clipElement;
        }

        #endregion

        //Event Registration

        private void RegisterEvents()
        {
            // 只在手柄上注册 MouseDown 事件（仅当手柄存在时）
            if (_leftHandle != null)
                _leftHandle.RegisterCallback<MouseDownEvent>(OnLeftHandleMouseDown);
            if (_rightHandle != null)
                _rightHandle.RegisterCallback<MouseDownEvent>(OnRightHandleMouseDown);

            // 在 clipElement 上注册所有移动和释放事件
            // 这样即使鼠标离开手柄区域，事件仍然能被处理
            _clipElement.RegisterCallback<MouseDownEvent>(OnClipMouseDown);
            _clipElement.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            _clipElement.RegisterCallback<MouseUpEvent>(OnMouseUp);
            _clipElement.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);

            _clipElement.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));
        }

        //Event Handlers

        private void OnLeftHandleMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0)
            {
                _currentDragMode = DragMode.ResizeLeft;
                _dragStartMouseX = evt.mousePosition.x;
                _dragStartTime = _clipData.StartTime;
                _dragStartEndTime = _clipData.EndTime;
                evt.StopPropagation();

                // 对 clipElement 而非手柄调用 CaptureMouse，确保所有鼠标事件都被捕获
                _clipElement.CaptureMouse();
            }
        }

        private void OnRightHandleMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0)
            {
                _currentDragMode = DragMode.ResizeRight;
                _dragStartMouseX = evt.mousePosition.x;
                _dragStartTime = _clipData.StartTime;
                _dragStartEndTime = _clipData.EndTime;
                evt.StopPropagation();

                // 对 clipElement 而非手柄调用 CaptureMouse，确保所有鼠标事件都被捕获
                _clipElement.CaptureMouse();
            }
        }

        private void OnClipMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0)
            {
                // 如果不是即时clip，检查是否点击在手柄区域，如果是则不处理（让手柄的事件处理）
                if (!_clipData.IsInstantClip && _leftHandle != null && _rightHandle != null)
                {
                    float localX = evt.localMousePosition.x;
                    if (localX <= Misc.HANDLE_WIDTH || localX >= _clipElement.resolvedStyle.width - Misc.HANDLE_WIDTH)
                        return;
                }

                _currentDragMode = DragMode.Move;
                _dragStartMouseX = evt.mousePosition.x;
                _dragStartTime = _clipData.StartTime;
                evt.StopPropagation();
                _clipElement.CaptureMouse();
                OnClipSelected?.Invoke(this);
            }
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (_currentDragMode == DragMode.None)
                return;

            float mouseDeltaX = evt.mousePosition.x - _dragStartMouseX;
            float timeDelta = mouseDeltaX / (_pixelsPerSecond * _zoom);

            switch (_currentDragMode)
            {
                case DragMode.Move:
                    HandleMove(timeDelta);
                    break;
                case DragMode.ResizeLeft:
                    HandleResizeLeft(timeDelta);
                    break;
                case DragMode.ResizeRight:
                    HandleResizeRight(timeDelta);
                    break;
            }

            evt.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button == 0 && _currentDragMode != DragMode.None)
            {
                _currentDragMode = DragMode.None;
                evt.StopPropagation();

                // 只需要释放 clipElement 的鼠标捕获
                _clipElement.ReleaseMouse();

                // 触发修改事件
                OnClipModified?.Invoke(this);
            }
        }

        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            // 如果正在拖动，不要重置状态
            // 因为已经使用了 CaptureMouse，即使鼠标离开元素区域，事件仍会继续触发
            // 只有在没有拖动时才处理鼠标离开事件
            // 这样可以防止拖动过快时意外中断
        }

        //Drag Handling

        /// <summary>
        /// 将时间对齐到最近的时间间隔
        /// </summary>
        private float SnapTimeToInterval(float time)
        {
            return Mathf.Round(time / Misc.TIME_SNAP_INTERVAL) * Misc.TIME_SNAP_INTERVAL;
        }

        private void HandleMove(float timeDelta)
        {
            float newStartTime = _dragStartTime + timeDelta;
            float duration = _clipData.Duration;

            // 对齐到时间间隔
            newStartTime = SnapTimeToInterval(newStartTime);

            // 约束到timeline范围
            newStartTime = Mathf.Clamp(newStartTime, _timelineStartTime, _timelineEndTime - duration);

            _clipData.MoveTo(newStartTime);
            UpdateVisualTransform();
        }

        private void HandleResizeLeft(float timeDelta)
        {
            float newStartTime = _dragStartTime + timeDelta;

            // 对齐到时间间隔
            newStartTime = SnapTimeToInterval(newStartTime);

            // 约束：不能超过timeline开始时间，不能超过结束时间减去最小时长
            newStartTime = Mathf.Clamp(newStartTime, _timelineStartTime, _dragStartEndTime - Misc.MIN_CLIP_DURATION);

            _clipData.StartTime = newStartTime;
            UpdateVisualTransform();
        }

        private void HandleResizeRight(float timeDelta)
        {
            float newEndTime = _dragStartEndTime + timeDelta;

            // 对齐到时间间隔
            newEndTime = SnapTimeToInterval(newEndTime);

            // 约束：不能小于开始时间加最小时长，不能超过timeline结束时间
            newEndTime = Mathf.Clamp(newEndTime, _dragStartTime + Misc.MIN_CLIP_DURATION, _timelineEndTime);

            _clipData.EndTime = newEndTime;
            UpdateVisualTransform();
        }

        #region Visual Update

        /// <summary>
        /// 更新clip的可视化位置和大小
        /// </summary>
        public void UpdateVisualTransform()
        {
            if (_clipElement == null)
                return;

            float startX = _clipData.StartTime * _pixelsPerSecond * _zoom;
            float width;

            // 即时clip使用固定宽度
            if (_clipData.IsInstantClip)
            {
                if (_clipData is EffectClipData)
                    width = Misc.EFFECT_CLIP_UI_WIDTH;
                else
                    width = Misc.DEFAULT_INSTANT_CLIP_UI_WIDTH;
            }
            else
            {
                width = _clipData.Duration * _pixelsPerSecond * _zoom;
                width = Mathf.Max(width, Misc.MIN_CLIP_WIDTH);
            }

            _clipElement.style.left = startX;
            _clipElement.style.width = width;

            // 更新标签显示
            UpdateLabel();
        }

        /// <summary>
        /// 更新标签文本
        /// </summary>
        private void UpdateLabel()
        {
            if (_clipLabel != null)
            {
                // 如果是EffectClipData，分两行显示ID和Name
                if (_clipData is EffectClipData effectClip)
                    _clipLabel.text = $"[{effectClip.EffectId}]\n{_clipData.ClipName}";
                else
                    _clipLabel.text = _clipData.ClipName;
            }

            if (_clipData.IsInstantClip)
            {
                // 即时clip只显示触发时间
                if (_startTimeLabel != null)
                    _startTimeLabel.text = $"{_clipData.StartTime:F2}s";
            }
            else
            {
                // 持续时间clip显示开始和结束时间
                if (_startTimeLabel != null)
                    _startTimeLabel.text = $"{_clipData.StartTime:F1}s";

                if (_endTimeLabel != null)
                    _endTimeLabel.text = $"{_clipData.EndTime:F1}s";
            }
        }

        /// <summary>
        /// 更新timeline参数（当zoom或其他参数变化时调用）
        /// </summary>
        public void UpdateTimelineParams(float pixelsPerSecond, float zoom, float timelineStartTime, float timelineEndTime)
        {
            _pixelsPerSecond = pixelsPerSecond;
            _zoom = zoom;
            _timelineStartTime = timelineStartTime;
            _timelineEndTime = timelineEndTime;
            UpdateVisualTransform();
        }

        /// <summary>
        /// 设置选中状态的视觉反馈
        /// </summary>
        public void SetSelected(bool selected)
        {
            if (_clipElement == null)
                return;

            if (selected)
            {
                _clipElement.style.borderLeftColor = Color.yellow;
                _clipElement.style.borderRightColor = Color.yellow;
                _clipElement.style.borderTopColor = Color.yellow;
                _clipElement.style.borderBottomColor = Color.yellow;
                _clipElement.style.borderLeftWidth = 2;
                _clipElement.style.borderRightWidth = 2;
                _clipElement.style.borderTopWidth = 2;
                _clipElement.style.borderBottomWidth = 2;
            }
            else
            {
                _clipElement.style.borderLeftColor = new Color(0, 0, 0, 0.5f);
                _clipElement.style.borderRightColor = new Color(0, 0, 0, 0.5f);
                _clipElement.style.borderTopColor = new Color(0, 0, 0, 0.5f);
                _clipElement.style.borderBottomColor = new Color(0, 0, 0, 0.5f);
                _clipElement.style.borderLeftWidth = 1;
                _clipElement.style.borderRightWidth = 1;
                _clipElement.style.borderTopWidth = 1;
                _clipElement.style.borderBottomWidth = 1;
            }
        }

        #endregion

        #region Context Menu

        private void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Delete Clip", action => OnClipDeleted?.Invoke(this));
            evt.menu.AppendAction("Duplicate Clip", action => DuplicateClip());
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("Properties", action => ShowProperties());
        }

        private void DuplicateClip()
        {
            // 这里需要通过事件通知外部进行复制操作
            Debug.Log($"Duplicate clip: {_clipData.ClipName}");
        }

        private void ShowProperties()
        {
            Debug.Log($"Show properties for clip: {_clipData.GetDisplayInfo()}");
        }

        #endregion


        private TimelineClipData _clipData;
        /// <summary>
        /// clip主体
        /// </summary>
        private VisualElement _clipElement;
        private Label _clipLabel;
        //左右手柄，用以处理拖动
        private VisualElement _leftHandle;
        private VisualElement _rightHandle;
        // 时间标签
        private Label _startTimeLabel;
        private Label _endTimeLabel;
    }
}
