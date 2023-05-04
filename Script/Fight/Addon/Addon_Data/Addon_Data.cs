using Aquila.Fight.Actor;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 数据组件 by yhc 
    /// </summary>
    public partial class Addon_Data : Addon_Base
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.DATA;

        public override void OnAdd()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Reset()
        {
            base.Reset();

        }

        public override void Init( TActorBase actor, GameObject target_gameobject, Transform target_transform )
        {
            base.Init( actor, target_gameobject, target_transform );
            _meta = GameEntry.DataTable.Table<Cfg.role.TB_RoleMeta>().Get( actor.RoleMetaID );
            if ( _meta is null )
                throw new GameFrameworkException( $"faild to set meta role id:{actor.RoleMetaID},meta is null" );
        }

        private Cfg.role.RoleMeta _meta = null;
    }
}

