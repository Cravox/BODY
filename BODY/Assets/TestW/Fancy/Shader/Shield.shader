Shader "GamesAcademy/Shield"
{
    Properties
    {
        _MainTex ("Texture", 2D)						= "white" {}
		_ShieldColorLight ("Shield Color Light", color) = (1 , 1, 1, 1)
		_ShieldColorDark("Shield Color Dark", color)	= (0 , 0, 0, 0)
		_WobbleSpeed("Wobble Speed", float)				= 0

		_CloudsTex("Texture", 2D)						= "black" {}
		_CloudsIntensity("Clouds Intensity", float)		= 0

		_FresnelParams ("Fresnel", vector) = (0, 1, 1, 0)
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
		ZWrite Off
		Blend One OneMinusSrcAlpha
		//Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct Input
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _ShieldColorLight;
			fixed4 _ShieldColorDark;

			float _WobbleSpeed;

			sampler2D _CloudsTex;
			float4 _CloudsTex_ST;
			float _CloudsIntensity;

			float4 _FresnelParams;

            v2f vert (Input v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.position);
				o.worldPos = mul(UNITY_MATRIX_M, v.position);
				o.normal = mul(UNITY_MATRIX_M, v.normal);
                o.uv = v.uv;
                return o;
            }

			float getFresnel(float3 worldPos, float3 normal)
			{
				float3 viewDirection = normalize(worldPos - _WorldSpaceCameraPos);
				normal = normalize(normal);
				float nDotV = dot(normal, viewDirection);
				
				//standard
				float fresnel = clamp(1 + nDotV, 0, 1);

				//backface culling : 
				//float fresnel = clamp(1-abs(nDotV), 0, 1);

				return lerp(_FresnelParams.x, _FresnelParams.y, pow(fresnel, _FresnelParams.z));
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				float fresnel = getFresnel(i.worldPos, i.normal);

                fixed4 texCol = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw);
				fixed4 cloudsCol = tex2D(_CloudsTex, i.uv * _CloudsTex_ST.xy + _CloudsTex_ST.zw * _Time.y);

				fixed4 newCol = lerp(_ShieldColorDark, _ShieldColorLight, texCol.r + cloudsCol.r * _CloudsIntensity * texCol.a);
				newCol.a = texCol.a; //* (0.5f + 0.5f * sin(_Time.y * _WobbleSpeed))
				newCol.rgb *= fresnel;
				
				return newCol * fresnel;
            }
            ENDCG
        }
    }
}
