//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace Cfg.role
{

public sealed partial class RoleBaseAttr :  Bright.Config.BeanBase 
{
    public RoleBaseAttr(ByteBuf _buf) 
    {
        id = _buf.ReadInt();
        name = _buf.ReadString();
        desc = _buf.ReadString();
        RoleClass = (Enum.Role_Class)_buf.ReadInt();
        test_boolean = _buf.ReadBool();
        {int n = System.Math.Min(_buf.ReadSize(), _buf.Size);test_map = new System.Collections.Generic.Dictionary<int, int>(n * 3 / 2);for(var i = 0 ; i < n ; i++) { int _k;  _k = _buf.ReadInt(); int _v;  _v = _buf.ReadInt();     test_map.Add(_k, _v);}}
        test_vector = _buf.ReadUnityVector3();
        {int n = System.Math.Min(_buf.ReadSize(), _buf.Size);test_arr = new int[n];for(var i = 0 ; i < n ; i++) { int _e;_e = _buf.ReadInt(); test_arr[i] = _e;}}
        test_string = _buf.ReadString();
        PostInit();
    }

    public static RoleBaseAttr DeserializeRoleBaseAttr(ByteBuf _buf)
    {
        return new role.RoleBaseAttr(_buf);
    }

    /// <summary>
    /// 这是id
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
    /// 职业
    /// </summary>
    public Enum.Role_Class RoleClass { get; private set; }
    /// <summary>
    /// 测试布尔值
    /// </summary>
    public bool test_boolean { get; private set; }
    /// <summary>
    /// 测试键值对
    /// </summary>
    public System.Collections.Generic.Dictionary<int, int> test_map { get; private set; }
    /// <summary>
    /// 测试向量
    /// </summary>
    public UnityEngine.Vector3 test_vector { get; private set; }
    /// <summary>
    /// 测试数组或集合
    /// </summary>
    public int[] test_arr { get; private set; }
    /// <summary>
    /// 测试字符串
    /// </summary>
    public string test_string { get; private set; }

    public const int __ID__ = -876445296;
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
        + "RoleClass:" + RoleClass + ","
        + "test_boolean:" + test_boolean + ","
        + "test_map:" + Bright.Common.StringUtil.CollectionToString(test_map) + ","
        + "test_vector:" + test_vector + ","
        + "test_arr:" + Bright.Common.StringUtil.CollectionToString(test_arr) + ","
        + "test_string:" + test_string + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
