<div align="center">

<img src="docs/blasteroids-title.jpg" alt="Blasteroids" width="720">

**An arcade space shooter**

Blast your way through an enemy-swarmed asteroid field and see how many points you can rack up.

[![Play on itch.io](https://img.shields.io/badge/Play%20on-itch.io-FA5C5C?style=for-the-badge&logo=itchdotio&logoColor=white)](https://redrookinteractive.itch.io/blasteroids)

![Unity](https://img.shields.io/badge/Unity-2021.3.16f1-000000?logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![Platforms](https://img.shields.io/badge/Platforms-WebGL%20%7C%20Windows%20%7C%20macOS-blue)
![Status](https://img.shields.io/badge/Status-Alpha%20%E2%80%94%20on%20hold-orange)

<!-- Add a gameplay GIF here: ![Blasteroids gameplay](docs/blasteroids-gameplay.gif) -->

</div>

---

> [!NOTE]
> **Unfinished.** Blasteroids is a playable alpha, and development has been on hold since December 2023, with plans to pick it back up. What's below is what works today.

## ✨ Features

- **Three weapons:** rapid-fire lasers, homing missiles that fan out and chase the nearest enemy, and a screen-clearing nuke
- **Limited ammo** with a 30-round visual ammo counter
- **Power-ups:** speed boost, triple shot, shields, health, ammo, and nukes
- **Negative power-ups:** a speed-down debuff you'll want to dodge
- **Tractor beam** that pulls nearby power-ups toward you
- **Smarter enemies** that dodge incoming fire, ram you when you're beside them, or chase you down
- **Smart spawning** that reads the game state, so you get ammo when you're empty and never a speed boost you can't use
- **Camera shake** and HUD messages for feedback

## 🎮 Controls

| Action | Input |
|---|---|
| Move | **WASD** / **Arrow keys** |
| Speed boost | **Left Shift** |
| Fire lasers | **Left click** |
| Homing missiles | **Right click** |
| Nuke | **Space** |
| Tractor beam | **C** (hold) |
| Pause | **Esc** |
| Restart / Quit (game over) | **R** / **Q** |

## 👥 Credits

| Name | Role |
|---|---|
| **Clint Wagoner** | Game design, programming, sound & music |

A **Red Rook Interactive** game. Built as a [GameDevHQ](https://gamedevhq.com) boot camp project and expanded well past the course.

3D ship models, textures, and some starter sprites are third-party assets from the Unity Asset Store and GameDevHQ.

## 🛠️ Tech

- **Engine:** Unity 2021.3.16f1 (LTS), C#
- **Audio:** Ableton Live
- **Art tools:** Aseprite, Photoshop
- **Builds:** WebGL, Windows, macOS

## 🧱 How It's Built

All gameplay code lives in `Assets/Scripts/`: 19 hand-written C# files, about 1,900 lines.

| Area | Scripts | What they do |
|---|---|---|
| **Player** | `Player` `Shooting` | Movement, speed boost, shields, health, damage, tractor beam, lasers, triple shot, ammo, missiles, nuke |
| **Enemies** | `Enemy` `Asteroid` `Swarmer` `HomingTrigger` | Dodging, ramming, homing pursuit, asteroid behavior |
| **Projectiles** | `Laser` `HomingMissile` `Nuke` `DestroyProjectileContainer` | Shots, nearest-target homing, the nuke's expanding blast |
| **Collectibles** | `PowerUp` | Seven power-up types, dispatched by ID |
| **Managers** | `GameManager` `SpawnManager` | Game flow, pause, restart, waves, state-aware power-up spawning |
| **Camera** | `CameraShake` `FollowTarget` | Hit feedback and tracking |
| **UI** | `UIManager` `MainMenu` `GameOverAnimation` | HUD, ammo display, messages, menus |
| **Audio** | `AudioManager` | Sound effects and music |

## 📝 Devlog

Every feature past the boot camp is written up step by step on [Medium](https://medium.com/@cwagoner78):

| Date | Article |
|---|---|
| Apr 3, 2023 | [Nuke Bomb Secondary Attack](https://medium.com/@cwagoner78/polishing-the-game-nuke-bomb-secondary-attack-9a39d7ca9a4f) |
| Apr 6, 2023 | [Adding Enemy Collision Detection](https://medium.com/@cwagoner78/more-polish-adding-enemy-collision-detection-more-movement-ceb6728498a9) |
| Apr 8, 2023 | [Visual Feedback for the Ammo System](https://medium.com/@cwagoner78/more-polish-creating-visual-feedback-for-the-ammo-system-39224e155051) |
| Apr 10, 2023 | [Balancing the Spawn Managers](https://medium.com/@cwagoner78/more-polish-balancing-the-spawn-managers-13e79256c1ff) |
| Apr 17, 2023 | [Smarter Enemies](https://medium.com/@cwagoner78/more-polish-enemies-dodge-lasers-2ce430b31e55) |
| Apr 19, 2023 | [Creating a Tractor Beam](https://medium.com/@cwagoner78/more-polish-creating-a-tractor-beam-f03a901478b0) |
| Apr 21, 2023 | [Negative Power-Ups and HUD Text Feedback](https://medium.com/@cwagoner78/more-polish-negative-power-ups-and-hud-text-feedback-e32ea00eae13) |
| Apr 25, 2023 | [Aggressive Enemies](https://medium.com/@cwagoner78/more-polish-aggressive-enemies-9e878e014cb5) |
| May 6, 2023 | [New Enemy Type](https://medium.com/@cwagoner78/more-polish-new-enemy-type-2bb739f445e0) |
| May 17, 2023 | [Homing Missiles](https://medium.com/@cwagoner78/more-polish-homing-missiles-1fdd7f55052d) |

## ⚠️ Known Limitations

- The Z-axis clamp on enemies doesn't work, and `Enemy.cs` says so in a comment.
- In `Player.cs`, the null check after finding the thruster tests `_speedBoostStream` instead of `_thruster`, so a missing thruster would go unreported.
- Systems locate each other with `GameObject.Find` and `FindObjectOfType` (26 calls across four scripts) rather than serialized references.

## ▶️ Running the Project

1. Install **Unity 2021.3.16f1** through Unity Hub.
2. Clone this repo and open the folder in Unity Hub.
3. Open **`Title`** from `Assets/Scenes` and press **Play**.

## 📜 Version History

Developed from March to December 2023 and on hold since, with more planned. The full commit history is here, so you can follow each feature as it was added.

---

<div align="center">

© 2023 **Red Rook Interactive**. All rights reserved.

</div>
