using Aquila.Extension;
using Aquila.Fight.Addon;
using Cfg.Enum;
using System;
using UnityEngine;

namespace Aquila.Module
{
    /// <summary>
    /// Actor的代理模块，此脚本处理通用接口和模块的主逻辑入口
    /// </summary>
    public partial class Module_ProxyActor : GameFrameworkModuleBase, IUpdate
    {
        //-----------------------pub-----------------------

        /// <summary>
        /// 返回一个actor的武器挂点的transforom，拿不到返回null
        /// </summary>
        public Transform GetWeaponHangPoint( int actorID )
        {
            return null;
        }

        /// <summary>
        /// 返回一个武器挂点的世界坐标
        /// </summary>
        public Vector3 GetWeaponHangPoint()
        {
            return GameEntry.GlobalVar.InvalidPosition;
        }

        /// <summary>
        /// 获取一个actor的位置，拿不到返回invlaidPosition
        /// </summary>
        public Vector3 GetPosition( int actorID )
        {
            var instance = Get( actorID );
            return instance is null ? GameEntry.GlobalVar.InvalidPosition : instance.Actor.CachedTransform.position;
        }
        
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
        public (bool succ, float value) GetCorrectionAttr(int actorID, /*Actor_Base_Attr*/ actor_attribute type)
        {
            var res = TryGet(actorID);
            if (!res.has)
                return (false, 0f);

            var correctionVal = res.instance.GetAddon<Addon_BaseAttrNumric>().GetCorrectionValue(type, 0);
            return (correctionVal != 0, correctionVal);
        }

        /// <summary>
        /// 获取指定actor的对应基础属性
        /// </summary>
        public (bool succ, float value) GetActorBaseAttr( int actorID, /*Actor_Attr*/ actor_attribute type )
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
            FightEnsureInit();
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