Shader "Toon/Basic Outlined Occlusion Ghosting" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_ShadowColor ("Shadow Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.03)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { Texgen CubeNormal }
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;
uniform float4 _ShadowColor;

 
v2f vert(appdata v) {
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
	float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * _Outline;
	o.color = _OutlineColor;
	return o;
}
v2f vert2(appdata v) {
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
	float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * _Outline;
	o.color = _ShadowColor;
	return o;
}
ENDCG
 
	SubShader {
		Tags { "Queue" = "Transparent" }
 
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			ColorMask RGB
			Blend One Zero
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			 
			half4 frag(v2f i) :COLOR {
				return i.color;
			}
			ENDCG
		}
		Pass {
			Name "SHADOW"
			Tags { "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			ZTest GEqual
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
 
			CGPROGRAM
			#pragma vertex vert2
			#pragma fragment frag
			 
			half4 frag(v2f i) :COLOR {
				return i.color;
			}
			ENDCG
		}
 		UsePass "Toon/Basic/BASE"
//		Pass {
//			Name "BASE"
//			ZWrite On
//			ZTest LEqual
//			Blend SrcAlpha OneMinusSrcAlpha
//			Material {
//				Diffuse [_Color]
//				Ambient [_Color]
//			}
//			Lighting On
//			SetTexture [_MainTex] {
//				ConstantColor [_Color]
//				Combine texture * constant
//			}
//			SetTexture [_MainTex] {
//				Combine previous * primary DOUBLE
//			}
//		}
	}
 
	SubShader {
		Tags { "Queue" = "Transparent" }
 
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite Off
			ColorMask RGB
			Blend One Zero
 
			CGPROGRAM
			#pragma vertex vert
			#pragma exclude_renderers gles xbox360 ps3
			ENDCG
			SetTexture [_MainTex] { combine primary }
		}
		
		Pass {
			Name "SHADOW"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite Off
			ZTest GEqual
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha 

 
			CGPROGRAM
			#pragma vertex vert2
			#pragma exclude_renderers gles xbox360 ps3
			ENDCG
			SetTexture [_MainTex] { combine primary }
		}
 		UsePass "Toon/Basic/BASE"
//		Pass {
//			Name "BASE"
//			ZWrite On
//			ZTest LEqual
//			Blend SrcAlpha OneMinusSrcAlpha
//			Material {
//				Diffuse [_Color]
//				Ambient [_Color]
//			}
//			Lighting On
//			SetTexture [_MainTex] {
//				ConstantColor [_Color]
//				Combine texture * constant
//			}
//			SetTexture [_MainTex] {
//				Combine previous * primary DOUBLE
//			}
//		}
	}
 
	Fallback "Diffuse"
}