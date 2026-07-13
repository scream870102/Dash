# Core

Unity 2D platformer game "Dash" (player dash/movement mechanics). Single Unity project, no sub-modules.

## Source map (code lives under `Assets/`)

- `Assets/Script/Player/` — player logic: `Player`, `Movement`, `Dash`, `RaycastController` (custom raycast-based physics), `FX`, `PlayerComponent` (base class for player parts)
- `Assets/Script/Controller/` — game-wide controllers: `GameManager`, `GameController`, `StageController`, `UIController`, `CameraController`, `SLController` (save/load), `StoryController`, `TitleController`, `FXController`, `GoalTrigger`
- `Assets/Script/MapObject/` — stage interactables, all derive from `AMapObject`: `Platform`, `Spring`, `Launcher`, `BreakableObj`, `PushableObj`, `UnstableObj`, `OneTouchObj`, area triggers
- `Assets/Script/Eccentric/` — in-house utility library: `DomainEvents` (event bus), `ObjectPool`/`IObjectPoolAble`, `TSingletonMonoBehavior`, `Timer`, `GamepadController`, `ParallaxBG`, `ReadOnlyAttribute`
- `Assets/Input/PlayerControl.cs` — generated Input System actions (regenerate via .inputactions asset, don't hand-edit)
- `Assets/Editor/` — editor scripts
- Non-code: `Assets/Prefab`, `Assets/Scenes`, `Assets/Sprite`, `Assets/Animation`, `Assets/Audio`, `Assets/Firebase`, `Assets/Parse`

Repo rules: `AGENTS.md` at root is source of truth (plan export to `plan/`, token discipline, Obsidian vault unset — ask user).

Tech/build: `mem:tech_stack`. Commands: `mem:suggested_commands`. Code style: `mem:conventions`. Done-checklist: `mem:task_completion`.
