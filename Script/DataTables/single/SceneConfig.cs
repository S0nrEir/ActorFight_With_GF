//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;

namespace Cfg.Single
{
   
public partial class SceneConfig
{

     private readonly Single.Table_SceneConfig _data;

    public SceneConfig(ByteBuf _buf)
    {
        int n = _buf.ReadSize();
        if (n != 1) throw new SerializationException("table mode=one, but size != 1");
        _data = Single.Table_SceneConfig.DeserializeTable_SceneConfig(_buf);
        PostInit();
    }


    /// <summary>
    /// 单个地块的默认偏移距离
    /// </summary>
     public float Terrain_Block_Offset_Distance => _data.Terrain_Block_Offset_Distance;
    /// <summary>
    /// 主相机默认旋转角度
    /// </summary>
     public UnityEngine.Vector3 Main_Camera_Default_Euler => _data.Main_Camera_Default_Euler;
    /// <summary>
    /// 战斗场景地块默认x方向长度
    /// </summary>
     public int Fight_Scene_Default_X_Width => _data.Fight_Scene_Default_X_Width;
    /// <summary>
    /// 战斗场景地块默认z方向长度
    /// </summary>
     public int Fight_Scene_Default_Y_Width => _data.Fight_Scene_Default_Y_Width;
    /// <summary>
    /// 场景地块两位坐标精度总范围
    /// </summary>
     public int Fight_Scene_Terrain_Coordinate_Range => _data.Fight_Scene_Terrain_Coordinate_Range;
    /// <summary>
    /// 场景地块两位数坐标精度系数
    /// </summary>
     public int Fight_Scene_Terrain_Coordinate_Precision => _data.Fight_Scene_Terrain_Coordinate_Precision;
    /// <summary>
    /// 主相机默认世界空间坐标位置
    /// </summary>
     public UnityEngine.Vector3 MainCameraDefaultPosition => _data.MainCameraDefaultPosition;

    public void Resolve(Dictionary<string, object> _tables)
    {
        _data.Resolve(_tables);
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        _data.TranslateText(translator);
    }

    
    partial void PostInit();
    partial void PostResolve();
}

}