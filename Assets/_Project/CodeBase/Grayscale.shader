Shader "Game/Grayscale" {
  Properties {
       _MainTex ("Texture", 2D) = "white" {}
     }
     SubShader {
       Tags { "RenderType" = "Opaque" }

       CGPROGRAM
       #pragma surface surf Lambert alpha

       struct Input {
           float2 uv_MainTex;
       };
     
       sampler2D _MainTex;

       void surf (Input IN, inout SurfaceOutput o) {

           half4 tex = tex2D(_MainTex, IN.uv_MainTex);
           
           o.Albedo = 0.299 * tex.r + 0.587 * tex.g + 0.184 * tex.b;
           o.Alpha = tex.a;
                   
       }
           
       ENDCG
           
     }
         
     Fallback "Diffuse"
         
   }