# Slice 01 — Boot, scenes, settings

## Delivered

- `GameKernel` lives in Boot. Boot never unloads.
- Additive load / unload by `SceneId`, not raw strings.
- Flow: Boot → Splash → Intro → MainMenu. From menu: Settings overlay, Solo table, Lobby.
- Settings: full AAA *data contract* + PlayerPrefs JSON + applicators. No sliders drawn for you.
- Localization: `en` / `hu` JSON.
- Input: project-wide `InputSystem_Actions` (Player + UI maps enabled on purpose).
- Editor menu creates empty scenes and patches Build Settings.

## Not in this slice

Chess rules, gore VFX, NGO, voice chat devices, generated canvases.

## Scene list the scaffold creates

Persistent: `Boot`
Menu stack: `Splash`, `Intro`, `MainMenu`, `Settings`
Multi: `Multiplayer_Lobby`
Gameplay: `Gameplay_Core`, `Map_Geometry`, `Map_Lighting_Persistent`
Campaign extra: `Ending_Cutscene`, `Credits`
Legacy template `Assets/Scenes/SampleScene.unity` stays until you delete it.

## How to hook your UI later

```
GameKernel.Events.Subscribe<FlowStateChanged>(OnFlow);
GameKernel.Settings.Current.Video.FovDegrees = 70;
GameKernel.Settings.ApplyAndSave();
GameKernel.Loc.Get("ui.menu.play_solo");
```

Settings UI should only mutate `GameSettings` and call `ApplyAndSave()`. Never touch `Screen` / URP from a button script.
