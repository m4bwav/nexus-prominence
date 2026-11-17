# Nexus Prominence (Hank Raid) Playground template

![A two-storey cutaway of a grey concrete industrial facility banded with red hazard stripes, showing a roller-shutter garage with a generator and a tapered pylon, stairwells and rooms where a few faceless suited stick figures stand](https://raw.githubusercontent.com/m4bwav/nexus-prominence/main/.github/images/banner.jpg)

A Playground map for MADNESS: Project Nexus, built with [madnesswright](https://github.com/m4bwav/madnesswright), the open-source toolkit for modding the game; this repository was split out of it on 2026-11-11 with its full history. On the Steam Workshop: https://steamcommunity.com/sharedfiles/filedetails/?id=3837705778 Wiki: https://github.com/m4bwav/nexus-prominence/wiki

A Playground map template after the fan collab "NEXUS: HANK RAID | MADNESS COMBAT" (hosted by KJ and DUALITY, 2025): Hank storms the Nexus Prominence control tower from the street, room by room. The requester asked for "a template of this map". The video's art belongs to its animators, so this template copies none of it: it rebuilds the route as ten rooms with the game's own proxies and Nexus enemies, in the collab's gray-and-red look.

Status 2026-11-11 (newest): the camera reaches every corner (`FitCamera`; check with `NexusShots.Render -shotDir <dir> -corners`, which logs OK/OUT per corner) and rooms carry Madness dressing (`Gags`). **Run `python tools/make_decals.py <SceneTools>/Assets/Templates/NexusProminence/Tex` before Build**, or the signs and splats are skipped with a warning. Deployed, not played.

Earlier status 2026-11-11: rooms differ in size (`Width`/`Depth` per room in the table; the camera pans between the stretched borders), cover spreads through the floor (`CoverAt`), bigger squads, banners, stripes and floor details; render renders/rooms-2026-11-11c.jpg. Earlier status 2026-11-11: rooms rebuilt to look like the video (shared red-band shell plus a set per room, fog off); render check with `NexusShots.Render -shotDir <dir>`; see renders/. Earlier status: built 2026-11-10 in Unity 2019.2.6f1 (batch mode): the generator compiled first time and made all ten rooms; SYNCH VISUALS, scene-lint (0 errors), the .sobj (1.1 MB), check-mod and deploy all passed. **Not yet played.** Batch route: `Unity.exe -batchmode -quit -projectPath <SceneTools> -executeMethod NexusProminenceBuilder.Build`, then `NexusBatch.SyncAndBuild` (Editor/NexusBatch.cs: SYNCH VISUALS on room 1, save, compressed bundle build).

Encounters 2026-11-11 (`Encounters`, after `ChainDoors`; toggle `JuiceEncounters`): spawn-only doors (`PX_Door` of type `VentSide` or `Rapel`, linked to nothing) feed `Squad_Spawner` waves: room 4 a 3-agent rappel team (14 s), room 7 four agents from two vents (10 s), room 10 reinforcements, 5 agents, 2 riot guards, 2 soldats, squads of 3, at most 6 on screen, from a vent and two rappel lines (12 s). Spike strips (`PX_Togglable` type `Spikes` + `CustomAddon_ToggleAutomator`, up 90 frames, down 150) in room 6 (two, out of step) and room 7 (one), on hazard-striped plates. Colours warmed to the film's maroon cast. Not played: unknown whether the automator runs on a plain PX_Togglable (its header says custom togglable) and whether the game draws its own vent and rope.

Reinforcements 2026-11-14 (build zk, ten rounds round 1): the game lets a wave squad in only while the room's living enemies of that faction plus the next squad stay at or under the wave's `MaxUnitCount` (`Wave_Tandem.CanSpawnWithoutBreachingMax` in the decompiled game), so the old caps held the room 4 rappel team back until the room was empty. Toggle `ReinforceWhileFighting` raises the caps of the three waves to 5, 5 and 7 (retired in round 8: FilmTotals removes those waves). Toggle `DoorReinforcements` (`DoorWaves`, plan in `DoorWavePlan`) sends one squad of 2 to 4 through each room's spare doors (the sample room's doors that are not chained and allow spawning), sized to a saw-tooth curve with peaks in rooms 4, 7 and 10; rooms 1-3 send theirs only once the alarm is up (`SpawnRequirement MyFactionAlerted`). Not played. Render of rooms 4, 7 and 10 (look unchanged; the change is who walks in): renders/rooms-4-7-10-2026-11-14zk.jpg.

![Rooms 4, 7 and 10 of build zk](renders/rooms-4-7-10-2026-11-14zk.jpg)

No Playground room 2026-11-14 (build zl, ten rounds round 2): room 1 used to keep the sample's `PX_Playground` proxy, which the game turns into the Playground control room (`Converter_Proxy.ReplaceProxy`), a white lab booth about 4 x 9 x 15 m whose window was masked through the street checkpoint's city skyline. A player called it "a playground room for no reason". Toggle `PlaygroundControlRoom` (off) leaves it out; the decompiled game shows the map does not need it (it is loaded from the Default Chamber's terminal, difficulty is global, the pause menu still leaves). Without it there is no in-map spawn or stage menu: set the difficulty in the Default Chamber before loading. Editor renders cannot show the booth (it exists only in game), so the room 1 render is the same as zk: renders/room-1-2026-11-14zl.jpg. Not played.

![Room 1 of build zl, the street checkpoint with no control room window](renders/room-1-2026-11-14zl.jpg)

Each room its own place 2026-11-14 (build zn, ten rounds round 4): lighter mid-gray walls and floors as in the film (`LighterWalls`; light tiles from `tools/make_textures.py`), a light level that rises and falls along the raid with one landmark per room and lit headers over the way in and the way on (`RoomIdentity`; table `IdentityLight`). Landmarks are thin and high, so cover, fights and enemy counts are unchanged; camera, zoom and room sizes too. Not played.

![Build zm (left) and zn (right), all ten rooms](renders/rooms-zm-vs-zn-2026-11-14zn.jpg)

Coherence pass 2026-11-14 (build zr, ten rounds round 8): the spotlight corridor and the shutter garage are graded from the film (`FilmSeams`), so the corridor is no longer a black gap in the run. Two toggles that no longer did anything were retired. Every toggle, its round, its default and how to undo it is in [ai-docs/notes/2026-11-14-toggle-table.md](ai-docs/notes/2026-11-14-toggle-table.md). Render: renders/room-7-zq-vs-zr-2026-11-14zr.jpg.

Own walls 2026-11-14 (build zs, ten rounds round 9): the rooms no longer draw the SceneTools sample's wall mesh; the map's own panels stand where it was (`MapWalls`), with the same size, doors and camera. Render: renders/room-1-map-walls-2026-11-14zs.jpg. Not played.

Backup mid-fight 2026-11-15 (build zv, toggle `ArrivalsMidFight`): a player met the backup only on walking back through finished rooms. In the decompiled game a squad's timer starts when its room becomes current, only runs while that room is current and never resets, and nothing holds the player in a room, so the zu delays (20, 18 and 20 s from entry in rooms 4, 7 and 10, 8 s after the alarm in room 2) lost to a fast clear and the squad came out on the way back. The unit cap was not the cause (it never holds back a spawner's first squad). Now each door squad waits for the alarm, then 2 to 4 s (hall 2, cargo bay 4, corridor and tower 3), comes as one squad, and a room emptied of its guards never arms it. Counts unchanged (placed + backup 3 + 1, 5 + 2, 3 + 2, 4 + 1). Played by the owner 2026-11-15 (log clean) and released to the Workshop the same day. Undo: `ArrivalsMidFight = false`, or deploy the lab's sobj-zu.

Round 10 (2026-11-14): no map change; docs made consistent and the end test prepared. zs is the build for the requester's test.

Film counts and finish 2026-11-14 (build zq, ten rounds round 7): no room holds more enemies than the film, waves included (toggle `FilmTotals`; placed + backup per room 2, 4, 3, 7, 4, 3, 5, 3, 4, 5, was up to 17 in the tower). The hall scout walks in once the alarm is up, as at 1:05 in the film. The garage console, both vending machines and the break room fridge are code-made props (`MoreModelledProps`), locker frames and bench slats read more clearly, and the office's red back window is as tall as the film's (`FilmRedFields`). The dark cone in earlier film-camera renders was SceneTools' camera-target gizmo, not part of the map; renders now hide it.

![Controls office, zp left, zq right](renders/room-3-zp-vs-zq-2026-11-14zq.jpg)

Every room its own fight 2026-11-14 (build zp, ten rounds round 6): before, every room placed its enemies the same way, a loose group right of centre. Toggle `StagedFights` (tables `Posts` and `FilmCountFoes`) gives each room one idea from the film: 1 ambush (two guards, backs turned), 2 walk-in (a scout enters by a side door; since round 7 a one-man door squad after the alarm, not `SpawnFromDoor`), 3 behind the desks, 4 shield wall (riot guards in front with `Defensive`), 5 sniper (a battle-rifle scout with `Camper`, which sends it to the loft's sniper points), 6 breather (unaware at the console), 7 gauntlet, 8 close quarters (`Hunter`), 9 false calm (unaware on break), 10 last stand (soldats holding the desks). Each enemy's place, facing, alertness, weapon and added behaviour are fields the game's own character marker reads (checked in the decompiled game). Rooms 6, 9 and 10 now start with 3, 4 and 4 placed enemies, inside the film count. No enemy starts nearer the way in than 45% of the room. Cover, camera, zoom and room sizes unchanged; markers do not render in editor shots, so the plan is drawn from the build log with `tools/fight_plan.py` (way in blue, way on red; white unaware, amber hunting, red combat; [ ] riot guard, c cover, D defensive, H hunter, R runner, S sniper camp, / sword, F battle rifle). Not played.

![Each room's fight from above, build zp](renders/fights-2026-11-14zp.png)

Care and finish 2026-11-14 (build zo, ten rounds round 5): the office counter, control tower desk, lockers, locker-room bench and cabinet are modelled props made in code (`ModelledProps`; `tools/make_props.py` writes OBJs in metres that import 1:1), at the size of the boxes they replace, so cover is unchanged. The controls office, sniper loft and locker room take a warmer, lighter grade from the film frames (`FilmGrade`: mauve office with a pale dado, brown loft with a red skirting line, locker room lit red from above). The Workshop text now says how the map was made; a new preview and gallery wait in the lab, not uploaded. Not played.

![Room 3 from the film camera, zn (left) and zo (right)](renders/room-3-zn-vs-zo-2026-11-14zo.jpg)

Drawn surfaces and a modelled crate 2026-11-14 (build zm, ten rounds round 3): walls, panels, floors and asphalt now wear tiling textures (outlined metal wall panels and floor slabs, generated with Z-Image through comfyui-gen, cropped to whole cells so they tile, tinted to the map's own colours by `tools/make_textures.py`; the full-size sources and recipe stay in the lab). UVs are set in metres (`TileMetres` 2.5) so a panel keeps one size on every box. The crate body is now a modelled crate with raised edge frames (made in code, imported as OBJ; the import came in at exactly 1.0 x 1.2 x 1.0, so OBJ in metres imports 1:1 into SceneTools). Toggles `TexturedSurfaces` and `HeroCrate`; false restores the zl look. Camera, zoom, room sizes and enemies unchanged. Not played.

![Room 4 of build zm, the cargo bay with tiled floor and wall panels](renders/room-4-2026-11-14zm.jpg)

Film framing: the shot renderer now has a film camera (`NexusShots.Render -filmcam -filmcams <file>`), placed per room from a hand-fitted list (low, close, level like the film) so the film-match pairs compare framing, not only the room. The editor-only view of the cargo bay from that camera:

![Room 4 of build zm from the film-framing camera](renders/room-4-film-cam-2026-11-14zm.jpg)

Juice 2026-11-11: shootable set pieces from the film (room 2 display glass, room 7 lamps, room 10 monitor wall) via `Breakable` and `CustomObj_Destructible`; toggle `JuiceBreakables`; details and revert in [juice-changes.md](juice-changes.md). Not played yet.

## Rooms (left to right, each door chains to the next)

Sets per room are in the generator (`Street`, `WantedHall`, ...); the Workshop description carries the same table with difficulty. Stations and pickups come from `Stations` (fun pass 2026-11-11).

| # | Room | Enemies (Nexus) | Alert | Stations (back wall) | Floor pickup | Difficulty |
|---|------|-----------------|-------|----------------------|--------------|------------|
| 1 | Street checkpoint | 2 agents, rifles | unaware | Ammo | R_NN57 pistol | Easy |
| 2 | Wanted hall | 3 agents, 1 scout | unaware | - | M_SwordNexus | Easy |
| 3 | Controls office | 2 agents, 1 support | unaware | (ammo: no clear wall, skipped) | R_NM46 SMG | Easy |
| 4 | Cargo bay | 2 riot guards, 2 agents, 1 soldat | hunting | Med cabinet, grenade ammo | Shield_RiotLight | Hard |
| 5 | Sniper loft | 2 scouts (battle rifles), 2 agents | hunting | - | R_Hrifle_NFAL | Medium |
| 6 | Shutter garage | 2 engineers, 2 agents | hunting | Ammo | R_NZucker | Medium |
| 7 | Spotlight corridor | 4 agents, 1 riot guard | combat | Med cabinet, ammo | R_Nc762 rifle | Hard |
| 8 | Locker room | 1 riot guard, 2 agents | combat | - | M_SwordNexus | Medium |
| 9 | Break room | 3 agents, 1 support, 1 scientist | combat | Med cabinet | R_NS-V.10 | Hard |
| 10 | Control tower | 2 soldats, 2 riot guards, 1 engineer, 2 agents | combat | Med cabinet, ammo, grenade ammo | R_Hrifle_NFAL | Very hard |

Glitch fixes 2026-11-16 (build zzh, on zzg; from the owner's seven glitch shots of zzg): flickering surfaces (z-fighting) gone across the map: the spike strips' warning paint now frames the strip instead of lying under it, the sniper loft's upright red stripe and the checkpoint skyline stand in front of the wall band, loose papers each sit at their own height, door headers, corner bands, skirts and swap pieces (lamp and screen glow and their dead copies) no longer share a plane (`mpn.py scene-lint` S26: 273 coplanar pairs on zzg, 0 on zzh). The spotlight cones in rooms 7 and 8 are round cone meshes with a floor glow (still shot out with the lamp). Room 2's recess has no post or band across it. Room 6's near pylon stands 1.6 units further out (about 3 units to walk between the pylons). Room 4's walkable slope is steel with a red front edge so it reads from the camera. Not played.

Film geometry 2026-11-16 (builds zzd to zzg, part C rounds 8 to 11, stacked; zzg holds all four): the wanted hall gets the film's red hall (set C, 1:21-2:13): one big wanted case on a red-framed panel, a jutting wall block (solid), a recessed bay, the vending machine at the film's place with the breakable glass moved onto it (solid) and an open floor hatch with a raised rim (one low collider) in place of the hung posters, fake back doors, centre case and flat hatches (`FilmRoom2Structures`). The controls office moves its counter (solid, 0.9 m clear of the wall) and red window left of centre, adds a solid low kerb along the back wall right of it, piers and the closed door under the "controls" sign; the filing cabinets (a known overlap clash) and clock are gone (`FilmRoom3Structures`). The spotlight corridor keeps two of its four shootable lamps at the film's cone places, adds a framed screen, a closed emblem door, a louvre panel and solid grey wall blocks near both ends, 3.6 units in so they clear the side doors (`FilmRoom7Structures`). The break room becomes the film's canteen (set A, 10:14-10:33): a solid serving counter at the right under a hatch, menu board and round red light, a deep red door frame, a solid wall jog and a taped carton; the fridge, vending machine, table and drum go (`FilmRoom9Structures`). The canteen's raised walkway is left out until the cargo-bay walkable deck has been played. All gates pass; not played; lab sobj-zzd to sobj-zzg.

Film geometry 2026-11-16 (build zzb, part C round 6): the street checkpoint gets the film's street-front bay (0:17-0:41): the window narrows to the film's span with wall each side, a waist-high sill (0.9 m, a dark ledge on a red-lit low wall) runs along it and a dark pilaster stands at its right end, both solid; the hung sign moves to the film's place left of centre. The folded boom barrier is gone (the film has none). The film's left wall block and counter are left out: they would stand by the left-wall door (toggle `FilmRoom1Structures`; undo with false or the lab's sobj-zza).

Walkable deck 2026-11-16 (build zzc): the cargo bay's back deck and its slope are the map's first walkable second height (the owner could not get up the slope in zza). The deck's collider is on layer Terrain with tag Floor, the drawn slope has no collider and an invisible straight ramp on layer Interactive lies under it (about 10.6 degrees, flush at both ends), and two waypoints at the ramp's foot and head are force-connected. Unverified in game (toggle `FilmRoom4Walkable`; undo with false or the lab's sobj-zzb).

Film geometry 2026-11-16 (build zza, part C round 5): the cargo bay gets the film's crate room (set H, 5:22-5:51): one strapped crate alone in the floor about 4 m out, a raised back deck 0.45 m high and 2 m deep with a red front edge and a sloped end at its right, all solid on the one floor (the deck and slope are solid blocks with their own colliders, not a route); a vent box over the deck and a red band with hazard stripes and an emblem plaque. The crates against the back wall, pallet, floor strips, banners and hung sign are gone (toggle `FilmRoom4Structures`; undo with false or the lab's sobj-zz).

Film geometry 2026-11-16 (build zz, part C round 4): the locker room gets the film's set (7:59-8:31): three tall lockers, a square column, a wall cabinet, a free-standing cabinet and a strapped crate, and a flight of steps with a landing at the right, all solid on the one floor (the steps are one solid block with a single collider, not a route); the emblem door in the back wall, two red spotlight cones that go dark when the lamp is shot, a wall box on a pipe. The six lockers, bench and board are gone (toggle `FilmRoom8Structures`; undo with false or the lab's sobj-zy).

Film geometry 2026-11-16 (build zy, part C round 3): the control tower gets the film's admin room (10:34-11:01): a hooded console with a red screen and a narrow cabinet against the back wall, a low bench on a floor mat and a tower unit (all solid on the one floor), two screens hung from ceiling mounts (they break when shot), a tall red light panel, a recessed panel with a diagonal trim, a left pilaster and a lit sign over the right-wall door. The monitor wall, desk, racks and banners are gone: the film's last room has none (toggle `FilmRoom10Structures`; undo with false or the lab's sobj-zx).

Film geometry 2026-11-16 (build zx, part C round 2): the sniper loft gets the film's balcony room (5:51-6:19): a big braced crate by the way in, a 4 m sloped low wall angled out from the back wall, stacked crates under the balcony and a wall pier, all solid on the one floor; the balcony is a thick slab with a chamfered underside and no rail, the ladder at its right end, a lit doorway on the deck and the red band rising to deck level. The balcony stays backdrop until a second floor height is researched (toggle `FilmRoom5Structures`; undo with false or the lab's sobj-zw).

Film geometry 2026-11-15 (build zw, part C round 1): the shutter garage gets the film's pillar hall pieces, all solid on the one floor: the pillar with a sloped barricade each side, a low barricade, two tall sloped blocks and the two pylons at film height (toggle `FilmRoom6Structures`; undo with false or the lab's sobj-zv). The stepped platform waits on research into a second floor height.

Where each room is in the film: [ai-docs/notes/film-geometry/film-room-map.md](ai-docs/notes/film-geometry/film-room-map.md) gives each room's film set and time range (read frame by frame on 2026-11-15), the nearby sets that are not that room, and how its enemies arrive. Several first guesses were wrong: the garage follows the pillar-and-pylon hall at 8:42-9:37, the sniper loft is at 5:51-6:19, and the control tower is the film's last room, an admin room at 10:34-11:01. The room-by-room structure sheets sit beside it.

Player start stays in room 1; the Playground control room is left out (toggle `PlaygroundControlRoom`). Weapons follow the film (`Loadout` in the generator, the game's own card names): rifles at the street checkpoint, pistols and SMGs in the hall, riot guards with `Shield_RiotLight` and a pistol, soldats with `R_Hrifle_NFAL`, scouts and a few later agents with `M_SwordNexus`; support, engineers and scientists keep their defaults. Wall signs and splats find a clear patch of wall (`CollectWallBusy`) or are dropped with a `no clear wall` log line. The Workshop preview comes from `tools/make_preview.py` over the room shots.

## Use it

1. Install Unity 2019.2.6f1 and open the SceneTools project (github.com/MPN-Modders/MPN_SceneTools); see the madnesswright-scenes skill.
2. Copy `Editor/NexusProminenceBuilder.cs` into `<SceneTools>/Assets/Editor/`.
3. Menu **Madness > Templates > Build Nexus Prominence _PG**. It writes `Assets/Scenes/NexusProminence_PG.unity` (asset label `nexusprominence`) and materials under `Assets/Templates/NexusProminence/`.
4. Select `Room_01_StreetCheckpoint`, press SYNCH VISUALS, look through each room, then `mpn.py scene-lint Assets/Scenes/NexusProminence_PG.unity`.
5. Madness > AssetBundle > Build, copy `nexusprominence.sobj` into `NexusProminence/` (the built .sobj is not kept in the repository, for size; the Workshop item 3837705778 carries it), then `mpn.py check-mod NexusProminence` and `mpn.py deploy NexusProminence`.
6. In game: enable the mod, open the Playground terminal, load Nexus Prominence.

## Playtest checklist

- Spawn in the street room; camera borders hold in every room.
- Every chained door leads to the next room and back; the spare doors still spawn.
- Red wall band sits at waist height on the back wall (the generator guesses its height; move it if not).
- Enemies path to every area; each room is clearable.
- `mpn.py log` shows no FAILED or ERROR line for the mod.

## Tweak

Rooms, cover and enemies are the `Rooms` table at the top of the generator; edit and re-run (it asks before overwriting).

Truer to the film 2026-11-14 (build zl, round 2, the requester's rule "don't betray the movie: the first room is only 2 enemies"): toggle `FilmTrueArrivals` keeps round 1's door backup only in the rooms where the film shows enemies walking in mid-fight (2 wanted hall, 4 cargo bay, 7 spotlight corridor, 10 control tower); the other rooms shut their spare doors (`DisableSpawn`), so the street checkpoint holds exactly its 2 agents. The film count per room is a standing table in ai-docs/plans/2026-11-14-ten-rounds-brief.md. Every round now pairs a film frame with the map render per room (lab script, reference only; no film art goes into the map). Small comparison, film frame left and the editor render of room 1 right: renders/film-vs-map-room-1-2026-11-14zl.jpg. Not played.

![Film frame of the street checkpoint beside the room 1 render of build zl](renders/film-vs-map-room-1-2026-11-14zl.jpg)
