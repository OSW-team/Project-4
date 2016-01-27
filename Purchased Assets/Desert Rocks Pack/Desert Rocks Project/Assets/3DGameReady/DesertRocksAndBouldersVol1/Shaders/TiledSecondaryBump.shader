    Shader "Egamea/TiledSecondaryBump" {

    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _Opacity ("Color over opacity", Range (0, 1)) = 1
        _MainTex ("Color over (RGBA)", 2D) = "white" {}
        _BumpMap ("Normalmap over", 2D) = "bump" {}
        _BumpMap2 ("Normalmap under", 2D) = "bump" {}
    }
     
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 400
       
    CGPROGRAM
    #pragma surface surf Lambert
    #pragma exclude_renderers flash
     
    sampler2D _MainTex;
    sampler2D _BumpMap;
    sampler2D _BumpMap2;
    fixed4 _Color;
    float _Opacity;
     
    struct Input {
        float2 uv_MainTex;
        float2 uv_BumpMap;
        float2 uv_BumpMap2;
    };
     
    void surf (Input IN, inout SurfaceOutput o) {
        float4 tex = tex2D(_MainTex, IN.uv_MainTex);
        float4 dest;
        _Opacity*=tex.a;
        o.Albedo = tex.rgb * _Color;
        o.Alpha = tex.a * _Color.a;
       
        float4 norm = tex2D(_BumpMap, IN.uv_BumpMap);
        float4 norm2 = tex2D(_BumpMap2, IN.uv_BumpMap2);
        dest = norm<= 0.5? 2*norm*norm2 : 1-2*(1-norm)*(1-norm2);
        dest = lerp(norm2, dest, _Opacity);
        o.Normal = UnpackNormal(dest);
    }
    ENDCG
     
    }
     
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 400
       
    CGPROGRAM
    #pragma surface surf Lambert
    #pragma only_renderers flash
     
    sampler2D _MainTex;
    fixed4 _Color;
    float _Opacity;
     
    struct Input {
        float2 uv_MainTex;
    };
     
    void surf (Input IN, inout SurfaceOutput o) {
        float4 tex = tex2D(_MainTex, IN.uv_MainTex);
        o.Albedo = tex.rgb * _Color;  
        o.Alpha = tex.a * _Color.a;
    }
    ENDCG
     
    }
     
    FallBack "Bumped Diffuse"
    }