using Aquila.Module;
using Aquila.ObjectPool;
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
            var attrAddon = _actorInstance.GetAddon<Addon_BaseAttrNumric>();
            if ( attrAddon is null )
            {
                Log.Warning( "<color=yellow>Addon_HP--->attr_addon is null</color>" );
                return;
            }

            var cur = attrAddon.GetCurrHPCorrection();
            // var max = attrAddon.GetCorrectionFinalValue(Actor_Attr.Max_HP, 0f);
            var max = attrAddon.GetCorrectionFinalValue( Actor_Base_Attr.HP, 0f );
            _hpObj.SetValue( ( int ) cur, ( int ) max.value );
        }

        /// <summary>
        /// 设置hp slider的值
        /// </summary>
        public void SetValue( int cur, int max )
        {
            _hpObj.SetValue( cur, max );
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.HP;

        public override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            if ( _cachedPos == _actorTransform.position )
                return;

            _hpObj.SetScreenPos( GameEntry.InfoBoard.WorldPos2BoardRectPos( _actorTransform.position + _offset, GameEntry.GlobalVar.MainCamera ) );
            _cachedPos = _actorTransform.position;
        }

        public override void OnAdd()
        {
        }

        public override void Init( Module_ProxyActor.ActorInstance instance )
        {
            base.Init( instance );
            _hpObj = GameEntry.InfoBoard.GenHPBar();
            _actorTransform = instance.Actor.transform;
            Refresh();
            _offset = GameEntry.DataTable.Tables.SceneConfig.HPBarPosOffset;
            _cachedPos = _actorTransform.position;
        }

        public override void Dispose()
        {
            // GameEntry.ObjectPool.GetObjectPool<Object_HPBar>(nameof(Object_HPBar)).Unspawn(_hp_obj);
            GameEntry.InfoBoard.UnSpawn<Object_HPBar>( typeof( Object_HPBar ).Name, _hpObj );
            _hpObj = null;
            _actorTransform = null;
            base.Dispose();
        }

        private Object_HPBar _hpObj = null;

        /// <summary>
        /// 持有缓存的actor的transform
        /// </summary>
        private Transform _actorTransform = null;

        /// <summary>
        /// 偏移位置
        /// </summary>
        private Vector3 _offset = Vector3.zero;

        /// <summary>
        /// 缓存actor位置
        /// </summary>
        private Vector3 _cachedPos = Vector3.zero;
    }

}
