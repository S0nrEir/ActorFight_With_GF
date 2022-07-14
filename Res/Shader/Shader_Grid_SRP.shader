// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MAST/SRP/Shader_Grid_SRP"
{
	Properties
	{
		_GridTexture("GridTexture", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		_Tint("Tint", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+2" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _GridTexture;
		uniform float4 _GridTexture_ST;
		uniform float4 _Tint;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_GridTexture = i.uv_texcoord * _GridTexture_ST.xy + _GridTexture_ST.zw;
			float4 tex2DNode3 = tex2D( _GridTexture, uv_GridTexture );
			float4 lerpResult7 = lerp( tex2DNode3 , _Tint , _Tint.a);
			o.Albedo = lerpResult7.rgb;
			o.Alpha = ( tex2DNode3.a * _Opacity );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
370.4;129.6;799;459;948.9081;465.047;1.721023;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;1;-813.5955,-42.11902;Inherit;True;Property;_GridTexture;GridTexture;0;0;Create;True;0;0;False;0;False;None;c856312c718314a45b10cb3716135d03;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;2;-465.6993,233.6767;Inherit;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-538.3035,-181.8024;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-447.8581,24.89569;Inherit;False;Property;_Tint;Tint;2;0;Create;True;0;0;False;0;False;1,1,1,1;0.238341,0.389676,0.490566,0.1568628;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-145.8632,165.4845;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;7;-173.328,-139.2696;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;40,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;MAST/SRP/Shader_Grid_SRP;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;2;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;4;0;3;4
WireConnection;4;1;2;0
WireConnection;7;0;3;0
WireConnection;7;1;5;0
WireConnection;7;2;5;4
WireConnection;0;0;7;0
WireConnection;0;9;4;0
ASEEND*/
//CHKSM=4CF14EE009875F4B127F80245C9160B695C13929