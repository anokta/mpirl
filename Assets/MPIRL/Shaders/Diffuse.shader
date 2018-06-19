Shader "MPIRL/DiffuseWithLightEstimation"
{
    Properties
    {
      _Color("Color", COLOR) = (0.5,0.5,0.5,1.0)        
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 150

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd finalcolor:lightEstimation

        fixed4 _Color;
        fixed3 _GlobalColorCorrection;

        struct Input
        {
            float2 uv_MainTex;
        };

        void lightEstimation(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            color.rgb *= _GlobalColorCorrection;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
          o.Albedo = _Color.rgb;
          o.Alpha = _Color.a;
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}
