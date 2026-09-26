using System.Collections.Generic;
using BoardTakes.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardTakes.Boot
{
    public sealed class SceneLoadService : ISceneLoadService
    {
        readonly HashSet<SceneId> _loaded = new();

        public SceneLoadService(SceneId alreadyLoaded)
        {
            _loaded.Add(alreadyLoaded);
        }

        public bool IsLoaded(SceneId id) => _loaded.Contains(id);

        public async Awaitable LoadAdditive(SceneId id)
        {
            if (id == SceneId.None || id == SceneId.Boot) return;
            if (_loaded.Contains(id)) return;

            var name = SceneCatalog.FileNameOf(id);
            if (!Application.CanStreamedLevelBeLoaded(name))
            {
                BoardLog.Warn($"Scene '{name}' is not in Build Settings yet. Scaffold it: The Board Takes / Slice 01 / Create Empty Scenes.");
                _loaded.Add(id);
                return;
            }

            var op = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            if (op == null)
            {
                BoardLog.Error($"LoadSceneAsync failed for {name}.");
                return;
            }

            while (!op.isDone)
                await Awaitable.NextFrameAsync();

            _loaded.Add(id);
            BoardLog.Info($"Loaded additive {id}.");
        }

        public async Awaitable Unload(SceneId id)
        {
            if (id == SceneId.None || id == SceneId.Boot) return;
            if (!_loaded.Contains(id)) return;

            var name = SceneCatalog.FileNameOf(id);
            var scene = SceneManager.GetSceneByName(name);
            if (!scene.IsValid() || !scene.isLoaded)
            {
                _loaded.Remove(id);
                return;
            }

            var op = SceneManager.UnloadSceneAsync(scene);
            if (op == null)
            {
                _loaded.Remove(id);
                return;
            }

            while (!op.isDone)
                await Awaitable.NextFrameAsync();

            _loaded.Remove(id);
            BoardLog.Info($"Unloaded {id}.");
        }

        public async Awaitable SwapSet(SceneId[] load, SceneId[] unload)
        {
            if (unload != null)
            {
                for (var i = 0; i < unload.Length; i++)
                    await Unload(unload[i]);
            }

            if (load != null)
            {
                for (var i = 0; i < load.Length; i++)
                    await LoadAdditive(load[i]);
            }
        }
    }
}
