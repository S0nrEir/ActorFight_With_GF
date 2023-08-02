using Cfg.Enum;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 子弹类型的actor
    /// </summary>
    public class Actor_Bullet : Actor_Base
    {
        public override RoleType ActorType => RoleType.Bullet;
    }

    /// <summary>
    /// 子弹类型actor的实体数据
    /// </summary>
    public class Actor_Bullet_EntityData : EntityData
    {
        public Actor_Bullet_EntityData( int entityID ) : base( entityID ,typeof(Actor_Bullet).GetHashCode())
        { 
        }
    }

}
