# Conventions

- Root namespace `CJStudio.Dash`, sub-namespace per folder: `CJStudio.Dash.Player`, `CJStudio.Dash.MapObject`; utility lib under `Eccentric` / `Eccentric.*` (own namespace, no CJStudio prefix)
- One class per file, file name = class name; abstract base classes prefixed `A` (`AMapObject`); generic singleton `TSingletonMonoBehavior`
- Events: domain-event pattern via `Eccentric.DomainEvents` — event args are small classes named `On<Something>` (`OnPlayerDead`, `OnAiming`) declared in same file as owner class
- Player behavior split into components deriving from `PlayerComponent`, composed by `Player`
- Interfaces `I` prefix (`IObjectPoolAble`); pooling via `Eccentric.Collections.ObjectPool`
- Custom platformer physics: `RaycastController` raycast-based, not pure Rigidbody2D — respect this when touching movement code
- `Assets/Input/PlayerControl.cs` is auto-generated — never hand-edit; edit the `.inputactions` asset in Editor instead
- Every asset/folder has a `.meta` file — when creating/deleting/moving assets outside Editor, keep `.meta` files consistent (prefer doing asset ops inside Editor)
