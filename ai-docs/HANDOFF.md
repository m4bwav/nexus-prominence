# Handoff

## Ten rounds (chain: [plans/2026-11-14-ten-rounds-continue-prompt.md](plans/2026-11-14-ten-rounds-continue-prompt.md), brief: [plans/2026-11-14-ten-rounds-brief.md](plans/2026-11-14-ten-rounds-brief.md))

Status: in progress, round 1 of 10 done. Last build is zk (round 1, not played; zj is the played one on the Workshop); the next build letter is zl. While this chain runs it replaces the older continue prompt (2026-11-10), which waits on a play after every change.

Scorecard (1 poor to 5 good; score each row with one line of evidence at the end of every round; round 1 sets the baseline before it changes anything):

| # | Concern | Score | Evidence | Rounds spent |
|---|---|---|---|---|
| 1 | Coherence and interest | 2 | Ten rooms share one shell and one kind of fight; round 1 only added backup (shots-zk = shots-zj) | 0 |
| 2 | The Playground room | 2 | Playground terminal still in room 1 (scene-lint PX_Playground 1); walls are the sample mesh | 0 |
| 3 | Details and textures | 2 | Solid 4x4 colour textures, box props (shots-zk/Room_04_CargoBay.png) | 0 |
| 4 | Difficulty and reinforcements | 3 | zk: every spare door feeds a squad and the three old waves arrive mid-fight (build-zk.log squad lines, decompiled cap rule); baseline 2; not played | 1 |
| 5 | Care and appeal | 2 | Sum of 1-4; Workshop text now describes the backup honestly as unconfirmed | 0 |

Baseline (round 1, on build zj, before any change):
- 1 Coherence and interest: 2. All ten rooms share one shell (band, pilasters, pipe; shots-zj) and one fight: walk in, shoot the placed enemies.
- 2 The Playground room: 2. Room 1 still holds the sample's Playground terminal (scene-lint prefab_counts PX_Playground 1) and every wall is the `room_pg_room2` mesh.
- 3 Details and textures: 2. Solid 4x4 colour textures and box props everywhere (shots-zj/Room_04_CargoBay.png: flat crates, flat floor).
- 4 Difficulty and reinforcements: 2. Spawner waves only in rooms 4, 7, 10, and the game's cap rule kept them back (see round 1); every room shows three framed spare doors nothing uses.
- 5 Care and appeal: 2. The sum of 1-4; the Workshop text itself says the waves are unconfirmed.

Round 1 plan (concern 4, ludowright levels-and-space rules 11, 15, 21; game code read with decomp.py):
- Can the zj waves fire? Yes, by the code, but late. `Squad_DataAssigner.Update` starts its timer when the room becomes current, then `Wave_Tandem.CanSpawnWithoutBreachingMax` lets a squad in only while max(living faction occupants, spawned alive) + next squad <= MaxUnitCount. Cargo bay: 5 placed, cap 3, squad 3, so the rappel team came only once the room was empty; corridor vents at 2 left; tower at 3 left. The owner once saw the rappel team land (2026-11-11), which fits.
- Fix 1 (`ReinforceWhileFighting`): caps 5, 5, 7, so those waves arrive with 2, 3 and 4 enemies still standing.
- Fix 2 (`DoorReinforcements`): every room sends one small squad through its own spare doors (the unchained sample doors, spawning allowed; the Playground already spawns through such doors). Rooms 1-3 only once the alarm is up (`SpawnRequirement MyFactionAlerted`), so the quiet start stays quiet. Units per room (placed + all waves, zj -> zk): 2->4, 4->7, 3->6, 8->11, 4->6, 4->7, 9->12, 3->5, 5->9, 16->20. A saw-tooth with peaks at 4, 7, 10 and breathers at 5 and 8, the top beat last, no run of three high beats (rule 11). The film's raid escalates to the tower, so it wins over rule 21 (hardest early).
- Risk (rule 15): enemies walk out of a side door in view; a player standing in a spare doorway may meet one at melee range. Unverified in play.

Round 1 of 10 (2026-11-14, build zk, concern 4 Difficulty and reinforcements; branch round1-reinforcements):
- Changed: `ReinforceWhileFighting` (caps of the three old waves 3/4/6 -> 5/5/7) and `DoorReinforcements` (one squad per room through its 3-4 spare doors, 29 units in all, plan above). Generator only; no box, camera, zoom or room size touched.
- Evidence: decompiled game (scratchpad only): `Converter_Marker.ReplaceMarker(Marker_Squad)`, `Squad_DataAssigner.Update/Start`, `Wave_Tandem.CanSpawnWithoutBreachingMax`, `Wave_Manager` entrance pick (`EntranceCanReceiveCharacter`, falls back to any room entrance). build-zk.log lists 13 squads (13 Squad_Spawner in scene-lint). Gates: compiles, corners 0 OUT, scene-lint 0 errors, check-mod clean, overlaps room 1: 2 clashes + 1 filler, room 3: 3 (the known sign rods and file cabinets; no boxes were added). Renders shots-zk (look unchanged by design). Way back: lab sobj-zk/. Screens: lab gal-zk/ (gallery candidates, same six rooms as gal-zj), repo renders/rooms-4-7-10-2026-11-14zk.jpg (480 px), wiki Rooms/FAQ/Versions updated.
- Unverified in game: no one has played zk. Whether unchained Standard doors take wave spawns in a custom scene (the Playground does it), whether backup appears in view at melee range, whether MyFactionAlerted fires on the first shot.
- Toolkit: madnesswright scene-lint S15 flags spawn-capable doors no squad uses (see madnesswright release notes).

