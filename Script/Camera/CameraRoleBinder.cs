using Aquila.Extension;
using UnityEngine;

namespace Aquila.CameraSystem
{
    /// <summary>
    /// 挂在相机对象上，自动把相机按角色注册到 CameraHub。
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent( typeof( Camera ) )]
    public class CameraRoleBinder : MonoBehaviour
    {
        private void OnEnable()
        {
            if ( _autoRegister )
                TryRegister();
        }

        private void Start()
        {
            if ( _autoRegister )
                TryRegister();
        }

        private void LateUpdate()
        {
            if ( _autoRegister && !_registered )
                TryRegister();
        }

        private void OnDisable()
        {
            TryUnregister();
        }

        private void OnDestroy()
        {
            TryUnregister();
        }

        public void Rebind( CameraRole role, int priority )
        {
            TryUnregister();
            _role = role;
            _priority = priority;
            TryRegister();
        }

        private void TryRegister()
        {
            if ( _registered )
                return;

            var cameraComp = GetComponent<Camera>();
            if ( cameraComp == null )
                return;

            var hub = ResolveOrCreateHub();
            if ( hub == null )
                return;

            if ( hub.Register( _role, cameraComp, _priority, nameof( CameraRoleBinder ) ) )
                _registered = true;
        }

        private void TryUnregister()
        {
            if ( !_registered )
                return;

            var cameraComp = GetComponent<Camera>();
            var hub = ResolveOrCreateHub();
            if ( cameraComp != null && hub != null )
                hub.Unregister( _role, cameraComp );

            _registered = false;
        }

        private Component_CameraHub ResolveOrCreateHub()
        {
            var hub = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_CameraHub>();
            if ( hub != null )
                return hub;

            return null;
//             var globalVar = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_GlobalVar>();
//             if ( globalVar != null )
//             {
//                 hub = globalVar.gameObject.GetComponent<Component_CameraHub>();
//                 if ( hub == null )
//                     hub = globalVar.gameObject.AddComponent<Component_CameraHub>();
//             }
//
//             if ( hub != null )
//                 return hub;
//
// #if UNITY_2023_1_OR_NEWER
//             return Object.FindFirstObjectByType<Component_CameraHub>();
// #else
//             return Object.FindObjectOfType<Component_CameraHub>();
// #endif
        }
        
        [SerializeField] private CameraRole _role = CameraRole.Custom;
        [SerializeField] private int _priority = 0;
        [SerializeField] private bool _autoRegister = true;
        
        public CameraRole Role => _role;
        public int Priority => _priority;
        private bool _registered = false;
    }
}
