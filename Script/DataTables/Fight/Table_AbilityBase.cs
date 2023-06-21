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

public sealed partial class Table_AbilityBase :  Bright.Config.BeanBase 
{
    public Table_AbilityBase(ByteBuf _buf) 
    {
        id = _buf.ReadInt();
        name = _buf.ReadString();
        desc = _buf.ReadString();
        CostEffectID = _buf.ReadInt();
        CoolDownEffectID = _buf.ReadInt();
        {int n = System.Math.Min(_buf.ReadSize(), _buf.Size);effects = new int[n];for(var i = 0 ; i < n ; i++) { int _e;_e = _buf.ReadInt(); effects[i] = _e;}}
        Timeline = _buf.ReadInt();
        PostInit();
    }

    public static Table_AbilityBase DeserializeTable_AbilityBase(ByteBuf _buf)
    {
        return new Fight.Table_AbilityBase(_buf);
    }

    /// <summary>
    /// id
    /// </summary>
    public int id { get; private set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string name { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string desc { get; private set; }
    /// <summary>
    /// 技能消耗
    /// </summary>
    public int CostEffectID { get; private set; }
    /// <summary>
    /// 技能冷却
    /// </summary>
    public int CoolDownEffectID { get; private set; }
    /// <summary>
    /// 携带的effect集合
    /// </summary>
    public int[] effects { get; private set; }
    /// <summary>
    /// TimelineID
    /// </summary>
    public int Timeline { get; private set; }

    public const int __ID__ = 766903660;
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
        + "name:" + name + ","
        + "desc:" + desc + ","
        + "CostEffectID:" + CostEffectID + ","
        + "CoolDownEffectID:" + CoolDownEffectID + ","
        + "effects:" + Bright.Common.StringUtil.CollectionToString(effects) + ","
        + "Timeline:" + Timeline + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
