# Elijah Blaze — Unity Game

## Current playable milestone: Hollow Court

The game is a third-person action-adventure prototype built around discovery, moral intention, and the gradual synchronization of Elijah Blue with AZEL.

### Core design rule
The player must feel that they are uncovering a truth that has patiently waited thousands of years to be remembered.

### Prototype status
- Player movement and chase camera: implemented
- Awakening seal and staged Shardling encounter: implemented, pending Unity play test
- Visible attack, stun, enemy windup, and guard feedback: implemented, pending Unity play test
- Objective and encounter progress HUD: implemented, pending Unity play test
- Encounter-clear and threshold-completion flow: implemented, pending Unity play test
- Resonance Gate: simple placeholder response only; full gate design and interaction remain future work

### Immediate vertical-slice loop
1. Enter Hollow Court and follow the luminous floor seal.
2. Approach the center seal; the dormant Shardlings awaken one at a time.
3. Read each red attack tell, stun the Shardlings, and guard their strikes.
4. Defeat all three; the HUD reports the encounter clear.
5. Cross to the far threshold while the placeholder Resonance Gate rises.
6. Reach the threshold to complete the Hollow Court slice.

### Development order
Mechanics first. Feedback second. Art third. Do not replace prototype geometry merely to make the scene prettier before the loop is fun.

See `GAME_BIBLE.md` and `CODEX_TASKS.md` before implementing new systems.