Grants and limits (date, scope):
- 2026-11-14, links: the owner asked that every Madness mod link the madnesswright plugin and its own repository. The description files, READMEs and wikis are covered outright. Reading of the session that wrote the prompt, not the owner's words: this also covers one update of each live Workshop page whose only change is the links, sent with the files and description of the release already live (zj here; the published version for Director), never with anything from an unplayed build. If the owner narrows this, write it here.
- Starting the game: not granted. Evidence stays editor-side.
- Unattended running (chain_loop): not agreed. It needs the owner's hours and budget first.

Open questions for the owner: none yet. Askwright set: none yet.

End test list (one line per change, filled by each round):
- Round 1 (zk), backup through doors: in rooms 1-3 stay unseen and check no one comes; then fire a shot and count the agents walking in from the side doors (2, 3, 3). Rooms 4-10: with about half the guards down, backup walks in from the side doors (undo: `DoorReinforcements = false`, or sobj-zk vs zj).
- Round 1 (zk), earlier waves: the cargo bay rappel team lands while 2 guards still stand, the corridor vents open with about 3 left, the tower wave starts with about 4 left (undo: `ReinforceWhileFighting = false`).
- Round 1: stand in a spare doorway and see whether backup appears on top of you (fairness risk).

Links side task: done 2026-11-14 (repo and wiki text; mpn-director#1, madnesswright#116, nexus-prominence mod-links PR; wiki Homes pushed). The live Workshop pages still carry the old text until the owner allows a page update.

State on 2026-11-12 (released zj): the owner played build-zj (mods.log 08:43, clean) and said release it. Workshop item 3837705778 updated with change-notes-2026-11-11q.txt and preview workshop-preview-2026-11-12zj.jpg (lab; repo copy 480 px): result OK. Gallery replaced with `previews --clear 7` (gal-zj rooms 1, 2, 4, 6, 7, 10 + film): result 1. Nothing waits on a play. Next: whatever the owner asks; the Workshop description still has no search-menu tags beyond `playground` (proposal stands).

State on 2026-11-12 (check only): mods.log still 2026-11-11 21:30, so build-zj is still unplayed; no rebuild and no upload. Next is unchanged: the owner plays zj, then upload with change-notes-2026-11-11q.txt and refresh the gallery from shots-zj.

State on 2026-11-11 (split): the map moved out of madnesswright (`examples/NexusProminence`) into this repository with its history. The lab rebuild script (`Ai/madnesswright-lab/nexus/rebuild.sh`) now copies the generator from and writes the .sobj into this clone; mpn.py still comes from the madnesswright clone next to it. Earlier state (from madnesswright's handoff) follows.

State on 2026-11-11 21:46 (pylons and generator remade, build-zj): the owner played zh again (clean, "it was good") and asked to make the generator and pylons properly. `Pylon` is now one tapered body (new `Taper` + cached `FrustumMesh` assets NX_Frustum_*) on a plinth with a red band and white stripe following the slope; `Generator` is a full genset (skid, tank on top, dial face, grille, wheels, exhaust up the wall, can, hose right of it). build-zj: 40/40 corners, check-mod clean, overlaps back to the known ones, deployed 21:46; lab render shots-zj/Room_06_ShutterGarage.png. **Not played, not uploaded** (mods.log still 21:30 at the last check). Next: the owner plays zj; on the OK upload with workshop/change-notes-2026-11-11q.txt and refresh preview + gallery from shots-zj (room 6 shot changed).

Also open:
- The requester's test of Workshop item 3837705778 and their notes.
- The item has only the `playground` tag: propose search-menu tags to the owner on the next upload.
- Workshop comments (notes/2026-11-11-nexus-workshop-comments.md): some hostile about AI authorship; one says basing the map on a community project is wrong, which the owner may want to answer.
- The Workshop description still links the old madnesswright folder (it redirects through a pointer README); point it at this repository on the next upload.
- Look proposals: [notes/2026-11-11-nexus-overlaps-and-look.md](notes/2026-11-11-nexus-overlaps-and-look.md), one at a time between plays.
- Level-technique harvest leads 12-19 live in madnesswright (ai-docs/notes/2026-11-11-level-technique-harvest.md); they need an in-game test.

Wiki (2026-11-11): https://github.com/m4bwav/nexus-prominence/wiki is live; how to update it is in notes/2026-11-11-github-wiki.md.
