Shader "Unlit/CircleFill"
{
    Properties
    {
        _BackgroundTex ("Background Texture", 2D) = "white" {}
        _ForegroundTex ("Foreground Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        _FillOrigin ("Fill Origin (UV)", Vector) = (0.5, 0.5, 0, 0)
        _Color ("Tint Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // ͸�����ģʽ
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
            float2 _FillOrigin;
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 bgColor = tex2D(_BackgroundTex, i.uv);
                fixed4 fgColor = tex2D(_ForegroundTex, i.uv);

                // ���㵱ǰ������������ľ���
                float dist = distance(i.uv, _FillOrigin);

                // ����������
                float mask = step(dist, _Progress);

                // ���ǰ���뱳��
                fixed4 finalColor = lerp(bgColor, fgColor, mask);
                return finalColor * _Color;
            }
            ENDCG
        }
    }
}
