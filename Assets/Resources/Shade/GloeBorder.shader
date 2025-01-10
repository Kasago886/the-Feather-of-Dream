Shader "Custom/UI/GlowBorder"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowWidth ("Glow Width", Range(0.0, 0.5)) = 0.05
        _EdgeSharpness ("Edge Sharpness", Range(0.0, 10.0)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowWidth;
            float _EdgeSharpness;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                // Calculate distance to nearest edge
                float2 distToEdge = min(IN.texcoord, 1 - IN.texcoord);
                float glowFactor = smoothstep(_GlowWidth, _GlowWidth - _GlowWidth / _EdgeSharpness, min(distToEdge.x, distToEdge.y));
                
                col.rgb += glowFactor * _GlowColor.rgb * _GlowColor.a;
                col.a = max(col.a, glowFactor * _GlowColor.a); // Ensure alpha blending works correctly
                
                return col;
            }
            ENDCG
        }
    }
}
