Shader "Custom/Mask/SimpleMaskBillBoard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CutOff("CutOff", Range(0,1)) = 0
        _Radius("Radius", Range(0,1)) = 0.2
        _Speed("speed", Float) = 1
        _ScaleX ("Scale X", Float) = 1.0
        _ScaleY ("Scale Y", Float) = 1.0
    }
    SubShader
    {
        LOD 100
        Blend One OneMinusSrcAlpha
        Tags { "Queue" = "Geometry-1" }  // Write to the stencil buffer before drawing any geometry to the screen
        ColorMask 0 // Don't write to any colour channels
        ZWrite Off // Don't write to the Depth buffer

        // Write the value 1 to the stencil buffer
        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CutOff;
            float _Speed;
            float _Radius;
            float _ScaleX,_ScaleY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_P, 
                    mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
                    + float4(v.vertex.x, v.vertex.y, 0.0, 0.0)
                    * float4(_ScaleX, _ScaleY, 1.0, 1.0));

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float dissolve = step(col, _CutOff);
                clip(_CutOff-dissolve);
                return dissolve;
            }
            ENDCG
        }
    }
}