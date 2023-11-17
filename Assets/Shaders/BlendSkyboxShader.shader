// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skybox/CubemapSkyboxBlend" {
	Properties {
	    //_Tint ("Tint Color", Color) = (.5, .5, .5, 1)
	    //_Tint2 ("Tint Color 2", Color) = (.5, .5, .5, 1)
	    //[Gamma] _Exposure ("Exposure", Range(0, 8)) = 0.5
	    _Rotation ("Rotation", Range(0, 360)) = 0
	    _BlendCubemaps0 ("Blend Cubemap 1", Range(0, 1)) = 0.5
	    _BlendCubemaps1 ("Blend Cubemap 2", Range(0, 1)) = 0.5
	    _BlendCubemaps2 ("Blend Cubemap 3", Range(0, 1)) = 0.5
	    _BlendCubemaps3 ("Blend Cubemap 4", Range(0, 1)) = 0.5
	    _BlendCubemaps4 ("Blend Cubemap 5", Range(0, 1)) = 0.5
	    _BlendCubemaps5 ("Blend Cubemap 6", Range(0, 1)) = 0.5
	    _BlendCubemaps6 ("Blend Cubemap 7", Range(0, 1)) = 0.5
	    _BlendCubemaps7 ("Blend Cubemap 8", Range(0, 1)) = 0.5
	    _BlendCubemaps8 ("Blend Cubemap 9", Range(0, 1)) = 0.5
	    _BlendCubemaps9 ("Blend Cubemap 10", Range(0, 1)) = 0.5
	    _BlendCubemaps10 ("Blend Cubemap 10", Range(0, 1)) = 0.5
	    [NoScaleOffset] _Tex0 ("Cubemap 0", Cube) = "grey" {}
	    [NoScaleOffset] _Tex1 ("Cubemap 1", Cube) = "grey" {}
	    [NoScaleOffset] _Tex2 ("Cubemap 2", Cube) = "grey" {}
	    [NoScaleOffset] _Tex3 ("Cubemap 3", Cube) = "grey" {}
	    [NoScaleOffset] _Tex4 ("Cubemap 4", Cube) = "grey" {}
	    [NoScaleOffset] _Tex5 ("Cubemap 5", Cube) = "grey" {}
	    [NoScaleOffset] _Tex6 ("Cubemap 6", Cube) = "grey" {}
	    [NoScaleOffset] _Tex7 ("Cubemap 7", Cube) = "grey" {}
	    [NoScaleOffset] _Tex8 ("Cubemap 8", Cube) = "grey" {}
	    [NoScaleOffset] _Tex9 ("Cubemap 9", Cube) = "grey" {}
	    [NoScaleOffset] _Tex10 ("Cubemap 10", Cube) = "grey" {}
	}
	SubShader {
	    Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	    Cull Off ZWrite Off
	    Blend SrcAlpha OneMinusSrcAlpha
	 
	    Pass {
	     
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	 
	        #include "UnityCG.cginc"
	 
	        samplerCUBE _Tex0;
	        samplerCUBE _Tex1;
	        samplerCUBE _Tex2;
	        samplerCUBE _Tex3;
	        samplerCUBE _Tex4;
	        samplerCUBE _Tex5;
	        samplerCUBE _Tex6;
	        samplerCUBE _Tex7;
	        samplerCUBE _Tex8;
	        samplerCUBE _Tex9;
	        samplerCUBE _Tex10;
	        float _BlendCubemaps0;
	        float _BlendCubemaps1;
	        float _BlendCubemaps2;
	        float _BlendCubemaps3;
	        float _BlendCubemaps4;
	        float _BlendCubemaps5;
	        float _BlendCubemaps6;
	        float _BlendCubemaps7;
	        float _BlendCubemaps8;
	        float _BlendCubemaps9;
	        float _BlendCubemaps10;
	        //half4 _Tex_HDR;
	        //half4 _Tint;
	        //half4 _Tint2;
	        //half _Exposure;
	        float _Rotation;
	 
	        float4 RotateAroundYInDegrees (float4 vertex, float degrees)
	        {
	            float alpha = degrees * UNITY_PI / 180.0;
	            float sina, cosa;
	            sincos(alpha, sina, cosa);
	            float2x2 m = float2x2(cosa, -sina, sina, cosa);
	            return float4(mul(m, vertex.xz), vertex.yw).xzyw;
	        }
	     
	        struct appdata_t {
	            float4 vertex : POSITION;
	            float3 normal : NORMAL;
	        };
	 
	        struct v2f {
	            float4 vertex : SV_POSITION;
	            float3 texcoord : TEXCOORD0;
	        };
	 
	        v2f vert (appdata_t v)
	        {
	            v2f o;
	            o.vertex = UnityObjectToClipPos(RotateAroundYInDegrees(v.vertex, _Rotation));
	            o.texcoord = v.vertex;
	            return o;
	        }
	 
	        fixed4 frag (v2f i) : SV_Target
	        {
				float4 base = (0.5f, 0.0f, 5.0f, 1.0f);
				float4 env0 = texCUBE (_Tex0, i.texcoord);
				float4 env1 = texCUBE (_Tex1, i.texcoord);
				float4 env2 = texCUBE (_Tex2, i.texcoord);
				float4 env3 = texCUBE (_Tex3, i.texcoord);
				float4 env4 = texCUBE (_Tex4, i.texcoord);
				float4 env5 = texCUBE (_Tex5, i.texcoord);
				float4 env6 = texCUBE (_Tex6, i.texcoord);
				float4 env7 = texCUBE (_Tex7, i.texcoord);
				float4 env8 = texCUBE (_Tex8, i.texcoord);
				float4 env9 = texCUBE (_Tex9, i.texcoord);
				float4 env10 = texCUBE (_Tex10, i.texcoord);

				float4 res_b_0 = lerp( base, env0,			_BlendCubemaps0	);
				float4 res_0_1 = lerp( res_b_0, env1,		_BlendCubemaps1	);
				float4 res_0_1_2 = lerp( res_0_1, env2,		_BlendCubemaps2	);
				float4 res_0_2_3 = lerp( res_0_1_2, env3,	_BlendCubemaps3	);
				float4 res_0_3_4 = lerp( res_0_2_3, env4,	_BlendCubemaps4	);
				float4 res_0_4_5 = lerp( res_0_3_4, env5,	_BlendCubemaps5	);
				float4 res_0_5_6 = lerp( res_0_4_5, env6,	_BlendCubemaps6	);
				float4 res_0_6_7 = lerp( res_0_5_6, env7,	_BlendCubemaps7	);
				float4 res_0_7_8 = lerp( res_0_6_7, env8,	_BlendCubemaps8	);
				float4 res_0_8_9 = lerp( res_0_7_8, env9,	_BlendCubemaps9	);
				float4 res_0_9_10 = lerp( res_0_8_9, env10, _BlendCubemaps10	 );
				//float4 tint = lerp( _Tint, _Tint2, _BlendCubemaps );
	            //half3 c = DecodeHDR (res1, _Tex_HDR);
	            //half4 c = res * unity_ColorSpaceDouble;
	            //c *= _Exposure;
	            return res_0_9_10;
	        }
	        ENDCG
	    }
	}
	Fallback Off
}