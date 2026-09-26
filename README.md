# THE BOARD TAKES
### A tábla elveszi

Psychological horror chess. Real rules. The stake is flesh.

Unity **6000.6.3f1** (6.3 LTS) · URP 17.6 · Input System 1.20  
Studio: OverBitCore · Repo: [GrilledSword/The-Board-Takes](https://github.com/GrilledSword/The-Board-Takes)

Someone sits you at a table. There is no why. You play until the board is paid.

- **Singleplayer** — one long night in a rotting room. Inscryption cabin + CloverPit cell + Lucas Baker's table.
- **Multiplayer (2–4)** — same table, same debt. Last standing. The dead stay seated.

This is **not** chess-flavoured cards. Castling, en passant, promotion, check, mate, stalemate all work. Horror comes from the rules being honest.

---

## Slice map

| Slice | Scope |
| --- | --- |
| **01 / 10** | Additive scene architecture, bootstrap, loading overlay, locale, AAA settings model + runtime menu |
| 02 | Chess rules engine + board presentation |
| 03 | Body Ledger + capture punishments |
| 04 | The Host (Nagyater), piece voices, check/mate presentation |
| 05 | Singleplayer room / tools / campaign loop |
| 06 | Multiplayer table, timing controls, voice chat hooks |
| 07 | Art / URP volume / table atmosphere pass |
| 08 | Accessibility + settings UI polish + input rebind |
| 09 | Content, endings, credits, audio bed |
| 10 | Netcode hardening, patch pipeline, ship checklist |

---

## Open in Unity (slice 01)

1. Clone the repo. Open with **Unity 6.3 LTS (6000.6.3f1)**.
2. Menu: **The Board Takes → Slice 01 → Scaffold Scenes + Build Settings**.
3. Press Play on `Assets/TheBoardTakes/Scenes/Persistent/Bootstrap.unity` (the scaffold sets it as scene 0).
4. Flow: Splash → Intro → Main Menu. **Settings** is a full AAA panel generated at runtime.

You do **not** hand-place thirteen scenes. The editor scaffold creates empty, correctly tagged shells and registers them in Build Settings.

---

## Scene architecture

Only `Bootstrap` is a single-load entry. Everything else is **additive**.

```
Bootstrap                    // index 0, dies after Persistent is up
Persistent_App               // EventBus, SceneDirector, Settings, Locale, AudioRouter
LoadingOverlay               // stays loaded, shown/hidden
SettingsOverlay              // stays loaded, shown/hidden

SplashScreen
IntroScreen
MainMenu

Multiplayer_Lobby
Gameplay_Core                // rules + pieces + HUD
Map_Geometry                 // table, room mesh
Map_Lighting_Persistent      // candles, volumes, baked probes later

Ending_Cutscene
Credits
```

Recipes live in `Assets/TheBoardTakes/Data/Scenes/`. Change a recipe, not the loader.

---

## Code layout

```
Assets/TheBoardTakes/
  Runtime/           asmdef TheBoardTakes.Runtime
  Editor/            asmdef TheBoardTakes.Editor
  Data/
  Localization/
  Scenes/
```

Patch rule: public contracts live in `TheBoardTakes.Core.*` interfaces. Implementations can be swapped without rewriting callers.

---

## Languages (slice 01)

`en`, `hu` JSON tables under `Assets/TheBoardTakes/Localization/`.  
`ILocaleService` is the seam — Unity Localization package can replace the JSON backend later without touching UI.

---

## License / content warning

Work-in-progress horror. Body horror, amputation as game rules, implied execution. No minors. No animals as players.
