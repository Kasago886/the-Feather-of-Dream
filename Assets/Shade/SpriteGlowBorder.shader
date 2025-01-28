Shader "Custom/UI/SpriteGlowBorder" {
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,0,0,1)  // 发光颜色（默认红色）
        _GlowWidth ("Glow Width", Range(0, 0.1)) = 0.02  // 发光宽度
    }

    SubShader {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _GlowColor;
            float _GlowWidth;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // 采样主纹理颜色
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // 检测边缘透明度变化
                float alpha = col.a;
                float glow = 0;

                // 检查上下左右四个方向的像素透明度
                float2 offsets[4] = {
                    float2(0, _GlowWidth),   // 上
                    float2(0, -_GlowWidth),  // 下
                    float2(_GlowWidth, 0),   // 右
                    float2(-_GlowWidth, 0)   // 左
                };

                for (int j = 0; j < 4; j++) {
                    float neighborAlpha = tex2D(_MainTex, i.uv + offsets[j]).a;
                    glow += max(0, alpha - neighborAlpha); // 透明度差异越大，发光越强
                }

                // 混合原始颜色和发光颜色
                fixed4 finalColor = col;
                finalColor.rgb = lerp(col.rgb, _GlowColor.rgb, glow);
                finalColor.a = max(col.a, glow * _GlowColor.a); // 控制发光透明度

                return finalColor;
            }
            ENDCG
        }
    }
}