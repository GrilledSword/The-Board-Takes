#if UNITY_EDITOR || DEVELOPMENT_BUILD
using BoardTakes.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BoardTakes.Boot
{
    /// <summary>
    /// Keyboard only, editor/dev builds. Not a UI generator.
    /// F1 menu, F2 settings, F3 solo table, F4 lobby, F5 hu/en toggle.
    /// </summary>
    public sealed class EditorDebugFlow : MonoBehaviour
    {
        void Update()
        {
            if (!GameKernel.Exists) return;
            var kbd = Keyboard.current;
            if (kbd == null) return;

            if (kbd.f1Key.wasPressedThisFrame)
                _ = GameKernel.Instance.Flow.Goto(FlowState.MainMenu);
            if (kbd.f2Key.wasPressedThisFrame)
                _ = GameKernel.Instance.Flow.Goto(FlowState.Settings);
            if (kbd.f3Key.wasPressedThisFrame)
                _ = GameKernel.Instance.Flow.EnterGameplay(GameMode.SoloDuel);
            if (kbd.f4Key.wasPressedThisFrame)
                _ = GameKernel.Instance.Flow.Goto(FlowState.Lobby);
            if (kbd.f5Key.wasPressedThisFrame)
            {
                var next = GameKernel.Instance.Loc.CurrentCode == "hu" ? "en" : "hu";
                GameKernel.Instance.Settings.Current.LocaleCode = next;
                GameKernel.Instance.Loc.SetLocale(next);
                BoardLog.Info("Locale → " + next + " / " + GameKernel.Instance.Loc.Get("game.subtitle"));
            }
        }
    }
}
#endif
