// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Veins"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlendMode("Blend Mode", Range(0, 1)) = 0
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _BlendMode;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 blendColor = col;

                    // Apply the selected blend mode
                    if (_BlendMode == 0) // Normal
                    {
                        blendColor = col;
                    }
                    else if (_BlendMode == 1) // Multiply
                    {
                        blendColor *= col;
                    }
                    else if (_BlendMode == 2) // Overlay
                    {
                        if (col.r < 0.5)
                            blendColor *= 2 * col * blendColor;
                        else
                            blendColor = 1 - 2 * (1 - col) * (1 - blendColor);
                    }

                    return blendColor;
                }
                ENDCG
            }
        }
}
