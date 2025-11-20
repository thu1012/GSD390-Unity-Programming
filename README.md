# Make a Small Game (Part 1)

### A win condition and lose condition. The game should indicate whether you have won or lost the game. I recommend using a TextMeshPro - Text component to display a message.

The game displays **"You collected all the spheres!"** when all collectibles are collected.

If the player falls below the fall threshold, the game shows **"You fell off the level!"**.

### A way of restarting the game after losing. The simplest way of doing this is restarting when the player presses the "R" key.

Press **R** at any time after win/lose to restart the scene.

### At least one custom component that reads input and uses Update() to change over time.

The **PlayerController** script reads keyboard + mouse input and updates the player's position and camera angle.

### At least one GameObject that is saved as a prefab. If you missed the lecture, here is a link to the Unity DocsLinks to an external site. that explains how to use prefabs.

The collectible spheres are created from a reusable **CollectibleItem** prefab.

---

![Gameplay Demo](SmallGame1.gif)

---