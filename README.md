# Insane Monsters in Classic

A MelonLoader mod for Drova - Forsaken Kin. It brings the extra monster spawns and the expanded movesets of Insane mode into the other game modes while keeping their original balance. Your savegame stays on Classic (or Immersive, Hardcore, Story), so monster health, attributes, poise and player damage modifiers remain untouched.

## What it does

- Insane spawn groups: bigger and additional monster waves, insane-only placed encounters and insane-only sub scenes
- Insane movesets and skills: alternate combat AI graphs, insane-only abilities and attack patterns, more aggressive group behavior
- Insane attack timings: faster attack phases where the game authors them, applied and reverted live when you flip the switch

Each feature has its own switch in the in-game options menu under the Modding tab. Spawn and moveset changes apply after loading a savegame, attack timings apply immediately.

## What it deliberately does not do

Insane equipment presets stay off. Insane weapon variants carry higher damage values, which would break the balance this mod wants to preserve. The mod also disables itself automatically when the loaded savegame already runs in an Insane mode.

## Co-op

In a co-op session the host's switches count on every machine, since different switches make different monsters. A client already in a world reloads its save once to pick up the host's spawn and moveset switches. A client's own switches come back when the session ends.

## Requirements

- [MelonLoader](https://melonwiki.xyz) 0.7 or newer
- [Drova Modding API](https://github.com/Drova-Modding) 0.6.0 or newer

## Installation

Drop `InsaneMonstersInClassicDrova.dll` into the `Mods` folder of your Drova installation. Make sure `Drova_Modding_API.dll` is present there as well.

## Building

Set your game path in `InsaneMonstersInClassicDrova/Directory.Build.props`, then run `dotnet build`. The build copies the dll into the game's `Mods` folder automatically and packages a release zip into `dist`. The zip contains the dll inside a `Mods` folder, so users can extract it directly into the game root.

## How it works

Everything Insane changes is authored in separate serialized fields on the game's components, gated by an insane-mode check. That includes stats, for example health has an extra `_maxHealthInsane` field next to `_maxHealth`, and the same pattern exists for attributes and poise. This mod copies only the spawn, moveset and timing fields over their normal counterparts at component initialization, so the game follows its regular non-insane code path with insane content. The insane stat fields are left alone, and since the savegame is not in Insane mode the game never reads them.
