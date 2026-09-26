# Architecture

Boot scene is the operating system. Everything else is a guest.

```
Boot (persistent)
  GameKernel
    SceneLoadService
    GameFlowController
    SettingsService
    LocalizationService
    InputService
    GameEventBus
```

## Why additive

Splash art, the table geometry, and the lighting rig have different lifetimes. Unloading `Map_Geometry` must not kill the kernel or the body-ledger state that slice 04 will park on the kernel.

Same pattern as a resident-evil room load, except the "room" is a chess table and the streaming budget is your remaining fingers.

## Events, not manager soup

`GameEventBus` is in-process, main-thread, no allocations on the hot path beyond the multicast. Chess captures in slice 04 publish `PieceCaptured`. UI listens. The board-drink animation listens. Nothing finds each other by name.

## Settings vs graphics

`GameSettings` is the serialized truth. `GraphicsApplicator` / `AudioApplicator` are the only types allowed to touch URP / AudioMixer / Screen. That is how a patch can add XeSS without rewriting the menu.

## Input

Project-wide asset: `Assets/InputSystem_Actions.inputactions`.
Slice 01 enables `Player` and `UI`. Slice 02 will add a `Board` map (cursor on squares, confirm, undo) — do it in the asset, not in generated C#.

## Multiplayer later

NGO + Unity Services live behind `INetworkGate` (slice 07). Slice 01 only stores `CrossPlayEnabled` and `Region`.
