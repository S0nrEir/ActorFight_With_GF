//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace Cfg.Bean
{

/// <summary>
/// actor基础属性数值
/// </summary>
public sealed partial class actor_base_attribute_value :  Bright.Config.BeanBase 
{
    public actor_base_attribute_value(ByteBuf _buf) 
    {
        max_hp = _buf.ReadInt();
        max_mp = _buf.ReadInt();
        atk = _buf.ReadInt();
        def = _buf.ReadInt();
        spd = _buf.ReadInt();
        mvt = _buf.ReadInt();
        str = _buf.ReadInt();
        agi = _buf.ReadInt();
        spw = _buf.ReadInt();
        PostInit();
    }

    public static actor_base_attribute_value Deserializeactor_base_attribute_value(ByteBuf _buf)
    {
        return new Bean.actor_base_attribute_value(_buf);
    }

    /// <summary>
    /// 最大hp
    /// </summary>
    public int max_hp { get; private set; }
    /// <summary>
    /// 最大mp
    /// </summary>
    public int max_mp { get; private set; }
    /// <summary>
    /// akt
    /// </summary>
    public int atk { get; private set; }
    public int def { get; private set; }
    public int spd { get; private set; }
    public int mvt { get; private set; }
    public int str { get; private set; }
    public int agi { get; private set; }
    public int spw { get; private set; }

    public const int __ID__ = 528390536;
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
        + "max_hp:" + max_hp + ","
        + "max_mp:" + max_mp + ","
        + "atk:" + atk + ","
        + "def:" + def + ","
        + "spd:" + spd + ","
        + "mvt:" + mvt + ","
        + "str:" + str + ","
        + "agi:" + agi + ","
        + "spw:" + spw + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
