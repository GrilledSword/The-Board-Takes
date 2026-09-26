using BoardTakes.Core;
using UnityEngine;

namespace BoardTakes.Boot
{
    public sealed class GameFlowController : IGameFlow
    {
        readonly ISceneLoadService _scenes;
        readonly GameEventBus _events;

        public FlowState State { get; private set; } = FlowState.Booting;
        public GameMode Mode { get; private set; } = GameMode.None;

        public GameFlowController(ISceneLoadService scenes, GameEventBus events)
        {
            _scenes = scenes;
            _events = events;
        }

        public async Awaitable Goto(FlowState next)
        {
            if (next == State) return;
            var prev = State;
            await Transition(prev, next);
            State = next;
            _events.Publish(new FlowStateChanged(prev, next));
            BoardLog.Info($"Flow {prev} → {next}");
        }

        public async Awaitable EnterGameplay(GameMode mode)
        {
            Mode = mode;
            await Goto(FlowState.Gameplay);
        }

        async Awaitable Transition(FlowState from, FlowState to)
        {
            switch (to)
            {
                case FlowState.Splash:
                    await _scenes.SwapSet(
                        new[] { SceneId.Splash },
                        MenuGuestsExcept(SceneId.Splash));
                    break;

                case FlowState.Intro:
                    await _scenes.SwapSet(
                        new[] { SceneId.Intro },
                        MenuGuestsExcept(SceneId.Intro));
                    break;

                case FlowState.MainMenu:
                    await _scenes.SwapSet(
                        new[] { SceneId.MainMenu },
                        Combine(
                            MenuGuestsExcept(SceneId.MainMenu),
                            PlayGuests()));
                    break;

                case FlowState.Settings:
                    await _scenes.LoadAdditive(SceneId.Settings);
                    break;

                case FlowState.Lobby:
                    await _scenes.SwapSet(
                        new[] { SceneId.MultiplayerLobby },
                        Combine(MenuGuestsExcept(SceneId.None), PlayGuests()));
                    break;

                case FlowState.Gameplay:
                    await _scenes.SwapSet(
                        new[]
                        {
                            SceneId.GameplayCore,
                            SceneId.MapLightingPersistent,
                            SceneId.MapGeometry
                        },
                        Combine(MenuGuestsExcept(SceneId.None), new[] { SceneId.MultiplayerLobby }));
                    break;

                case FlowState.Ending:
                    await _scenes.SwapSet(
                        new[] { SceneId.EndingCutscene },
                        PlayGuests());
                    break;

                case FlowState.Credits:
                    await _scenes.SwapSet(
                        new[] { SceneId.Credits },
                        new[] { SceneId.EndingCutscene });
                    break;
            }

            // Leaving settings pops only the overlay.
            if (from == FlowState.Settings && to != FlowState.Settings)
                await _scenes.Unload(SceneId.Settings);
        }

        static SceneId[] MenuGuestsExcept(SceneId keep)
        {
            var all = new[]
            {
                SceneId.Splash,
                SceneId.Intro,
                SceneId.MainMenu,
                SceneId.Settings
            };
            return Filter(all, keep);
        }

        static SceneId[] PlayGuests() => new[]
        {
            SceneId.GameplayCore,
            SceneId.MapGeometry,
            SceneId.MapLightingPersistent,
            SceneId.EndingCutscene,
            SceneId.Credits
        };

        static SceneId[] Filter(SceneId[] source, SceneId keep)
        {
            var count = 0;
            for (var i = 0; i < source.Length; i++)
                if (source[i] != keep) count++;

            var result = new SceneId[count];
            var w = 0;
            for (var i = 0; i < source.Length; i++)
                if (source[i] != keep) result[w++] = source[i];
            return result;
        }

        static SceneId[] Combine(SceneId[] a, SceneId[] b)
        {
            var result = new SceneId[a.Length + b.Length];
            a.CopyTo(result, 0);
            b.CopyTo(result, a.Length);
            return result;
        }
    }
}
