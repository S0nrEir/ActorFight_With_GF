using Aquila.Extension;
using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Module
{
    /// <summary>
    /// Actor的代理模块，此脚本处理通用接口和模块的主逻辑入口
    /// </summary>
    public partial class Module_ProxyActor : GameFrameworkModuleBase, IUpdate
    {
        //-----------------------pub-----------------------

        /// <summary>
        /// 获取一个技能的冷却
        /// </summary>
        public (float remain, float duration) GetCoolDown( int actorID, int abilityID )
        {
            var instance = Get( actorID );
            if ( instance is null )
                return (1f, 1f);

            var addon = instance.GetAddon<Addon_Ability>();
            var cd = addon.CoolDown( abilityID );
            return (cd.remain, cd.duration);
        }

        /// <summary>
        /// 获取指定actor对应的修正属性
        /// </summary>
        public (bool succ, float value) GetCorrectionAttr( int actorID, Actor_Base_Attr type )
        {
            var res = TryGet( actorID );
            if ( !res.has )
                return (false, 0f);

            return res.instance.GetAddon<Addon_BaseAttrNumric>().GetCorrectionFinalValue( type );
        }

        /// <summary>
        /// 获取指定actor的对应基础属性
        /// </summary>
        public (bool succ, float value) GetActorBaseAttr( int actorID, Actor_Attr type )
        {
            var res = TryGet( actorID );
            if ( !res.has )
                return (false, 0f);

            return res.instance.GetAddon<Addon_BaseAttrNumric>().GetBaseValue( type );
        }

        //----------------------- override-----------------------
        public override void Open( object param )
        {
            base.Open( param );
            MgrOpen();
            SystemOpen();
        }

        public override void Close()
        {
            OnSystemClose();
            MgrClose();
            base.Close();
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            MgrEnsureInit();
            SystemEnsureInit();
        }

        public void OnUpdate( float elapsed, float realElapsed )
        {
            if ( !_open_flag )
                return;

            SystemUpdate( elapsed, realElapsed );
        }
    }
}