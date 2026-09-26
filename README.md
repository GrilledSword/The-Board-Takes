# The Board Takes

Horror chess. The stake is not points. The stake is flesh.

Someone sits you at a table. There is no why at the start — only a rule: you play until the board takes what it is owed. Singleplayer is one long night in a rotting room. Multiplayer is the same table with more debtors. The board does not choose. All of you pay.

This is **not** chess-flavoured cards. It is real chess (check, castle, en passant, promotion) with a body ledger instead of an HP bar.

- Studio: OverBitCore
- Engine: Unity **6000.6.3f1** / URP 17.6 / Input System 1.20
- Platforms: Windows, Linux, Android — cross-play planned (Netcode slice)
- Repo: https://github.com/GrilledSword/The-Board-Takes

## What this repo is (and is not)

Code and contracts first. **No generated UI canvases, no generated meshes, no generated animations.** You build those. The systems expose events and data so a hand-made menu / table / pawn can plug in without rewriting the core.

## Slice map

| Slice | Scope |
| --- | --- |
| **01** | Boot kernel, additive scene flow, settings *data* + applicators, loc, input wrapper |
| 02 | Main menu / settings screen *wiring* (you author the UI) |
| 03 | Rules-legal chess engine + clocks |
| 04 | Body ledger + capture ritual (the board drinks) |
| 05 | Grandfather, piece voices, check-as-warning |
| 06 | Singleplayer room between games (drawers, tools that cost flesh) |
| 07 | Multiplayer lobby, NGO, cross-play toggle |
| 08 | Mixer, spatial audio, voice chat hooks |
| 09 | Accessibility + QoL pass |
| 10 | Content pipeline, patch versioning, shipping toggles |

## Open in Unity

1. Unity Hub → Add → this folder.
2. Editor **6000.6.3f1**.
3. Menu: `The Board Takes / Slice 01 / Create Empty Scenes`.
4. Play from `Assets/_BoardTakes/Scenes/Boot/Boot.unity`.

First scene in Build Settings must be Boot. The scaffold writes that for you.

## Folder law

```
Assets/_BoardTakes/
  Boot/           additive loader + flow
  Core/           kernel, events, contracts
  Settings/       AAA settings model + apply
  Input/          InputSystem_Actions wrapper
  Localization/   JSON tables, no extra package
  Scenes/         empty scene slots (created by scaffold)
  Editor/         one-shot scene scaffold only
```

Do not dump scripts into `Assets/` root. Do not use `FindObjectOfType` — Unity 6 wants `FindFirstObjectByType` / `FindAnyObjectByType` / `FindObjectsByType(..., FindObjectsSortMode.None)`.

## Languages

`en`, `hu` ship in slice 01. Add a JSON next to them and register the code in `LocalizationService`.
