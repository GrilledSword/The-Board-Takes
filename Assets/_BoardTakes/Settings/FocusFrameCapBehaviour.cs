using UnityEngine;

namespace BoardTakes.Settings
{
    /// <summary>
    /// Background FPS cap. Lives on the kernel object once settings apply.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class FocusFrameCapBehaviour : MonoBehaviour
    {
        public static int FocusedLimit = 0;
        public static int UnfocusedLimit = 30;

        public static void Ensure()
        {
            if (!Core.GameKernel.Exists) return;
            var host = Core.GameKernel.Instance.gameObject;
            if (host.GetComponent<FocusFrameCapBehaviour>() == null)
                host.AddComponent<FocusFrameCapBehaviour>();
        }

        void OnApplicationFocus(bool hasFocus)
        {
            Apply(hasFocus);
        }

        void OnApplicationPause(bool paused)
        {
            Apply(!paused);
        }

        static void Apply(bool focused)
        {
            if (focused)
                Application.targetFrameRate = FocusedLimit <= 0 ? -1 : FocusedLimit;
            else
                Application.targetFrameRate = UnfocusedLimit <= 0 ? 30 : UnfocusedLimit;
        }
    }
}
