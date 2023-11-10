Shader"Unlit/ColumnOverlay"
{
    Properties
    {
        _ColumnSize("Column Size", Float) = 3
        _GridColor("Grid Color", Color) = (1,0,0.4,1)
        _Alpha ("Alpha", Range(0,1)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Overlay"
        }
LOD100
        ZTestAlways

        Pass
        {
Blend SrcAlpha
OneMinusSrcAlpha
            Offset -10, -10
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

#include "UnityCG.cginc"

float _ColumnSize;
float4 _GridColor;
float _Alpha;

struct appdata
{
    float4 vertex : POSITION;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};


v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = mul(unity_ObjectToWorld, v.vertex).xy;
    return o;
}


float DrawColumn(float2 uv, float sz, float aa)
{
    float aaThresh = aa;
    float aaMin = aa * 0.001;

    float2 gUV = uv / sz + aaThresh;

    float2 fl = floor(gUV);
    gUV = frac(gUV);
    gUV -= aaThresh;
    gUV = smoothstep(aaThresh, aaMin, abs(gUV));
    float d = gUV.x;

    return d;
}

fixed4 frag(v2f i) : SV_Target
{
    fixed r = DrawColumn(i.uv, _ColumnSize, 0.03);
    return float4(_GridColor.xyz * r, r * _Alpha);
}
            ENDCG
        }
    }
}