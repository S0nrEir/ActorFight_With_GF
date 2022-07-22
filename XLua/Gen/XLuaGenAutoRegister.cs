#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
        
        
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Tutorial.BaseClass), TutorialBaseClassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.TestEnum), TutorialTestEnumWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClass), TutorialDerivedClassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.ICalc), TutorialICalcWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClassExtensions), TutorialDerivedClassExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClass.TestEnumInner), TutorialDerivedClassTestEnumInnerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Animator), UnityEngineAnimatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.StateMachineBehaviour), UnityEngineStateMachineBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Animation), UnityEngineAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationState), UnityEngineAnimationStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationEvent), UnityEngineAnimationEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationClip), UnityEngineAnimationClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorClipInfo), UnityEngineAnimatorClipInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorStateInfo), UnityEngineAnimatorStateInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorTransitionInfo), UnityEngineAnimatorTransitionInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MatchTargetWeightMask), UnityEngineMatchTargetWeightMaskWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorControllerParameter), UnityEngineAnimatorControllerParameterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorOverrideController), UnityEngineAnimatorOverrideControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorUtility), UnityEngineAnimatorUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Avatar), UnityEngineAvatarWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SkeletonBone), UnityEngineSkeletonBoneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HumanLimit), UnityEngineHumanLimitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HumanBone), UnityEngineHumanBoneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HumanDescription), UnityEngineHumanDescriptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AvatarBuilder), UnityEngineAvatarBuilderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AvatarMask), UnityEngineAvatarMaskWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HumanPose), UnityEngineHumanPoseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HumanPoseHandler), UnityEngineHumanPoseHandlerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HumanTrait), UnityEngineHumanTraitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RuntimeAnimatorController), UnityEngineRuntimeAnimatorControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AssetBundle), UnityEngineAssetBundleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AssetBundleCreateRequest), UnityEngineAssetBundleCreateRequestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AssetBundleManifest), UnityEngineAssetBundleManifestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AssetBundleRecompressOperation), UnityEngineAssetBundleRecompressOperationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AssetBundleRequest), UnityEngineAssetBundleRequestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BuildCompression), UnityEngineBuildCompressionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioSettings), UnityEngineAudioSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioSettings.Mobile), UnityEngineAudioSettingsMobileWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioSource), UnityEngineAudioSourceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioLowPassFilter), UnityEngineAudioLowPassFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioHighPassFilter), UnityEngineAudioHighPassFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioReverbFilter), UnityEngineAudioReverbFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioConfiguration), UnityEngineAudioConfigurationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioClip), UnityEngineAudioClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioBehaviour), UnityEngineAudioBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioListener), UnityEngineAudioListenerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioReverbZone), UnityEngineAudioReverbZoneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioDistortionFilter), UnityEngineAudioDistortionFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioEchoFilter), UnityEngineAudioEchoFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioChorusFilter), UnityEngineAudioChorusFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Microphone), UnityEngineMicrophoneWrap.__Register);
        
        }
        
        static void wrapInit1(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioRenderer), UnityEngineAudioRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WebCamDevice), UnityEngineWebCamDeviceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WebCamTexture), UnityEngineWebCamTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ClothSphereColliderPair), UnityEngineClothSphereColliderPairWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ClothSkinningCoefficient), UnityEngineClothSkinningCoefficientWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Cloth), UnityEngineClothWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ClusterSerialization), UnityEngineClusterSerializationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SortingLayer), UnityEngineSortingLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Keyframe), UnityEngineKeyframeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationCurve), UnityEngineAnimationCurveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Application), UnityEngineApplicationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CachedAssetBundle), UnityEngineCachedAssetBundleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Cache), UnityEngineCacheWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Caching), UnityEngineCachingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Camera), UnityEngineCameraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Camera.GateFitParameters), UnityEngineCameraGateFitParametersWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BoundingSphere), UnityEngineBoundingSphereWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CullingGroupEvent), UnityEngineCullingGroupEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CullingGroup), UnityEngineCullingGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.FlareLayer), UnityEngineFlareLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ReflectionProbe), UnityEngineReflectionProbeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CrashReport), UnityEngineCrashReportWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Debug), UnityEngineDebugWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ExposedPropertyResolver), UnityEngineExposedPropertyResolverWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Bounds), UnityEngineBoundsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BoundsInt), UnityEngineBoundsIntWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GeometryUtility), UnityEngineGeometryUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Plane), UnityEnginePlaneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Ray), UnityEngineRayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Ray2D), UnityEngineRay2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Rect), UnityEngineRectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RectInt), UnityEngineRectIntWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RectOffset), UnityEngineRectOffsetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.DynamicGI), UnityEngineDynamicGIWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LightingSettings), UnityEngineLightingSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BillboardAsset), UnityEngineBillboardAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BillboardRenderer), UnityEngineBillboardRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Display), UnityEngineDisplayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SleepTimeout), UnityEngineSleepTimeoutWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Screen), UnityEngineScreenWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RenderBuffer), UnityEngineRenderBufferWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Graphics), UnityEngineGraphicsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GL), UnityEngineGLWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ScalableBufferManager), UnityEngineScalableBufferManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.FrameTiming), UnityEngineFrameTimingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.FrameTimingManager), UnityEngineFrameTimingManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LightmapData), UnityEngineLightmapDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LightmapSettings), UnityEngineLightmapSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LightProbes), UnityEngineLightProbesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HDROutputSettings), UnityEngineHDROutputSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Resolution), UnityEngineResolutionWrap.__Register);
        
        }
        
        static void wrapInit2(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.RenderTargetSetup), UnityEngineRenderTargetSetupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.QualitySettings), UnityEngineQualitySettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RendererExtensions), UnityEngineRendererExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ImageEffectTransformsToLDR), UnityEngineImageEffectTransformsToLDRWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ImageEffectAllowedInSceneView), UnityEngineImageEffectAllowedInSceneViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ImageEffectOpaque), UnityEngineImageEffectOpaqueWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ImageEffectAfterScale), UnityEngineImageEffectAfterScaleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ImageEffectUsesCommandBuffer), UnityEngineImageEffectUsesCommandBufferWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Mesh), UnityEngineMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Mesh.MeshData), UnityEngineMeshMeshDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Mesh.MeshDataArray), UnityEngineMeshMeshDataArrayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Renderer), UnityEngineRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Projector), UnityEngineProjectorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Shader), UnityEngineShaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TrailRenderer), UnityEngineTrailRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LineRenderer), UnityEngineLineRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MaterialPropertyBlock), UnityEngineMaterialPropertyBlockWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RenderSettings), UnityEngineRenderSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Material), UnityEngineMaterialWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GraphicsBuffer), UnityEngineGraphicsBufferWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.OcclusionPortal), UnityEngineOcclusionPortalWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.OcclusionArea), UnityEngineOcclusionAreaWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Flare), UnityEngineFlareWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LensFlare), UnityEngineLensFlareWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LightBakingOutput), UnityEngineLightBakingOutputWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Light), UnityEngineLightWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Skybox), UnityEngineSkyboxWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MeshFilter), UnityEngineMeshFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LightProbeProxyVolume), UnityEngineLightProbeProxyVolumeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SkinnedMeshRenderer), UnityEngineSkinnedMeshRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MeshRenderer), UnityEngineMeshRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LightProbeGroup), UnityEngineLightProbeGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LineUtility), UnityEngineLineUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LOD), UnityEngineLODWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LODGroup), UnityEngineLODGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BoneWeight), UnityEngineBoneWeightWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BoneWeight1), UnityEngineBoneWeight1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CombineInstance), UnityEngineCombineInstanceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Texture), UnityEngineTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Texture2D), UnityEngineTexture2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Cubemap), UnityEngineCubemapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Texture3D), UnityEngineTexture3DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Texture2DArray), UnityEngineTexture2DArrayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CubemapArray), UnityEngineCubemapArrayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SparseTexture), UnityEngineSparseTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RenderTexture), UnityEngineRenderTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CustomRenderTextureUpdateZone), UnityEngineCustomRenderTextureUpdateZoneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CustomRenderTexture), UnityEngineCustomRenderTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RenderTextureDescriptor), UnityEngineRenderTextureDescriptorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Hash128), UnityEngineHash128Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HashUtilities), UnityEngineHashUtilitiesWrap.__Register);
        
        }
        
        static void wrapInit3(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.HashUnsafeUtilities), UnityEngineHashUnsafeUtilitiesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Logger), UnityEngineLoggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Color), UnityEngineColorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Color32), UnityEngineColor32Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ColorUtility), UnityEngineColorUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GradientColorKey), UnityEngineGradientColorKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GradientAlphaKey), UnityEngineGradientAlphaKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Gradient), UnityEngineGradientWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.FrustumPlanes), UnityEngineFrustumPlanesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Matrix4x4), UnityEngineMatrix4x4Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector3), UnityEngineVector3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Quaternion), UnityEngineQuaternionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Mathf), UnityEngineMathfWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector2), UnityEngineVector2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector2Int), UnityEngineVector2IntWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector3Int), UnityEngineVector3IntWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector4), UnityEngineVector4Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PlayerPrefsException), UnityEnginePlayerPrefsExceptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PlayerPrefs), UnityEnginePlayerPrefsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PropertyName), UnityEnginePropertyNameWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Random), UnityEngineRandomWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Random.State), UnityEngineRandomStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ResourceRequest), UnityEngineResourceRequestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Resources), UnityEngineResourcesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AsyncOperation), UnityEngineAsyncOperationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ExecuteAlways), UnityEngineExecuteAlwaysWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.DefaultExecutionOrder), UnityEngineDefaultExecutionOrderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Behaviour), UnityEngineBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Component), UnityEngineComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Coroutine), UnityEngineCoroutineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CustomYieldInstruction), UnityEngineCustomYieldInstructionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GameObject), UnityEngineGameObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LayerMask), UnityEngineLayerMaskWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MonoBehaviour), UnityEngineMonoBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RangeInt), UnityEngineRangeIntWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ScriptableObject), UnityEngineScriptableObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.StackTraceUtility), UnityEngineStackTraceUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UnityException), UnityEngineUnityExceptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MissingComponentException), UnityEngineMissingComponentExceptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UnassignedReferenceException), UnityEngineUnassignedReferenceExceptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MissingReferenceException), UnityEngineMissingReferenceExceptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TextAsset), UnityEngineTextAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Object), UnityEngineObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForEndOfFrame), UnityEngineWaitForEndOfFrameWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForFixedUpdate), UnityEngineWaitForFixedUpdateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForSeconds), UnityEngineWaitForSecondsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForSecondsRealtime), UnityEngineWaitForSecondsRealtimeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitUntil), UnityEngineWaitUntilWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitWhile), UnityEngineWaitWhileWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.YieldInstruction), UnityEngineYieldInstructionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Security), UnityEngineSecurityWrap.__Register);
        
        }
        
        static void wrapInit4(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.SerializeReference), UnityEngineSerializeReferenceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PreferBinarySerialization), UnityEnginePreferBinarySerializationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ComputeBuffer), UnityEngineComputeBufferWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ComputeShader), UnityEngineComputeShaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Snapping), UnityEngineSnappingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.StaticBatchingUtility), UnityEngineStaticBatchingUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SystemInfo), UnityEngineSystemInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Time), UnityEngineTimeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UnityEventQueueSystem), UnityEngineUnityEventQueueSystemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Pose), UnityEnginePoseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.DrivenRectTransformTracker), UnityEngineDrivenRectTransformTrackerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RectTransform), UnityEngineRectTransformWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Transform), UnityEngineTransformWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SpriteRenderer), UnityEngineSpriteRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SecondarySpriteTexture), UnityEngineSecondarySpriteTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Sprite), UnityEngineSpriteWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Grid), UnityEngineGridWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GridLayout), UnityEngineGridLayoutWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Event), UnityEngineEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ImageConversion), UnityEngineImageConversionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Touch), UnityEngineTouchWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AccelerationEvent), UnityEngineAccelerationEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Gyroscope), UnityEngineGyroscopeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LocationInfo), UnityEngineLocationInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LocationService), UnityEngineLocationServiceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Compass), UnityEngineCompassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Input), UnityEngineInputWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JsonUtility), UnityEngineJsonUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LocalizationAsset), UnityEngineLocalizationAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem), UnityEngineParticleSystemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.MinMaxCurve), UnityEngineParticleSystemMinMaxCurveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.MainModule), UnityEngineParticleSystemMainModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.EmissionModule), UnityEngineParticleSystemEmissionModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.ShapeModule), UnityEngineParticleSystemShapeModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.SubEmittersModule), UnityEngineParticleSystemSubEmittersModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.TextureSheetAnimationModule), UnityEngineParticleSystemTextureSheetAnimationModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.Particle), UnityEngineParticleSystemParticleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.Burst), UnityEngineParticleSystemBurstWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.MinMaxGradient), UnityEngineParticleSystemMinMaxGradientWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.EmitParams), UnityEngineParticleSystemEmitParamsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.PlaybackState), UnityEngineParticleSystemPlaybackStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.Trails), UnityEngineParticleSystemTrailsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.VelocityOverLifetimeModule), UnityEngineParticleSystemVelocityOverLifetimeModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.LimitVelocityOverLifetimeModule), UnityEngineParticleSystemLimitVelocityOverLifetimeModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.InheritVelocityModule), UnityEngineParticleSystemInheritVelocityModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.LifetimeByEmitterSpeedModule), UnityEngineParticleSystemLifetimeByEmitterSpeedModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.ForceOverLifetimeModule), UnityEngineParticleSystemForceOverLifetimeModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.ColorOverLifetimeModule), UnityEngineParticleSystemColorOverLifetimeModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.ColorBySpeedModule), UnityEngineParticleSystemColorBySpeedModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.SizeOverLifetimeModule), UnityEngineParticleSystemSizeOverLifetimeModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.SizeBySpeedModule), UnityEngineParticleSystemSizeBySpeedModuleWrap.__Register);
        
        }
        
        static void wrapInit5(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.RotationOverLifetimeModule), UnityEngineParticleSystemRotationOverLifetimeModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.RotationBySpeedModule), UnityEngineParticleSystemRotationBySpeedModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.ExternalForcesModule), UnityEngineParticleSystemExternalForcesModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.NoiseModule), UnityEngineParticleSystemNoiseModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.CollisionModule), UnityEngineParticleSystemCollisionModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.TriggerModule), UnityEngineParticleSystemTriggerModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.LightsModule), UnityEngineParticleSystemLightsModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.TrailModule), UnityEngineParticleSystemTrailModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem.CustomDataModule), UnityEngineParticleSystemCustomDataModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticlePhysicsExtensions), UnityEngineParticlePhysicsExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleCollisionEvent), UnityEngineParticleCollisionEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystemRenderer), UnityEngineParticleSystemRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystemForceField), UnityEngineParticleSystemForceFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WheelFrictionCurve), UnityEngineWheelFrictionCurveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SoftJointLimit), UnityEngineSoftJointLimitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SoftJointLimitSpring), UnityEngineSoftJointLimitSpringWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointDrive), UnityEngineJointDriveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointMotor), UnityEngineJointMotorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointSpring), UnityEngineJointSpringWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointLimits), UnityEngineJointLimitsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ControllerColliderHit), UnityEngineControllerColliderHitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Collision), UnityEngineCollisionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicMaterial), UnityEnginePhysicMaterialWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RaycastHit), UnityEngineRaycastHitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Rigidbody), UnityEngineRigidbodyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Collider), UnityEngineColliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CharacterController), UnityEngineCharacterControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MeshCollider), UnityEngineMeshColliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CapsuleCollider), UnityEngineCapsuleColliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BoxCollider), UnityEngineBoxColliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SphereCollider), UnityEngineSphereColliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ConstantForce), UnityEngineConstantForceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Joint), UnityEngineJointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HingeJoint), UnityEngineHingeJointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SpringJoint), UnityEngineSpringJointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.FixedJoint), UnityEngineFixedJointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CharacterJoint), UnityEngineCharacterJointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ConfigurableJoint), UnityEngineConfigurableJointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ContactPoint), UnityEngineContactPointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicsScene), UnityEnginePhysicsSceneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicsSceneExtensions), UnityEnginePhysicsSceneExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ArticulationDrive), UnityEngineArticulationDriveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ArticulationReducedSpace), UnityEngineArticulationReducedSpaceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ArticulationBody), UnityEngineArticulationBodyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Physics), UnityEnginePhysicsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RaycastCommand), UnityEngineRaycastCommandWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SpherecastCommand), UnityEngineSpherecastCommandWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CapsulecastCommand), UnityEngineCapsulecastCommandWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BoxcastCommand), UnityEngineBoxcastCommandWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicsScene2D), UnityEnginePhysicsScene2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicsSceneExtensions2D), UnityEnginePhysicsSceneExtensions2DWrap.__Register);
        
        }
        
        static void wrapInit6(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.Physics2D), UnityEnginePhysics2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ColliderDistance2D), UnityEngineColliderDistance2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ContactFilter2D), UnityEngineContactFilter2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Collision2D), UnityEngineCollision2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ContactPoint2D), UnityEngineContactPoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointAngleLimits2D), UnityEngineJointAngleLimits2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointTranslationLimits2D), UnityEngineJointTranslationLimits2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointMotor2D), UnityEngineJointMotor2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.JointSuspension2D), UnityEngineJointSuspension2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RaycastHit2D), UnityEngineRaycastHit2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicsJobOptions2D), UnityEnginePhysicsJobOptions2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Rigidbody2D), UnityEngineRigidbody2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Collider2D), UnityEngineCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CircleCollider2D), UnityEngineCircleCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CapsuleCollider2D), UnityEngineCapsuleCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.EdgeCollider2D), UnityEngineEdgeCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BoxCollider2D), UnityEngineBoxCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PolygonCollider2D), UnityEnginePolygonCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CompositeCollider2D), UnityEngineCompositeCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Joint2D), UnityEngineJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnchoredJoint2D), UnityEngineAnchoredJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SpringJoint2D), UnityEngineSpringJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.DistanceJoint2D), UnityEngineDistanceJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.FrictionJoint2D), UnityEngineFrictionJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.HingeJoint2D), UnityEngineHingeJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RelativeJoint2D), UnityEngineRelativeJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SliderJoint2D), UnityEngineSliderJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TargetJoint2D), UnityEngineTargetJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.FixedJoint2D), UnityEngineFixedJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WheelJoint2D), UnityEngineWheelJoint2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Effector2D), UnityEngineEffector2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AreaEffector2D), UnityEngineAreaEffector2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.BuoyancyEffector2D), UnityEngineBuoyancyEffector2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PointEffector2D), UnityEnginePointEffector2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PlatformEffector2D), UnityEnginePlatformEffector2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SurfaceEffector2D), UnityEngineSurfaceEffector2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicsUpdateBehaviour2D), UnityEnginePhysicsUpdateBehaviour2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ConstantForce2D), UnityEngineConstantForce2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PhysicsMaterial2D), UnityEnginePhysicsMaterial2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ScreenCapture), UnityEngineScreenCaptureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SpriteMask), UnityEngineSpriteMaskWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.StreamingController), UnityEngineStreamingControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.IntegratedSubsystemDescriptor), UnityEngineIntegratedSubsystemDescriptorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SubsystemDescriptor), UnityEngineSubsystemDescriptorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SubsystemManager), UnityEngineSubsystemManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.IntegratedSubsystem), UnityEngineIntegratedSubsystemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Subsystem), UnityEngineSubsystemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.PatchExtents), UnityEnginePatchExtentsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TextGenerationSettings), UnityEngineTextGenerationSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TextMesh), UnityEngineTextMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CharacterInfo), UnityEngineCharacterInfoWrap.__Register);
        
        }
        
        static void wrapInit7(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.UICharInfo), UnityEngineUICharInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UILineInfo), UnityEngineUILineInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UIVertex), UnityEngineUIVertexWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Font), UnityEngineFontWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GridBrushBase), UnityEngineGridBrushBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CanvasGroup), UnityEngineCanvasGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CanvasRenderer), UnityEngineCanvasRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RectTransformUtility), UnityEngineRectTransformUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Canvas), UnityEngineCanvasWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UISystemProfilerApi), UnityEngineUISystemProfilerApiWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RemoteSettings), UnityEngineRemoteSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RemoteConfigSettings), UnityEngineRemoteConfigSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WWWForm), UnityEngineWWWFormWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WheelHit), UnityEngineWheelHitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WheelCollider), UnityEngineWheelColliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WindZone), UnityEngineWindZoneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.ObjectPool.Aquila_Object_Base), AquilaObjectPoolAquila_Object_BaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.AnimationTriggers), UnityEngineUIAnimationTriggersWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Button), UnityEngineUIButtonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.CanvasUpdateRegistry), UnityEngineUICanvasUpdateRegistryWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ColorBlock), UnityEngineUIColorBlockWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ClipperRegistry), UnityEngineUIClipperRegistryWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Clipping), UnityEngineUIClippingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.DefaultControls), UnityEngineUIDefaultControlsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Dropdown), UnityEngineUIDropdownWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.FontData), UnityEngineUIFontDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.FontUpdateTracker), UnityEngineUIFontUpdateTrackerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Graphic), UnityEngineUIGraphicWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.GraphicRaycaster), UnityEngineUIGraphicRaycasterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.GraphicRegistry), UnityEngineUIGraphicRegistryWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Image), UnityEngineUIImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.InputField), UnityEngineUIInputFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.AspectRatioFitter), UnityEngineUIAspectRatioFitterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.CanvasScaler), UnityEngineUICanvasScalerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ContentSizeFitter), UnityEngineUIContentSizeFitterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.GridLayoutGroup), UnityEngineUIGridLayoutGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.HorizontalLayoutGroup), UnityEngineUIHorizontalLayoutGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.HorizontalOrVerticalLayoutGroup), UnityEngineUIHorizontalOrVerticalLayoutGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.LayoutElement), UnityEngineUILayoutElementWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.LayoutGroup), UnityEngineUILayoutGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.LayoutRebuilder), UnityEngineUILayoutRebuilderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.LayoutUtility), UnityEngineUILayoutUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.VerticalLayoutGroup), UnityEngineUIVerticalLayoutGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Mask), UnityEngineUIMaskWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.MaskUtilities), UnityEngineUIMaskUtilitiesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.MaskableGraphic), UnityEngineUIMaskableGraphicWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Navigation), UnityEngineUINavigationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.RawImage), UnityEngineUIRawImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.RectMask2D), UnityEngineUIRectMask2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ScrollRect), UnityEngineUIScrollRectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Scrollbar), UnityEngineUIScrollbarWrap.__Register);
        
        }
        
        static void wrapInit8(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Selectable), UnityEngineUISelectableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Slider), UnityEngineUISliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.SpriteState), UnityEngineUISpriteStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.StencilMaterial), UnityEngineUIStencilMaterialWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Text), UnityEngineUITextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Toggle), UnityEngineUIToggleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ToggleGroup), UnityEngineUIToggleGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.VertexHelper), UnityEngineUIVertexHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.BaseMeshEffect), UnityEngineUIBaseMeshEffectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Outline), UnityEngineUIOutlineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.PositionAsUV1), UnityEngineUIPositionAsUV1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Shadow), UnityEngineUIShadowWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Button.ButtonClickedEvent), UnityEngineUIButtonButtonClickedEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.DefaultControls.Resources), UnityEngineUIDefaultControlsResourcesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Dropdown.OptionData), UnityEngineUIDropdownOptionDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Dropdown.OptionDataList), UnityEngineUIDropdownOptionDataListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Dropdown.DropdownEvent), UnityEngineUIDropdownDropdownEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.InputField.SubmitEvent), UnityEngineUIInputFieldSubmitEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.InputField.OnChangeEvent), UnityEngineUIInputFieldOnChangeEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.MaskableGraphic.CullStateChangedEvent), UnityEngineUIMaskableGraphicCullStateChangedEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ScrollRect.ScrollRectEvent), UnityEngineUIScrollRectScrollRectEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Scrollbar.ScrollEvent), UnityEngineUIScrollbarScrollEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Slider.SliderEvent), UnityEngineUISliderSliderEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Toggle.ToggleEvent), UnityEngineUIToggleToggleEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(StartUp), StartUpWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(LuaCallCs), LuaCallCsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.CSCallLua), TutorialCSCallLuaWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.ByFile), TutorialByFileWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.ByString), TutorialByStringWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.CustomLoader), TutorialCustomLoaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.Param1), TutorialParam1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Bright.Common.StringUtil), BrightCommonStringUtilWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Bright.Serialization.SerializationException), BrightSerializationSerializationExceptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Bright.Serialization.SegmentSaveState), BrightSerializationSegmentSaveStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Bright.Serialization.ByteBuf), BrightSerializationByteBufWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Bright.Serialization.ByteBufExtensions), BrightSerializationByteBufExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Bright.Config.BeanBase), BrightConfigBeanBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.Tables), cfgTablesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test2.Rectangle), cfgtest2RectangleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Circle), cfgtestCircleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Color), cfgtestColorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.CompactString), cfgtestCompactStringWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.CompositeJsonTable1), cfgtestCompositeJsonTable1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.CompositeJsonTable2), cfgtestCompositeJsonTable2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.CompositeJsonTable3), cfgtestCompositeJsonTable3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DateTimeRange), cfgtestDateTimeRangeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Decorator), cfgtestDecoratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DefineFromExcel), cfgtestDefineFromExcelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DefineFromExcel2), cfgtestDefineFromExcel2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DefineFromExcelOne), cfgtestDefineFromExcelOneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoD2), cfgtestDemoD2Wrap.__Register);
        
        }
        
        static void wrapInit9(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoD3), cfgtestDemoD3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoD5), cfgtestDemoD5Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoDynamic), cfgtestDemoDynamicWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoE1), cfgtestDemoE1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoE2), cfgtestDemoE2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoGroup), cfgtestDemoGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoPrimitiveTypesTable), cfgtestDemoPrimitiveTypesTableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoSingletonType), cfgtestDemoSingletonTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoType1), cfgtestDemoType1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DemoType2), cfgtestDemoType2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.DetectEncoding), cfgtestDetectEncodingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Equipment), cfgtestEquipmentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.ExcelFromJson), cfgtestExcelFromJsonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.ExcelFromJsonMultiRow), cfgtestExcelFromJsonMultiRowWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Foo), cfgtestFooWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.H1), cfgtestH1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.H2), cfgtestH2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.InnerGroup), cfgtestInnerGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Item), cfgtestItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.ItemBase), cfgtestItemBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.MultiIndexList), cfgtestMultiIndexListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.MultiRowRecord), cfgtestMultiRowRecordWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.MultiRowTitle), cfgtestMultiRowTitleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.MultiRowType1), cfgtestMultiRowType1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.MultiRowType2), cfgtestMultiRowType2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.MultiRowType3), cfgtestMultiRowType3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.MultiUnionIndexList), cfgtestMultiUnionIndexListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.NotIndexList), cfgtestNotIndexListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.SepBean1), cfgtestSepBean1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.SepVector), cfgtestSepVectorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Shape), cfgtestShapeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbCompositeJsonTable1), cfgtestTbCompositeJsonTable1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbCompositeJsonTable2), cfgtestTbCompositeJsonTable2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbCompositeJsonTable3), cfgtestTbCompositeJsonTable3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDataFromMisc), cfgtestTbDataFromMiscWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDefineFromExcel), cfgtestTbDefineFromExcelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDefineFromExcel2), cfgtestTbDefineFromExcel2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDefineFromExcelOne), cfgtestTbDefineFromExcelOneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDemoGroup), cfgtestTbDemoGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDemoGroupDefineFromExcel), cfgtestTbDemoGroupDefineFromExcelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDemoGroup_C), cfgtestTbDemoGroup_CWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDemoGroup_E), cfgtestTbDemoGroup_EWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDemoGroup_S), cfgtestTbDemoGroup_SWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDemoPrimitive), cfgtestTbDemoPrimitiveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbDetectCsvEncoding), cfgtestTbDetectCsvEncodingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbExcelFromJson), cfgtestTbExcelFromJsonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbExcelFromJsonMultiRow), cfgtestTbExcelFromJsonMultiRowWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbFullTypes), cfgtestTbFullTypesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbItem2), cfgtestTbItem2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbMultiIndexList), cfgtestTbMultiIndexListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbMultiRowRecord), cfgtestTbMultiRowRecordWrap.__Register);
        
        }
        
        static void wrapInit10(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(cfg.test.TbMultiRowTitle), cfgtestTbMultiRowTitleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbMultiUnionIndexList), cfgtestTbMultiUnionIndexListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbNotIndexList), cfgtestTbNotIndexListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbSingleton), cfgtestTbSingletonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestBeRef), cfgtestTbTestBeRefWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestBeRef2), cfgtestTbTestBeRef2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestDesc), cfgtestTbTestDescWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestExcelBean), cfgtestTbTestExcelBeanWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestExternalType), cfgtestTbTestExternalTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestGlobal), cfgtestTbTestGlobalWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestIndex), cfgtestTbTestIndexWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestMap), cfgtestTbTestMapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestMultiColumn), cfgtestTbTestMultiColumnWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestNull), cfgtestTbTestNullWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestRef), cfgtestTbTestRefWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestScriptableObject), cfgtestTbTestScriptableObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestSep), cfgtestTbTestSepWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestSet), cfgtestTbTestSetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestSize), cfgtestTbTestSizeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TbTestString), cfgtestTbTestStringWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.Test3), cfgtestTest3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestBeRef), cfgtestTestBeRefWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestDesc), cfgtestTestDescWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestExcelBean1), cfgtestTestExcelBean1Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestExternalType), cfgtestTestExternalTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestGlobal), cfgtestTestGlobalWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestIndex), cfgtestTestIndexWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestMap), cfgtestTestMapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestMultiColumn), cfgtestTestMultiColumnWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestNull), cfgtestTestNullWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestRef), cfgtestTestRefWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestRow), cfgtestTestRowWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestScriptableObject), cfgtestTestScriptableObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestSep), cfgtestTestSepWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestSet), cfgtestTestSetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestSize), cfgtestTestSizeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.TestString), cfgtestTestStringWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.test.login.RoleInfo), cfgtestloginRoleInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.tag.TbTestTag), cfgtagTbTestTagWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.tag.TestTag), cfgtagTestTagWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.role.BonusInfo), cfgroleBonusInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.role.DistinctBonusInfos), cfgroleDistinctBonusInfosWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.role.LevelBonus), cfgroleLevelBonusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.role.LevelExpAttr), cfgroleLevelExpAttrWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.role.TbRoleLevelBonusCoefficient), cfgroleTbRoleLevelBonusCoefficientWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.role.TbRoleLevelExpAttr), cfgroleTbRoleLevelExpAttrWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.mail.GlobalMail), cfgmailGlobalMailWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.mail.SystemMail), cfgmailSystemMailWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.mail.TbGlobalMail), cfgmailTbGlobalMailWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.mail.TbSystemMail), cfgmailTbSystemMailWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.l10n.L10NDemo), cfgl10nL10NDemoWrap.__Register);
        
        }
        
        static void wrapInit11(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(cfg.l10n.PatchDemo), cfgl10nPatchDemoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.l10n.TbL10NDemo), cfgl10nTbL10NDemoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.l10n.TbPatchDemo), cfgl10nTbPatchDemoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.ChooseOneBonus), cfgitemChooseOneBonusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.Clothes), cfgitemClothesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.DesignDrawing), cfgitemDesignDrawingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.Dymmy), cfgitemDymmyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.InteractionItem), cfgitemInteractionItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.Item), cfgitemItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.ItemExtra), cfgitemItemExtraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.ItemFunction), cfgitemItemFunctionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.TbItem), cfgitemTbItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.TbItemExtra), cfgitemTbItemExtraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.TbItemFunc), cfgitemTbItemFuncWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.item.TreasureBox), cfgitemTreasureBoxWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.CodeInfo), cfgerrorCodeInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.ErrorInfo), cfgerrorErrorInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.ErrorStyle), cfgerrorErrorStyleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.ErrorStyleDlgOk), cfgerrorErrorStyleDlgOkWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.ErrorStyleDlgOkCancel), cfgerrorErrorStyleDlgOkCancelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.ErrorStyleMsgbox), cfgerrorErrorStyleMsgboxWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.ErrorStyleTip), cfgerrorErrorStyleTipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.TbCodeInfo), cfgerrorTbCodeInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.error.TbErrorInfo), cfgerrorTbErrorInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.cost.Cost), cfgcostCostWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.cost.CostCurrencies), cfgcostCostCurrenciesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.cost.CostCurrency), cfgcostCostCurrencyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.cost.CostItem), cfgcostCostItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.cost.CostItems), cfgcostCostItemsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.cost.CostOneItem), cfgcostCostOneItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.BoolRoleCondition), cfgconditionBoolRoleConditionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.ClothesPropertyScoreGreaterThan), cfgconditionClothesPropertyScoreGreaterThanWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.Condition), cfgconditionConditionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.ContainsItem), cfgconditionContainsItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.GenderLimit), cfgconditionGenderLimitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.MaxLevel), cfgconditionMaxLevelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.MinLevel), cfgconditionMinLevelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.MinMaxLevel), cfgconditionMinMaxLevelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.MultiRoleCondition), cfgconditionMultiRoleConditionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.RoleCondition), cfgconditionRoleConditionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.condition.TimeRange), cfgconditionTimeRangeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.common.DateTimeRange), cfgcommonDateTimeRangeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.common.GlobalConfig), cfgcommonGlobalConfigWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.common.TbGlobalConfig), cfgcommonTbGlobalConfigWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.Bonus), cfgbonusBonusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.CoefficientItem), cfgbonusCoefficientItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.DropBonus), cfgbonusDropBonusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.DropInfo), cfgbonusDropInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.Item), cfgbonusItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.Items), cfgbonusItemsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.MultiBonus), cfgbonusMultiBonusWrap.__Register);
        
        }
        
        static void wrapInit12(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(cfg.bonus.OneItem), cfgbonusOneItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.OneItems), cfgbonusOneItemsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.ProbabilityBonus), cfgbonusProbabilityBonusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.ProbabilityBonusInfo), cfgbonusProbabilityBonusInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.ProbabilityItemInfo), cfgbonusProbabilityItemInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.ProbabilityItems), cfgbonusProbabilityItemsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.ShowItemInfo), cfgbonusShowItemInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.TbDrop), cfgbonusTbDropWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.WeightBonus), cfgbonusWeightBonusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.WeightBonusInfo), cfgbonusWeightBonusInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.WeightItemInfo), cfgbonusWeightItemInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.bonus.WeightItems), cfgbonusWeightItemsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.AbstraceMethod), cfgblueprintAbstraceMethodWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.BlueprintMethod), cfgblueprintBlueprintMethodWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.Clazz), cfgblueprintClazzWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.EnumClazz), cfgblueprintEnumClazzWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.EnumField), cfgblueprintEnumFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.ExternalMethod), cfgblueprintExternalMethodWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.Field), cfgblueprintFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.Interface), cfgblueprintInterfaceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.Method), cfgblueprintMethodWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.NormalClazz), cfgblueprintNormalClazzWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.ParamInfo), cfgblueprintParamInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.blueprint.TbClazz), cfgblueprintTbClazzWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.BehaviorTree), cfgaiBehaviorTreeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.BinaryOperator), cfgaiBinaryOperatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.Blackboard), cfgaiBlackboardWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.BlackboardKey), cfgaiBlackboardKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.BlackboardKeyData), cfgaiBlackboardKeyDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.ChooseSkill), cfgaiChooseSkillWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.ChooseTarget), cfgaiChooseTargetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.ComposeNode), cfgaiComposeNodeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.DebugPrint), cfgaiDebugPrintWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.Decorator), cfgaiDecoratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.DistanceLessThan), cfgaiDistanceLessThanWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.ExecuteTimeStatistic), cfgaiExecuteTimeStatisticWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.FloatKeyData), cfgaiFloatKeyDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.FlowNode), cfgaiFlowNodeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.GetOwnerPlayer), cfgaiGetOwnerPlayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.IntKeyData), cfgaiIntKeyDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.IsAtLocation), cfgaiIsAtLocationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.IsNotSet), cfgaiIsNotSetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.IsSet), cfgaiIsSetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.KeepFaceTarget), cfgaiKeepFaceTargetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.KeyData), cfgaiKeyDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.KeyQueryOperator), cfgaiKeyQueryOperatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.MoveToLocation), cfgaiMoveToLocationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.MoveToRandomLocation), cfgaiMoveToRandomLocationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.MoveToTarget), cfgaiMoveToTargetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.Node), cfgaiNodeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.Selector), cfgaiSelectorWrap.__Register);
        
        }
        
        static void wrapInit13(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(cfg.ai.Sequence), cfgaiSequenceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.Service), cfgaiServiceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.SimpleParallel), cfgaiSimpleParallelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.StringKeyData), cfgaiStringKeyDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.Task), cfgaiTaskWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.TbBehaviorTree), cfgaiTbBehaviorTreeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.TbBlackboard), cfgaiTbBlackboardWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeBlackboard), cfgaiUeBlackboardWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeCooldown), cfgaiUeCooldownWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeForceSuccess), cfgaiUeForceSuccessWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeLoop), cfgaiUeLoopWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeSetDefaultFocus), cfgaiUeSetDefaultFocusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeTimeLimit), cfgaiUeTimeLimitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeWait), cfgaiUeWaitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UeWaitBlackboardTime), cfgaiUeWaitBlackboardTimeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(cfg.ai.UpdateDailyBehaviorProps), cfgaiUpdateDailyBehaviorPropsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.EntityData), AquilaEntityDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Tools), AquilaToolsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.GameEntry), AquilaGameEntryWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.GameFrameworkModuleBase), AquilaGameFrameworkModuleBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Procedure.Procedure_Enter), AquilaProcedureProcedure_EnterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Procedure.Procedure_Fight), AquilaProcedureProcedure_FightWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Procedure.Procedure_Prelaod), AquilaProcedureProcedure_PrelaodWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Procedure.Procedure_Start), AquilaProcedureProcedure_StartWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.ObjectPool.Object_Terrain), AquilaObjectPoolObject_TerrainWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Module.GameFrameworkModule), AquilaModuleGameFrameworkModuleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Module.Module_Actor), AquilaModuleModule_ActorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Module.Module_Fight), AquilaModuleModule_FightWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Module.Module_Terrain), AquilaModuleModule_TerrainWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Module.Module_UI), AquilaModuleModule_UIWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Event.ActorDieEventArgs), AquilaEventActorDieEventArgsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.Constant), AquilaConfigConstantWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GameConfig), AquilaConfigGameConfigWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GlobalVar), AquilaConfigGlobalVarWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Extension.Component_LuBan), AquilaExtensionComponent_LuBanWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Extension.Component_Lua), AquilaExtensionComponent_LuaWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Extension.Component_Timer), AquilaExtensionComponent_TimerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.ActorEffect), AquilaFightActorEffectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FightPrcd), AquilaFightFightPrcdWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.ActorFSM), AquilaFightFSMActorFSMWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.ActorStateBase), AquilaFightFSMActorStateBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.ActorAbilityStateBase), AquilaFightFSMActorAbilityStateBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.ActorIdleStateBase), AquilaFightFSMActorIdleStateBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.ActorMoveStateBase), AquilaFightFSMActorMoveStateBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.ActorDieStateBase), AquilaFightFSMActorDieStateBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.HeroStateAddon), AquilaFightFSMHeroStateAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.HeroIdleState), AquilaFightFSMHeroIdleStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.HeroMoveState), AquilaFightFSMHeroMoveStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.HeroAbilityState), AquilaFightFSMHeroAbilityStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.FSM.HeroDieState), AquilaFightFSMHeroDieStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.AnimAddon), AquilaFightAddonAnimAddonWrap.__Register);
        
        }
        
        static void wrapInit14(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.ColliderTriggerAddon), AquilaFightAddonColliderTriggerAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.DataAddon), AquilaFightAddonDataAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.EffectAddon), AquilaFightAddonEffectAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.ActorEffectEntityData), AquilaFightAddonActorEffectEntityDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.EventAddon), AquilaFightAddonEventAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.FSMAddon), AquilaFightAddonFSMAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.InfoBoardAddon), AquilaFightAddonInfoBoardAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.MoveAddon), AquilaFightAddonMoveAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.NavAddon), AquilaFightAddonNavAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.ProcessorAddon), AquilaFightAddonProcessorAddonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.AddonBase), AquilaFightAddonAddonBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.ACTOR_ID_POOL), AquilaFightActorACTOR_ID_POOLWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.BulletActor), AquilaFightActorBulletActorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.BulletActorEntityData), AquilaFightActorBulletActorEntityDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.FlyActor), AquilaFightActorFlyActorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.ProjectileActor), AquilaFightActorProjectileActorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.ProjectileActorEntityData), AquilaFightActorProjectileActorEntityDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.HeroActor), AquilaFightActorHeroActorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.HeroActorEntityData), AquilaFightActorHeroActorEntityDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.TActorBase), AquilaFightActorTActorBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.TriggerActor), AquilaFightActorTriggerActorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.TriggerActorEntityData), AquilaFightActorTriggerActorEntityDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.RoleBaseSubTypeEnum), AquilaFightActorRoleBaseSubTypeEnumWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Buff.Component_Buff), AquilaFightBuffComponent_BuffWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Buff.BuffBase), AquilaFightBuffBuffBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Buff.BuffEntity), AquilaFightBuffBuffEntityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UGFExtensions.DataTableExtension), UGFExtensionsDataTableExtensionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UGFExtensions.Await.AwaitableExtensions), UGFExtensionsAwaitAwaitableExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UGFExtensions.Await.DownLoadResult), UGFExtensionsAwaitDownLoadResultWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UGFExtensions.Await.WebResult), UGFExtensionsAwaitWebResultWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.CSCallLua.DClass), TutorialCSCallLuaDClassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Tools.Fight), AquilaToolsFightWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Tools.Lua), AquilaToolsLuaWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.Constant.AssetPriority), AquilaConfigConstantAssetPriorityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GameConfig.Lua), AquilaConfigGameConfigLuaWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GameConfig.ObjectPool), AquilaConfigGameConfigObjectPoolWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GameConfig.Entity), AquilaConfigGameConfigEntityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GameConfig.Layer), AquilaConfigGameConfigLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GameConfig.Scene), AquilaConfigGameConfigSceneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Config.GameConfig.Tags), AquilaConfigGameConfigTagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Extension.Component_Timer.Timer), AquilaExtensionComponent_TimerTimerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Addon.AddonBase.AddonValidErrorCodeEnum), AquilaFightAddonAddonBaseAddonValidErrorCodeEnumWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.RoleBaseSubTypeEnum.Minion), AquilaFightActorRoleBaseSubTypeEnumMinionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Aquila.Fight.Actor.RoleBaseSubTypeEnum.Bullet), AquilaFightActorRoleBaseSubTypeEnumBulletWrap.__Register);
        
        
        
        }
        
        static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            
            wrapInit0(luaenv, translator);
            
            wrapInit1(luaenv, translator);
            
            wrapInit2(luaenv, translator);
            
            wrapInit3(luaenv, translator);
            
            wrapInit4(luaenv, translator);
            
            wrapInit5(luaenv, translator);
            
            wrapInit6(luaenv, translator);
            
            wrapInit7(luaenv, translator);
            
            wrapInit8(luaenv, translator);
            
            wrapInit9(luaenv, translator);
            
            wrapInit10(luaenv, translator);
            
            wrapInit11(luaenv, translator);
            
            wrapInit12(luaenv, translator);
            
            wrapInit13(luaenv, translator);
            
            wrapInit14(luaenv, translator);
            
            
            translator.AddInterfaceBridgeCreator(typeof(Tutorial.CSCallLua.ItfD), TutorialCSCallLuaItfDBridge.__Create);
            
        }
        
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter(Init);
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
		delegate UnityEngine.PhysicsScene __GEN_DELEGATE0( UnityEngine.SceneManagement.Scene scene);
		
		delegate UnityEngine.PhysicsScene2D __GEN_DELEGATE1( UnityEngine.SceneManagement.Scene scene);
		
		delegate void __GEN_DELEGATE2( UnityGameFramework.Runtime.DataTableComponent dataTableComponent,  string dataTableName,  string dataTableAssetName,  object userData);
		
		delegate System.Threading.Tasks.Task<UnityGameFramework.Runtime.UIForm> __GEN_DELEGATE3( UnityGameFramework.Runtime.UIComponent uiComponent,  string uiFormAssetName,  string uiGroupName,  int priority,  bool pauseCoveredUIForm,  object userData);
		
		delegate System.Threading.Tasks.Task<UnityGameFramework.Runtime.Entity> __GEN_DELEGATE4( UnityGameFramework.Runtime.EntityComponent entityComponent,  int entityID,  System.Type logicType,  string assetPath,  string entityGroup,  int priority,  object userData);
		
		delegate System.Threading.Tasks.Task<UnityGameFramework.Runtime.Entity> __GEN_DELEGATE5( UnityGameFramework.Runtime.EntityComponent entityComponent,  int entityId,  System.Type entityLogicType,  string entityAssetName,  string entityGroupName,  int priority,  object userData);
		
		delegate System.Threading.Tasks.Task<bool> __GEN_DELEGATE6( UnityGameFramework.Runtime.SceneComponent sceneComponent,  string sceneAssetName);
		
		delegate System.Threading.Tasks.Task<bool> __GEN_DELEGATE7( UnityGameFramework.Runtime.SceneComponent sceneComponent,  string sceneAssetName);
		
		delegate System.Threading.Tasks.Task<UGFExtensions.Await.WebResult> __GEN_DELEGATE8( UnityGameFramework.Runtime.WebRequestComponent webRequestComponent,  string webRequestUri,  UnityEngine.WWWForm wwwForm,  object userdata);
		
		delegate System.Threading.Tasks.Task<UGFExtensions.Await.WebResult> __GEN_DELEGATE9( UnityGameFramework.Runtime.WebRequestComponent webRequestComponent,  string webRequestUri,  byte[] postData,  object userdata);
		
		delegate System.Threading.Tasks.Task<UGFExtensions.Await.DownLoadResult> __GEN_DELEGATE10( UnityGameFramework.Runtime.DownloadComponent downloadComponent,  string downloadPath,  string downloadUri,  object userdata);
		
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
				{typeof(UnityEngine.SceneManagement.Scene), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE0(UnityEngine.PhysicsSceneExtensions.GetPhysicsScene)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE1(UnityEngine.PhysicsSceneExtensions2D.GetPhysicsScene2D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityGameFramework.Runtime.DataTableComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE2(UGFExtensions.DataTableExtension.LoadDataTable)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityGameFramework.Runtime.UIComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE3(UGFExtensions.Await.AwaitableExtensions.OpenUIFormAsync)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityGameFramework.Runtime.EntityComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE4(UGFExtensions.Await.AwaitableExtensions.ShowEntity)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE5(UGFExtensions.Await.AwaitableExtensions.ShowEntityAsync)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityGameFramework.Runtime.SceneComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE6(UGFExtensions.Await.AwaitableExtensions.LoadSceneAsync)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE7(UGFExtensions.Await.AwaitableExtensions.UnLoadSceneAsync)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityGameFramework.Runtime.WebRequestComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE8(UGFExtensions.Await.AwaitableExtensions.AddWebRequestAsync)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE9(UGFExtensions.Await.AwaitableExtensions.AddWebRequestAsync)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityGameFramework.Runtime.DownloadComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE10(UGFExtensions.Await.AwaitableExtensions.AddDownloadAsync)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}
