# Toggle table (for the owner's end test)

Every feature the generator adds sits behind a `const bool` in [Editor/NexusProminenceBuilder.cs](../../Editor/NexusProminenceBuilder.cs). To undo one, set it to the other value and rebuild (`rebuild.sh <letter>` in the lab). To undo a whole round without a rebuild, deploy that round's `.sobj` from the lab folder `sobj-<letter>/`. Audited in round 8 (build zr), 2026-11-14.

| Toggle | Round / build | Default | What it does | How to undo |
|---|---|---|---|---|
| PlaygroundControlRoom | 2 / zl | false | false drops the sample's Playground booth from room 1 | true, or sobj-zk |
| TexturedSurfaces | 3 / zm | true | tiled, outlined wall panels and floor slabs | false, or sobj-zl |
| HeroCrate | 3 / zm | true | modelled cargo crate, same collider | false |
| LighterWalls | 4 / zn | true | mid-gray walls, panels, floor (film walls are mid gray) | false, or sobj-zm |
| RoomIdentity | 4 / zn | true | per-room light and colour, one landmark per room, way-in and way-on headers | false |
| WallHeaders | 8 / zr | false | headers flat along the wall; the judge preferred the old ones, so left off | true to try it |
| ModelledProps | 5 / zo | true | counter, desk, lockers, bench, cabinet as code-made models | false, or sobj-zn |
| FilmGrade | 5 / zo | true | rooms 3, 5, 8 graded from the film | false |
| FilmSeams | 8 / zr | true | garage red-brown, corridor lifted to dark red (was near black), from film medians | false, or sobj-zq |
| StagedFights | 6 / zp | true | each room its own fight: posts, facing, alertness, weapon, behaviours | false, or sobj-zo |
| MoreModelledProps | 7 / zq | true | console, vending machines, fridge; clearer locker frames and bench slats | false, or sobj-zp |
| FilmRedFields | 7 / zq | true | office red window as tall as the film's | false |
| JuiceEncounters | fun pass | true | squads, spike strips, spawn doors | false |
| DoorReinforcements | 1 / zk | true | a door squad through the spare doors | false |
| FilmTrueArrivals | 2 / zl | true | door squads only where the film shows a walk-in (rooms 2, 4, 7, 10); other spare doors shut | false brings back round 1's squads in every room |
| FilmTotals | 7 / zq | true | placed + backup inside the film's count in every room (2,4,3,7,4,3,5,3,4,5); no rappel, vent or tower wave | false = the zp counts |
| VaryBands, JuiceBreakables, FrontDebris, SolidProps | before the chain | true | per-room bands, breakables, front debris, solid props | false |
| FakeDoors | before the chain | false | painted extra doors (off since 2026-11-11) | true |

The arrival toggles stack: DoorReinforcements makes the squads, FilmTrueArrivals limits them to the film's rooms, FilmTotals sets their size. Turning off a lower one turns off the ones above it.

Retired in round 8, with no change to the build (zr behaves like zq):
- ReinforceWhileFighting (round 1): it only raised the caps of the rappel, vent and tower squads, which FilmTotals removes. The zk caps stay in code for FilmTotals false.
- Round 6's SpawnFromDoor walker in the hall. In the decompiled game, `Converter_Marker` copies `Marker_Character.SpawnFromDoor` to `Char_DataAssigner.SpawnToEntranceOrVehicle`. The character spawns as soon as its room becomes current (`CanSpawnOnSceneStart`, `Update`) and is handed straight to the door (`GiveCharacterToMe`), so it walks in when the player enters, with no delay and no alarm condition. A door squad (`Squad_DataAssigner.Update`) is different: it waits for `SpawnRequirement` (MyFactionAlerted = any faction member not Unaware, checked on a pulse tick), then counts `SecondsInRoomTilSpawn` from that moment. The hall scout (8 s after the alarm) uses the squad. To get the walker back, use sobj-zp.
