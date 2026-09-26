using UnityEngine;

namespace BoardTakes.Core
{
    /// <summary>
    /// Process singleton. Created by BootRunner in the Boot scene.
    /// Do not DontDestroyOnLoad this — Boot itself stays loaded.
    /// </summary>
    public sealed class GameKernel : MonoBehaviour
    {
        public static GameKernel Instance { get; private set; }

        public GameEventBus Events { get; } = new GameEventBus();
        public ISceneLoadService Scenes { get; private set; }
        public IGameFlow Flow { get; private set; }
        public ISettingsService Settings { get; private set; }
        public ILocalizationService Loc { get; private set; }
        public IInputService Input { get; private set; }

        public static bool Exists => Instance != null;

        public void Bind(
            ISceneLoadService scenes,
            IGameFlow flow,
            ISettingsService settings,
            ILocalizationService loc,
            IInputService input)
        {
            if (Instance != null && Instance != this)
            {
                BoardLog.Error("Second GameKernel is illegal. The table already has a host.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Scenes = scenes;
            Flow = flow;
            Settings = settings;
            Loc = loc;
            Input = input;
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Events.Clear();
                Input?.DisableAll();
                Instance = null;
            }
        }
    }

    public interface ISceneLoadService
    {
        bool IsLoaded(SceneId id);
        Awaitable LoadAdditive(SceneId id);
        Awaitable Unload(SceneId id);
        Awaitable SwapSet(SceneId[] load, SceneId[] unload);
    }

    public interface IGameFlow
    {
        FlowState State { get; }
        GameMode Mode { get; }
        Awaitable Goto(FlowState next);
        Awaitable EnterGameplay(GameMode mode);
    }

    public interface ISettingsService
    {
        GameSettings Current { get; }
        void Load();
        void ApplyAndSave();
        void ResetToDefaults();
    }

    public interface ILocalizationService
    {
        string CurrentCode { get; }
        string Get(string key);
        void SetLocale(string code);
    }

    public interface IInputService
    {
        void EnableGameplay();
        void EnableUi();
        void DisableAll();
    }
}
