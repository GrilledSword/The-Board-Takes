using BoardTakes.Core;
using UnityEngine;

namespace BoardTakes.Boot
{
    /// <summary>
    /// Bind these from *your* buttons. No canvas ships with slice 01.
    /// </summary>
    public static class MenuHooks
    {
        public static void PlaySolo()
        {
            if (!GameKernel.Exists) return;
            GameKernel.Instance.Input.EnableGameplay();
            _ = GameKernel.Instance.Flow.EnterGameplay(GameMode.SoloDuel);
        }

        public static void PlayTable()
        {
            if (!GameKernel.Exists) return;
            GameKernel.Instance.Input.EnableGameplay();
            _ = GameKernel.Instance.Flow.EnterGameplay(GameMode.TableTwoToFour);
        }

        public static void OpenLobby()
        {
            if (!GameKernel.Exists) return;
            GameKernel.Instance.Input.EnableUi();
            _ = GameKernel.Instance.Flow.Goto(FlowState.Lobby);
        }

        public static void OpenSettings()
        {
            if (!GameKernel.Exists) return;
            GameKernel.Instance.Input.EnableUi();
            _ = GameKernel.Instance.Flow.Goto(FlowState.Settings);
        }

        public static void BackToMenu()
        {
            if (!GameKernel.Exists) return;
            GameKernel.Instance.Input.EnableUi();
            _ = GameKernel.Instance.Flow.Goto(FlowState.MainMenu);
        }

        public static void OpenCredits()
        {
            if (!GameKernel.Exists) return;
            _ = GameKernel.Instance.Flow.Goto(FlowState.Credits);
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
