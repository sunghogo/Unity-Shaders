Shader "Custom/PhongLighting"
{
    Properties
    {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _modelColor ("Model Color", Color) = (1, 1, 1, 1)
        _ambientColor ("Ambient Color", Color) = (0, 0, 1, 1)
    }
    SubShader
    {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf PhongLighting

        struct Input
        {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _modelColor;
        fixed4 _ambientColor;


        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _modelColor.rgb;
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _modelColor.a;
            o.Normal = UnpackNormal(tex2D(_normalMap, IN.uv_normalMap));
        }

        half4 LightingPhongLighting(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half diffuse = saturate(dot(normalize(s.Normal), normalize(lightDir)));
            return half4(s.Albedo * _ambientColor * diffuse * atten, s.Alpha);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
