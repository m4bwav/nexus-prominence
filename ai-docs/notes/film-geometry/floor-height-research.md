# Research: a walkable second floor height in a SceneTools map (2026-11-16)

Question: can the player and the AI stand on, walk up to (ramp or steps) and path across a raised platform or balcony in a MADNESS: Project Nexus SceneTools map? Read-only research; nothing was built or played.

## Verdict

Yes, in principle, with conditions. Nothing in the movement or pathing code assumes one flat floor: characters are rigidbodies that stand on any collider of a walkable layer whose contact normal is steep enough, and the AI paths over a per-room graph of waypoints linked by line of sight, at any height. But there is no step-up logic (stairs must be a smooth invisible ramp), the AI drops off edges unless they are fenced with PathBlock, and neither sample scene has a walkable second height, so every claim below is from code, unverified in play.

## Sources

- Decompile: `ilspycmd -p` of `D:/SteamLibrary/steamapps/common/Madness Project Nexus 2/Madness Project Nexus_Data/Managed/Assembly-CSharp.dll` (owner's install, 2026-11-16) into the session scratchpad (not kept, never publish). Re-run with `madnesswright/skills/madnesswright-modtools/scripts/decomp.py decompile`.
- SceneTools project: `D:/m4bwa/Claude/Projects/Ai/MPN_SceneTools` (path from `madnesswright-lab/nexus/rebuild.sh`), `Assets/Code/SceneObjs/*.cs`, `Assets/Scenes/SamplePlayground_PG.unity`, `SampleSiegeArena.unity`.
- madnesswright-scenes `references/scenetools-rules.md` (layer list from the official SceneTools docs: "Interactive (12) blocks standing characters only: invisible stair ramps"; "PathBlock (25) blocks only AI pathing and waypoint links: cliff edges") and SKILL.md ("Stairs need an invisible Interactive-layer ramp").

## 1. Player and NPC ground check (same code for both)

- `Combatant_Base.OnCollisionEnter/Stay` -> `ReturnContactIsValidFloorLayer` (Combatant_Base.cs ~2311): a contact counts if the collider is tagged `Floor` **or** its layer is in `Cam_Main.walkableSurfacesList` (Cam_Main.cs ~181): Terrain, Obstacle, Default, TerrainInvis, TerrainObstacle, Interactive, Fence.
- `AdjustStanding` (~2328): standing only if `Mathf.Abs(contacts[0].normal.y) > 0.7` -> **slope limit about 45 degrees** (cos 45.6). Steeper = not touching floor.
- `OnCollisionExit` from a floor layer sets `amTouchingFloor = false`.
- Movement `FixedMove_CalculateVelocity` (~1170): with gravity the direction is `ReturnVectorNoY(waypoint - position)`, i.e. horizontal; `FixedMove_ApplyVelocity` (~1220) lerps the rigidbody velocity toward that horizontal velocity only while `amTouchingFloor`. Going up a ramp therefore relies on the physics contact pushing the capsule up; vertical velocity is kept only on a `CustomAddon_Platform` (`myPlatform`).
- **No step offset / step-up code** exists (no CharacterController, no step logic in Combatant_Base). A vertical lip of more than a few centimetres stops a walker. Real steps need a smooth ramp collider over them.
- Falls: `BeginFalling` if not touching floor and falling faster than 5 with no Terrain-mask ground within 5 below (~1062). `DealFallingDamage` starts at landing speed 8 (about a 3.3 m drop); fall-land state above 23 (`fallVelocityThresh`, ~27 m). A 3 m balcony drop is harmless.
- Waypoint snapping: `Converter_Marker.ReplaceMarker(Marker_Waypoint)` -> `Room_Point.SnapToFloor` -> `Char_DataAssigner.SnapToFloor(t, 0.1, 5)`: raycast from 0.5 above down 5.5 against **Terrain and TerrainObstacle only**. A waypoint on a Terrain balcony snaps to the balcony; one placed over an Interactive ramp snaps through to the Terrain below. `Floating` on the marker disables the snap.

## 2. How the AI paths

- No NavMesh anywhere in Assembly-CSharp (grep empty). Graph of `Path_Node` per room (waypoints), Dijkstra-style `RegisterNodeConnections`.
- Links (`Path_Node.FindSeenNodes`, Path_Node.cs ~68-125): node B is linked from A if (a) within 100 units, (b) a 0.3 sphere cast and a ray from 0.55 above A to B hit nothing on `hitLayers` (Terrain, TerrainInvis, TerrainObstacle, Interactive, Fence, PathBlock, CharacterBlock, HazardTrigger), and (c) sampling every unit along the straight line, a down ray finds ground within 2 units (or, when A is more than 2 above B, within lerp(15, 3, angle) units: **downward drop links are allowed**). `ForceCanSeeTheseNodes` (Marker_Waypoint.ForceConnect) bypasses all checks.
- Fully 3D: node positions keep their Y; nothing flattens the graph. Local avoidance (`Pathfind_Main.ReturnPathClear`, lift 0.3) only runs when the character's Y is within 0.5 of the next waypoint's Y.
- `Pathfind_Main.ValidatePointAccessibleForCharacter` (~706): a target point is valid if a ray from 3 above hits Terrain or a `Floor`-tagged collider, so goal points on a balcony are accepted.
- Consequences: Interactive is in `hitLayers`, so a link along a ramp must clear the ramp by 0.25 (fine on one straight plane, nodes at both ends); the balcony slab blocks sight between levels except where intended; a balcony edge with no PathBlock gets one-way drop links and NPCs will walk off it.
- `Leap_Area` (game class, not exposed in SceneTools) exists for NPC jumps between areas; not usable from a scene.

## 3. Sample scenes

- `SamplePlayground_PG`: one floor (`room_pg_room2_floor`, y 0.09, tag Floor). No raised walkable.
- `SampleSiegeArena`: floors at y 0 and 0.09; the Terrain "Plane (1)...(9)" at y 4.3 to 16.8 are rotated 90 degrees on X: vertical backdrops, not floors. **No walkable second height in either sample.** Indirect evidence the game has upper floors: `Effect_RendererVanisher` (VanishWhenBelowCeiling, "hide roofs and upper floors") in SceneTools Widgets.

## 4. Numbers

| Limit | Value | Where |
|---|---|---|
| Max standable slope | normal.y > 0.7 (about 45 deg); use 30 deg or less | Combatant_Base.AdjustStanding |
| Step height | none (no step-up) | absence in Combatant_Base |
| Link ground gap | 2 units below the line (more when dropping) | Path_Node.FindSeenNodes |
| Waypoint snap | 5.5 down, Terrain/TerrainObstacle | Char_DataAssigner.SnapToFloor |
| Fall damage | landing speed >= 8 | Combatant_Base.DealFallingDamage |

## 5. Safest first proof (one room)

1. One room, ground floor as now. A balcony slab along the back half: Box, layer Terrain, tag Floor, Static, top at y = 2.5 (under the ~3.3 m damage drop), at least 3 units deep, stopped at the back wall.
2. Ramp: one straight invisible Box on layer Interactive (no renderer), 25-30 degrees, flush with the floor at the foot and the slab top at the head (no lip at either end; overlap the slab slightly). Visual stairs, if any, on a non-colliding layer under it (Decal or no collider).
3. Waypoints: one at the ramp foot (on floor), one at the ramp head (on slab), two or three on the balcony, the usual floor set. Add `ForceConnect` foot <-> head both ways as insurance.
4. PathBlock box (invisible, thin, about 1 unit tall) along every open balcony edge except the ramp head, so no drop links form. Optional: Fence for a visual railing that blocks bodies but not bullets.
5. Ceiling: invisible collider above the balcony with head room (>= 2.5 units).
6. Test with one Char_Start on the balcony (Hunting) and the player starting downstairs: watch whether it walks the ramp, and the player walks up without sticking or bouncing.

## 6. What would break

- Ramp steeper than ~45 deg, or a lip at either end: characters stop or lose `amTouchingFloor` (fall state at the joint). Mesh steps without the ramp: no climbing at all.
- Ramp on Terrain instead of Interactive: works for walking but blocks bullets, aim and ragdolls; Interactive lets ragdolls fall onto the visual steps.
- Waypoint placed over the ramp: snaps to the floor below (ramp is not in the snap mask). Keep nodes off the ramp or mark them Floating.
- Missing PathBlock: NPCs path off the balcony edge (one-way drop links), land harmlessly but break the encounter.
- Balcony higher than ~5 units: fall checks and snap ranges (5 / 5.5) start to matter; drops above ~3.3 m deal damage.
- Camera: `Cam_Main` borders and Leeway were tuned for one floor in this mod; a balcony may sit out of frame or behind the camera-side cutaway (unchecked).
- Cover, Squad Spawner door entries and `Spawn_Manager.ReturnSpawnLocation(checkForFloor)` were not read for height handling: treat cover and spawns on the balcony as untested.
- Down-slope walking: horizontal-only velocity means a brief airborne hop on steep down ramps; keep it gentle.
