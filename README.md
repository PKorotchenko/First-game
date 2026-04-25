# Star Hopper: Jump Through the Stars

A minimalist 2D mobile arcade game built for Unity with Flappy Bird style tap controls, adaptive screen support, procedural obstacle generation, and progression systems.

## What is included

- `Assets/Scripts/`:
  - `GameManager.cs`
  - `PlayerController.cs`
  - `ObstacleSpawner.cs`
  - `ObstacleBase.cs`
  - `AsteroidObstacle.cs`
  - `StationObstacle.cs`
  - `BlackHoleObstacle.cs`
  - `MeteoriteObstacle.cs`
  - `HighScoreManager.cs`
  - `AchievementManager.cs`
  - `DailyChallengeManager.cs`
  - `UIManager.cs`

## Core gameplay systems

- Tap to make the starship rise and let gravity fall.
- Four obstacle mechanics:
  - Asteroids: static upper/lower obstacles
  - Stations: vertical passages with moving gaps
  - Black holes: attraction zones that pull the ship
  - Meteorites: temporary dense zones that increase fall force
- Procedural level generation with difficulty scaling.
- Score based on distance traveled.
- Local high score leaderboard and simulated global leaderboard.
- Daily challenges with special objectives.
- Achievement notifications.

## Unity setup guide

1. Open Unity and create a new 2D project.
2. Copy the `Assets/Scripts/` folder into your Unity project.
3. Create a new scene named `MainScene`.
4. Create a `GameManager` GameObject and attach `GameManager`, `HighScoreManager`, `AchievementManager`, `DailyChallengeManager`, and `UIManager`.
5. Create a `Player` GameObject with a `SpriteRenderer`, `Rigidbody2D`, `CircleCollider2D`, and `PlayerController`.
   - Set `Rigidbody2D` gravity scale to around `1.2`.
   - Tag the player object as `Player`.
6. Create an `ObstacleSpawner` GameObject and attach `ObstacleSpawner`.
   - Assign each obstacle prefab in the correct order: Asteroid, Station, BlackHole, Meteorite.
7. Create obstacle prefabs for each type using colliders and attach the corresponding scripts.
   - Asteroid/Station: `BoxCollider2D` and `ObstacleBase` variants
   - BlackHole: `CircleCollider2D` set as trigger and `BlackHoleObstacle`
   - Meteorite: `BoxCollider2D` set as trigger and `MeteoriteObstacle`
   - Set all obstacle prefab tags to `Obstacle`
8. Create a Canvas UI with `Text` objects for score, final score, leaderboards, challenge text, and notifications.
   - Use `Canvas Scaler` with `UI Scale Mode` set to `Scale With Screen Size`.
   - Reference a baseline resolution such as `1080 x 1920`.
9. Configure the camera and background art as minimal shapes or gradients.
10. Make sure the player and obstacles use `Sorting Layers` or `Order in Layer` for clarity.

## Mobile optimization tips

- Use `Canvas Scaler` for adaptive 16:9, 18:9, 19.5:9 screens.
- Keep art minimal and use solid shapes.
- Target 60 FPS by disabling unnecessary particle effects and limiting physics objects.
- Use lightweight colliders and smaller update loops.

## Notes

- `HighScoreManager` includes a local persistent leaderboard and a simulated global leaderboard.
- `DailyChallengeManager` creates a new challenge each day and saves the current challenge.
- The system is ready for iOS 13+ and Android 8.0+ once the project is built from Unity.

## Next steps

- Add art assets, animations, and particle effects.
- Create obstacle prefabs and assign them in the scene.
- Hook UI buttons to `UIManager.OnStartButton()` and `UIManager.OnRestartButton()`.
- Test on device resolutions to confirm adaptive layout.
