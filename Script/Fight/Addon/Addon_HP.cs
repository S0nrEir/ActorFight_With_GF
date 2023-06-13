using Aquila.Config;
using Aquila.Fight.Actor;
using Aquila.ObjectPool;
using Aquila.Toolkit;
using Cfg.Enum;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    public class Addon_HP : Addon_Base
    {
        //-------------------------pub-------------------------
        
        /// <summary>
        /// 基于当前血量刷新slider
        /// </summary>
        public void Refresh()
        {
            //基于当前血量和血量上限刷新
            var attr_addon = _actor_instance.GetAddon<Addon_BaseAttrNumric>();
            if (attr_addon is null)
            {
                Log.Warning("<color=yellow>Addon_HP--->attr_addon is null</color>");
                return;
            }

            var cur = attr_addon.GetCurrHPCorrection();
            var max = attr_addon.GetCorrectionFinalValue(Actor_Attr.Max_HP, 0f);
            _hp_obj.SetValue((int)cur,(int)max.value);
        }

        /// <summary>
        /// 设置hp slider的值
        /// </summary>
        public void SetValue(int cur,int max)
        {
            _hp_obj.SetValue(cur,max);
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.HP;

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if(_hp_obj is null)
                return;

            _hp_obj.SetScreenPos(GameEntry.InfoBoard.WorldPos2BoardRectPos(_actor_transform.position,GameEntry.GlobalVar.MainCamera));
        }

        public override void OnAdd()
        {
        }

        public override void Init(TActorBase actor, GameObject target_go, Transform target_transform)
        {
            base.Init(actor, target_go, target_transform);
            _actor_transform = _actor_instance.Actor.transform;
            _hp_obj = GameEntry.InfoBoard.GenHPBar();
            // _hp_obj.Setup();
            //set pos
        }

        public override void Dispose()
        {
            // GameEntry.ObjectPool.GetObjectPool<Object_HPBar>(nameof(Object_HPBar)).Unspawn(_hp_obj);
            GameEntry.InfoBoard.UnSpawn<Object_HPBar>(typeof(Object_HPBar).Name,_hp_obj);
            _hp_obj          = null;
            _actor_transform = null;
            base.Dispose();
        }

        private Object_HPBar _hp_obj = null;

        /// <summary>
        /// 持有缓存的actor的transform
        /// </summary>
        private Transform _actor_transform = null;
    }
   
}
