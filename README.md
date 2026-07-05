# Bedroom Adventure (Unity Project)

This is a 3D game made with Unity. In the game, you play a small character in a big bedroom. You explore the room, look at objects, and try to survive with your energy (sanity) bar.

## About the Game

The game starts with a main menu. When you press the **Play** button, the camera moves in a smooth arc, the screen fades to black, and the game scene loads.

In the game, you can change between two forms of the character:

- **Mini character**: small and uses less energy.
- **Big character**: strong but uses more energy.

Press the **F** key to switch between the two forms. Switching costs a little energy, and you must wait a short time before you can switch again.

Your energy goes down over time. When the energy bar gets low, the screen slowly becomes dark. If your energy reaches zero, the game is over and the "Game Over" scene loads.

## Controls

| Key / Input | Action |
|---|---|
| Mouse | Look around |
| W, A, S, D | Move |
| F | Switch character (mini / big) |

## Main Scripts

The main game code is in `Assets/a Scripts` and `Assets/Scripts`:

- `SanityManager.cs` – controls the energy bar, the dark screen effect, character switching, and the game-over state.
- `MouseLook.cs` – lets the player look around with the mouse.
- `CameraArcMovement.cs` – moves the camera at the start of the game and changes the scene with a fade effect.
- `MenuManager.cs` – controls the Play and Quit buttons in the menu.
- `GroundDetector.cs` – checks if the player is on the ground.
- `PlayerSoundManager.cs`, `MusicManager.cs`, `SoundManager.cs` – play sounds and music.
- `BookInteraction.cs` – lets the player interact with books.
- `CharacterSwitch.cs` – helps with changing the character form.

## Scenes

Important scenes are in `Assets/Scenes`:

- `BedRoom.unity` – the main game scene (the bedroom).
- `SampleScene.unity` – a test scene.
- `mekanik.unity` – a scene for testing game mechanics.

## How to Open the Project

1. Install **Unity 6000.4.3f1** (Unity 6) or a newer version.
2. Open **Unity Hub** and click **Add**.
3. Choose this project folder.
4. Open the project and wait for Unity to import the files.
5. Open the scene `Assets/Scenes/BedRoom.unity` and press **Play**.

## Assets

The project uses free low-poly asset packs, for example: furniture models, kids' toys, a playground set, and character models. These assets are in the `Assets` folder.
