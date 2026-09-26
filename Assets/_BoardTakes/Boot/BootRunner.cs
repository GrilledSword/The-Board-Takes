using BoardTakes.Core;
using BoardTakes.Input;
using BoardTakes.Localization;
using BoardTakes.Settings;
using UnityEngine;

namespace BoardTakes.Boot
{
    /// <summary>
    /// Drop this on an empty GameObject in Boot.unity.
    /// The scaffold creates that object for you.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public sealed class BootRunner : MonoBehaviour
    {
        [SerializeField] float _splashSeconds = 1.6f;
        [SerializeField] bool _skipIntroInEditor = true;

        async void Start()
        {
            var kernel = GetComponent<GameKernel>() ?? gameObject.AddComponent<GameKernel>();
            // PATCH slice-01: empty scaffold scenes have no camera. Boot keeps a fallback eye.
            if (GetComponent<PersistentViewRig>() == null)
                gameObject.AddComponent<PersistentViewRig>();

            var scenes = new SceneLoadService(SceneId.Boot);
            var settings = new SettingsService();
            settings.Load();

            var loc = new LocalizationService();
            loc.SetLocale(string.IsNullOrEmpty(settings.Current.LocaleCode)
                ? "en"
                : settings.Current.LocaleCode);

            var input = new InputService();
            input.EnableUi();

            var flow = new GameFlowController(scenes, kernel.Events);
            kernel.Bind(scenes, flow, settings, loc, input);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (GetComponent<EditorDebugFlow>() == null)
                gameObject.AddComponent<EditorDebugFlow>();
#endif
            settings.ApplyAndSave();
            BoardLog.Info("Kernel up. The table is set. Nobody has paid yet.");

            await flow.Goto(FlowState.Splash);
            if (_splashSeconds > 0f)
                await Awaitable.WaitForSecondsAsync(_splashSeconds);

#if UNITY_EDITOR
            if (_skipIntroInEditor)
            {
                await flow.Goto(FlowState.MainMenu);
                return;
            }
#endif
            await flow.Goto(FlowState.Intro);
            await flow.Goto(FlowState.MainMenu);
        }
    }
}