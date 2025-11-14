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

        // 配置参数
        private const float MIN_CLIP_WIDTH = 20f; // 最小clip宽度（像素）
        private const float MIN_CLIP_DURATION = 0.1f; // 最小clip时长（秒）
        private const float HANDLE_WIDTH = 8f; // 拖动手柄宽度
        private const float CLIP_HEIGHT = 30f; // Clip高度
        private const float TIME_SNAP_INTERVAL = 0.1f; // 时间对齐间隔（秒）

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

            // 创建clip容器
            _clipElement = new VisualElement
            {
                name = $"Clip_{_clipData.ClipId}",
                userData = this,
                style =
                {
                    position = Position.Absolute,
                    height = CLIP_HEIGHT,
                    backgroundColor = _clipData.ClipColor,
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftColor = new Color(0, 0, 0, 0.5f),
                    borderRightColor = new Color(0, 0, 0, 0.5f),
                    borderTopColor = new Color(0, 0, 0, 0.5f),
                    borderBottomColor = new Color(0, 0, 0, 0.5f),
                    overflow = Overflow.Visible // 改为Visible以允许时间标签显示在外部
                }
            };

            // 创建clip标签
            _clipLabel = new Label(_clipData.ClipName)
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    fontSize = 10,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    paddingLeft = HANDLE_WIDTH + 2,
                    paddingRight = HANDLE_WIDTH + 2,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    overflow = Overflow.Hidden,
                    textOverflow = TextOverflow.Ellipsis,
                    whiteSpace = WhiteSpace.NoWrap,
                    position = Position.Absolute,
                    left = 0,
                    right = 0,
                    top = 0,
                    bottom = 0
                }
            };

            _leftHandle = new VisualElement
            {
                name = "LeftHandle",
                style =
                {
                    position = Position.Absolute,
                    left = 0,
                    top = 0,
                    bottom = 0,
                    width = HANDLE_WIDTH,
                    backgroundColor = new Color(1f, 1f, 1f, 0.2f),
                    cursor = new UnityEngine.UIElements.Cursor { texture = null, hotspot = Vector2.zero },
                    borderTopLeftRadius = 4,
                    borderBottomLeftRadius = 4
                }
            };

            _rightHandle = new VisualElement
            {
                name = "RightHandle",
                style =
                {
                    position = Position.Absolute,
                    right = 0,
                    top = 0,
                    bottom = 0,
                    width = HANDLE_WIDTH,
                    backgroundColor = new Color(1f, 1f, 1f, 0.2f),
                    cursor = new UnityEngine.UIElements.Cursor { texture = null, hotspot = Vector2.zero },
                    borderTopRightRadius = 4,
                    borderBottomRightRadius = 4
                }
            };

            // 创建开始时间标签（显示在clip左侧外部）
            _startTimeLabel = new Label()
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    position = Position.Absolute,
                    fontSize = 9,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleRight,
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f),
                    paddingLeft = 3,
                    paddingRight = 3,
                    paddingTop = 1,
                    paddingBottom = 1,
                    borderTopLeftRadius = 2,
                    borderTopRightRadius = 2,
                    borderBottomLeftRadius = 2,
                    borderBottomRightRadius = 2,
                    right = Length.Percent(100), // 显示在clip左侧外部
                    marginRight = 2,
                    top = 0,
                    bottom = 0,
                    alignSelf = Align.Center,
                    height = 16
                }
            };

            // 创建结束时间标签（显示在clip右侧外部）
            _endTimeLabel = new Label()
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    position = Position.Absolute,
                    fontSize = 9,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f),
                    paddingLeft = 3,
                    paddingRight = 3,
                    paddingTop = 1,
                    paddingBottom = 1,
                    borderTopLeftRadius = 2,
                    borderTopRightRadius = 2,
                    borderBottomLeftRadius = 2,
                    borderBottomRightRadius = 2,
                    left = Length.Percent(100), // 显示在clip右侧外部
                    marginLeft = 2,
                    top = 0,
                    bottom = 0,
                    alignSelf = Align.Center,
                    height = 16
                }
            };

            _clipElement.Add(_clipLabel);
            _clipElement.Add(_leftHandle);
            _clipElement.Add(_rightHandle);
            _clipElement.Add(_startTimeLabel);
            _clipElement.Add(_endTimeLabel);

            RegisterEvents();
            UpdateVisualTransform();

            return _clipElement;
        }

        #endregion

        #region Event Registration

        private void RegisterEvents()
        {
            // 只在手柄上注册 MouseDown 事件
            _leftHandle.RegisterCallback<MouseDownEvent>(OnLeftHandleMouseDown);
            _rightHandle.RegisterCallback<MouseDownEvent>(OnRightHandleMouseDown);

            // 在 clipElement 上注册所有移动和释放事件
            // 这样即使鼠标离开手柄区域，事件仍然能被处理
            _clipElement.RegisterCallback<MouseDownEvent>(OnClipMouseDown);
            _clipElement.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            _clipElement.RegisterCallback<MouseUpEvent>(OnMouseUp);
            _clipElement.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);

            _clipElement.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));
        }

        #endregion

        #region Event Handlers

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
                // 检查是否点击在手柄区域，如果是则不处理（让手柄的事件处理）
                float localX = evt.localMousePosition.x;
                if (localX <= HANDLE_WIDTH || localX >= _clipElement.resolvedStyle.width - HANDLE_WIDTH)
                    return;

                _currentDragMode = DragMode.Move;
                _dragStartMouseX = evt.mousePosition.x;
                _dragStartTime = _clipData.StartTime;
                evt.StopPropagation();
                _clipElement.CaptureMouse();

                // 触发选中事件
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

        #endregion

        #region Drag Handling

        /// <summary>
        /// 将时间对齐到最近的时间间隔
        /// </summary>
        private float SnapTimeToInterval(float time)
        {
            return Mathf.Round(time / TIME_SNAP_INTERVAL) * TIME_SNAP_INTERVAL;
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
            newStartTime = Mathf.Clamp(newStartTime, _timelineStartTime, _dragStartEndTime - MIN_CLIP_DURATION);

            _clipData.StartTime = newStartTime;
            UpdateVisualTransform();
        }

        private void HandleResizeRight(float timeDelta)
        {
            float newEndTime = _dragStartEndTime + timeDelta;

            // 对齐到时间间隔
            newEndTime = SnapTimeToInterval(newEndTime);

            // 约束：不能小于开始时间加最小时长，不能超过timeline结束时间
            newEndTime = Mathf.Clamp(newEndTime, _dragStartTime + MIN_CLIP_DURATION, _timelineEndTime);

            _clipData.EndTime = newEndTime;
            UpdateVisualTransform();
        }

        #endregion

        #region Visual Update

        /// <summary>
        /// 更新clip的可视化位置和大小
        /// </summary>
        public void UpdateVisualTransform()
        {
            if (_clipElement == null)
                return;

            float startX = _clipData.StartTime * _pixelsPerSecond * _zoom;
            float width = _clipData.Duration * _pixelsPerSecond * _zoom;
            width = Mathf.Max(width, MIN_CLIP_WIDTH);

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
                _clipLabel.text = _clipData.ClipName;

            // 更新时间标签
            if (_startTimeLabel != null)
                _startTimeLabel.text = $"{_clipData.StartTime:F2}s";

            if (_endTimeLabel != null)
                _endTimeLabel.text = $"{_clipData.EndTime:F2}s";
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
