Shader "Aquila/Outlined_" 
{
    //第一个Pass用来画红色描边，第二个Pass用来画原始模型。
    //第一个Pass通过将模型的顶点向其法向量方向进行微小位移的方式来实现描边，
    //并通过关闭Z写入和颜色混合的方式使描边显示在模型的背面。
    //第二个Pass就是一个简单的纹理绘制，应用于原始模型，由于开启了深度测试，并且关闭了颜色混合，
    //所以它会覆盖在第一个Pass的结果之上。
    Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1)
        _OutlineSize ("Outline Size", Range(0.0, 1.0)) = 0.05
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalRenderPipeline" 
        }
        
        Pass 
        {
            Tags { "LightMode"="UniversalForward" }
            
            ZWrite Off
//            ColorMask RGB
//            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            // #pragma shader_feature_local _ALPHATEST_ON
            // #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma target 2.0
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

            struct Attributes 
            {
                float4 positionOS   : POSITION;
            };
            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
            };
            float _OutlineSize;
            half4 _OutlineColor;
            Varyings Vert(Attributes input)
            {
                Varyings output;
                float3 finalPosition = input.positionOS.xyz + normalize(input.positionOS.xyz) * _OutlineSize;
                output.positionCS = TransformObjectToHClip(finalPosition);
                return output;
            }
            half4 Frag(Varyings input) : SV_Target
            {
                half4 finalColor = _OutlineColor;
                return finalColor;
            }
            ENDHLSL
        }
        
//        Pass 
//        {
////            ZTest LEqual
////            ZWrite On
////            ColorMask RGB
//            
//            HLSLPROGRAM
//            // #define LIGHTMAP_OFF 0
//            // #define LIGHTMAP_ON 1
//            // #define DIRLIGHTMAP_COMBINED 2
//            // #define DYNAMICLIGHTMAP_ON 3
//            
//            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
//
//            #pragma target 2.0
//            #pragma vertex Vert
//            #pragma fragment Frag
//            
//            TEXTURE2D(_MainTex);
//            SAMPLER(sampler_MainTex);
//            // CBUFFER_START(UnityPerMaterial)
//            //     TEXTURE2D(_MainTex);
//            //     SAMPLER(sampler_MainTex);
//            // CBUFFER_END
//            
//            struct Attributes
//            {
//                float3 positionOS   : POSITION;
//            };
//            struct Varyings
//            {
//                float4 positionCS   : SV_POSITION;
//            };
//            
//            Varyings Vert(Attributes input)
//            {
//                Varyings output;
//                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
//                return output;
//            }
//            
//            half4 Frag(Varyings input) : SV_Target
//            {
//                //return half4(1.0, 1.0, 1.0, 1.0);
//                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.positionCS.xy);
//                return albedo;
//            }
//            ENDHLSL
//        }
    }
//    FallBack "Diffuse"
}