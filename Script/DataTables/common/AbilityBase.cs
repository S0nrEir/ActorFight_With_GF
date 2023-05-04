//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace Cfg.common
{

public sealed partial class AbilityBase :  Bright.Config.BeanBase 
{
    public AbilityBase(ByteBuf _buf) 
    {
        id = _buf.ReadInt();
        name = _buf.ReadString();
        desc = _buf.ReadString();
        {int n = System.Math.Min(_buf.ReadSize(), _buf.Size);effects = new int[n];for(var i = 0 ; i < n ; i++) { int _e;_e = _buf.ReadInt(); effects[i] = _e;}}
        TargetType = (Enum.AbilityTargetType)_buf.ReadInt();
        PostInit();
    }

    public static AbilityBase DeserializeAbilityBase(ByteBuf _buf)
    {
        return new common.AbilityBase(_buf);
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
    /// 携带的effect集合
    /// </summary>
    public int[] effects { get; private set; }
    /// <summary>
    /// 目标类型
    /// </summary>
    public Enum.AbilityTargetType TargetType { get; private set; }

    public const int __ID__ = -1922790536;
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
        + "effects:" + Bright.Common.StringUtil.CollectionToString(effects) + ","
        + "TargetType:" + TargetType + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
