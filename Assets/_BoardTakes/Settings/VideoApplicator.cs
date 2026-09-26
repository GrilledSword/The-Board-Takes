using BoardTakes.Core;
using UnityEngine;

namespace BoardTakes.Settings
{
    public sealed class VideoApplicator
    {
        public void Apply(VideoSettings v)
        {
            if (v == null) return;

            var mode = v.ScreenMode switch
            {
                ScreenModeOption.Fullscreen => FullScreenMode.ExclusiveFullScreen,
                ScreenModeOption.Borderless => FullScreenMode.FullScreenWindow,
                _ => FullScreenMode.Windowed
            };

            var w = v.ResolutionWidth > 0 ? v.ResolutionWidth : Screen.width;
            var h = v.ResolutionHeight > 0 ? v.ResolutionHeight : Screen.height;

            if (v.RefreshRateHz > 0)
            {
                var rr = new RefreshRate { numerator = (uint)v.RefreshRateHz, denominator = 1 };
                Screen.SetResolution(w, h, mode, rr);
            }
            else
            {
                Screen.SetResolution(w, h, mode);
            }

            QualitySettings.vSyncCount = v.VSync ? 1 : 0;
            Application.targetFrameRate = v.FrameLimit <= 0 ? -1 : v.FrameLimit;

            // Unfocused cap is applied by FocusFrameCapBehaviour on the kernel object.
            FocusFrameCapBehaviour.UnfocusedLimit = v.UnfocusedFrameLimit;
            FocusFrameCapBehaviour.FocusedLimit = v.FrameLimit;
            FocusFrameCapBehaviour.Ensure();
        }
    }
}
