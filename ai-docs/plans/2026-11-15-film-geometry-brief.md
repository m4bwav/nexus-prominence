# Film geometry and arrivals: the brief

The fixed brief for [the film geometry continue prompt](2026-11-15-film-geometry-continue-prompt.md). State lives in [HANDOFF.md](../HANDOFF.md), not here.

## The ask

The owner, 2026-11-15, with five screenshots of the film:

> One thing I've noticed is that there are ramps and steps and higher areas and pillars not really shown on the levels. Can we do a closer inspection of the screenshots and really try to get them close to the real movie.

The same day the owner passed on a player's report about the reinforcements and asked that this chain fix whatever it describes. As in the ten rounds, the map work is also how madnesswright and the plugins beside it get better.

## The player's report

Quoted as written, with the author's name left out:

> I see reinforcements have been added. And... They're bugged. Don't even know if i really see all of them. I just encountered them after coming back to past rooms after finishing the last one. But they doesn't appear when they should. Don't know if their spawn happens when some time pass or the player needs to walk over some part of the map to activate a trigger, but you can complete the level without seeing reinforcements.

Take it as a play report: backup met only on the way back through finished rooms, never during the fights, and the level can be finished without seeing any. Nobody replies to the player from this chain; any reply is the owner's.

## Reinforcements: where to start looking

Leads from the generator and from the game code read in the ten rounds. None has been watched in play; check each before building on it.

- Only rooms 2, 4, 7 and 10 have a backup squad (`FilmDoorWaves`), each one small squad through the room's spare doors (`DoorWaves`, `Squad`).
- Room 2's squad waits for the alarm (`SpawnRequirement MyFactionAlerted`) and then its timer. The squads in rooms 4, 7 and 10 only have a timer (`SecondsInRoomTilSpawn`, about 20 s) that starts when the room becomes the current one.
- The game lets a squad in only while the room's living enemies plus the next squad fit under the wave's `MaxUnitCount` (`Wave_Tandem.CanSpawnWithoutBreachingMax`, read in round 1). With the caps `FilmTotals` sets, the cargo bay's squad cannot enter until most of the placed enemies are dead.
- So a player who shoots fast can empty a room before the timer and the cap line up, and walk on. If the timer only runs while the room is current, a squad that never fired is still armed when the player walks back, which is what the report describes.
- Nothing in the generator ties a room's way on to its backup having arrived.

Not known: what the game does with an unspawned squad when its room is left, whether a room counts as cleared, and what the report's player saw room by room.

Fixed means, on a straight run with no waiting and no walking back: rooms 2, 4, 7 and 10 each show their backup walking in while the fight is still on, inside the film count; walking back into a finished room meets nobody new; rooms where the film shows no arrivals still get none; and the README and Workshop text say what a player will see.

## The five screenshots: a first reading

The files are in the lab's `frames/` folder. This is one reading by the session that wrote the brief, done from the stills alone. The room each still belongs to is a guess to check against the film at those times.

| File | Film time | Likely room | Structures to check |
|---|---|---|---|
| `owner-2026-11-15-0849-pillar-and-sloped-blocks.png` | 8:49 | 5 Sniper loft | A square pillar from floor to ceiling on a red-banded plinth, standing clear of the back wall. Low barricades with a sloped face and a red top edge. On the right, tall sloped blocks at two or more heights with red edges, and a red-striped run that reads as steps or a ramp up to a higher floor. A glass case with the emblem set into the back wall. The wall line steps forward at the left. |
| `owner-2026-11-15-garage-pylons-generator.png` | not shown | 6 Shutter garage | Two tapered pylons of different heights, one behind the other. A generator with a fuel can against the back wall, a hose coil on a wall rail, a wall pilaster beside it. A long case on the floor. A line across the floor behind the pylons and a red-edged diagonal at the left that read as a change of floor level. |
| `owner-2026-11-15-garage-wide-floor-edges.png` | not shown | 6 Shutter garage | The same room wider: the back strip of floor sits behind an edge line, a red-edged diagonal runs to the lower left (a ramp side or a kerb), a large rectangle is outlined on the near floor (a raised slab or a marked bay), a sloped barrier with a red top stands at the lower left, a cable reel at the left. |
| `owner-2026-11-15-admin-room.png` | not shown | 2 Wanted hall or 3 Controls office | Back wall with recessed panels and a diagonal-cut trim. Two screens hung from ceiling mounts. A tall red light panel. A doorway in the right side wall under a sign. A hooded console at the left, a low bench on a rectangle outlined on the floor, a tower unit, bins. The red band here is a low skirting, not waist high, and a red strip runs along the ceiling. Both side walls are in view. |
| `owner-2026-11-15-sniper-balcony-ladder.png` | not shown | 5 Sniper loft | A thick balcony slab standing out of the wall at about head height with a chamfered underside, a ladder at its right end, a doorway with a lit window behind the sniper, and the red band wrapping round a wall block that juts into the room. |

What the map has now, by a first look at the generator: every room's floor is the sample room's single flat slab; the garage has tapered pylons (`Pylon`) and the loft a ledge with a ladder (`SniperLoft`); nothing is named as a ramp, steps, a raised floor, a pillar or a balcony. Confirm it from the newest renders before planning.

## What to look for in every room

Fixed structure, as against props and dressing: pillars and columns; pilasters and wall blocks that jut into the room; recesses and cases set into a wall; ramps; steps; raised decks and the edges of a floor at another height; balconies and ledges; ladders; sloped barricades and low walls; doorways in the side walls; and what hangs overhead (ceiling strips, hung screens, pipes). For each one the film shows: where it stands, how tall it is in character heights, and what it does in the fight (cover, a route, a perch, or only a backdrop).

## Staying true to the film

The film is the fan collab NEXUS: HANK RAID, https://www.youtube.com/watch?v=v0suvMY3GgM. The rule from [the ten rounds brief](2026-11-14-ten-rounds-brief.md), "Staying true to the video", still holds: the route, what each room is and holds, the gray-and-red look, who the enemies are and how they arrive. It does not mean copying the film's drawings. Enemy counts stay inside the film table in HANDOFF.md. The film cuts between short fights drawn by different animators, so one README room may be several film sets; where they disagree, pick the set the owner's screenshots show, then the one on screen longest, and write down which.

## The played feel

Camera, zoom and room sizes were tuned over many plays, and one attempt to change several at once was reverted. This chain may change what stands in a room, because that is the ask, but a structure changes cover, sight lines and routes, so each room's structures sit behind their own toggle and go on the owner's test list. Camera, zoom and room sizes change only if a structure cannot be seen or walked without it, behind a toggle that is off by default.

## Which plugin helps where

The table in [the ten rounds brief](2026-11-14-ten-rounds-brief.md) stands. For this chain in particular:

| Need | Plugin or skill |
|---|---|
| Pulling film frames, measuring a set in character heights, blocking | cinewright-analyze |
| How the game treats floors at more than one height, squads, timers, cleared rooms | madnesswright-modtools (`decomp.py`); never publish decompiled code |
| Ramps, steps, decks, pillars as meshes made in code, imported 1:1 | polywright (code route), `tools/make_props.py` |
| What height does to a fight: high ground, cover, routes, fairness | ludowright |
| Blind judging of renders against the film frames | artwright-judge |
| Lint rules, references and learnings the work turns up | madnesswright (`mpn.py scene-lint`, the scenes skill's references) |
| The owner's questions and tests | askwright |
| The chain itself | chainwright (`chain.py check` on the prompt after any edit) |
