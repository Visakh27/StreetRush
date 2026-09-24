# Street Rush — Unity Starter

Stylized anime cel-shaded arcade racing vertical slice gaming for Android.

## Target
- Android mobile
- Landscape orientation
- 30 FPS minimum target, 60 FPS on capable devices
- TPP chase camera
- Touch steering + optional tilt
- Arcade acceleration, drift and nitro
- Checkpoints, laps and race position

## Suggested scene
Create a scene named `NeonDowntown` containing:
- Player car with Rigidbody + `ArcadeCarController`
- `RaceManager`
- `RaceCheckpoint` objects in track order
- `RaceCamera`
- Canvas with steering buttons and Nitro button

## Player car setup
Rigidbody:
- Mass: 1200
- Drag: 0.05
- Angular Drag: 3
- Interpolate: Interpolate
- Collision Detection: Continuous Dynamic

Add `ArcadeCarController` and assign the Rigidbody automatically.

For the first prototype, the car uses Rigidbody forces rather than WheelColliders. This keeps the handling intentionally arcade-like and easy to tune.

## Mobile
Use `MobileInput` as the single input source. UI buttons call:
- `PressLeft()` / `ReleaseLeft()`
- `PressRight()` / `ReleaseRight()`
- `PressBrake()` / `ReleaseBrake()`
- `PressNitro()` / `ReleaseNitro()`

Keyboard is supported in-editor with A/D, Space and Left Shift.

## Build order
1. Greybox Neon Downtown.
2. Tune Bolt handling.
3. Add 3 AI cars using `AICarController`.
4. Add checkpoints/laps.
5. Add HUD.
6. Add stylized materials, rain, neon and VFX.
