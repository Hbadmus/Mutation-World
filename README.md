# Mutation World

[![Build Status](https://img.shields.io/badge/build-link-blue)](https://play.unity.com/en/games/b2855764-2eda-48a4-9e04-fbde50358e63/mutation-world-prototype) 

# Design Documentation

**Tech:** Unity, C#  
**Role:** Character Design, Boss Encounter Design, Playtesting and Iteration  
**Type:** Collaborative Game Development Project

---

## Overview

Mutation World is a first-person shooter set in a post-apocalyptic Earth overrun by mutations. Players choose between two survivors, Jack and Fiona, each with distinct combat styles, navigating enemy-filled environments toward a final boss encounter. My responsibility covered the character ability systems, boss encounter ruleset, and iterating on both through playtesting.

---

## Character Design

The core design challenge was making two characters feel meaningfully different without one being strictly better than the other. The goal was to create genuine playstyle choice, not just a cosmetic one.

**Jack** was designed for players who want to stay in the fight aggressively. His kit rewards positioning in the middle of encounters: a passive damage resistance, a rapid-fire ability that punishes grouped enemies, and an ultimate that chains damage across targets. The design intent was that Jack players should feel comfortable standing their ground.

**Fiona** was designed as the counterplay option. Her increased movement speed as a passive changes how she interacts with the environment entirely, rewarding players who kite and reposition rather than tank hits. Her abilities scale with distance and line-of-sight, so good Fiona play looks completely different from good Jack play.

The constraint I kept coming back to: every ability had to have a readable downside, otherwise there was no decision-making. Cooldowns and positioning requirements were the main tools used to create that tension.

---

## Boss Encounter Design

The Zombie Boss was designed to be a moment of escalation, not just a stat check. Key design decisions:

**The Car Throw ability** was the centerpiece. A projectile attack with a wind-up animation gives players a readable tell to dodge, rewarding attentiveness over raw damage output. The intent was that a skilled player should be able to avoid most of the damage entirely.

**High health, low speed** was an intentional tradeoff. A slow boss that hits extremely hard creates a different kind of pressure than a fast one. Players have to respect the damage output while managing the zombies that continue spawning throughout the encounter, preventing them from just kiting indefinitely.

The encounter was tuned so that both Jack and Fiona felt viable, just through different approaches: Jack absorbing hits and dealing sustained damage up close, Fiona maintaining distance and using her arc abilities to clear adds while chipping the boss.

---

## Playtesting and Iteration

Several design issues surfaced through playtesting that changed the final ruleset:

- **Boss felt unkillable early on.** Initial health values were too high relative to player damage output, making the encounter feel like a grind rather than a climax. Tuned health down and increased ability damage to keep the pacing tight.
- **Fiona felt underpowered in close quarters.** Early playtesters defaulted to Jack because enclosed spaces neutralized Fiona's range advantage. Added wider corridors in the boss arena to give her kit room to breathe.
- **Ability cooldowns were too punishing.** First iteration had long cooldowns that made abilities feel unreliable. Shortened them based on feedback that players wanted to feel powerful, not constrained.

Every change came from watching real players, not assumptions about how they would play.All models, textures, and assets are sourced from the Asset Store for showcasing coding and game development skills. Original ownership and rights belong to the respective creators.
