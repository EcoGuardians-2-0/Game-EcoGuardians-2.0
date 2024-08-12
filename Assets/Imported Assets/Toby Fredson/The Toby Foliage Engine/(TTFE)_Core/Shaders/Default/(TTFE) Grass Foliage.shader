// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toby Fredson/The Toby Foliage Engine/(TTFE) Grass Foliage"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_AlphaClip("Alpha Clip", Float) = 1.4
		[Header(__________(TTFE) GRASS FOLIAGE SHADER___________)][Header(_____________________________________________________)][Header(Texture Maps)][NoScaleOffset]_AlbedoMap("Albedo Map", 2D) = "white" {}
		[NoScaleOffset][Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		[NoScaleOffset]_MaskMapRGBA("Mask Map *RGB(A)", 2D) = "white" {}
		[NoScaleOffset]_SpecularMapGrayscale("Specular Map (Grayscale)", 2D) = "white" {}
		[NoScaleOffset]_NoiseMapGrayscale("Noise Map (Grayscale)", 2D) = "white" {}
		[Header(_____________________________________________________)][Header(Texture settings)][Header((Albedo))]_AlbedoColor("Albedo Color", Color) = (1,1,1,0)
		[Header((Normal))]_NormalIntensity("Normal Intensity", Range( -3 , 3)) = 1
		[Header((Smoothness))]_SmoothnessIntensity("Smoothness Intensity", Range( -3 , 3)) = 1
		[Header((Ambient Occlusion))]_AmbientOcclusionIntensity("Ambient Occlusion Intensity", Range( 0 , 1)) = 1
		[Header((Specular))]_SpecularPower("Specular Power", Range( 0 , 1)) = 1
		[Toggle(_SPECULARBACKFACEOCCLUSION_ON)] _SpecularBackfaceOcclusion("Specular Backface Occlusion", Float) = 0
		_SpecularMapScale("Specular Map Scale", Float) = 1
		_SpecularMapOffset("Specular Map Offset", Float) = 0
		[Header((Translucency))]_TranslucencyPower("Translucency Power", Range( 1 , 10)) = 3
		_TranslucencyRange("Translucency Range", Float) = 1
		[Toggle]_TranslucencyFluffiness("Translucency Fluffiness", Float) = 0
		[Header(Season Settings)][Header((Season Control))]_ColorVariation("Color Variation", Range( 0 , 1)) = 0
		_DryLeafColor("Dry Leaf Color", Color) = (0.5568628,0.3730685,0.1764706,0)
		_DryLeavesScale("Dry Leaves - Scale", Float) = 1
		_DryLeavesOffset("Dry Leaves - Offset", Float) = 1
		_SeasonChangeGlobal("Season Change - Global", Range( -2 , 2)) = 0
		[Toggle]_BranchMaskR("Branch Mask *(R)", Float) = 0
		[Header(_____________________________________________________)][Header(Wind Settings)][Header((Global Wind Settings))]_GlobalWindStrength("Global Wind Strength", Range( 0 , 1)) = 1
		_StrongWindSpeed("Strong Wind Speed", Range( 1 , 3)) = 1
		[KeywordEnum(GentleBreeze,WindOff)] _WindType("Wind Type", Float) = 0
		[Toggle(_CONTINUOUSWINDGENTLE_ON)] _ContinuousWindGentle("Continuous Wind (Gentle)", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}


		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector][ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[HideInInspector][ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0
		[HideInInspector][ToggleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0

		[HideInInspector] _QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector] _QueueControl("_QueueControl", Float) = -1

        [HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" "UniversalMaterialType"="Lit" }

		Cull Off
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		AlphaToMask Off

		

		HLSLINCLUDE
		#pragma target 4.5
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _SPECULAR_SETUP 1
			#pragma shader_feature_local_fragment _SPECULAR_SETUP
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
			#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS

			
            #pragma multi_compile _ EVALUATE_SH_MIXED EVALUATE_SH_VERTEX
		

			#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION

			
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
           

			

			#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
			#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile _ _LIGHT_LAYERS
			#pragma multi_compile_fragment _ _LIGHT_COOKIES
			#pragma multi_compile _ _FORWARD_PLUS
		
			

			

			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment _ DEBUG_DISPLAY

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_FORWARD

			
            #if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#endif
		

			
			#if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
			#endif
		

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#if defined(LOD_FADE_CROSSFADE)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
            #endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
				#define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON
			#pragma shader_feature_local _SPECULARBACKFACEOCCLUSION_ON


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				float4 lightmapUVOrVertexSH : TEXCOORD1;
				half4 fogFactorAndVertexLight : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					float4 shadowCoord : TEXCOORD6;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
					float2 dynamicLightmapUV : TEXCOORD7;
				#endif
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;
			sampler2D _NoiseMapGrayscale;
			sampler2D _MaskMapRGBA;
			sampler2D _NormalMap;
			sampler2D _SpecularMapGrayscale;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, gradient.colorsLength-1));
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = SRGBToLinear(color);
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, gradient.alphasLength-1));
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord8.xy = v.texcoord.xy;
				o.ase_texcoord9 = v.positionOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normalOS = LightDetectBackface_Output156_g2140;
				v.tangentOS = v.tangentOS;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );
				VertexNormalInputs normalInput = GetVertexNormalInputs( v.normalOS, v.tangentOS );

				o.tSpace0 = float4( normalInput.normalWS, vertexInput.positionWS.x );
				o.tSpace1 = float4( normalInput.tangentWS, vertexInput.positionWS.y );
				o.tSpace2 = float4( normalInput.bitangentWS, vertexInput.positionWS.z );

				#if defined(LIGHTMAP_ON)
					OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				#endif

				#if !defined(LIGHTMAP_ON)
					OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );
				#endif

				#if defined(DYNAMICLIGHTMAP_ON)
					o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord.xy;
					o.lightmapUVOrVertexSH.xy = v.texcoord.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( vertexInput.positionWS, normalInput.normalWS );

				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( vertexInput.positionCS.z );
				#else
					half fogFactor = 0;
				#endif

				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.positionCS = vertexInput.positionCS;
				o.clipPosV = vertexInput.positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						#ifdef _WRITE_RENDERING_LAYERS
						, out float4 outRenderingLayers : SV_Target1
						#endif
						 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#if defined(LOD_FADE_CROSSFADE)
					LODFadeCrossFade( IN.positionCS );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif

				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				float2 NormalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif

				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float2 uv_AlbedoMap285_g2140 = IN.ase_texcoord8.xy;
				float2 uv_AlbedoMap295_g2140 = IN.ase_texcoord8.xy;
				float4 tex2DNode295_g2140 = tex2D( _AlbedoMap, uv_AlbedoMap295_g2140 );
				float2 uv_NoiseMapGrayscale302_g2140 = IN.ase_texcoord8.xy;
				float4 transform307_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2142 = dot( transform307_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2142 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2142 ) * 43758.55 ) ));
				float3 normalizeResult259_g2140 = normalize( IN.ase_texcoord9.xyz );
				float DryLeafPositionMask263_g2140 = ( (distance( normalizeResult259_g2140 , float3( 0,0.8,0 ) )*1.0 + 0.0) * 1 );
				float4 lerpResult294_g2140 = lerp( ( _DryLeafColor * ( tex2DNode295_g2140.g * 2 ) ) , tex2DNode295_g2140 , saturate( (( ( tex2D( _NoiseMapGrayscale, uv_NoiseMapGrayscale302_g2140 ).r * lerpResult10_g2142 * DryLeafPositionMask263_g2140 ) - _SeasonChangeGlobal )*_DryLeavesScale + _DryLeavesOffset) ));
				float4 SeasonControl_Output311_g2140 = lerpResult294_g2140;
				Gradient gradient271_g2140 = NewGradient( 0, 2, 2, float4( 1, 0.276868, 0, 0 ), float4( 0, 1, 0.7818019, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float4 transform273_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2141 = dot( transform273_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2141 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2141 ) * 43758.55 ) ));
				float4 lerpResult282_g2140 = lerp( SeasonControl_Output311_g2140 , ( ( SeasonControl_Output311_g2140 * 0.5 ) + ( SampleGradient( gradient271_g2140, lerpResult10_g2141 ) * SeasonControl_Output311_g2140 ) ) , _ColorVariation);
				float2 uv_MaskMapRGBA313_g2140 = IN.ase_texcoord8.xy;
				float4 lerpResult283_g2140 = lerp( tex2D( _AlbedoMap, uv_AlbedoMap285_g2140 ) , lerpResult282_g2140 , (( _BranchMaskR )?( tex2D( _MaskMapRGBA, uv_MaskMapRGBA313_g2140 ).r ):( 1.0 )));
				float4 GrassColorVariation_Output109_g2140 = lerpResult283_g2140;
				float4 transform39_g2140 = mul(GetObjectToWorldMatrix(),float4( IN.ase_texcoord9.xyz , 0.0 ));
				float dotResult52_g2140 = dot( float4( WorldViewDirection , 0.0 ) , -( float4( _MainLightPosition.xyz , 0.0 ) + ( (( _TranslucencyFluffiness )?( transform39_g2140 ):( float4( IN.ase_texcoord9.xyz , 0.0 ) )) * _TranslucencyRange ) ) );
				float2 uv_MaskMapRGBA58_g2140 = IN.ase_texcoord8.xy;
				float ase_lightIntensity = max( max( _MainLightColor.r, _MainLightColor.g ), _MainLightColor.b );
				float4 ase_lightColor = float4( _MainLightColor.rgb / ase_lightIntensity, ase_lightIntensity );
				float TobyTranslucency66_g2140 = ( saturate( dotResult52_g2140 ) * tex2D( _MaskMapRGBA, uv_MaskMapRGBA58_g2140 ).b * ase_lightColor.a );
				float TranslucencyIntensity72_g2140 = _TranslucencyPower;
				float4 Albedo_Output160_g2140 = ( ( _AlbedoColor * GrassColorVariation_Output109_g2140 ) * (1.0 + (TobyTranslucency66_g2140 - 0.0) * (TranslucencyIntensity72_g2140 - 1.0) / (1.0 - 0.0)) );
				
				float2 uv_NormalMap189_g2140 = IN.ase_texcoord8.xy;
				float3 unpack189_g2140 = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap189_g2140 ), _NormalIntensity );
				unpack189_g2140.z = lerp( 1, unpack189_g2140.z, saturate(_NormalIntensity) );
				float3 Normal_Output154_g2140 = unpack189_g2140;
				
				float temp_output_349_0_g2140 = ( 0.2 * _SpecularPower );
				float2 uv_SpecularMapGrayscale251_g2140 = IN.ase_texcoord8.xy;
				#ifdef _SPECULARBACKFACEOCCLUSION_ON
				float staticSwitch340_g2140 = ( temp_output_349_0_g2140 * saturate( (tex2D( _SpecularMapGrayscale, uv_SpecularMapGrayscale251_g2140 ).r*_SpecularMapScale + _SpecularMapOffset) ) );
				#else
				float staticSwitch340_g2140 = temp_output_349_0_g2140;
				#endif
				float Specular_Output158_g2140 = staticSwitch340_g2140;
				float3 temp_cast_7 = (Specular_Output158_g2140).xxx;
				
				float2 uv_MaskMapRGBA89_g2140 = IN.ase_texcoord8.xy;
				float4 tex2DNode89_g2140 = tex2D( _MaskMapRGBA, uv_MaskMapRGBA89_g2140 );
				float Smoothness_Output159_g2140 = ( tex2DNode89_g2140.a * _SmoothnessIntensity );
				
				float AoMapBase103_g2140 = tex2DNode89_g2140.g;
				float Ao_Output157_g2140 = ( pow( abs( AoMapBase103_g2140 ) , _AmbientOcclusionIntensity ) * ( 1.5 / ( ( saturate( TobyTranslucency66_g2140 ) * TranslucencyIntensity72_g2140 ) + 1.5 ) ) );
				
				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord8.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				float3 BaseColor = Albedo_Output160_g2140.rgb;
				float3 Normal = Normal_Output154_g2140;
				float3 Emission = 0;
				float3 Specular = temp_cast_7;
				float Metallic = 0;
				float Smoothness = Smoothness_Output159_g2140;
				float Occlusion = Ao_Output157_g2140;
				float Alpha = 1;
				float AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.positionCS.z;
				#endif

				#ifdef _CLEARCOAT
					float CoatMask = 0;
					float CoatSmoothness = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData = (InputData)0;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;

				#ifdef _NORMALMAP
						#if _NORMAL_DROPOFF_TS
							inputData.normalWS = TransformTangentToWorld(Normal, half3x3(WorldTangent, WorldBiTangent, WorldNormal));
						#elif _NORMAL_DROPOFF_OS
							inputData.normalWS = TransformObjectToWorldNormal(Normal);
						#elif _NORMAL_DROPOFF_WS
							inputData.normalWS = Normal;
						#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					inputData.shadowCoord = ShadowCoords;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
				#else
					inputData.shadowCoord = float4(0, 0, 0, 0);
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif
					inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				#if defined(DYNAMICLIGHTMAP_ON)
					inputData.bakedGI = SAMPLE_GI(IN.lightmapUVOrVertexSH.xy, IN.dynamicLightmapUV.xy, SH, inputData.normalWS);
				#else
					inputData.bakedGI = SAMPLE_GI(IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS);
				#endif

				#ifdef ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif

				inputData.normalizedScreenSpaceUV = NormalizedScreenSpaceUV;
				inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUVOrVertexSH.xy);

				#if defined(DEBUG_DISPLAY)
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
					#endif
					#if defined(LIGHTMAP_ON)
						inputData.staticLightmapUV = IN.lightmapUVOrVertexSH.xy;
					#else
						inputData.vertexSH = SH;
					#endif
				#endif

				SurfaceData surfaceData;
				surfaceData.albedo              = BaseColor;
				surfaceData.metallic            = saturate(Metallic);
				surfaceData.specular            = Specular;
				surfaceData.smoothness          = saturate(Smoothness),
				surfaceData.occlusion           = Occlusion,
				surfaceData.emission            = Emission,
				surfaceData.alpha               = saturate(Alpha);
				surfaceData.normalTS            = Normal;
				surfaceData.clearCoatMask       = 0;
				surfaceData.clearCoatSmoothness = 1;

				#ifdef _CLEARCOAT
					surfaceData.clearCoatMask       = saturate(CoatMask);
					surfaceData.clearCoatSmoothness = saturate(CoatSmoothness);
				#endif

				#ifdef _DBUFFER
					ApplyDecalToSurfaceData(IN.positionCS, surfaceData, inputData);
				#endif

				half4 color = UniversalFragmentPBR( inputData, surfaceData);

				#ifdef ASE_TRANSMISSION
				{
					float shadow = _TransmissionShadow;

					#define SUM_LIGHT_TRANSMISSION(Light)\
						float3 atten = Light.color * Light.distanceAttenuation;\
						atten = lerp( atten, atten * Light.shadowAttenuation, shadow );\
						half3 transmission = max( 0, -dot( inputData.normalWS, Light.direction ) ) * atten * Transmission;\
						color.rgb += BaseColor * transmission;

					SUM_LIGHT_TRANSMISSION( GetMainLight( inputData.shadowCoord ) );

					#if defined(_ADDITIONAL_LIGHTS)
						uint meshRenderingLayers = GetMeshRenderingLayer();
						uint pixelLightCount = GetAdditionalLightsCount();
						#if USE_FORWARD_PLUS
							for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
							{
								FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK

								Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
								#ifdef _LIGHT_LAYERS
								if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
								#endif
								{
									SUM_LIGHT_TRANSMISSION( light );
								}
							}
						#endif
						LIGHT_LOOP_BEGIN( pixelLightCount )
							Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
							#ifdef _LIGHT_LAYERS
							if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
							#endif
							{
								SUM_LIGHT_TRANSMISSION( light );
							}
						LIGHT_LOOP_END
					#endif
				}
				#endif

				#ifdef ASE_TRANSLUCENCY
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					#define SUM_LIGHT_TRANSLUCENCY(Light)\
						float3 atten = Light.color * Light.distanceAttenuation;\
						atten = lerp( atten, atten * Light.shadowAttenuation, shadow );\
						half3 lightDir = Light.direction + inputData.normalWS * normal;\
						half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );\
						half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;\
						color.rgb += BaseColor * translucency * strength;

					SUM_LIGHT_TRANSLUCENCY( GetMainLight( inputData.shadowCoord ) );

					#if defined(_ADDITIONAL_LIGHTS)
						uint meshRenderingLayers = GetMeshRenderingLayer();
						uint pixelLightCount = GetAdditionalLightsCount();
						#if USE_FORWARD_PLUS
							for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
							{
								FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK

								Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
								#ifdef _LIGHT_LAYERS
								if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
								#endif
								{
									SUM_LIGHT_TRANSLUCENCY( light );
								}
							}
						#endif
						LIGHT_LOOP_BEGIN( pixelLightCount )
							Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
							#ifdef _LIGHT_LAYERS
							if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
							#endif
							{
								SUM_LIGHT_TRANSLUCENCY( light );
							}
						LIGHT_LOOP_END
					#endif
				}
				#endif

				#ifdef ASE_REFRACTION
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, float4( WorldNormal,0 ) ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos.xy ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				#ifdef _WRITE_RENDERING_LAYERS
					uint renderingLayers = GetMeshRenderingLayer();
					outRenderingLayers = float4( EncodeMeshRenderingLayer( renderingLayers ), 0, 0, 0 );
				#endif

				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off
			ColorMask 0

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#define ASE_FOG 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _SPECULAR_SETUP 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

			

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_SHADOWCASTER

			
            #if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#endif
		

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#if defined(LOD_FADE_CROSSFADE)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
            #endif

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD1;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD2;
				#endif				
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			float3 _LightDirection;
			float3 _LightPosition;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir(v.normalOS);

				#if _CASTING_PUNCTUAL_LIGHT_SHADOW
					float3 lightDirectionWS = normalize(_LightPosition - positionWS);
				#else
					float3 lightDirectionWS = _LightDirection;
				#endif

				float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

				#if UNITY_REVERSED_Z
					positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#else
					positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.positionCS = positionCS;
				o.clipPosV = positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(	VertexOutput IN
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord3.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				float Alpha = 1;
				float AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );
				float AlphaClipThresholdShadow = 0.5;

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.positionCS.z;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#if defined(LOD_FADE_CROSSFADE)
					LODFadeCrossFade( IN.positionCS );
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

			

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#define ASE_FOG 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _SPECULAR_SETUP 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_DEPTHONLY

			
            #if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#endif
		

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#if defined(LOD_FADE_CROSSFADE)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
            #endif

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 positionWS : TEXCOORD1;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = vertexInput.positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.positionCS = vertexInput.positionCS;
				o.clipPosV = vertexInput.positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(	VertexOutput IN
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord3.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				float Alpha = 1;
				float AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.positionCS.z;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#if defined(LOD_FADE_CROSSFADE)
					LODFadeCrossFade( IN.positionCS );
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#pragma shader_feature_local_fragment _SPECULAR_SETUP
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature EDITOR_VISUALIZATION

			#define SHADERPASS SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef EDITOR_VISUALIZATION
					float4 VizUV : TEXCOORD2;
					float4 LightCoord : TEXCOORD3;
				#endif
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;
			sampler2D _NoiseMapGrayscale;
			sampler2D _MaskMapRGBA;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, gradient.colorsLength-1));
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = SRGBToLinear(color);
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, gradient.alphasLength-1));
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord4.xy = v.texcoord0.xy;
				o.ase_texcoord5 = v.positionOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				o.positionCS = MetaVertexPosition( v.positionOS, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );

				#ifdef EDITOR_VISUALIZATION
					float2 VizUV = 0;
					float4 LightCoord = 0;
					UnityEditorVizData(v.positionOS.xyz, v.texcoord0.xy, v.texcoord1.xy, v.texcoord2.xy, VizUV, LightCoord);
					o.VizUV = float4(VizUV, 0, 0);
					o.LightCoord = LightCoord;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.texcoord0 = v.texcoord0;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.texcoord0 = patch[0].texcoord0 * bary.x + patch[1].texcoord0 * bary.y + patch[2].texcoord0 * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_AlbedoMap285_g2140 = IN.ase_texcoord4.xy;
				float2 uv_AlbedoMap295_g2140 = IN.ase_texcoord4.xy;
				float4 tex2DNode295_g2140 = tex2D( _AlbedoMap, uv_AlbedoMap295_g2140 );
				float2 uv_NoiseMapGrayscale302_g2140 = IN.ase_texcoord4.xy;
				float4 transform307_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2142 = dot( transform307_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2142 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2142 ) * 43758.55 ) ));
				float3 normalizeResult259_g2140 = normalize( IN.ase_texcoord5.xyz );
				float DryLeafPositionMask263_g2140 = ( (distance( normalizeResult259_g2140 , float3( 0,0.8,0 ) )*1.0 + 0.0) * 1 );
				float4 lerpResult294_g2140 = lerp( ( _DryLeafColor * ( tex2DNode295_g2140.g * 2 ) ) , tex2DNode295_g2140 , saturate( (( ( tex2D( _NoiseMapGrayscale, uv_NoiseMapGrayscale302_g2140 ).r * lerpResult10_g2142 * DryLeafPositionMask263_g2140 ) - _SeasonChangeGlobal )*_DryLeavesScale + _DryLeavesOffset) ));
				float4 SeasonControl_Output311_g2140 = lerpResult294_g2140;
				Gradient gradient271_g2140 = NewGradient( 0, 2, 2, float4( 1, 0.276868, 0, 0 ), float4( 0, 1, 0.7818019, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float4 transform273_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2141 = dot( transform273_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2141 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2141 ) * 43758.55 ) ));
				float4 lerpResult282_g2140 = lerp( SeasonControl_Output311_g2140 , ( ( SeasonControl_Output311_g2140 * 0.5 ) + ( SampleGradient( gradient271_g2140, lerpResult10_g2141 ) * SeasonControl_Output311_g2140 ) ) , _ColorVariation);
				float2 uv_MaskMapRGBA313_g2140 = IN.ase_texcoord4.xy;
				float4 lerpResult283_g2140 = lerp( tex2D( _AlbedoMap, uv_AlbedoMap285_g2140 ) , lerpResult282_g2140 , (( _BranchMaskR )?( tex2D( _MaskMapRGBA, uv_MaskMapRGBA313_g2140 ).r ):( 1.0 )));
				float4 GrassColorVariation_Output109_g2140 = lerpResult283_g2140;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float4 transform39_g2140 = mul(GetObjectToWorldMatrix(),float4( IN.ase_texcoord5.xyz , 0.0 ));
				float dotResult52_g2140 = dot( float4( ase_worldViewDir , 0.0 ) , -( float4( _MainLightPosition.xyz , 0.0 ) + ( (( _TranslucencyFluffiness )?( transform39_g2140 ):( float4( IN.ase_texcoord5.xyz , 0.0 ) )) * _TranslucencyRange ) ) );
				float2 uv_MaskMapRGBA58_g2140 = IN.ase_texcoord4.xy;
				float ase_lightIntensity = max( max( _MainLightColor.r, _MainLightColor.g ), _MainLightColor.b );
				float4 ase_lightColor = float4( _MainLightColor.rgb / ase_lightIntensity, ase_lightIntensity );
				float TobyTranslucency66_g2140 = ( saturate( dotResult52_g2140 ) * tex2D( _MaskMapRGBA, uv_MaskMapRGBA58_g2140 ).b * ase_lightColor.a );
				float TranslucencyIntensity72_g2140 = _TranslucencyPower;
				float4 Albedo_Output160_g2140 = ( ( _AlbedoColor * GrassColorVariation_Output109_g2140 ) * (1.0 + (TobyTranslucency66_g2140 - 0.0) * (TranslucencyIntensity72_g2140 - 1.0) / (1.0 - 0.0)) );
				
				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord4.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				float3 BaseColor = Albedo_Output160_g2140.rgb;
				float3 Emission = 0;
				float Alpha = 1;
				float AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = BaseColor;
				metaInput.Emission = Emission;
				#ifdef EDITOR_VISUALIZATION
					metaInput.VizUV = IN.VizUV.xy;
					metaInput.LightCoord = IN.LightCoord;
				#endif

				return UnityMetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;
			sampler2D _NoiseMapGrayscale;
			sampler2D _MaskMapRGBA;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, gradient.colorsLength-1));
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = SRGBToLinear(color);
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, gradient.alphasLength-1));
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.positionOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = vertexInput.positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.positionCS = vertexInput.positionCS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_AlbedoMap285_g2140 = IN.ase_texcoord2.xy;
				float2 uv_AlbedoMap295_g2140 = IN.ase_texcoord2.xy;
				float4 tex2DNode295_g2140 = tex2D( _AlbedoMap, uv_AlbedoMap295_g2140 );
				float2 uv_NoiseMapGrayscale302_g2140 = IN.ase_texcoord2.xy;
				float4 transform307_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2142 = dot( transform307_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2142 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2142 ) * 43758.55 ) ));
				float3 normalizeResult259_g2140 = normalize( IN.ase_texcoord3.xyz );
				float DryLeafPositionMask263_g2140 = ( (distance( normalizeResult259_g2140 , float3( 0,0.8,0 ) )*1.0 + 0.0) * 1 );
				float4 lerpResult294_g2140 = lerp( ( _DryLeafColor * ( tex2DNode295_g2140.g * 2 ) ) , tex2DNode295_g2140 , saturate( (( ( tex2D( _NoiseMapGrayscale, uv_NoiseMapGrayscale302_g2140 ).r * lerpResult10_g2142 * DryLeafPositionMask263_g2140 ) - _SeasonChangeGlobal )*_DryLeavesScale + _DryLeavesOffset) ));
				float4 SeasonControl_Output311_g2140 = lerpResult294_g2140;
				Gradient gradient271_g2140 = NewGradient( 0, 2, 2, float4( 1, 0.276868, 0, 0 ), float4( 0, 1, 0.7818019, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float4 transform273_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2141 = dot( transform273_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2141 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2141 ) * 43758.55 ) ));
				float4 lerpResult282_g2140 = lerp( SeasonControl_Output311_g2140 , ( ( SeasonControl_Output311_g2140 * 0.5 ) + ( SampleGradient( gradient271_g2140, lerpResult10_g2141 ) * SeasonControl_Output311_g2140 ) ) , _ColorVariation);
				float2 uv_MaskMapRGBA313_g2140 = IN.ase_texcoord2.xy;
				float4 lerpResult283_g2140 = lerp( tex2D( _AlbedoMap, uv_AlbedoMap285_g2140 ) , lerpResult282_g2140 , (( _BranchMaskR )?( tex2D( _MaskMapRGBA, uv_MaskMapRGBA313_g2140 ).r ):( 1.0 )));
				float4 GrassColorVariation_Output109_g2140 = lerpResult283_g2140;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float4 transform39_g2140 = mul(GetObjectToWorldMatrix(),float4( IN.ase_texcoord3.xyz , 0.0 ));
				float dotResult52_g2140 = dot( float4( ase_worldViewDir , 0.0 ) , -( float4( _MainLightPosition.xyz , 0.0 ) + ( (( _TranslucencyFluffiness )?( transform39_g2140 ):( float4( IN.ase_texcoord3.xyz , 0.0 ) )) * _TranslucencyRange ) ) );
				float2 uv_MaskMapRGBA58_g2140 = IN.ase_texcoord2.xy;
				float ase_lightIntensity = max( max( _MainLightColor.r, _MainLightColor.g ), _MainLightColor.b );
				float4 ase_lightColor = float4( _MainLightColor.rgb / ase_lightIntensity, ase_lightIntensity );
				float TobyTranslucency66_g2140 = ( saturate( dotResult52_g2140 ) * tex2D( _MaskMapRGBA, uv_MaskMapRGBA58_g2140 ).b * ase_lightColor.a );
				float TranslucencyIntensity72_g2140 = _TranslucencyPower;
				float4 Albedo_Output160_g2140 = ( ( _AlbedoColor * GrassColorVariation_Output109_g2140 ) * (1.0 + (TobyTranslucency66_g2140 - 0.0) * (TranslucencyIntensity72_g2140 - 1.0) / (1.0 - 0.0)) );
				
				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord2.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				float3 BaseColor = Albedo_Output160_g2140.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );

				half4 color = half4(BaseColor, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthNormals"
			Tags { "LightMode"="DepthNormals" }

			ZWrite On
			Blend One Zero
			ZTest LEqual
			ZWrite On

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#define ASE_FOG 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _SPECULAR_SETUP 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			#pragma vertex vert
			#pragma fragment frag

			

			

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
			//#define SHADERPASS SHADERPASS_DEPTHNORMALS

			
            #if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#endif
		

			
			#if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
			#endif
		

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#if defined(LOD_FADE_CROSSFADE)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
            #endif

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float4 worldTangent : TEXCOORD2;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD3;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD4;
				#endif
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _NormalMap;
			sampler2D _AlbedoMap;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord5.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;
				v.tangentOS = v.tangentOS;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );

				float3 normalWS = TransformObjectToWorldNormal( v.normalOS );
				float4 tangentWS = float4( TransformObjectToWorldDir( v.tangentOS.xyz ), v.tangentOS.w );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = vertexInput.positionWS;
				#endif

				o.worldNormal = normalWS;
				o.worldTangent = tangentWS;

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.positionCS = vertexInput.positionCS;
				o.clipPosV = vertexInput.positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void frag(	VertexOutput IN
						, out half4 outNormalWS : SV_Target0
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						#ifdef _WRITE_RENDERING_LAYERS
						, out float4 outRenderingLayers : SV_Target1
						#endif
						 )
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				float3 WorldNormal = IN.worldNormal;
				float4 WorldTangent = IN.worldTangent;

				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_NormalMap189_g2140 = IN.ase_texcoord5.xy;
				float3 unpack189_g2140 = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap189_g2140 ), _NormalIntensity );
				unpack189_g2140.z = lerp( 1, unpack189_g2140.z, saturate(_NormalIntensity) );
				float3 Normal_Output154_g2140 = unpack189_g2140;
				
				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord5.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				float3 Normal = Normal_Output154_g2140;
				float Alpha = 1;
				float AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.positionCS.z;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#if defined(LOD_FADE_CROSSFADE)
					LODFadeCrossFade( IN.positionCS );
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				#if defined(_GBUFFER_NORMALS_OCT)
					float2 octNormalWS = PackNormalOctQuadEncode(WorldNormal);
					float2 remappedOctNormalWS = saturate(octNormalWS * 0.5 + 0.5);
					half3 packedNormalWS = PackFloat2To888(remappedOctNormalWS);
					outNormalWS = half4(packedNormalWS, 0.0);
				#else
					#if defined(_NORMALMAP)
						#if _NORMAL_DROPOFF_TS
							float crossSign = (WorldTangent.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
							float3 bitangent = crossSign * cross(WorldNormal.xyz, WorldTangent.xyz);
							float3 normalWS = TransformTangentToWorld(Normal, half3x3(WorldTangent.xyz, bitangent, WorldNormal.xyz));
						#elif _NORMAL_DROPOFF_OS
							float3 normalWS = TransformObjectToWorldNormal(Normal);
						#elif _NORMAL_DROPOFF_WS
							float3 normalWS = Normal;
						#endif
					#else
						float3 normalWS = WorldNormal;
					#endif
					outNormalWS = half4(NormalizeNormalPerPixel(normalWS), 0.0);
				#endif

				#ifdef _WRITE_RENDERING_LAYERS
					uint renderingLayers = GetMeshRenderingLayer();
					outRenderingLayers = float4( EncodeMeshRenderingLayer( renderingLayers ), 0, 0, 0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "GBuffer"
			Tags { "LightMode"="UniversalGBuffer" }

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _SPECULAR_SETUP 1
			#pragma shader_feature_local_fragment _SPECULAR_SETUP
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
			#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION

			
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
           

			

			#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
			#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
      
			

			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment _ DEBUG_DISPLAY

			

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_GBUFFER

			
            #if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#endif
		

			
			#if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
			#endif
		

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#if defined(LOD_FADE_CROSSFADE)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
            #endif
			
			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
				#define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON
			#pragma shader_feature_local _SPECULARBACKFACEOCCLUSION_ON


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				float4 lightmapUVOrVertexSH : TEXCOORD1;
				half4 fogFactorAndVertexLight : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD6;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
				float2 dynamicLightmapUV : TEXCOORD7;
				#endif
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;
			sampler2D _NoiseMapGrayscale;
			sampler2D _MaskMapRGBA;
			sampler2D _NormalMap;
			sampler2D _SpecularMapGrayscale;


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, gradient.colorsLength-1));
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = SRGBToLinear(color);
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, gradient.alphasLength-1));
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord8.xy = v.texcoord.xy;
				o.ase_texcoord9 = v.positionOS;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;
				v.tangentOS = v.tangentOS;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );
				VertexNormalInputs normalInput = GetVertexNormalInputs( v.normalOS, v.tangentOS );

				o.tSpace0 = float4( normalInput.normalWS, vertexInput.positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, vertexInput.positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, vertexInput.positionWS.z);

				#if defined(LIGHTMAP_ON)
					OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy);
				#endif

				#if defined(DYNAMICLIGHTMAP_ON)
					o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif

				#if !defined(LIGHTMAP_ON)
					OUTPUT_SH(normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz);
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord.xy;
					o.lightmapUVOrVertexSH.xy = v.texcoord.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( vertexInput.positionWS, normalInput.normalWS );

				o.fogFactorAndVertexLight = half4(0, vertexLight);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.positionCS = vertexInput.positionCS;
				o.clipPosV = vertexInput.positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			FragmentOutput frag ( VertexOutput IN
								#ifdef ASE_DEPTH_WRITE_ON
								,out float outputDepth : ASE_SV_DEPTH
								#endif
								 )
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#if defined(LOD_FADE_CROSSFADE)
					LODFadeCrossFade( IN.positionCS );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif

				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				float2 NormalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#else
					ShadowCoords = float4(0, 0, 0, 0);
				#endif

				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float2 uv_AlbedoMap285_g2140 = IN.ase_texcoord8.xy;
				float2 uv_AlbedoMap295_g2140 = IN.ase_texcoord8.xy;
				float4 tex2DNode295_g2140 = tex2D( _AlbedoMap, uv_AlbedoMap295_g2140 );
				float2 uv_NoiseMapGrayscale302_g2140 = IN.ase_texcoord8.xy;
				float4 transform307_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2142 = dot( transform307_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2142 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2142 ) * 43758.55 ) ));
				float3 normalizeResult259_g2140 = normalize( IN.ase_texcoord9.xyz );
				float DryLeafPositionMask263_g2140 = ( (distance( normalizeResult259_g2140 , float3( 0,0.8,0 ) )*1.0 + 0.0) * 1 );
				float4 lerpResult294_g2140 = lerp( ( _DryLeafColor * ( tex2DNode295_g2140.g * 2 ) ) , tex2DNode295_g2140 , saturate( (( ( tex2D( _NoiseMapGrayscale, uv_NoiseMapGrayscale302_g2140 ).r * lerpResult10_g2142 * DryLeafPositionMask263_g2140 ) - _SeasonChangeGlobal )*_DryLeavesScale + _DryLeavesOffset) ));
				float4 SeasonControl_Output311_g2140 = lerpResult294_g2140;
				Gradient gradient271_g2140 = NewGradient( 0, 2, 2, float4( 1, 0.276868, 0, 0 ), float4( 0, 1, 0.7818019, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float4 transform273_g2140 = mul(GetObjectToWorldMatrix(),float4( 1,1,1,1 ));
				float dotResult4_g2141 = dot( transform273_g2140.xy , float2( 12.9898,78.233 ) );
				float lerpResult10_g2141 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g2141 ) * 43758.55 ) ));
				float4 lerpResult282_g2140 = lerp( SeasonControl_Output311_g2140 , ( ( SeasonControl_Output311_g2140 * 0.5 ) + ( SampleGradient( gradient271_g2140, lerpResult10_g2141 ) * SeasonControl_Output311_g2140 ) ) , _ColorVariation);
				float2 uv_MaskMapRGBA313_g2140 = IN.ase_texcoord8.xy;
				float4 lerpResult283_g2140 = lerp( tex2D( _AlbedoMap, uv_AlbedoMap285_g2140 ) , lerpResult282_g2140 , (( _BranchMaskR )?( tex2D( _MaskMapRGBA, uv_MaskMapRGBA313_g2140 ).r ):( 1.0 )));
				float4 GrassColorVariation_Output109_g2140 = lerpResult283_g2140;
				float4 transform39_g2140 = mul(GetObjectToWorldMatrix(),float4( IN.ase_texcoord9.xyz , 0.0 ));
				float dotResult52_g2140 = dot( float4( WorldViewDirection , 0.0 ) , -( float4( _MainLightPosition.xyz , 0.0 ) + ( (( _TranslucencyFluffiness )?( transform39_g2140 ):( float4( IN.ase_texcoord9.xyz , 0.0 ) )) * _TranslucencyRange ) ) );
				float2 uv_MaskMapRGBA58_g2140 = IN.ase_texcoord8.xy;
				float ase_lightIntensity = max( max( _MainLightColor.r, _MainLightColor.g ), _MainLightColor.b );
				float4 ase_lightColor = float4( _MainLightColor.rgb / ase_lightIntensity, ase_lightIntensity );
				float TobyTranslucency66_g2140 = ( saturate( dotResult52_g2140 ) * tex2D( _MaskMapRGBA, uv_MaskMapRGBA58_g2140 ).b * ase_lightColor.a );
				float TranslucencyIntensity72_g2140 = _TranslucencyPower;
				float4 Albedo_Output160_g2140 = ( ( _AlbedoColor * GrassColorVariation_Output109_g2140 ) * (1.0 + (TobyTranslucency66_g2140 - 0.0) * (TranslucencyIntensity72_g2140 - 1.0) / (1.0 - 0.0)) );
				
				float2 uv_NormalMap189_g2140 = IN.ase_texcoord8.xy;
				float3 unpack189_g2140 = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap189_g2140 ), _NormalIntensity );
				unpack189_g2140.z = lerp( 1, unpack189_g2140.z, saturate(_NormalIntensity) );
				float3 Normal_Output154_g2140 = unpack189_g2140;
				
				float temp_output_349_0_g2140 = ( 0.2 * _SpecularPower );
				float2 uv_SpecularMapGrayscale251_g2140 = IN.ase_texcoord8.xy;
				#ifdef _SPECULARBACKFACEOCCLUSION_ON
				float staticSwitch340_g2140 = ( temp_output_349_0_g2140 * saturate( (tex2D( _SpecularMapGrayscale, uv_SpecularMapGrayscale251_g2140 ).r*_SpecularMapScale + _SpecularMapOffset) ) );
				#else
				float staticSwitch340_g2140 = temp_output_349_0_g2140;
				#endif
				float Specular_Output158_g2140 = staticSwitch340_g2140;
				float3 temp_cast_7 = (Specular_Output158_g2140).xxx;
				
				float2 uv_MaskMapRGBA89_g2140 = IN.ase_texcoord8.xy;
				float4 tex2DNode89_g2140 = tex2D( _MaskMapRGBA, uv_MaskMapRGBA89_g2140 );
				float Smoothness_Output159_g2140 = ( tex2DNode89_g2140.a * _SmoothnessIntensity );
				
				float AoMapBase103_g2140 = tex2DNode89_g2140.g;
				float Ao_Output157_g2140 = ( pow( abs( AoMapBase103_g2140 ) , _AmbientOcclusionIntensity ) * ( 1.5 / ( ( saturate( TobyTranslucency66_g2140 ) * TranslucencyIntensity72_g2140 ) + 1.5 ) ) );
				
				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord8.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				float3 BaseColor = Albedo_Output160_g2140.rgb;
				float3 Normal = Normal_Output154_g2140;
				float3 Emission = 0;
				float3 Specular = temp_cast_7;
				float Metallic = 0;
				float Smoothness = Smoothness_Output159_g2140;
				float Occlusion = Ao_Output157_g2140;
				float Alpha = 1;
				float AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.positionCS.z;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData = (InputData)0;
				inputData.positionWS = WorldPosition;
				inputData.positionCS = IN.positionCS;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
						inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
						inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
						inputData.normalWS = Normal;
					#endif
				#else
					inputData.normalWS = WorldNormal;
				#endif

				inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				inputData.viewDirectionWS = SafeNormalize( WorldViewDirection );

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				#ifdef ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#else
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, IN.dynamicLightmapUV.xy, SH, inputData.normalWS);
					#else
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
					#endif
				#endif

				inputData.normalizedScreenSpaceUV = NormalizedScreenSpaceUV;
				inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUVOrVertexSH.xy);

				#if defined(DEBUG_DISPLAY)
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
						#endif
					#if defined(LIGHTMAP_ON)
						inputData.staticLightmapUV = IN.lightmapUVOrVertexSH.xy;
					#else
						inputData.vertexSH = SH;
					#endif
				#endif

				#ifdef _DBUFFER
					ApplyDecal(IN.positionCS,
						BaseColor,
						Specular,
						inputData.normalWS,
						Metallic,
						Occlusion,
						Smoothness);
				#endif

				BRDFData brdfData;
				InitializeBRDFData
				(BaseColor, Metallic, Specular, Smoothness, Alpha, brdfData);

				Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
				half4 color;
				MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, inputData.shadowMask);
				color.rgb = GlobalIllumination(brdfData, inputData.bakedGI, Occlusion, inputData.positionWS, inputData.normalWS, inputData.viewDirectionWS);
				color.a = Alpha;

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return BRDFDataToGbuffer(brdfData, inputData, Smoothness, Emission + color.rgb, Occlusion);
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull Off
			AlphaToMask Off

			HLSLPROGRAM

			

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			

			#pragma vertex vert
			#pragma fragment frag

			#define SCENESELECTIONPASS 1

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			
            #if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#endif
		

			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				surfaceDescription.Alpha = 1;
				surfaceDescription.AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;

				#ifdef SCENESELECTIONPASS
					outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				#elif defined(SCENEPICKINGPASS)
					outColor = _SelectionID;
				#endif

				return outColor;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ScenePickingPass"
			Tags { "LightMode"="Picking" }

			AlphaToMask Off

			HLSLPROGRAM

			

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 140008


			

			#pragma vertex vert
			#pragma fragment frag

		    #define SCENEPICKINGPASS 1

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"

			

			

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			
            #if ASE_SRP_VERSION >=140007
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#endif
		

			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _WINDTYPE_GENTLEBREEZE _WINDTYPE_WINDOFF
			#pragma shader_feature_local _CONTINUOUSWINDGENTLE_ON


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _AlbedoColor;
			float4 _DryLeafColor;
			float _GlobalWindStrength;
			float _SmoothnessIntensity;
			float _SpecularMapOffset;
			float _SpecularMapScale;
			float _SpecularPower;
			float _NormalIntensity;
			float _TranslucencyPower;
			float _TranslucencyRange;
			float _TranslucencyFluffiness;
			float _BranchMaskR;
			float _ColorVariation;
			float _DryLeavesOffset;
			float _DryLeavesScale;
			float _SeasonChangeGlobal;
			float _StrongWindSpeed;
			float _AmbientOcclusionIntensity;
			float _AlphaClip;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			sampler2D _AlbedoMap;


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float StrongWindSpeed921_g2152 = _StrongWindSpeed;
				float mulTime673_g2152 = _TimeParameters.x * 1.5;
				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				float2 appendResult684_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float simpleNoise678_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * mulTime673_g2152 ) + appendResult684_g2152 )*4.0 );
				float simpleNoise672_g2152 = SimpleNoise( ( ( StrongWindSpeed921_g2152 * _TimeParameters.x ) + appendResult684_g2152 ) );
				float temp_output_683_0_g2152 = ( (simpleNoise672_g2152*2.2 + -1.05) * 1.6 );
				float3 temp_output_696_0_g2152 = ( v.positionOS.xyz * 0.3 );
				float3 NormaliseRotationAxis218_g2152 = float3(0,1,0);
				float simplePerlin2D691_g2152 = snoise( ( ase_worldPos + ( _TimeParameters.z * 1.5 ) ).xy*1.2 );
				simplePerlin2D691_g2152 = simplePerlin2D691_g2152*0.5 + 0.5;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 clampResult99_g2152 = clamp( (ase_objectScale*1.8 + -0.8) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskB112_g2152 = clampResult99_g2152;
				float3 rotatedValue695_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_696_0_g2152, NormaliseRotationAxis218_g2152, ( simplePerlin2D691_g2152 * 1.0 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir731_g2152 = mul( GetWorldToObjectMatrix(), float4( ( ( (simpleNoise678_g2152*3.09 + -1.5) * 1.6 ) * temp_output_683_0_g2152 * ( temp_output_683_0_g2152 * ( temp_output_696_0_g2152 - rotatedValue695_g2152 ) ) * sin( _TimeParameters.x * 0.125 ) * v.positionOS.xyz.y ), 0 ) ).xyz;
				float2 appendResult644_g2152 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 BasicWindPositionAndTime39_g2152 = ( appendResult644_g2152 + _TimeParameters.x );
				float simplePerlin2D69_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.6 );
				simplePerlin2D69_g2152 = simplePerlin2D69_g2152*0.5 + 0.5;
				float NoiseLarge101_g2152 = simplePerlin2D69_g2152;
				float mulTime702_g2152 = _TimeParameters.x * 2.0;
				float3 rotatedValue711_g2152 = RotateAroundAxis( ( saturate( v.positionOS.xyz.y ) * v.positionOS.xyz ), ( cos( ( ( ase_worldPos * 0.2 ) + mulTime702_g2152 ) ) * v.positionOS.xyz.y * float3(0.5,0.05,0.5) ), NormaliseRotationAxis218_g2152, NoiseLarge101_g2152 );
				float3 worldToObjDir715_g2152 = mul( GetWorldToObjectMatrix(), float4( rotatedValue711_g2152, 0 ) ).xyz;
				float3 clampResult148_g2152 = clamp( (ase_objectScale*1.0 + -0.6) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskA177_g2152 = clampResult148_g2152;
				float simplePerlin2D211_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.1 );
				float MaskRotation268_g2152 = saturate( simplePerlin2D211_g2152 );
				float3 clampResult195_g2152 = clamp( (ase_objectScale*-0.7 + 2.2) , float3( 0,0,0 ) , float3( 1,1,1 ) );
				float3 Scale_MaskC216_g2152 = clampResult195_g2152;
				float simplePerlin2D651_g2152 = snoise( BasicWindPositionAndTime39_g2152*0.09995 );
				simplePerlin2D651_g2152 = simplePerlin2D651_g2152*0.5 + 0.5;
				float MaskRotation2649_g2152 = saturate( simplePerlin2D651_g2152 );
				float3 temp_output_725_0_g2152 = ( v.positionOS.xyz * 0.2 );
				float3 rotatedValue727_g2152 = RotateAroundAxis( float3( 0,0,0 ), temp_output_725_0_g2152, NormaliseRotationAxis218_g2152, ( ( NoiseLarge101_g2152 * StrongWindSpeed921_g2152 ) * v.positionOS.xyz.y * Scale_MaskC216_g2152 * Scale_MaskB112_g2152 ).x );
				float3 worldToObjDir732_g2152 = mul( GetWorldToObjectMatrix(), float4( ( rotatedValue727_g2152 - temp_output_725_0_g2152 ), 0 ) ).xyz;
				#ifdef _CONTINUOUSWINDGENTLE_ON
				float3 staticSwitch735_g2152 = worldToObjDir732_g2152;
				#else
				float3 staticSwitch735_g2152 = ( MaskRotation2649_g2152 * worldToObjDir732_g2152 );
				#endif
				float3 temp_cast_3 = (0.0).xxx;
				#if defined(_WINDTYPE_GENTLEBREEZE)
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#elif defined(_WINDTYPE_WINDOFF)
				float3 staticSwitch1011_g2152 = temp_cast_3;
				#else
				float3 staticSwitch1011_g2152 = (float3( 0,0,0 ) + (( ( worldToObjDir731_g2152 - ( worldToObjDir715_g2152 * v.positionOS.xyz.y * Scale_MaskA177_g2152 * MaskRotation268_g2152 * Scale_MaskC216_g2152 ) ) + staticSwitch735_g2152 ) - float3( 0,0,0 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( 0,0,0 )));
				#endif
				float3 FinalWind_Output350_g2152 = ( _GlobalWindStrength * staticSwitch1011_g2152 );
				
				float3 LightDetectBackface_Output156_g2140 = float3(0,1,0);
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = FinalWind_Output350_g2152;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = LightDetectBackface_Output156_g2140;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 uv_AlbedoMap86_g2140 = IN.ase_texcoord.xy;
				float Opacity_Output155_g2140 = ( 1.0 - tex2D( _AlbedoMap, uv_AlbedoMap86_g2140 ).a );
				

				surfaceDescription.Alpha = 1;
				surfaceDescription.AlphaClipThreshold = ( Opacity_Output155_g2140 * _AlphaClip );

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
						clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;

				#ifdef SCENESELECTIONPASS
					outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				#elif defined(SCENEPICKINGPASS)
					outColor = _SelectionID;
				#endif

				return outColor;
			}

			ENDHLSL
		}
		
	}
	
	CustomEditor "UnityEditor.ShaderGraphLitGUI"
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback Off
}
/*ASEBEGIN
Version=19303
Node;AmplifyShaderEditor.FunctionNode;3972;-512,0;Inherit;False;(TTFE) Grass Foliage_Shading;1;;2140;c37c82b3855ad564a8be75e0d3f5f6bc;0;0;7;COLOR;0;FLOAT3;180;FLOAT;186;FLOAT;182;FLOAT;183;FLOAT;184;FLOAT3;188
Node;AmplifyShaderEditor.RangedFloatNode;3948;-368,352;Inherit;False;Property;_AlphaClip;Alpha Clip;0;0;Create;True;0;0;0;False;0;False;1.4;1.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;3975;-528,256;Inherit;False;(TTFE) Grass Foliage_Wind System;24;;2152;587ec12ad6a469d48af93c852f23db70;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3947;-192,320;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3931;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3932;0,0;Float;False;True;-1;2;UnityEditor.ShaderGraphLitGUI;0;12;Toby Fredson/The Toby Foliage Engine/(TTFE) Grass Foliage;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;21;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;2;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;1;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;;0;0;Standard;39;Workflow;0;638455986805275178;Surface;0;638455976694645656;  Refraction Model;0;0;  Blend;0;0;Two Sided;0;638455961779527604;Fragment Normal Space,InvertActionOnDeselection;0;0;Forward Only;0;0;Transmission;0;0;  Transmission Shadow;0.5,False,;0;Translucency;0;0;  Translucency Strength;1,False,;0;  Normal Distortion;0.5,False,;0;  Scattering;2,False,;0;  Direct;0.9,False,;0;  Ambient;0.1,False,;0;  Shadow;0.5,False,;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;GPU Instancing;1;0;LOD CrossFade;1;638455983964265153;Built-in Fog;1;0;_FinalColorxAlpha;0;0;Meta Pass;1;0;Override Baked GI;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Write Depth;0;0;  Early Z;0;0;Vertex Position,InvertActionOnDeselection;1;0;Debug Display;0;0;Clear Coat;0;0;0;10;False;True;True;True;True;True;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3933;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3934;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3935;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3936;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;1;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3937;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthNormals;0;6;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormals;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3938;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;GBuffer;0;7;GBuffer;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;1;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalGBuffer;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3939;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;SceneSelectionPass;0;8;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3940;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphLitGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ScenePickingPass;0;9;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Lit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
WireConnection;3947;0;3972;184
WireConnection;3947;1;3948;0
WireConnection;3932;0;3972;0
WireConnection;3932;1;3972;180
WireConnection;3932;9;3972;186
WireConnection;3932;4;3972;182
WireConnection;3932;5;3972;183
WireConnection;3932;7;3947;0
WireConnection;3932;8;3975;0
WireConnection;3932;10;3972;188
ASEEND*/
//CHKSM=3E0ECC94FE525536168C330AFEBEE32A08E38CDE