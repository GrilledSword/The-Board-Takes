using System.Collections.Generic;
using System.IO;
using BoardTakes.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BoardTakes.EditorTools
{
    public static class Slice01SceneScaffold
    {
        const string BootObjectName = "GameKernel";

        [MenuItem("The Board Takes/Slice 01/Create Empty Scenes")]
        public static void CreateEmptyScenes()
        {
            var created = 0;
            foreach (var pair in SceneCatalog.All)
            {
                var path = pair.Value;
                var dir = Path.GetDirectoryName(path)?.Replace('\\', '/');
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(path))
                    continue;

                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                scene.name = SceneCatalog.FileNameOf(pair.Key);

                if (pair.Key == SceneId.Boot)
                    SpawnBootRig();

                EditorSceneManager.SaveScene(scene, path);
                created++;
            }

            PatchBuildSettings();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog(
                "The Board Takes",
                created == 0
                    ? "Scenes already existed. Build Settings refreshed."
                    : $"Created {created} empty scene(s). Boot is first in Build Settings.\n\nYou dress the rooms. The kernel only keeps the lights on.",
                "OK");
        }

        [MenuItem("The Board Takes/Slice 01/Patch Build Settings")]
        public static void PatchBuildSettingsMenu()
        {
            PatchBuildSettings();
            EditorUtility.DisplayDialog("The Board Takes", "Build Settings patched. Boot first.", "OK");
        }

        static void SpawnBootRig()
        {
            var go = new GameObject(BootObjectName);
            go.AddComponent<BoardTakes.Core.GameKernel>();
            // BootRunner lives in BoardTakes.Boot. Resolved via TypeCache so Editor does not reference Boot.
            var runnerType = FindBootRunnerType();
            if (runnerType != null)
                go.AddComponent(runnerType);
            else
                Debug.LogError("[BoardTakes] BoardTakes.Boot.BootRunner not compiled yet. Reimport, then run the scaffold again.");
        }

        // PATCH slice-01: TypeCache instead of AppDomain.GetAssemblies (UAC0005).
        static System.Type FindBootRunnerType()
        {
            var types = TypeCache.GetTypesDerivedFrom<MonoBehaviour>();
            for (var i = 0; i < types.Count; i++)
            {
                var t = types[i];
                if (t != null && t.FullName == "BoardTakes.Boot.BootRunner")
                    return t;
            }
            return null;
        }

        static void PatchBuildSettings()
        {
            var list = new List<EditorBuildSettingsScene>();
            var seen = new HashSet<string>();

            void Add(SceneId id, bool enabled)
            {
                var path = SceneCatalog.PathOf(id);
                if (!File.Exists(path)) return;
                if (!seen.Add(path)) return;
                list.Add(new EditorBuildSettingsScene(path, enabled));
            }

            Add(SceneId.Boot, true);
            Add(SceneId.Splash, true);
            Add(SceneId.Intro, true);
            Add(SceneId.MainMenu, true);
            Add(SceneId.Settings, true);
            Add(SceneId.MultiplayerLobby, true);
            Add(SceneId.GameplayCore, true);
            Add(SceneId.MapGeometry, true);
            Add(SceneId.MapLightingPersistent, true);
            Add(SceneId.EndingCutscene, true);
            Add(SceneId.Credits, true);

            foreach (var existing in EditorBuildSettings.scenes)
            {
                if (existing == null || string.IsNullOrEmpty(existing.path)) continue;
                if (seen.Contains(existing.path)) continue;
                list.Add(existing);
            }

            EditorBuildSettings.scenes = list.ToArray();
        }
    }
}