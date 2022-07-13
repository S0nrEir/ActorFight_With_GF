Shader "Buff/Petrify" 
{
    Properties 
    {
        //灰度系数
        _PetrifyFac("Petrify Factor",Range(0,1)) = 0.5
        //石化纹理
        _PetrifyTex ("Petrify Tex", 2D) = "white" {}
        //主纹理
        _MainTex ("Main Tex", 2D) = "white" {}
        //亮度
        _Brightness ("Brightness",Range(0,1)) = 0.5
    }
        SubShader
        {
            Tags{"RenderType"="Opaque"}

            Pass
            {
                Tags{"LightMode"="ForwardBase"}

                Cull Off
                Zwrite On

                CGPROGRAM

                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdbase

                float _PetrifyFac;

                sampler2D _PetrifyTex;
                float4 _PetrifyTex_ST;

                sampler2D _MainTex; 
                float4 _MainTex_ST;

                float _Brightness;

                struct a2v
                {
                    float4 vertex : POSITION;
                    float4 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    //两张纹理的uv坐标
                    float2 uv_1 : TEXCOORD0;
                    float2 uv_2 : TEXCOORD1;
                };

                v2f vert(a2v v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv_1 = TRANSFORM_TEX(v.texcoord,_MainTex);
                    o.uv_2 = TRANSFORM_TEX(v.texcoord,_PetrifyTex);

                    return o;
                }

                fixed4 frag(v2f o) : SV_Target
                {
                    fixed4 color = tex2D(_MainTex,o.uv_1);
                    
                    //对应的rgb通道置灰
                    float grey = dot(color.rgb, float3(0.21256 , 0.7152 , 0.0722)); 
                    color.rgb = lerp(color.rgb , grey , _PetrifyFac);

                    //石化纹理采样，算出真正的灰度
                    fixed4 colorSta = tex2D(_PetrifyTex , o.uv_2);
                    // color.rgb *= lerp(fixed3(1,1,1) , colorSta.rgb , _PetrifyFac) * lerp(1 , _Brightness , _PetrifyFac);
                    color.rgb *= colorSta.rgb * _PetrifyFac * lerp(1 , _Brightness , _PetrifyFac);
                    return color.rgba;
                }
                ENDCG
            }

        }
    // FallBack "Diffuse"
    FallBack "VertexLit",1
}