using System;
using System.Collections.Generic;
using Aquila.CameraSystem;
using Aquila.Toolkit;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// 统一相机管理组件：负责相机注册、查询、基础参数设置与失效恢复。
    /// </summary>
    public class Component_CameraHub : GameFrameworkComponent
    {
        public Camera GetWorldCamera()
        {
            return _mainCamera;
        }

        public bool Register( CameraRole role, Camera camera, int priority = 0, string source = "manual" )
        {
            if ( role == CameraRole.None || camera == null )
                return false;

            if ( !_roleBindings.TryGetValue( role, out var list ) )
            {
                list = new List<CameraBinding>( 0b0011 );
                _roleBindings.Add( role, list );
            }

            bool exists = false;
            for ( int i = 0; i < list.Count; i++ )
            {
                if ( list[i].Camera == camera )
                {
                    list[i].Priority = priority;
                    list[i].Source = source;
                    exists = true;
                    break;
                }
            }

            if ( !exists )
                list.Add( new CameraBinding { Camera = camera, Priority = priority, Source = source } );

            return RefreshRole( role );
        }

        public bool Unregister( CameraRole role, Camera camera )
        {
            if ( role == CameraRole.None || camera == null )
                return false;

            if ( !_roleBindings.TryGetValue( role, out var list ) )
                return false;

            for ( int i = list.Count - 1; i >= 0; i-- )
            {
                if ( list[i].Camera == camera )
                    list.RemoveAt( i );
            }

            return RefreshRole( role );
        }

        public void UnregisterFromAllRoles( Camera camera )
        {
            if ( camera == null )
                return;

            foreach ( var pair in _roleBindings )
            {
                var role = pair.Key;
                var list = pair.Value;
                bool changed = false;
                for ( int i = list.Count - 1; i >= 0; i-- )
                {
                    if ( list[i].Camera == camera )
                    {
                        list.RemoveAt( i );
                        changed = true;
                    }
                }

                if ( changed )
                    RefreshRole( role );
            }
        }

        public bool TryGetCamera( CameraRole role, out Camera camera )
        {
            camera = null;
            if ( role == CameraRole.None )
                return false;

            RefreshRole( role );
            if ( _primaryByRole.TryGetValue( role, out var cached ) && IsCameraAvailable( cached ) )
            {
                camera = cached;
                return true;
            }

            return false;
        }

        public Camera GetCameraOrNull( CameraRole role )
        {
            TryGetCamera( role, out var camera );
            return camera;
        }

        public bool SetPrimaryCamera( CameraRole role, Camera camera, string source = "manual" )
        {
            if ( !Register( role, camera, int.MaxValue, source ) )
                return false;

            SetPrimaryInternal( role, camera );
            return true;
        }

        public bool TryAttachOverlayToBaseCamera(CameraRole role, Camera overlayCamera)
        {
            TryGetCamera(role,out var baseCamera );
            return TryAttachOverlayToBaseCamera(baseCamera, overlayCamera);
        }

        /// <summary>
        /// 将 overlay 相机叠加到 base 相机（URP 相机栈）。
        /// </summary>
        public bool TryAttachOverlayToBaseCamera( Camera baseCamera, Camera overlayCamera )
        {
            if ( baseCamera == null || overlayCamera == null )
                return false;

            if ( baseCamera == overlayCamera )
                return true;

            if ( !baseCamera.isActiveAndEnabled || !overlayCamera.isActiveAndEnabled )
                return false;

            var baseData = baseCamera.GetUniversalAdditionalCameraData();
            var overlayData = overlayCamera.GetUniversalAdditionalCameraData();
            if ( baseData == null || overlayData == null )
                return false;

            if ( overlayData.renderType != CameraRenderType.Overlay )
                overlayData.renderType = CameraRenderType.Overlay;

            var stack = baseData.cameraStack;
            if ( stack == null )
                return false;

            for ( int i = 0; i < stack.Count; i++ )
            {
                if ( stack[i] == overlayCamera )
                    return true;
            }

            stack.Add( overlayCamera );
            return true;
        }

        public bool RefreshRole( CameraRole role )
        {
            if ( role == CameraRole.None )
                return false;

            CleanupInvalidBindings( role );

            Camera best = PickBest( role );
            if ( best == null )
                best = ResolveFallback( role );

            return SetPrimaryInternal( role, best );
        }

        public void RefreshAllRoles()
        {
            foreach ( CameraRole role in Enum.GetValues( typeof( CameraRole ) ) )
            {
                if ( role == CameraRole.None )
                    continue;

                RefreshRole( role );
            }
        }

        public bool SetCameraEnabled( CameraRole role, bool enabled )
        {
            return Apply( role, camera => camera.enabled = enabled );
        }

        public bool SetFieldOfView( CameraRole role, float fieldOfView )
        {
            return Apply( role, camera => camera.fieldOfView = fieldOfView );
        }

        public bool SetOrthographic( CameraRole role, bool orthographic, float orthographicSize = -1f )
        {
            return Apply( role, camera =>
            {
                camera.orthographic = orthographic;
                if ( orthographicSize > 0f )
                    camera.orthographicSize = orthographicSize;
            } );
        }

        public bool SetDepth( CameraRole role, float depth )
        {
            return Apply( role, camera => camera.depth = depth );
        }

        public bool SetCullingMask( CameraRole role, int cullingMask )
        {
            return Apply( role, camera => camera.cullingMask = cullingMask );
        }

        public bool SetClearFlags( CameraRole role, CameraClearFlags clearFlags )
        {
            return Apply( role, camera => camera.clearFlags = clearFlags );
        }

        public bool SetBackgroundColor( CameraRole role, Color color )
        {
            return Apply( role, camera => camera.backgroundColor = color );
        }

        public bool SetViewportRect( CameraRole role, Rect rect )
        {
            return Apply( role, camera => camera.rect = rect );
        }

        public bool SetTransform( CameraRole role, Vector3 position, Vector3 eulerAngles )
        {
            return Apply( role, camera =>
            {
                camera.transform.position = position;
                camera.transform.eulerAngles = eulerAngles;
            } );
        }

        public bool Apply( CameraRole role, Action<Camera> action )
        {
            if ( action == null )
                return false;

            if ( !TryGetCamera( role, out var camera ) )
                return false;

            action.Invoke( camera );
            return true;
        }

        public void ClearRole( CameraRole role )
        {
            if ( _roleBindings.TryGetValue( role, out var list ) )
                list.Clear();

            SetPrimaryInternal( role, null );
        }

        public void ClearAll()
        {
            _roleBindings.Clear();
            _primaryByRole.Clear();
        }

        private Camera PickBest( CameraRole role )
        {
            if ( !_roleBindings.TryGetValue( role, out var list ) || list.Count == 0 )
                return null;

            Camera best = null;
            int bestPriority = int.MinValue;
            for ( int i = 0; i < list.Count; i++ )
            {
                var item = list[i];
                if ( !IsCameraAvailable( item.Camera ) )
                    continue;

                if ( item.Priority >= bestPriority )
                {
                    bestPriority = item.Priority;
                    best = item.Camera;
                }
            }

            return best;
        }

        private Camera ResolveFallback( CameraRole role )
        {
            if ( role == CameraRole.MainWorld )
            {
                if ( _useCameraMainFallback && Camera.main != null && Camera.main.isActiveAndEnabled )
                    return Camera.main;
            }
            else if ( role == CameraRole.UI )
            {
                return FindCameraByTag( _uiCameraTag );
            }
            else if ( role == CameraRole.InfoBoard )
            {
                return FindCameraByTag( _infoBoardCameraTag );
            }

            return null;
        }

        private Camera FindCameraByTag( string tag )
        {
            if ( string.IsNullOrEmpty( tag ) )
                return null;

            var cameras = Camera.allCameras;
            for ( int i = 0; i < cameras.Length; i++ )
            {
                var camera = cameras[i];
                if ( !IsCameraAvailable( camera ) )
                    continue;

                if ( camera.gameObject.tag == tag )
                    return camera;
            }

            return null;
        }

        private void CleanupInvalidBindings( CameraRole role )
        {
            if ( !_roleBindings.TryGetValue( role, out var list ) )
                return;

            for ( int i = list.Count - 1; i >= 0; i-- )
            {
                if ( !IsCameraAvailable( list[i].Camera ) )
                    list.RemoveAt( i );
            }
        }

        private bool SetPrimaryInternal( CameraRole role, Camera camera )
        {
            _primaryByRole.TryGetValue( role, out var old );

            if ( old == camera )
                return camera != null;

            if ( camera == null )
                _primaryByRole.Remove( role );
            else
                _primaryByRole[role] = camera;

            OnRoleCameraChanged?.Invoke( role, camera );
            return camera != null;
        }

        private bool IsCameraAvailable( Camera camera )
        {
            return camera != null && camera.gameObject != null && camera.isActiveAndEnabled;
        }

        private void OnSceneLoaded( Scene scene, LoadSceneMode mode )
        {
            RefreshAllRoles();
        }

        private void OnSceneUnloaded( Scene scene )
        {
            RefreshAllRoles();
        }

        private void Start()
        {
            if ( !TryGetCamera( CameraRole.MainWorld, out _mainCamera ) )
                Tools.Logger.Warning( "<color=yellow>MainWorld camera not found at start.</color>" );
        }

        protected override void Awake()
        {
            base.Awake();
            RefreshAllRoles();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private sealed class CameraBinding
        {
            public Camera Camera;
            public int Priority;
            public string Source;
        }

        private readonly Dictionary<CameraRole, List<CameraBinding>> _roleBindings = new Dictionary<CameraRole, List<CameraBinding>>( 8 );

        private readonly Dictionary<CameraRole, Camera> _primaryByRole = new Dictionary<CameraRole, Camera>( 8 );

        [SerializeField] private bool _useCameraMainFallback = true;
        [SerializeField] private string _uiCameraTag = "UICamera";
        [SerializeField] private string _infoBoardCameraTag = "InfoBoardCamera";
        public event Action<CameraRole, Camera> OnRoleCameraChanged;
        private Camera _mainCamera;
    }
}
