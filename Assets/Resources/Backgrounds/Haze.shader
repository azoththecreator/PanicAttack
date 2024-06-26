Shader "Custom/Haze"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RefStrength ("Reflection Strength", Range(0,0.1)) = 0.05
        _RefSpeed ("Reflection Speed", float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        zwrite off

		GrabPass {}

        CGPROGRAM
        #pragma surface surf nolight noambient alpha:fade

		sampler2D _GrabTexture;
        sampler2D _MainTex;
		float _RefStrength;
		float _RefSpeed;

        struct Input
        {
			float4 color:COLOR;
			float4 screenPos;
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
			float4 ref = tex2D(_MainTex, float2(IN.uv_MainTex.x+_Time.x * _RefSpeed, IN.uv_MainTex.y));

			float3 screenUV = IN.screenPos.rgb / IN.screenPos.a;
			o.Emission = tex2D(_GrabTexture, (screenUV.xy + ref.x * _RefStrength));
        }

		float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(0, 0, 0, 1);
		}

        ENDCG
    }
    FallBack "Regacy Shaders/Transparent/Vertexlit"
}
