using BoardTakes.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BoardTakes.Input
{
    /// <summary>
    /// Wraps the project-wide InputSystem_Actions asset.
    /// Maps are enabled explicitly — Input System 1.14+ no longer auto-enables them.
    /// </summary>
    public sealed class InputService : IInputService
    {
        const string PlayerMap = "Player";
        const string UiMap = "UI";

        InputActionAsset _asset;
        bool _bound;

        public void EnableGameplay()
        {
            EnsureBound();
            SetMap(PlayerMap, true);
            SetMap(UiMap, true);
        }

        public void EnableUi()
        {
            EnsureBound();
            SetMap(PlayerMap, false);
            SetMap(UiMap, true);
        }

        public void DisableAll()
        {
            if (_asset == null) return;
            _asset.Disable();
        }

        void EnsureBound()
        {
            if (_bound && _asset != null) return;

            _asset = InputSystem.actions;
            if (_asset == null)
            {
                BoardLog.Warn("InputSystem.actions is null. Assign Assets/InputSystem_Actions.inputactions as Project-wide Actions.");
                return;
            }

            _asset.Enable();
            _bound = true;
        }

        void SetMap(string name, bool on)
        {
            if (_asset == null) return;
            var map = _asset.FindActionMap(name, throwIfNotFound: false);
            if (map == null)
            {
                BoardLog.Warn($"Action map '{name}' missing from InputSystem_Actions.");
                return;
            }

            if (on) map.Enable();
            else map.Disable();
        }
    }
}
