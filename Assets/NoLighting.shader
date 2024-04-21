Shader "Custom/NoLighting"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf CustomLighting

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        float4 _Color;
        
        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
        }

        half4 LightingCustomLighting(SurfaceOutput s, half3 lightDir, half atten) {
            return half4(s.Albedo, s.Alpha);
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
