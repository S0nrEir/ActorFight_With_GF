using Aquila.Fight.Actor;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 数据组件 by yhc 
    /// </summary>
    public partial class Addon_Data : AddonBase
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.DATA;

        public override void OnAdd()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override uint Valid()
        {
            return ( uint ) AddonValidErrorCodeEnum.NONE;
        }

        public override void Reset()
        {
            base.Reset();

        }

        public override void Init( TActorBase actor, GameObject target_gameobject_, Transform target_transform_ )
        {
            base.Init( actor, target_gameobject_, target_transform_ );
            _meta = GameEntry.DataTable.Tables.TB_RoleMeta.Get( actor.RoleMetaID );
            if ( _meta is null )
                throw new GameFrameworkException( $"faild to set meta role id:{actor.RoleMetaID},meta is null" );
        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
        }

        private Cfg.role.RoleMeta _meta = null;
    }
}

