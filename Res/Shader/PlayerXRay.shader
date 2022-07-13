// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AuilaFramework/Outline_/PlayerXRay"
{
	Properties
	{
		_Color  ("Color (RGB)", Color) = (0.745,0.631,0.529,1)
	}

	SubShader 
	{
		Tags { "Queue" = "Transparent-20"}
		LOD 200

		CGINCLUDE  
        #include "UnityCG.cginc"

        struct appdata 
        {
            float4 vertex : POSITION;   
            float3 normal : NORMAL;  
			float3 texcoord : TEXCOORD0;  
        };  
        struct v2f 
        {
			float2 uv_MainTex:TEXCOORD0;
            float4 pos : POSITION;  
            float4 color : COLOR;  
        };  

        float4 _Color ;
		 
        v2f vert(appdata v) 
        {
            v2f o;
            o.pos = UnityObjectToClipPos (v.vertex);
            float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
            o.color = _Color;

			float N = saturate(dot(viewDir,v.normal));

			o.color = _Color * N ;
			o.color.a = 0;
			o.uv_MainTex = v.texcoord.xy;
            return o;
        }

    	ENDCG  

		Pass
		{
			Cull Back  
			ZTest Greater
			ZWrite Off 
			
			Blend One One 
			
			CGPROGRAM 
            #pragma vertex vert  
            #pragma fragment frag  
            float4 frag(v2f i) :COLOR
            {
                return i.color;
            }  
            ENDCG
        }
		
	}
	FallBack "Diffuse"
}
