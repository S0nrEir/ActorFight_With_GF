//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace Cfg.Struct
{

/// <summary>
/// Effect扩展参数
/// </summary>
public sealed partial class EffectExtensionParam :  Bright.Config.BeanBase 
{
    public EffectExtensionParam(ByteBuf _buf) 
    {
        FloatParam_1 = _buf.ReadFloat();
        FloatParam_2 = _buf.ReadFloat();
        FloatParam_3 = _buf.ReadFloat();
        FloatParam_4 = _buf.ReadFloat();
        IntParam_1 = _buf.ReadInt();
        IntParam_2 = _buf.ReadInt();
        IntParam_3 = _buf.ReadInt();
        IntParam_4 = _buf.ReadInt();
        PostInit();
    }

    public static EffectExtensionParam DeserializeEffectExtensionParam(ByteBuf _buf)
    {
        return new Struct.EffectExtensionParam(_buf);
    }

    /// <summary>
    /// 浮点参数1
    /// </summary>
    public float FloatParam_1 { get; private set; }
    /// <summary>
    /// 浮点参数2
    /// </summary>
    public float FloatParam_2 { get; private set; }
    /// <summary>
    /// 浮点参数3
    /// </summary>
    public float FloatParam_3 { get; private set; }
    /// <summary>
    /// 浮点参数4
    /// </summary>
    public float FloatParam_4 { get; private set; }
    /// <summary>
    /// 整型参数_1
    /// </summary>
    public int IntParam_1 { get; private set; }
    /// <summary>
    /// 整型参数_2
    /// </summary>
    public int IntParam_2 { get; private set; }
    /// <summary>
    /// 整型参数_3
    /// </summary>
    public int IntParam_3 { get; private set; }
    /// <summary>
    /// 整型参数_4
    /// </summary>
    public int IntParam_4 { get; private set; }

    public const int __ID__ = -1637178376;
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
        + "FloatParam_1:" + FloatParam_1 + ","
        + "FloatParam_2:" + FloatParam_2 + ","
        + "FloatParam_3:" + FloatParam_3 + ","
        + "FloatParam_4:" + FloatParam_4 + ","
        + "IntParam_1:" + IntParam_1 + ","
        + "IntParam_2:" + IntParam_2 + ","
        + "IntParam_3:" + IntParam_3 + ","
        + "IntParam_4:" + IntParam_4 + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
