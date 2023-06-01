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

public sealed partial class Table_SceneConfig :  Bright.Config.BeanBase 
{
    public Table_SceneConfig(ByteBuf _buf) 
    {
        Terrain_Block_Offset_Distance = _buf.ReadFloat();
        Main_Camera_Default_Euler = _buf.ReadUnityVector3();
        Fight_Scene_Default_X_Width = _buf.ReadInt();
        Fight_Scene_Default_Y_Width = _buf.ReadInt();
        Fight_Scene_Terrain_Coordinate_Range = _buf.ReadInt();
        Fight_Scene_Terrain_Coordinate_Precision = _buf.ReadInt();
        PostInit();
    }

    public static Table_SceneConfig DeserializeTable_SceneConfig(ByteBuf _buf)
    {
        return new Single.Table_SceneConfig(_buf);
    }

    /// <summary>
    /// 单个地块的默认偏移距离
    /// </summary>
    public float Terrain_Block_Offset_Distance { get; private set; }
    /// <summary>
    /// 主相机默认旋转角度
    /// </summary>
    public UnityEngine.Vector3 Main_Camera_Default_Euler { get; private set; }
    /// <summary>
    /// 战斗场景地块默认x方向长度
    /// </summary>
    public int Fight_Scene_Default_X_Width { get; private set; }
    /// <summary>
    /// 战斗场景地块默认z方向长度
    /// </summary>
    public int Fight_Scene_Default_Y_Width { get; private set; }
    /// <summary>
    /// 场景地块两位坐标精度总范围
    /// </summary>
    public int Fight_Scene_Terrain_Coordinate_Range { get; private set; }
    /// <summary>
    /// 场景地块两位数坐标精度系数
    /// </summary>
    public int Fight_Scene_Terrain_Coordinate_Precision { get; private set; }

    public const int __ID__ = -58264649;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Terrain_Block_Offset_Distance:" + Terrain_Block_Offset_Distance + ","
        + "Main_Camera_Default_Euler:" + Main_Camera_Default_Euler + ","
        + "Fight_Scene_Default_X_Width:" + Fight_Scene_Default_X_Width + ","
        + "Fight_Scene_Default_Y_Width:" + Fight_Scene_Default_Y_Width + ","
        + "Fight_Scene_Terrain_Coordinate_Range:" + Fight_Scene_Terrain_Coordinate_Range + ","
        + "Fight_Scene_Terrain_Coordinate_Precision:" + Fight_Scene_Terrain_Coordinate_Precision + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
