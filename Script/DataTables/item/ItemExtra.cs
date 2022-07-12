//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace cfg.item
{

public abstract partial class ItemExtra :  Bright.Config.BeanBase 
{
    public ItemExtra(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        PostInit();
    }

    public static ItemExtra DeserializeItemExtra(ByteBuf _buf)
    {
        switch (_buf.ReadInt())
        {
            case item.TreasureBox.__ID__: return new item.TreasureBox(_buf);
            case item.InteractionItem.__ID__: return new item.InteractionItem(_buf);
            case item.Clothes.__ID__: return new item.Clothes(_buf);
            case item.DesignDrawing.__ID__: return new item.DesignDrawing(_buf);
            case item.Dymmy.__ID__: return new item.Dymmy(_buf);
            default: throw new SerializationException();
        }
    }

    public int Id { get; private set; }


    public virtual void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public virtual void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
