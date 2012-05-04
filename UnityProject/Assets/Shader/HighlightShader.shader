Shader "Custom/Highlight"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ColorGlow ("Color Glow", Color) = (1.0,1.0,1.0,1.0)
		_Scale ("Scale", Float ) = 2.0
		_Alpha ("Alpha", Float ) = 0.8
		_AlphaLoss ("AlphaLoss", Float ) = 1.0
		_ObjectCenter ("Center of Object", Vector ) = ( 0, 0, 0, 1 )
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
		LOD 200

		Pass //First pass drawing texture to surface
		{       
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _MainTex_ST; //this is needed by TRANSFROM_TEX makro in UnityCG.cginc
			sampler2D _MainTex;
			float _Scale;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_base i)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				o.uv = TRANSFORM_TEX (i.texcoord, _MainTex);
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
		Pass
		{       			
            Lighting On
			Blend SrcAlpha OneMinusSrcAlpha 
			CULL FRONT
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _ColorGlow;
			float _Scale;
			float _Alpha;
			float _AlphaLoss;
			float4 _ObjectCenter;
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 radius : TEXCOORD0; //radius of the object
			};

			//getting all needed vertexiformation for fragment shader
			v2f vert(appdata_base i)
			{
				v2f o;
				float4 vertexScaled = float4( (i.vertex + i.normal * _Scale).xyz, 1 );
				o.radius = i.vertex.xyz; //get radius coordinates
				o.pos = mul(UNITY_MATRIX_MVP, vertexScaled );
				return o;
			}

			//alpha gradient due to vertex positon
			half4 frag(v2f i) : COLOR
			{
				float r = length( i.radius - _ObjectCenter );
				float alpha = ( _Alpha / ( r * r ) ) / _AlphaLoss;
				return half4 (_ColorGlow.x, _ColorGlow.y, _ColorGlow.z, alpha );
			}
			ENDCG
		}
		
	}
	FallBack "Diffuse"

}