Shader "Customize/Sprite-Lit-Default"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _MainOffset("MainOffset", Vector) = (0, 0, 1, 1)  // 偏移和尺寸：x, y, width, height
        _Color("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite Off

            HLSLPROGRAM
            #pragma target 4.5
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // 定义顶点和片段着色器
            #pragma vertex UnlitVertex
            #pragma fragment UnlitFragment

            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // 定义纹理
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _MainOffset;
            CBUFFER_END
            // 顶点着色器
            Varyings UnlitVertex(Attributes attributes)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(attributes);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // 计算顶点位置，转换到裁剪空间
                o.positionCS = TransformObjectToHClip(attributes.positionOS);

                // 使用 _MainOffset 对 UV 坐标进行偏移和缩放
                o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
                o.uv = o.uv * _MainOffset.zw + _MainOffset.xy;  // 偏移并按图集尺寸调整

                o.color = attributes.color * _Color;  // 顶点颜色与材质颜色相乘

                return o;
            }

            // 片段着色器
            float4 UnlitFragment(Varyings i) : SV_Target
            {
                // 使用偏移后的纹理坐标进行纹理采样
                float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                return mainTex;
            }

            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
