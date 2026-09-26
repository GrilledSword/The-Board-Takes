using BoardTakes.Core;
using UnityEngine;

namespace BoardTakes.Settings
{
    public sealed class SettingsService : ISettingsService
    {
        const string PrefsKey = "boardtakes.settings.v1";

        public GameSettings Current { get; private set; } = GameSettings.CreateDefault();

        readonly GraphicsApplicator _graphics = new();
        readonly AudioApplicator _audio = new();
        readonly VideoApplicator _video = new();

        public void Load()
        {
            if (!PlayerPrefs.HasKey(PrefsKey))
            {
                Current = GameSettings.CreateDefault();
                DetectNativeVideo(Current.Video);
                return;
            }

            var json = PlayerPrefs.GetString(PrefsKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                Current = GameSettings.CreateDefault();
                DetectNativeVideo(Current.Video);
                return;
            }

            try
            {
                Current = JsonUtility.FromJson<GameSettings>(json) ?? GameSettings.CreateDefault();
            }
            catch
            {
                BoardLog.Warn("Settings JSON rotten. Defaults. The board resets the ledger, not the GPU.");
                Current = GameSettings.CreateDefault();
            }

            if (Current.Video.ResolutionWidth <= 0)
                DetectNativeVideo(Current.Video);
        }

        public void ApplyAndSave()
        {
            _video.Apply(Current.Video);
            _graphics.Apply(Current.Graphics);
            _audio.Apply(Current.Audio);
            ApplyLocaleSideEffect();
            Persist();
            if (GameKernel.Exists)
                GameKernel.Instance.Events.Publish(new SettingsApplied(true));
        }

        public void ResetToDefaults()
        {
            Current = GameSettings.CreateDefault();
            DetectNativeVideo(Current.Video);
            ApplyAndSave();
        }

        void Persist()
        {
            var json = JsonUtility.ToJson(Current, prettyPrint: false);
            PlayerPrefs.SetString(PrefsKey, json);
            PlayerPrefs.Save();
        }

        static void DetectNativeVideo(VideoSettings video)
        {
            video.ResolutionWidth = Screen.currentResolution.width;
            video.ResolutionHeight = Screen.currentResolution.height;
            video.RefreshRateHz = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
        }

        void ApplyLocaleSideEffect()
        {
            if (!GameKernel.Exists || GameKernel.Instance.Loc == null) return;
            if (GameKernel.Instance.Loc.CurrentCode == Current.LocaleCode) return;
            GameKernel.Instance.Loc.SetLocale(Current.LocaleCode);
        }
    }
}
