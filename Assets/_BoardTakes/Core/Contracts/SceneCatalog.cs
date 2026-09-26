using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardTakes.Core
{
    /// <summary>
    /// Single map of SceneId → editor path. Keep paths in one place so a rename
    /// is a one-line patch, not a graveyard of string literals.
    /// </summary>
    public static class SceneCatalog
    {
        public const string Root = "Assets/_BoardTakes/Scenes";

        static readonly Dictionary<SceneId, string> Paths = new()
        {
            { SceneId.Boot,                   Root + "/Boot/Boot.unity" },
            { SceneId.Splash,                 Root + "/Menu/Splash.unity" },
            { SceneId.Intro,                  Root + "/Menu/Intro.unity" },
            { SceneId.MainMenu,               Root + "/Menu/MainMenu.unity" },
            { SceneId.Settings,               Root + "/Menu/Settings.unity" },
            { SceneId.MultiplayerLobby,       Root + "/Multiplayer/Multiplayer_Lobby.unity" },
            { SceneId.GameplayCore,           Root + "/Gameplay/Gameplay_Core.unity" },
            { SceneId.MapGeometry,            Root + "/Gameplay/Map_Geometry.unity" },
            { SceneId.MapLightingPersistent,  Root + "/Gameplay/Map_Lighting_Persistent.unity" },
            { SceneId.EndingCutscene,         Root + "/Campaign/Ending_Cutscene.unity" },
            { SceneId.Credits,                Root + "/Campaign/Credits.unity" }
        };

        public static string PathOf(SceneId id)
        {
            if (id == SceneId.None) throw new ArgumentOutOfRangeException(nameof(id));
            if (!Paths.TryGetValue(id, out var path))
                throw new InvalidOperationException($"No path registered for {id}.");
            return path;
        }

        public static string FileNameOf(SceneId id)
        {
            var path = PathOf(id);
            var slash = path.LastIndexOf('/');
            var file = slash >= 0 ? path[(slash + 1)..] : path;
            return file.EndsWith(".unity", StringComparison.OrdinalIgnoreCase)
                ? file[..^6]
                : file;
        }

        public static IReadOnlyDictionary<SceneId, string> All => Paths;
    }
}
