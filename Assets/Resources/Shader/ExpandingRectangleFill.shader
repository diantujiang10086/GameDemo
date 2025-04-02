Shader "Unlit/ExpandingRectangleFill"
{
    Properties
    {
        _BackgroundTex ("Background Texture", 2D) = "white" {}
        _ForegroundTex ("Foreground Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        _FillOrigin ("Fill Origin (UV)", Vector) = (0.5, 0.5, 0, 0)  // ������
        _RectSize ("Rectangle Size", Vector) = (0.5, 0.5, 0, 0)  // ��ʼ���δ�С
        _Color ("Tint Color", Color) = (1, 1, 1, 1) // ���� _Color
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
            float2 _FillOrigin;  // ������ (UV����)
            float2 _RectSize;    // ���γߴ�
            fixed4 _Color;       // ��ɫ����ɫ

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ������ǰ��������ɫ
                fixed4 bgColor = tex2D(_BackgroundTex, i.uv);
                fixed4 fgColor = tex2D(_ForegroundTex, i.uv);

                // ������εı߽�
                float2 minCorner = _FillOrigin - _RectSize * _Progress; // ���½ǣ�����չ
                float2 maxCorner = _FillOrigin + _RectSize * _Progress; // ���Ͻǣ�����չ

                // �жϵ�ǰ�����Ƿ�����չ������
                bool inside = (i.uv.x >= minCorner.x && i.uv.x <= maxCorner.x) &&
                              (i.uv.y >= minCorner.y && i.uv.y <= maxCorner.y);

                // ���ݽ��ȼ����Ƿ����
                float mask = inside ? 1.0 : 0.0;

                // ���ǰ���뱳��
                fixed4 finalColor = lerp(bgColor, fgColor, mask);

                // ����������ɫֵ
                return finalColor * _Color;
            }
            ENDCG
        }
    }
}
