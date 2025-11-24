Shader "Custom/UVProjectorReveal_URP"
{
    Properties
    {
        _BaseTex ("Base (Albedo)", 2D) = "white" {}
        _RevealTex ("Reveal Texture", 2D) = "white" {}
        _CookieTex ("Cookie (optional)", 2D) = "white" {}
        _RevealStrength ("Reveal Strength", Range(0,1)) = 1.0
        _Range ("Range (distance)", Float) = 5.0
        _Softness ("Edge Softness", Range(0.001,1)) = 0.2
        _Intensity ("Reveal Intensity", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "PROJECTOR_REVEAL"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnityCG.cginc"

            // Properties
            TEXTURE2D(_BaseTex);
            SAMPLER(sampler_BaseTex);
            TEXTURE2D(_RevealTex);
            SAMPLER(sampler_RevealTex);
            TEXTURE2D(_CookieTex);
            SAMPLER(sampler_CookieTex);

            float _RevealStrength;
            float _Range;
            float _Softness;
            float _Intensity;

            // Matrix from script: projection * view (GPU-ready)
            float4x4 _ProjectorVP;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                o.worldPos = TransformObjectToWorld(v.positionOS);
                return o;
            }

            // utility: sample texture with sampler
            float4 SampleTexture(Texture2D tex, SamplerState samp, float2 uv)
            {
                return tex.Sample(samp, uv);
            }

            float4 frag(Varyings i) : SV_Target
            {
                // base color from regular UV
                float4 baseCol = _BaseTex.Sample(sampler_BaseTex, i.uv);

                // project world position into projector space
                float4 projPos = mul(_ProjectorVP, float4(i.worldPos, 1.0));
                // if w <= 0 then behind projector; still handle gracefully
                float projW = projPos.w;
                // convert to NDC, then to UV
                float2 projUV = projPos.xy / max(projW, 1e-6);
                projUV = projUV * 0.5 + 0.5;

                // check if inside projector frustum (0..1)
                bool inside = (projUV.x >= 0.0 && projUV.x <= 1.0 && projUV.y >= 0.0 && projUV.y <= 1.0 && projW > 0.0);

                float revealMask = 0.0;

                if (inside)
                {
                    // sample cookie if present, else compute radial falloff
                    float cookieValue = 1.0;
                    #if defined(_CookieTex)
                    cookieValue = _CookieTex.Sample(sampler_CookieTex, projUV).r;
                    #endif

                    // distance attenuation: relative to projW (or compute in script provide distance)
                    // using projW as inverse depth; better to pass distance via script for exact falloff.
                    // We'll approximate: stronger when projW small (closer).
                    float distFactor = saturate(1.0 - (projW / _Range));
                    // soften edge via softness param using cookie alpha
                    // combine cookie with radial softness
                    float edge = smoothstep(0.0, _Softness, cookieValue);
                    revealMask = distFactor * edge;
                }

                // sample reveal texture using the object's normal UV (or the projected UV if you want reveal in projector space)
                // two options: reveal texture mapped by object's UV, or reveal texture mapped by projector UV.
                // We'll use projector UV so the message aligns with flashlight.
                float4 revealCol = _RevealTex.Sample(sampler_RevealTex, projUV);

                // final blend (lerp)
                float blend = saturate(revealMask * _RevealStrength * _Intensity);

                // compose
                float4 final = lerp(baseCol, revealCol, blend);

                return final;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
