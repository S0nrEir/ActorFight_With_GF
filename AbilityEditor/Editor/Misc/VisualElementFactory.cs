using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.AbilityEditor
{
    public class VisualElementFactory
    {

        #region Clip UI Components

        /// <summary>
        /// 创建Clip容器元素
        /// </summary>
        public static VisualElement GenClipContainer(string clipId, Color clipColor, bool isInstant)
        {
            return new VisualElement
            {
                name = $"Clip_{clipId}",
                style =
                {
                    position = Position.Absolute,
                    height = Misc.CLIP_HEIGHT,
                    backgroundColor = clipColor,
                    borderTopLeftRadius = isInstant ? 2 : 4,
                    borderTopRightRadius = isInstant ? 2 : 4,
                    borderBottomLeftRadius = isInstant ? 2 : 4,
                    borderBottomRightRadius = isInstant ? 2 : 4,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftColor = new Color(0, 0, 0, 0.5f),
                    borderRightColor = new Color(0, 0, 0, 0.5f),
                    borderTopColor = new Color(0, 0, 0, 0.5f),
                    borderBottomColor = new Color(0, 0, 0, 0.5f),
                    overflow = Overflow.Visible
                }
            };
        }

        /// <summary>
        /// 创建Clip标签
        /// </summary>
        public static Label GenClipLabel(string clipName, bool isInstant)
        {
            return new Label(clipName)
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    fontSize = 8, // 从10缩小到8
                    color = Color.white,
                    unityTextAlign = isInstant ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft,
                    paddingLeft = isInstant ? 2 : (Misc.HANDLE_WIDTH + 2),
                    paddingRight = isInstant ? 2 : (Misc.HANDLE_WIDTH + 2),
                    paddingTop = 1,
                    paddingBottom = 1,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    overflow = Overflow.Hidden,
                    textOverflow = TextOverflow.Ellipsis,
                    whiteSpace = WhiteSpace.Normal, // 改为Normal支持换行
                    position = Position.Absolute,
                    left = 0,
                    right = 0,
                    top = 0,
                    bottom = 0
                }
            };
        }

        /// <summary>
        /// 创建左侧手柄
        /// </summary>
        public static VisualElement GenLeftHandle()
        {
            return new VisualElement
            {
                name = "LeftHandle",
                style =
                {
                    position = Position.Absolute,
                    left = 0,
                    top = 0,
                    bottom = 0,
                    width = Misc.HANDLE_WIDTH,
                    backgroundColor = new Color(1f, 1f, 1f, 0.2f),
                    cursor = new UnityEngine.UIElements.Cursor { texture = null, hotspot = Vector2.zero },
                    borderTopLeftRadius = 4,
                    borderBottomLeftRadius = 4
                }
            };
        }

        /// <summary>
        /// 创建右侧手柄
        /// </summary>
        public static VisualElement GenRightHandle()
        {
            return new VisualElement
            {
                name = "RightHandle",
                style =
                {
                    position = Position.Absolute,
                    right = 0,
                    top = 0,
                    bottom = 0,
                    width = Misc.HANDLE_WIDTH,
                    backgroundColor = new Color(1f, 1f, 1f, 0.2f),
                    cursor = new UnityEngine.UIElements.Cursor { texture = null, hotspot = Vector2.zero },
                    borderTopRightRadius = 4,
                    borderBottomRightRadius = 4
                }
            };
        }

        /// <summary>
        /// 创建即时Clip的时间标签（显示在clip下方）
        /// </summary>
        public static Label GenInstantClipTimeLabel()
        {
            return new Label()
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    position = Position.Absolute,
                    fontSize = 9,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f),
                    paddingLeft = 3,
                    paddingRight = 3,
                    paddingTop = 1,
                    paddingBottom = 1,
                    borderTopLeftRadius = 2,
                    borderTopRightRadius = 2,
                    borderBottomLeftRadius = 2,
                    borderBottomRightRadius = 2,
                    top = Length.Percent(100),
                    marginTop = 2,
                    left = Length.Percent(50),
                    translate = new Translate(Length.Percent(-50), 0),
                    height = 16
                }
            };
        }

        /// <summary>
        /// 创建持续时间Clip的开始时间标签（显示在clip左侧外部）
        /// </summary>
        public static Label GenDurationClipStartTimeLabel()
        {
            return new Label()
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
                    right = Length.Percent(100),
                    marginRight = 2,
                    top = 0,
                    bottom = 0,
                    alignSelf = Align.Center,
                    height = 16
                }
            };
        }

        /// <summary>
        /// 创建持续时间Clip的结束时间标签（显示在clip右侧外部）
        /// </summary>
        public static Label GenDurationClipEndTimeLabel()
        {
            return new Label()
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
                    left = Length.Percent(100),
                    marginLeft = 2,
                    top = 0,
                    bottom = 0,
                    alignSelf = Align.Center,
                    height = 16
                }
            };
        }

        #endregion

        /// <summary>
        /// 生成一个新的track的ui element项
        /// </summary>
        public static VisualElement GenTrack( Color color )
        {
            return new VisualElement
            {
                style =
                {
                    flexDirection     = FlexDirection.Row,
                    height            = 40,
                    marginBottom      = 2,
                    backgroundColor   = color,
                    paddingLeft       = 5,
                    paddingRight      = 5,
                    borderLeftWidth   = 0,
                    borderRightWidth  = 0,
                    borderTopWidth    = 0,
                    borderBottomWidth = 0,
                    borderLeftColor   = Color.clear,
                    borderRightColor  = Color.clear,
                    borderTopColor    = Color.clear,
                    borderBottomColor = Color.clear
                }
            };
        }

        /// <summary>
        /// 生成一个track的名称项
        /// </summary>
        public static VisualElement GenTrackNameLabel( string trackName )
        {
            return new Label( trackName )
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleLeft,
                    flexGrow = 1,
                    color = Color.white,
                    fontSize = 12
                }
            };
        }

        /// <summary>
        /// 生成时间轴刻度标记
        /// </summary>
        public static VisualElement GenScaleMark( float leftPosition, bool isMajor )
        {
            return new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    left = leftPosition,
                    top = 0, // 明确设置 top 位置
                    width = 1,
                    height = isMajor ? 20 : 10,
                    backgroundColor = isMajor ? Color.white : new Color( 0.7f, 0.7f, 0.7f )
                }
            };
        }

        /// <summary>
        /// 生成时间轴刻度标签
        /// </summary>
        public static Label GenScaleLabel( string text, float leftPosition )
        {
            return new Label( text )
            {
                style =
                {
                    position = Position.Absolute,
                    left = leftPosition,
                    top = 20,
                    fontSize = 10,
                    color = Color.white,
                    unityTextAlign = TextAnchor.UpperCenter
  }
            };
        }

        /// <summary>
        /// 生成轨道时间线容器
        /// </summary>
        public static VisualElement GenTrackTimeline( Color trackColor, float width, float height )
        {
            return new VisualElement
            {
                style =
                {
                    position          = Position.Relative, // 支持clips使用绝对定位
                    flexGrow          = 1,
                    backgroundColor   = trackColor,
                    width             = width,
                    height            = height,
                    borderLeftWidth   = 1,
                    borderRightWidth  = 1,
                    borderTopWidth    = 1,
                    borderBottomWidth = 1,
                    borderLeftColor   = new Color( 0.3f, 0.3f, 0.3f ),
                    borderRightColor  = new Color( 0.3f, 0.3f, 0.3f ),
                    borderTopColor    = new Color( 0.3f, 0.3f, 0.3f ),
                    borderBottomColor = new Color( 0.3f, 0.3f, 0.3f )
                }
            };
        }

        /// <summary>
        /// 创建水平滚动视图
        /// </summary>
        public static ScrollView GenHorizontalScrollView()
        {
            return new ScrollView( ScrollViewMode.Horizontal )
            {
                style =
                {
                    flexGrow = 1,
                    maxHeight = 600
                }
            };
        }

        #region Timeline UI Components

        /// <summary>
        /// 创建时间轴刻度容器
        /// </summary>
        public static VisualElement GenScaleContainer(float totalWidth, float scaleHeight)
        {
            return new VisualElement
            {
                style =
                {
                    height = scaleHeight,
                    marginBottom = 5,
                    marginLeft = 50,
                    width = totalWidth,
                    minWidth = totalWidth,
                    position = Position.Relative,
                    overflow = Overflow.Visible
                }
            };
        }

        /// <summary>
        /// 创建轨道行容器
        /// </summary>
        public static VisualElement GenTrackRow(float totalWidth, float trackHeight)
        {
            return new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    height = trackHeight,
                    marginBottom = 2,
                    width = totalWidth + 50,
                    minWidth = totalWidth + 50
                }
            };
        }

        /// <summary>
        /// 创建轨道名称标签（用于时间轴中）
        /// </summary>
        public static Label GenTrackNameLabelForTimeline(string trackName)
        {
            return new Label(trackName)
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
        }

        /// <summary>
        /// 创建时间轴拖动线
        /// </summary>
        public static VisualElement GenTimelineScrubber()
        {
            return new VisualElement
            {
                name = "TimelineScrubber",
                pickingMode = PickingMode.Ignore,
                style =
                {
                    position = Position.Absolute,
                    width = 2,
                    backgroundColor = new Color(1f, 0.5f, 0f, 1f),
                    left = 50,
                    top = 0,
                    bottom = 0,
                    display = DisplayStyle.None
                }
            };
        }

        /// <summary>
        /// 创建拖动线的时间标签
        /// </summary>
        public static Label GenScrubberTimeLabel()
        {
            return new Label("0.0s")
            {
                name = "ScrubberTimeLabel",
                pickingMode = PickingMode.Ignore,
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
        }

        #endregion
    }
}
