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
        /// 基于当前血量刷新 slider
        /// </summary>
        public void Refresh()
        {
            // 基于当前血量和血量上限刷新
            var attrAddon = _actorInstance.GetAddon<Addon_BaseAttrNumric>();
            if ( attrAddon is null )
            {
                Log.Warning( "<color=yellow>Addon_HP--->attr_addon is null</color>" );
                return;
            }

            var cur = attrAddon.GetCurrHPCorrection();
            var max = attrAddon.GetCorrectionValue( actor_attribute.Max_HP, 0f );
            _hpObj.SetValue( ( int ) cur, ( int ) max );
        }

        /// <summary>
        /// 设置 hp slider 的值
        /// </summary>
        public void SetValue( int cur, int max )
        {
            _hpObj.SetValue( cur, max );
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.HP;

        public override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            UpdateScreenPos( false );
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
            _offset = GameEntry.LuBan.Tables.SceneConfig.HPBarPosOffset;
            _cachedPos = Vector3.zero;
            _cachedCameraPos = Vector3.zero;
            _cachedCameraRot = Quaternion.identity;
            _hasCachedScreenState = false;

            // 首次初始化后立即定位，避免静止 actor 不刷新导致血条不在头顶。
            UpdateScreenPos( true );
        }

        public override void Dispose()
        {
            if ( _hpObj != null )
                GameEntry.InfoBoard.UnSpawn<Object_HPBar>( typeof( Object_HPBar ).Name, _hpObj.Target );

            _hpObj = null;
            _actorTransform = null;
            _hasCachedScreenState = false;
            base.Dispose();
        }

        private void UpdateScreenPos( bool force )
        {
            if ( _hpObj == null || _actorTransform == null )
                return;

            //#todo：可能的高频调用
            var worldCamera = ResolveWorldCamera();
            if ( worldCamera == null )
                return;

            var actorPos = _actorTransform.position;
            var cameraTransform = worldCamera.transform;
            var cameraPos = cameraTransform.position;
            var cameraRot = cameraTransform.rotation;

            if ( !force
                && _hasCachedScreenState
                && _cachedPos == actorPos
                && _cachedCameraPos == cameraPos
                && _cachedCameraRot == cameraRot )
            {
                return;
            }

            _hpObj.SetScreenPos( GameEntry.InfoBoard.WorldPos2BoardRectPos( actorPos + _offset, worldCamera ) );
            _cachedPos = actorPos;
            _cachedCameraPos = cameraPos;
            _cachedCameraRot = cameraRot;
            _hasCachedScreenState = true;
        }

        private Camera ResolveWorldCamera()
        {
            var mainCamera = GameEntry.CameraHub.GetWorldCamera();
            if ( mainCamera != null && mainCamera.isActiveAndEnabled )
                return mainCamera;

            return Camera.main;
        }

        private Object_HPBar _hpObj = null;

        /// <summary>
        /// 持有缓存的 actor transform
        /// </summary>
        private Transform _actorTransform = null;

        /// <summary>
        /// 偏移位置
        /// </summary>
        private Vector3 _offset = Vector3.zero;

        /// <summary>
        /// 缓存 actor 与相机状态，用于减少重复坐标转换。
        /// </summary>
        private Vector3 _cachedPos = Vector3.zero;
        private Vector3 _cachedCameraPos = Vector3.zero;
        private Quaternion _cachedCameraRot = Quaternion.identity;
        private bool _hasCachedScreenState = false;
    }
}
