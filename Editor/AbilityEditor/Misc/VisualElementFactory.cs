using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.AbilityEditor
{
    public class VisualElementFactory
    {
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
                    height            = 30,
                    marginBottom      = 5,
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
    }
}
