# Open the new Elijah Blaze Unity project

This folder is now the canonical Unity project for the game.

## Add it to Unity Hub
1. Pull the latest `main` branch in GitHub Desktop.
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
- Attack: left mouse or Space

## First loop
Enter the court → survive three Shardlings → defeat the encounter → the Resonance Gate rises.

Do not manually create a second Unity project. This folder is the game project from now on.
