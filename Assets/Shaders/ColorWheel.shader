Shader "Custom/ColorWheel"
{
    Properties
    {
        _outerRadius ("Outer Radius", Range(0, 0.5)) = 0.5
        _innerRadius ("Inner Radius", Range(0, 0.5)) = 0.4
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _outerRadius;
            float _innerRadius;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : POSITION;
            };

            // Function to convert HSV to RGB
            float3 hsv2rgb(float3 hsv)
            {
                float h = hsv.x;
                float s = hsv.y;
                float v = hsv.z;

                float p = v * (1 - s);
                float q = v * (1 - s * (h * 6 - floor(h * 6)));
                float t = v * (1 - s * (1 - (h * 6 - floor(h * 6))));

                if (h < 1.0 / 6) return float3(v, t, p);
                if (h < 2.0 / 6) return float3(q, v, p);
                if (h < 3.0 / 6) return float3(p, v, t);
                if (h < 4.0 / 6) return float3(p, q, v);
                if (h < 5.0 / 6) return float3(t, p, v);
                return float3(v, p, q);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calculate distance from center
                float radius = length(i.uv - 0.5);

                // Return transparent if outside specified radii
                if (radius <= _innerRadius || _outerRadius <= radius )
                {
                    return float4(0, 0, 0, 0);
                }

                // Calculate angle for hue
                float angle = atan2(i.uv.y - 0.5, i.uv.x - 0.5);
                if (angle < 0) angle += 2 * UNITY_PI;
                float hue = angle / (2 * UNITY_PI);
            
                // Convert hue to RGB using hsv2rgb function
                float3 color = hsv2rgb(float3(hue, 1, 1));

                return float4(color, 1.0);
            }

            ENDCG
        }
    }
}