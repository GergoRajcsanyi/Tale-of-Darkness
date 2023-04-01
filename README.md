# Tale of Darkness (Final Year Project)
![CoverArt](https://user-images.githubusercontent.com/129614966/229308733-11a418cc-b536-4874-970f-005ca89c028c.jpg)

Tale of Darkness is an adventure puzzle-platform game in which you play as Sol, a boy who is unable to wake up from a dream that has been overshadowed by a mysterious solar eclipse. Your goal is to help him through this oppressive and unknown realm, called ”Dreamlands” while solving various jumping puzzles and gaining 4 new abilities. The adventure spans the plains of a ruined dream, a long-forgotten cave and a dark forest to finally end at the edge of a by then disintegrated nightmare, where you must defeat an otherworldly entity that shouldn't even exist.

# Project features
* 2D Platformer Character Controller
* **Player Ability System**
  * Core ability: *Shadow Platforms* (allows the player to draw their objects on the screen)
  * Eye of Wisdom: a flashlight to navigate inside dark places
  * Corruption: transforms the player into a dark orb (allows levitation and the use of portals)
  * Insight: the only way to banish the Darkness
* **Save-Load System** for persisting progress, such as world state and unlocked character abilities
* Heavy use of Events and delegates to drive UI and gameplay reactions
* **Multi-Phase Boss Fight**
  * BossFightManager to manage the boss logic, phases and attacks
  * BossHealth to configure the length of the fight
  * BossTrigger to close and re-open the arena
  * Attacks, such as Meteors and Projectiles based on object pool design pattern
* UI elements for player interactions, navigation and pop-ups for unlocked abilities and secrets
* **Menu System** to start/continue the game and adjust settings
* Dynamic 2D camera with boundaries and parallax effect
* Reusable and interactable objects, traps for various levels across the entire game
* AudioManager for sound control and optimization
* **Sprite Sheets** and custom animations for every element of the game

# Play
[Download for devices with Android 6.0 and above](https://play.google.com/store/apps/details?id=com.UniversityofPannonia.TaleofDarkness)
