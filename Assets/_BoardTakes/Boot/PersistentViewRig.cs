using BoardTakes.Core;
using UnityEngine;

namespace BoardTakes.Boot
{
    /// <summary>
    /// Fallback camera + listener on Boot. Guest scenes may spawn their own
    /// cameras later; this rig yields so you do not get a stacked dual-view.
    /// Not UI. Not a generated menu. Just an eye so Display 1 is not a coffin lid.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-900)]
    public sealed class PersistentViewRig : MonoBehaviour
    {
        Camera _cam;
        AudioListener _listener;

        void Awake()
        {
            EnsureRig();
        }

        void LateUpdate()
        {
            EnsureRig();
            var guestHasCamera = GuestCameraExists();
            if (_cam != null) _cam.enabled = !guestHasCamera;
            if (_listener != null) _listener.enabled = !guestHasCamera;

            if (_cam != null && _cam.enabled && GameKernel.Exists)
                _cam.fieldOfView = Mathf.Clamp(GameKernel.Instance.Settings.Current.Video.FovDegrees, 40f, 110f);
        }

        void EnsureRig()
        {
            if (_cam != null) return;

            var existing = GetComponentInChildren<Camera>(true);
            if (existing != null)
            {
                _cam = existing;
                _listener = existing.GetComponent<AudioListener>() ?? existing.gameObject.AddComponent<AudioListener>();
                ApplyLook(_cam);
                return;
            }

            var go = new GameObject("PersistentCamera");
            go.transform.SetParent(transform, false);
            go.transform.position = new Vector3(0f, 1.4f, -2.4f);
            go.transform.rotation = Quaternion.Euler(12f, 0f, 0f);
            _cam = go.AddComponent<Camera>();
            _listener = go.AddComponent<AudioListener>();
            ApplyLook(_cam);
            BoardLog.Info("Persistent camera seated. The room is still empty. That is on you.");
        }

        static void ApplyLook(Camera cam)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.02f, 0.015f, 0.015f, 1f);
            cam.nearClipPlane = 0.05f;
            cam.farClipPlane = 80f;
            cam.depth = -100f;
            cam.tag = "MainCamera";
        }

        bool GuestCameraExists()
        {
            var cameras = Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (var i = 0; i < cameras.Length; i++)
            {
                var c = cameras[i];
                if (c == null || c == _cam) continue;
                if (!c.isActiveAndEnabled) continue;
                return true;
            }
            return false;
        }
    }
}