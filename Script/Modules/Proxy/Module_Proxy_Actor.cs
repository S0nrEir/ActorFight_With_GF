using Aquila.Extension;
using Aquila.Fight.Addon;
using Aquila.Numric;
using Cfg.Enum;

namespace Aquila.Module
{

    /// <summary>
    /// Actor代理模块
    /// </summary>
    public partial class Module_Proxy_Actor : GameFrameworkModuleBase
    {
        #region pub
        
        /// <summary>
        /// 获取指定actor对应的修正属性
        /// </summary>
        public (bool succ, float value) GetCorrectionAttr( int actor_id_, Actor_Attr type_ )
        {
            var res = TryGet( actor_id_ );
            if(!res.has)
                return (false,0f);

            return res.instance.GetAddon<Addon_BaseAttrNumric>().GetCorrectionFinalValue( type_ );
        }

        /// <summary>
        /// 获取指定actor的对应基础属性
        /// </summary>
        public (bool succ, float value) GetActorBaseAttr( int actor_id_ , Actor_Attr type_ )
        {
            var res = TryGet( actor_id_ );
            if ( !res.has )
                return (false, 0f);

            return res.instance.GetAddon<Addon_BaseAttrNumric>().GetBaseValue( type_ );
        }

        #endregion

        #region override

        public override void Start( object param )
        {
            base.Start( param );
            MgrStart();
        }

        public override void End()
        {
            MgrEnd();
            base.End();
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            MgrEnsureInit();
        }

        protected override bool Contains_Sub_Module => false;

        #endregion
    }

}