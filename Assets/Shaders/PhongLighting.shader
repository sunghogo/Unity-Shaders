Shader "Custom/PhongLighting"
{
    Properties
    {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Phong

        struct Input
        {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _color;


        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _color.rgb;
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _color.a;
            o.Normal = UnpackNormal(tex2D(_normalMap, IN.uv_normalMap));
        }

        fixed4 LightingPhong(SurfaceOutput s, half3 lightDir, half atten) {
            fixed diffuse = saturate(dot(normalize(s.Normal), normalize(lightDir)));
            return fixed4(s.Albedo * diffuse * atten, s.Alpha);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
