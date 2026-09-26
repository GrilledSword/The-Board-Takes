using BoardTakes.Core;
using UnityEngine;
#if BOARDTAKES_URP
using UnityEngine.Rendering.Universal;
#endif

namespace BoardTakes.Settings
{
    public sealed class GraphicsApplicator
    {
        public void Apply(GraphicsSettingsBlock g)
        {
            if (g == null) return;

            ApplyPresetFloor(g);

#if BOARDTAKES_URP
            var urp = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (urp != null)
            {
                urp.renderScale = Mathf.Clamp(g.RenderScale, 0.25f, 2f);
                urp.shadowDistance = g.Shadows == ShadowQualityOption.Off ? 0f : urp.shadowDistance;
            }
#endif
            QualitySettings.globalTextureMipmapLimit = Mathf.Clamp(g.TextureMipBias, 0, 3);
            // PATCH slice-01: URP also ships ShadowQuality — qualify the built-in enum.
            QualitySettings.shadows = g.Shadows == ShadowQualityOption.Off
                ? UnityEngine.ShadowQuality.Disable
                : g.Shadows == ShadowQualityOption.Hard
                    ? UnityEngine.ShadowQuality.HardOnly
                    : UnityEngine.ShadowQuality.All;
            QualitySettings.shadowCascades = g.Shadows == ShadowQualityOption.Off ? 0 : Mathf.Clamp(g.ShadowCascades, 1, 4);

            // Film grain / bloom / SSAO / AA are volume-profile knobs.
            // Slice 02 binds them when you drop a Volume into Settings or Gameplay_Core.
            PostFxState.Current = g;
        }

        static void ApplyPresetFloor(GraphicsSettingsBlock g)
        {
            if (g.Preset == GraphicsPreset.Custom) return;
            switch (g.Preset)
            {
                case GraphicsPreset.Low:
                    g.RenderScale = 0.7f;
                    g.Shadows = ShadowQualityOption.Hard;
                    g.ShadowCascades = 1;
                    g.VolumetricFog = false;
                    g.Bloom = false;
                    g.AmbientOcclusion = false;
                    g.AntiAliasing = AntiAliasOption.FXAA;
                    g.TextureMipBias = 2;
                    break;
                case GraphicsPreset.Medium:
                    g.RenderScale = 0.85f;
                    g.Shadows = ShadowQualityOption.SoftLow;
                    g.ShadowCascades = 2;
                    g.VolumetricFog = false;
                    g.AntiAliasing = AntiAliasOption.SMAA;
                    g.TextureMipBias = 1;
                    break;
                case GraphicsPreset.High:
                    g.RenderScale = 1f;
                    g.Shadows = ShadowQualityOption.SoftHigh;
                    g.ShadowCascades = 4;
                    g.VolumetricFog = true;
                    g.AntiAliasing = AntiAliasOption.TAA;
                    g.TextureMipBias = 0;
                    break;
                case GraphicsPreset.Ultra:
                    g.RenderScale = 1f;
                    g.Shadows = ShadowQualityOption.SoftHigh;
                    g.ShadowCascades = 4;
                    g.VolumetricFog = true;
                    g.Bloom = true;
                    g.AmbientOcclusion = true;
                    g.AntiAliasing = AntiAliasOption.TAA;
                    g.TextureMipBias = 0;
                    break;
            }
        }
    }

    public static class PostFxState
    {
        public static GraphicsSettingsBlock Current;
    }
}