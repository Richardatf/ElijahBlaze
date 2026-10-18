# Open the new Elijah Blaze Unity project

This folder is now the canonical Unity project for the game.

## Add it to Unity Hub
1. In GitHub Desktop, pull and check out the latest `codex/hc-001-health-death-fallback` branch.
2. Open Unity Hub.
3. Click **Add** / **Add project from disk**.
4. Select this folder: `ElijahBlaze/Game/Unity`.
5. Open it with Unity 6. If Hub asks to use a newer installed Unity 6 editor, allow the upgrade and commit the resulting `ProjectVersion.txt` change afterward.

## Build the first playable Hollow Court
After Unity finishes importing and there are no compile errors:
1. In Unity's top menu choose **Elijah Blaze > Build Hollow Court Prototype**.
2. The builder creates and saves `Assets/Scenes/HollowCourt.unity`.
3. Press Play.

## Prototype controls
- Move: WASD / arrow keys
- Vitality: shown in the upper-right of the objective panel
- Attack: hold left mouse or Space to swing repeatedly; Elijah's blue slash appears, and each hit stuns the Shardling, marked by a cyan ring. Three hits defeat it
- Guard: hold Left Shift, Right Shift, or right mouse button to block the next incoming Shardling hit from any side; a short recovery follows each block
- Enemy attack tell: a red line marks a Shardling winding up; the line flashes gold when it strikes

## First loop
Enter the court → follow the luminous floor seal → meet the Shardlings as they wake in sequence → defeat all three → cross to the far threshold → reach the Resonance Gate to complete the Hollow Court slice.

The gate remains a simple placeholder in this build; its fuller visual and interaction design comes later.

Do not manually create a second Unity project. This folder is the game project from now on.
