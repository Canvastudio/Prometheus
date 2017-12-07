Shader "Unlit/ChipBoardColorer"
{
	Properties
	{
        Color1 ("Deactive", Color) = (1,1,1,1)
		Color2 ("Preactive", Color) = (1,1,1,1)
		U ("R", Float) = 0

	}
	SubShader
	{
		Tags 
		{ 
			"RenderType"="Transparent"
	        "Queue"="Transparent"
            "IgnoreProjector"="True"
		}

		 Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
			#include "UnityCG.cginc"

			fixed4 Color1;
			fixed4 Color2;
			float U;
		

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD1;
			};

			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;

				float h1 =  U / 11;
 				float h2 =  (U+1) / 11;
				float v1 = U / 23;
				float v2 = (U+1)/23;

				//动态分支消耗好像比较大 之后有问题再优化这里
				if (i.uv.x < 0.5 + h1 && i.uv.x > 0.5 - h1 && i.uv.y < 0.5 + v1 && i.uv.y > 0.5 - v1)
				{
					col = fixed4(0,0,0,0);
				}
				else if (i.uv.x < 0.5 + h2 && i.uv.x > 0.5 - h2 && i.uv.y < 0.5 + v2 && i.uv.y > 0.5 - v2)
				{
					col = Color2;
				}
				else
				{
					col = Color1;
				}

				return col;
			}
			ENDCG
		}
	}
}
