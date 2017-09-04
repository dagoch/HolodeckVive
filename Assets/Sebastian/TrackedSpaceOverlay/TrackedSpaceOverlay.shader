Shader "Unlit/TrackedSpaceOverlay"
{
	Properties
	{
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_Space_Color("Space Color", Color) = (0, 0, 0, 0)
		_Grain ("Grain", Vector) = (1, 1, 1)
		_Width ("Width", Vector) = (0.05, 0.05, 0.05)
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 wpos : TEXCOORD1;
			};

			fixed4 _Color;
			fixed4 _Space_Color;
			fixed3 _Grain;
			fixed3 _Width;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.wpos = worldPos;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float xvalue = abs(fmod(i.wpos.x, _Grain.x));
				if (xvalue > (_Grain.x/2.)) {
					xvalue = _Grain.x - xvalue;
				}
				float yvalue = abs(fmod(i.wpos.y, _Grain.y));
				if (yvalue > (_Grain.y/2.)) {
					yvalue = _Grain.y - yvalue;
				}
				float zvalue = abs(fmod(i.wpos.z, _Grain.z));
				if (zvalue > (_Grain.z/2.)) {
					zvalue = _Grain.z - zvalue;
				}
				fixed4 col;
				if (xvalue < _Width.x && yvalue < _Width.y && zvalue < _Width.z) {
					col = _Color;
				} else {
					col = _Space_Color;
				}
				// sample the texture
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
