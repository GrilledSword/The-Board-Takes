using System;
using UnityEngine;

namespace BoardTakes.Core
{
    /// <summary>
    /// Full AAA settings contract. UI mutates this object, then
    /// SettingsService.ApplyAndSave() pushes it to the device.
    /// Fields stay public so JsonUtility can round-trip them.
    /// </summary>
    [Serializable]
    public sealed class GameSettings
    {
        public VideoSettings Video = new();
        public GraphicsSettingsBlock Graphics = new();
        public AudioSettingsBlock Audio = new();
        public ControlsSettings Controls = new();
        public GameplaySettings Gameplay = new();
        public AccessibilitySettings Accessibility = new();
        public string LocaleCode = "en";
        public int SchemaVersion = 1;

        public static GameSettings CreateDefault() => new();
    }

    public enum ScreenModeOption { Fullscreen = 0, Borderless = 1, Windowed = 2 }
    public enum GraphicsPreset { Low = 0, Medium = 1, High = 2, Ultra = 3, Custom = 4 }
    public enum UpscalerOption { None = 0, FSR = 1, DLSS = 2, XeSS = 3 }
    public enum AntiAliasOption { None = 0, FXAA = 1, SMAA = 2, TAA = 3 }
    public enum ShadowQualityOption { Off = 0, Hard = 1, SoftLow = 2, SoftHigh = 3 }
    public enum AudioProfileOption { Headphones = 0, Stereo = 1, Surround51 = 2, Surround71 = 3 }
    public enum VoiceActivationMode { PushToTalk = 0, VoiceActivation = 1 }
    public enum InputDeviceProfile { KeyboardMouse = 0, Gamepad = 1 }
    public enum DifficultyOption { Narrative = 0, Standard = 1, Hardcore = 2 }
    public enum ColorBlindOption { Off = 0, Protanopia = 1, Deuteranopia = 2, Tritanopia = 3 }
    public enum MinimapOrientation { Fixed = 0, Rotating = 1 }

    [Serializable]
    public sealed class VideoSettings
    {
        public ScreenModeOption ScreenMode = ScreenModeOption.Fullscreen;
        public int ResolutionWidth = 0;   // 0 = native
        public int ResolutionHeight = 0;
        public int RefreshRateHz = 0;     // 0 = default
        public bool VSync = true;
        public int FrameLimit = 0;        // 0 = unlimited
        public int UnfocusedFrameLimit = 30;
        public float FovDegrees = 60f;
        public float Brightness = 1f;
        public float Gamma = 1f;
        public bool HdrOutput = false;
    }

    [Serializable]
    public sealed class GraphicsSettingsBlock
    {
        public GraphicsPreset Preset = GraphicsPreset.High;
        public float RenderScale = 1f;
        public UpscalerOption Upscaler = UpscalerOption.None;
        public int TextureMipBias = 0; // 0 full, 1 half, 2 quarter
        public ShadowQualityOption Shadows = ShadowQualityOption.SoftHigh;
        public int ShadowCascades = 4;
        public bool VolumetricFog = true;
        public bool MotionBlur = false;
        public bool FilmGrain = true;
        public bool ChromaticAberration = true;
        public bool Bloom = true;
        public float BloomIntensity = 0.35f;
        public float BloomThreshold = 1.0f;
        public bool AmbientOcclusion = true;
        public AntiAliasOption AntiAliasing = AntiAliasOption.TAA;
    }

    [Serializable]
    public sealed class AudioSettingsBlock
    {
        [Range(0f, 1f)] public float Master = 1f;
        [Range(0f, 1f)] public float Music = 0.7f;
        [Range(0f, 1f)] public float Sfx = 1f;
        [Range(0f, 1f)] public float Dialogue = 1f;
        [Range(0f, 1f)] public float Ui = 0.8f;
        public AudioProfileOption Profile = AudioProfileOption.Stereo;
        public bool SpatialAudio = true;
        public string VoiceInputDevice = "";
        public string VoiceOutputDevice = "";
        public VoiceActivationMode VoiceMode = VoiceActivationMode.PushToTalk;
        [Range(0f, 1f)] public float MicSensitivity = 0.4f;
    }

    [Serializable]
    public sealed class ControlsSettings
    {
        public InputDeviceProfile Device = InputDeviceProfile.KeyboardMouse;
        public float LookSensitivity = 1f;
        public float AdsSensitivity = 0.7f;
        public bool RawMouse = true;
        [Range(0f, 0.4f)] public float StickInnerDeadzone = 0.15f;
        [Range(0.5f, 1f)] public float StickOuterDeadzone = 0.9f;
        [Range(0f, 1f)] public float AimAssist = 0.25f;
        [Range(0f, 1f)] public float Haptics = 0.6f;
        public bool AutoSprint = false;
        public bool CrouchToggle = true;
    }

    [Serializable]
    public sealed class GameplaySettings
    {
        public DifficultyOption Difficulty = DifficultyOption.Standard;
        public bool CrossPlayEnabled = true;
        public string Region = "auto";
        public bool NetworkHud = false;
        public Color CrosshairColor = Color.white;
        public float CrosshairSize = 1f;
        public float CrosshairOpacity = 0.85f;
        public bool DamageNumbers = false;
        public MinimapOrientation Minimap = MinimapOrientation.Fixed;
    }

    [Serializable]
    public sealed class AccessibilitySettings
    {
        public ColorBlindOption ColorBlind = ColorBlindOption.Off;
        public int SubtitleSize = 28;
        [Range(0f, 1f)] public float SubtitleBgOpacity = 0.55f;
        public bool SpeakerNames = true;
        public bool DirectionalSoundIndicators = true;
        public bool PhotosensitiveSafe = false;
        public bool TinnitusMute = false;
        [Range(0f, 1f)] public float CameraShake = 0.4f;
        public bool AutoQte = false;
    }
}
