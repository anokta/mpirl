Shader "MPIRL/TransparentDiffuseWithLightEstimation" {
  Properties
  {
    _Color("Color", COLOR) = (0.5, 0.5, 0.5, 1.0)
  }
    
  SubShader
  {
    Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
    Blend SrcAlpha OneMinusSrcAlpha
    LOD 150

    CGPROGRAM
    #pragma surface surf Lambert noforwardadd finalcolor:lightEstimation alpha:blend

    fixed4 _Color;
    fixed _GlobalLightEstimation;

    struct Input 
    {
      float2 uv_MainTex;
    };

    void lightEstimation(Input IN, SurfaceOutput o, inout fixed4 color) 
    {
      color.rgb *= _GlobalLightEstimation;
    }

    void surf(Input IN, inout SurfaceOutput o) 
    {
      o.Albedo = _Color.rgb;
      o.Alpha = _Color.a;
    }
    ENDCG
  }
    
  FallBack "Mobile/VertexLit"
}