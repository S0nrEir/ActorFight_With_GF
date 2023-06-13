using Aquila.Fight.Actor;
using Cfg.Role;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 数据组件
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

        public override void Init( Actor_Base actor, GameObject target_gameobject, Transform target_transform )
        {
            base.Init( actor, target_gameobject, target_transform );
            _meta = GameEntry.DataTable.Table<RoleMeta>().Get( actor.RoleMetaID );
            if ( _meta is null )
                throw new GameFrameworkException( $"faild to set meta role id:{actor.RoleMetaID},meta is null" );
        }

        private Table_RoleMeta _meta = null;
    }
}

