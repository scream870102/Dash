# Tech Stack

- Unity **2020.2.6f1** (`ProjectSettings/ProjectVersion.txt`) — C#, .NET/Mono per Unity 2020.2
- Render: URP 10.3.1 + Post Processing 2.3.0
- Camera: Cinemachine 2.5.0
- Input: **new Input System 1.0.2** (mouse support removed per recent commit; keyboard + gamepad via generated `PlayerControl` + `Eccentric.Input.GamepadController`)
- UI: uGUI + TextMeshPro 3.0.1
- 2D: sprite/tilemap modules, Physics2D + custom raycast controller for player
- Services: Firebase + Parse SDKs vendored under `Assets/Firebase`, `Assets/Parse`, `Assets/PlayServicesResolver` (Android target implied)
- Test framework package present (1.1.22) but no test assemblies observed
- No external package manager beyond Unity Package Manager (`Packages/manifest.json`)
