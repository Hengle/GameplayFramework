// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "LightMapShaderBG" 
{
	Properties {
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	    _LightMap ("Lightmap (RGB)", 2D) = "white" {}
	} 
	
	SubShader {
	Tags {"Queue"="Transparent" }
    LOD 200
		Pass {
		cull back
		blend srcalpha oneminussrcalpha
			CGPROGRAM
			
			#pragma vertex vert 
			#pragma fragment frag 									
			#include "UnityCG.cginc"
			//#include "shaderlib.cginc"
			sampler2D _MainTex; 
			sampler2D _LightMap;
			uniform fixed4 _MainTex_ST;		
							
			struct v2f { 
				half4 pos : POSITION; 
				fixed2 uv :TEXCOORD0; 
				fixed2 uv2 :TEXCOORD1;		
			}; 			
			
			v2f vert (appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv2 = v.texcoord1;	
				return o; 
			}
			
			fixed4 frag(v2f i):COLOR {
				fixed4 color; 
				color = tex2D(_MainTex,i.uv);	
				color.xyz =color.xyz;
				
				clip(color.w-0.6);									
				float3 lmap = tex2D(_LightMap,i.uv2).xyz;
//				return float4(lmap,1);
				color.xyz *=lmap*1.6;
				return color;
			}
			ENDCG
		}
	}	
}