// Don't remove the following line. It is used to bypass Unity
// upgrader change. This is necessary to make sure the shader
// continues to compile on Unity 5.2
// UNITY_SHADER_NO_UPGRADE
Shader "MPIRL/PointCloudRandom" {
Properties{
        _NoiseTex("Base (RGB)", 2D) = "white" {}
        _PointSize("Point Size", Float) = 5.0
}
  SubShader {
     Tags{ "Queue" = "Background" }
     Offset 0, 10
     ZWrite Off
     Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        struct appdata
        {
           float4 vertex : POSITION;
        };

        struct v2f
        {
           float4 vertex : SV_POSITION;
           float size : PSIZE;
           float2 uv_NoiseTex : TEXCOORD;
        };

        float _PointSize;
        sampler2D _NoiseTex;

        v2f vert (appdata v)
        {
           v2f o;
           o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
           o.size = _PointSize;
           o.uv_NoiseTex = o.vertex;

           return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
           fixed4 c = 1.75f * tex2D(_NoiseTex, i.uv_NoiseTex + _Time.x);
           return c;
        }
        ENDCG
     }
  }
}
