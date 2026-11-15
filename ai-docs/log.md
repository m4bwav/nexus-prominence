# Log

Earlier history: madnesswright's ai-docs/log.md and its git history (this repository keeps the commits that touched examples/NexusProminence).

- 2026-11-11 split out of madnesswright with `git subtree split` (22 commits); added LICENSE, SECURITY.md, agent docs, banner, wiki; lab rebuild.sh repointed here; notes and continue prompt moved here.

- 2026-11-11: GitHub wiki written and published for Nexus Prominence build-zj (wikiwright); note in ai-docs/notes/2026-11-11-github-wiki.md.

## 2026-11-12
- Continue-prompt session: zj still unplayed (mods.log 2026-11-11 21:30). Stopped at the owner-play gate; nothing built or uploaded.
- 2026-11-12: build-zj played (mods.log 08:43, clean); owner: release it. wsup.exe updated 3837705778 (notes q, preview zj): result OK; gallery --clear 7 with gal-zj + film: result 1.
- 2026-11-12: tools/release.sh wraps the zj release steps (rediscovering them cost ~8 calls and a 120 s timeout); DRY=1 run on zj rebuilt preview and 6 gallery JPEGs.

## 2026-11-14
- Owner asked for a chained ten-round improvement run answering a 2026-11-13 Workshop comment (boring, Playground room, poor textures, no reinforcements, unused enemy entrances), tested only at the end, plus madnesswright and repository links on every Madness mod. Wrote plans/2026-11-14-ten-rounds-continue-prompt.md and its brief; HANDOFF has the round counter, scorecard and grants. Nothing built.
- Links side task done: Workshop description files, READMEs and wiki Homes of Nexus Prominence and Director link madnesswright and their own repos (Director no longer points at examples/SwapUnaware); madnesswright README lists both mods. No other Madness mod repo on the account. Live Workshop pages not updated (owner's call).
- Round 1 of ten (build zk): decompiled cap rule held the zj waves back (cargo bay rappel only once the room was empty); raised caps (ReinforceWhileFighting) and added DoorReinforcements through every room's spare doors. Gates pass; not played. rebuild.sh now takes the worktree as an optional 2nd argument (`sh rebuild.sh zk <worktree>`). sobj-zk in the lab.
- Owner rule 2026-11-14: every change ships with updated screenshots and descriptions (README, wiki, lab gal-<letter>/); added to prompt step 7. Round 1: gal-zk, renders/rooms-4-7-10-2026-11-14zk.jpg, wiki Rooms/FAQ/Versions. madnesswright 0.1.31 released (S15).
- Round 2 of ten (build zl): the "playground room" was the Playground control room booth the game builds from room 1's PX_Playground proxy, its window masked through the street skyline. Removed behind `PlaygroundControlRoom = false`; decompiled game shows the map loads without it. Editor renders cannot show the booth (shots-zl = shots-zk pixel for pixel), so the judge step is a tie by construction. Gates pass; not played. sobj-zl, gal-zl in the lab. madnesswright 0.1.32 (scene-lint S16).
- Round 2 mid-round owner rules: room 1 only 2 enemies (film); film-shot recreation in every round's judging. FilmTrueArrivals keeps door waves in rooms 2, 4, 7, 10 only; film table in the brief and HANDOFF; lab film_match.py makes film-match-<letter>/.
- 2026-11-14 Round 3 of ten (build zm): tiled wall/floor textures (Z-Image via comfyui-gen, cropped to whole cells, tinted by tools/make_textures.py; pw.py --seamless ghosts grid patterns) and a coded OBJ hero crate; OBJ in metres imports 1:1 into SceneTools (measured). Film camera per room hand-fitted in lab filmcams.txt; lab filmshots.sh re-renders and film_match.py <l> film writes film|render|50% overlay. rebuild.sh now also copies NexusShots.cs, runs make_textures.py and copies the crate OBJ. Gates pass; judge prefers zm (low confidence); not played. sobj-zm, gal-zm in the lab.
- 2026-11-14 Round 4 of ten (build zn): LighterWalls + RoomIdentity (per-room ambient/lamp arc, landmarks, way-in/way-on headers). Blind judge (fresh subagent): zn 6 rooms, zm 3 (red casts in rooms 1, 5, 8, reworked to neutral gray), 1 tie. Landmark boxes can cross door lightboxes that are not in f.Doors (pw.py overlaps caught it in room 2).
