# Codex Implementation Queue — Hollow Court

Work in this order. Keep each task small enough to test in Unity before advancing.

## HC-001 — Stabilize Health and Death Feedback
**Goal:** Elijah and Shardlings have unmistakable death states without compiler errors.

Acceptance criteria:
- `Health` cannot die twice.
- Damage after death is ignored.
- Death emits one debug event.
- If `DeathFeedback` exists it is invoked; otherwise the object is safely destroyed.
- Enemy colliders/attack/chase behavior stop during death feedback.
- Player death does not silently delete the player; it raises a player-death event or calls a restart flow.
- Existing working movement/combat behavior is not broken.

## HC-002 — Hit Feedback
Add a short readable reaction when damage lands. Keep it modular so animation/VFX can replace prototype feedback later.

Acceptance criteria:
- Attacker cannot register uncontrolled repeated hits from one attack window.
- Victim visibly reacts.
- Feedback does not alter permanent scale/material values incorrectly.

## HC-003 — Hollow Court Encounter Controller
Create a controller that knows when the required Shardlings in the court have been defeated.

Acceptance criteria:
- Encounter begins from an explicit trigger or scene start configuration.
- Tracks living encounter enemies.
- Fires `OnEncounterCleared` once.
- Does not search the entire scene every frame.

## HC-004 — Resonance Gate
Create a dormant architectural gate that listens for the encounter-cleared event.

Acceptance criteria:
- Gate has Dormant, Awakening, and Open states.
- Prototype animation may use transform movement/light intensity.
- Gate cannot repeatedly reopen.
- Opening is event-driven, not coupled directly to a specific enemy class.

## HC-005 — Prototype Game Loop
Connect arrival → architectural signal → encounter start → encounter clear → threshold reached.

Acceptance criteria:
- The player discovers the central resonance seal before the Shardlings awaken.
- Shardlings wake in sequence to prevent an unreadable opening rush.
- A HUD communicates the current objective, encounter count, and controls without relying on the Console.
- Attacks, stuns, enemy windups, blocks, deaths, and completion have visible feedback.
- The clear state directs Elijah to the far threshold; the existing gate remains a placeholder until its dedicated build-out.
- The loop restarts cleanly after player death.

## Codex guardrails
- Read the repository `AGENTS.md` and `Game/GAME_BIBLE.md` first.
- Prefer complete, compilable C# files over fragmentary edits.
- One responsibility per MonoBehaviour.
- Do not introduce packages without explicit approval.
- Do not redesign the Elijah Blaze avatar or lore while solving programming tasks.
- Do not modify working systems unrelated to the current task unless required to compile.
