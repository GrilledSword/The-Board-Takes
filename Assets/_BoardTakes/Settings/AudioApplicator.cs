using BoardTakes.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace BoardTakes.Settings
{
    /// <summary>
    /// Applies mixer sliders when an AudioMixer is assigned later.
    /// Without a mixer, only AudioListener.volume (master) is touched.
    /// </summary>
    public sealed class AudioApplicator
    {
        const string MixerResource = "BoardTakes/Audio/MasterMixer";

        public void Apply(AudioSettingsBlock a)
        {
            if (a == null) return;
            AudioListener.volume = Mathf.Clamp01(a.Master);

            var mixer = Resources.Load<AudioMixer>(MixerResource);
            if (mixer == null) return;

            SetDb(mixer, "Master", a.Master);
            SetDb(mixer, "Music", a.Music);
            SetDb(mixer, "SFX", a.Sfx);
            SetDb(mixer, "Dialogue", a.Dialogue);
            SetDb(mixer, "UI", a.Ui);
        }

        static void SetDb(AudioMixer mixer, string param, float linear)
        {
            var clamped = Mathf.Clamp(linear, 0.0001f, 1f);
            var db = Mathf.Log10(clamped) * 20f;
            mixer.SetFloat(param, db);
        }
    }
}
