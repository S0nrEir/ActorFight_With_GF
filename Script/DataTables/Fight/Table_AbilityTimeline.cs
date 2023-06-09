//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace Cfg.Fight
{

public sealed partial class Table_AbilityTimeline :  Bright.Config.BeanBase 
{
    public Table_AbilityTimeline(ByteBuf _buf) 
    {
        id = _buf.ReadInt();
        AssetPath = _buf.ReadString();
        Duration = _buf.ReadFloat();
        TriggerTime = _buf.ReadFloat();
        PostInit();
    }

    public static Table_AbilityTimeline DeserializeTable_AbilityTimeline(ByteBuf _buf)
    {
        return new Fight.Table_AbilityTimeline(_buf);
    }

    /// <summary>
    /// id
    /// </summary>
    public int id { get; private set; }
    /// <summary>
    /// Timeline资产路径
    /// </summary>
    public string AssetPath { get; private set; }
    /// <summary>
    /// 总时长，单位秒
    /// </summary>
    public float Duration { get; private set; }
    /// <summary>
    /// 触发秒数
    /// </summary>
    public float TriggerTime { get; private set; }

    public const int __ID__ = -1709568676;
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
        + "id:" + id + ","
        + "AssetPath:" + AssetPath + ","
        + "Duration:" + Duration + ","
        + "TriggerTime:" + TriggerTime + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}