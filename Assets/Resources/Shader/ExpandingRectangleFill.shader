Shader "Unlit/ExpandingRectangleFill"
{
    Properties
    {
        _BackgroundTex ("Background Texture", 2D) = "white" {}
        _ForegroundTex ("Foreground Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        _FillOrigin ("Fill Origin (UV)", Vector) = (0.5, 0.5, 0, 0)  // 填充起点
        _RectSize ("Rectangle Size", Vector) = (0.5, 0.5, 0, 0)  // 初始矩形大小
        _Color ("Tint Color", Color) = (1, 1, 1, 1) // 定义 _Color
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _BackgroundTex;
            sampler2D _ForegroundTex;
            float _Progress;
            float2 _FillOrigin;  // 填充起点 (UV坐标)
            float2 _RectSize;    // 矩形尺寸
            fixed4 _Color;       // 调色板颜色

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 背景和前景纹理颜色
                fixed4 bgColor = tex2D(_BackgroundTex, i.uv);
                fixed4 fgColor = tex2D(_ForegroundTex, i.uv);

                // 计算矩形的边界
                float2 minCorner = _FillOrigin - _RectSize * _Progress; // 左下角，逐渐扩展
                float2 maxCorner = _FillOrigin + _RectSize * _Progress; // 右上角，逐渐扩展

                // 判断当前像素是否在扩展矩形内
                bool inside = (i.uv.x >= minCorner.x && i.uv.x <= maxCorner.x) &&
                              (i.uv.y >= minCorner.y && i.uv.y <= maxCorner.y);

                // 根据进度计算是否填充
                float mask = inside ? 1.0 : 0.0;

                // 混合前景与背景
                fixed4 finalColor = lerp(bgColor, fgColor, mask);

                // 返回最终颜色值
                return finalColor * _Color;
            }
            ENDCG
        }
    }
}
