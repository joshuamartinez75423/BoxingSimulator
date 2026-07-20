# VR Punch Golf Simulator

A virtual reality game where you play golf by **punching the ball** instead of swinging a club. Put on a VR headset, throw a real physical punch at the ball floating in front of you, and watch it fly down a golf course toward the hole — complete with fairway/rough friction, a magnetic "sink" effect on the cup, distance tracking, and golf-style scoring (birdie, eagle, bogey, etc.).

This project was built as a hands-on exploration of VR game development: hand tracking, real-world physics behavior, and turning a simple idea ("what if golf, but you punch it") into a playable prototype.

---

## Table of Contents

- [What This Project Demonstrates](#what-this-project-demonstrates)
- [Tech Stack](#tech-stack)
- [How the Game Works](#how-the-game-works)
- [Technical Deep Dive](#technical-deep-dive)
  - [VR Movement & Locomotion](#vr-movement--locomotion)
  - [The Punching Mechanic](#the-punching-mechanic)
  - [Ball Physics & Surface Friction](#ball-physics--surface-friction)
  - [Hole Detection & the "Sink" Effect](#hole-detection--the-sink-effect)
  - [Scoring & UI](#scoring--ui)
- [Project Structure](#project-structure)
- [Software Design Notes](#software-design-notes)
- [Project Status](#project-status)

---

## What This Project Demonstrates

This isn't just "drag a 3D model into a scene" — it required solving real gameplay-engineering problems:

- **Translating a real-world punch into believable ball flight.** A VR controller gives you raw position/velocity data; the code has to interpret an imprecise human hand motion and turn it into a golf shot that feels fair and fun, not random.
- **Simulating different ground surfaces** (fairway vs. rough vs. sand) so the ball behaves differently depending on where it lands, the same way real golf terrain affects a real ball.
- **Multi-headset VR support.** The game isn't locked to one brand of headset — it works across Vive, Oculus/Meta Quest (via Link), Valve Index, and Windows Mixed Reality, because it's built on an input system that abstracts controller differences away.
- **Full gameplay loop:** movement, physical interaction, physics, win detection, and a scoring UI — the pieces that turn a tech demo into an actual game.

---

## Tech Stack

| Layer | Technology |
|---|---|
| **Game Engine** | Unity 2022.3 (LTS) |
| **Language** | C# |
| **VR Runtime** | SteamVR (Valve's OpenVR), supporting Vive, Oculus/Quest via Link, Valve Index, and Windows Mixed Reality headsets |
| **Physics** | Unity's built-in physics engine (PhysX), extended with custom scripts for surface friction and shot-direction correction |
| **UI Text** | TextMesh Pro |
| **3D Modeling** | Autodesk Maya (course terrain, golf gloves, flagpole — modeled externally, then imported into Unity) |
| **Course Art** | Custom-modeled fairway/rough/terrain, plus a free third-party tree asset pack for course scenery |

**Why these choices:** Unity is the industry-standard engine for indie and mid-size game development, with the largest ecosystem for VR tooling. SteamVR was chosen over a single-vendor SDK (like Meta's) specifically so the game isn't locked to one headset brand — it works on essentially any PC VR headset through Valve's runtime.

---

## How the Game Works

1. **Put on the headset and walk the course.** Player movement uses the joystick, oriented relative to where you're actually looking (not just the controller), with a sprint option for covering ground faster.
2. **Walk up to the ball and punch it.** Each hand has a boxing-glove-shaped hitbox. Throwing a real punch at the ball is detected by tracking hand velocity — a slow-moving hand doesn't count as a punch, so the game only reacts to genuine hits, filtering out accidental brushes.
3. **The ball reacts realistically.** How hard and in what direction you punched determines how far and where the ball flies. Landing in short grass (fairway) lets it roll further; landing in tall grass (rough) slows it down hard, just like real golf.
4. **Get it in the hole.** Near the cup, the ball is gently pulled in (so you're not fighting frustrating near-misses), then plays a satisfying shrink-and-sink animation.
5. **See your score.** A UI panel pops up showing your stroke count translated into real golf terminology — Eagle, Birdie, Par, Bogey, etc., relative to the hole's par value — plus how far your shots traveled in yards.

---

## Technical Deep Dive

### VR Movement & Locomotion

`PlayerMovement.cs` reads joystick input through SteamVR's action system and moves the player using Unity's `CharacterController` (a specialized component for player movement that handles collision without the jitter you'd get from full physics simulation — the standard approach for VR locomotion). Movement direction is rotated to match the direction the player's headset is facing, so "push forward" always means "walk toward where you're looking," not an arbitrary world direction. Gravity is applied manually so the player falls naturally off ledges or slopes. A sprint toggle swaps between a walk speed and a faster sprint speed on the fly.

### The Punching Mechanic

This is the core gameplay hook, and the most custom engineering in the project (`PunchDetector.cs`).

Each glove tracks its own real-world velocity every frame. A punch is only registered if the hand is moving fast enough at the moment of contact — this is what separates "I bumped the ball" from "I punched the ball." When contact is detected, the code:

1. Reads the peak speed the hand reached during the swing.
2. Computes a direction vector from the hand to the ball at the moment of impact.
3. **Reshapes that direction based on how vertical or horizontal the punch was.** A pure human punch is rarely a perfectly clean, game-ready launch angle — swings that are too steep, too flat, or glancing get corrected toward a direction that produces a satisfying shot. This is a hand-tuned heuristic (three angle "bands" checked via vector math), essentially a small piece of game-feel design encoded directly in physics math.
4. Applies the resulting force to the ball's physics body and triggers controller haptic feedback (a vibration pulse), so the player feels the hit.

### Ball Physics & Surface Friction

The ball uses Unity's standard rigid-body physics for gravity, bouncing, and rolling — but surface behavior is layered on top (`EffectGrass.cs`). As the ball rolls, the code checks what physical material it's currently touching (fairway, rough, or sand, each a distinct asset with its own friction profile) and adjusts the ball's drag in real time:

- **Fairway:** low friction — the ball rolls further, with a small extra speed decay the longer it rolls, so long fairway rolls still eventually settle.
- **Rough:** high friction — the ball stops quickly, punishing an errant shot the same way real long grass does.

When the ball is airborne, its original physics properties are restored so surface friction only applies while it's actually touching the ground. There's also automatic long-roll dampening so a ball that's been rolling for a while (e.g., stuck oscillating at low speed) is forced to settle rather than rolling forever.

### Hole Detection & the "Sink" Effect

Real golf holes are unforgiving about precision; a satisfying VR game shouldn't be. When the ball gets close to the cup, a gentle "magnetic" pull (an inverse-distance force with speed damping, so it doesn't just fling the ball around in circles) draws it toward the center — this is a common game-feel technique that rewards a good shot without requiring pixel-perfect real-world aim. Once the ball is over the hole, a short animation smoothly sinks and shrinks the ball into the cup, the flag lowers and fades out, and the win condition fires: stroke count is locked in and handed to the scoring UI.

### Scoring & UI

The scoring panel translates a raw stroke count into real golf terminology relative to the hole's par (Albatross, Eagle, Birdie, Par, Bogey, Double Bogey, etc.) — the same scoring language real golfers use. A separate yardage tracker measures how far each shot traveled by watching the ball's position from the moment it's hit until it comes to rest, then converts the distance into yards for an on-screen display. Menu navigation and buttons are handled through a VR "laser pointer" — a virtual laser beam from the controller that lets you point-and-click UI buttons the same way you'd use a real laser pointer, since there's no mouse in VR.

---

## Project Structure

```
Assets/
├── Scripts/            All gameplay code (C#) — movement, punching, ball physics, UI, scoring
├── Scenes/             Playable/test scenes
├── Prefabs/            Reusable game objects (boxing gloves, golf hole)
├── Models/              Imported 3D models (gloves)
├── Maps/                Golf course geometry (fairway, rough, terrain, flagpole), modeled in Maya
├── Materials/           Visual materials (grass, flag, gloves, etc.)
├── PhysicsMaterials/    Friction profiles for fairway / rough / sand
├── SteamVR/             VR input & runtime plugin (third-party, by Valve)
└── TextMesh Pro/        UI text rendering plugin

BoxGolfModels/           Source Maya files for the course and models (art pipeline, pre-Unity)
Packages/                Unity package dependencies
```

---

## Software Design Notes

A few patterns worth calling out for anyone reviewing the code:

- **Global event for cross-UI communication:** ball-stop detection and the yardage display are decoupled using a shared C# event, so the UI doesn't need a direct reference to the ball — any listener can subscribe independently. A lightweight version of the publish/subscribe pattern common in larger game architectures.
- **Component-based composition:** each gameplay concern (punch detection, ball state, surface friction, hole attraction, scoring) lives in its own small, focused script attached to the relevant object, rather than one large monolithic controller — Unity's idiomatic style, and good practice for keeping gameplay systems independently testable and swappable.
- **Coroutine-driven animation:** effects like the ball sinking into the hole, the flag lowering, and UI fade-ins are hand-written smoothed animations (eased over time) rather than instant snaps, which is what makes those moments feel polished instead of mechanical.

---

## Project Status

This is an actively-developed **prototype**, built iteratively over about two weeks by two collaborators. Core mechanics are in place and playable in the Unity Editor: movement, punching, ball physics, surface friction, hole detection, win condition, and scoring UI. It has not yet been packaged into a standalone installable build — the current stage of development is proving out and refining the gameplay feel before packaging it for distribution.

**Recent work** has focused on: grass/terrain friction behavior, flagpole animation, and a full "win the hole" scoring screen — the last remaining pieces to make a full hole playable start-to-finish.