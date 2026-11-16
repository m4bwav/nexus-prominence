# Handoff

Live chain: film geometry and arrivals. Prompt: [plans/2026-11-15-film-geometry-continue-prompt.md](plans/2026-11-15-film-geometry-continue-prompt.md), brief: [plans/2026-11-15-film-geometry-brief.md](plans/2026-11-15-film-geometry-brief.md).

## Film geometry and arrivals

Status: open. Set up 2026-11-15; nothing studied or built yet.

- The ask (2026-11-15): the owner sees ramps, steps, higher areas and pillars in the film that the rooms do not show, and wants a closer inspection and rooms close to the film. A player also reported that the backup is only met when walking back through finished rooms and that the level can be finished without seeing any. Both are quoted in the brief.
- The owner's five screenshots are in the lab's `frames/` folder as `owner-2026-11-15-*.png`. Placed in the film on 2026-11-15: three are the pillar-and-pylon hall (room 6, 8:49, 9:02, 9:13), the balcony is the sniper loft (room 5, 6:03), the admin room is the film's last room (room 10, 10:47). The brief's first guesses for the 8:49 shot and the admin room were wrong.
- Build players have now: zv (Workshop item 3837705778, released 2026-11-15 on the owner's word after their play; mods.log 23:14 and Player.log 23:23 clean apart from the stock terrain-shader warning; deployed .sobj = lab sobj-zv). Next build letter: zw.
- Reinforcements (part A) done 2026-11-15: build zv (lab sobj-zv), PR #22, toggle ArrivalsMidFight. Cause: Squad_DataAssigner.Update stamps a squad's timer when its room becomes current, checks it only while current and never resets it; nothing holds the player in a room, so zu's timers from entry (20/18/20 s in rooms 4, 7, 10) lost to a fast clear and fired on re-entry. The unit cap was not the cause (Wave_Manager.CheckReadyToSpawn skips it for a first squad). zv: every door squad waits for the alarm, then 2/4/3/3 s (rooms 2/4/7/10), one squad, cap placed + squad; counts and positions unchanged. Gates pass; unverified in game. madnesswright 1.0.3 adds scene-lint S22 and corrects scenetools-rules.md.
- Reinforcement fix: played and released 2026-11-15 (change-notes-2026-11-15zv.txt). Later builds may stay deployed.
- Structure sheets (prompt, part B): done 2026-11-15 for all ten rooms plus the film's generator room, in [notes/film-geometry/](notes/film-geometry/). Which film set and time range is each room: [notes/film-geometry/film-room-map.md](notes/film-geometry/film-room-map.md) (the owner asked for this table so rooms are not guessed). Rooms 4 and 7 were read from contact sheets only, sizes rough. Open questions from the studies: room 3 shows about 10 enemies and room 8 about 10 in the film, against 3 each on the map and the older film table below; that table needs re-reading against the new spans before counts change. Build order from the sheets: rooms in the owner's screenshots first (6 pillar hall, 5 loft, 10 admin room), then the widest gaps (8 steps and spotlights, 4 raised deck, 1 window sill).
- Rooms built (part C): none. How the game treats a floor at a second height: not yet researched.
- Askwright set for this chain: nexus-zv-test-2026-11-15 (lab ask/zv-test-2026-11-15): play zv, five look-fors. Still to ask: may a session start the game itself to time the backup (the grant below says no)?
- Waits on the owner: nothing for zv (played, released). Askwright set nexus-zv-test-2026-11-15: consume it next session if answered.
- Owner's test list: zv, the five lines in the askwright set (room 2 scout within 2 s of the alarm; room 4 two agents about 4 s in; room 7 two about 3 s in; room 10 one soldat about 3 s in; walking back meets nobody). Undo: ArrivalsMidFight = false, or deploy sobj-zu.
- Next: part C, first round on room 6 (the pillar hall), solid structures only; the first walkable structure needs the floor-height research first (prompt C1).
- Workshop page pictures fixed 2026-11-15 (PR #20): cover shows all ten rooms, shots hide editor placeholders; map file unchanged.

## Grants and limits (date, scope)

- 2026-11-14, links: the owner asked that every Madness mod link the madnesswright plugin and its own repository. The description files, READMEs and wikis are covered outright. Reading of the session that wrote the prompt, not the owner's words: this also covers one update of each live Workshop page whose only change is the links, sent with the files and description of the release already live (zj here; the published version for Director), never with anything from an unplayed build. If the owner narrows this, write it here.
- Starting the game: not granted. Evidence stays editor-side.
- Unattended running (chain_loop): not agreed. It needs the owner's hours and budget first.

## Film count per room

Standing table (owner 2026-11-14: "Don't betray the movie: the first room is only 2 enemies"). Read from the film at one frame per 5 s (lab `film-frames/sheet5s_*.jpg`), so counts are approximate except room 1; the film often cuts between short fights, mapped here to the README rooms. "Walk in" means enemies are seen entering through a door mid-fight. A round may not push a room past this table; where the film shows no one walking in, the spare doors stay shut (`DisableSpawn`).

| Room | Film (time) | Enemies in the film | Arrival in the film | Map zl (placed + backup) |
|---|---|---|---|---|
| 1 Street checkpoint | 0:20-0:35 | 2 | standing at the checkpoint, no backup | 2 + 0 |
| 2 Wanted hall | 0:50-2:00 | about 4-5 | one walks in through the hall door (1:05) | 4 + 3 door |
| 3 Controls office | 2:30-3:15 | about 4-5 | already in the office | 3 + 0 |
| 4 Cargo bay | 4:05-5:45 | about 7 or more | more keep coming through the doors (5:35) | 5 + 3 door + 3 rappel |
| 5 Sniper loft | 8:40-9:35 | about 4-5 | placed, shooting from cover | 4 + 0 |
| 6 Shutter garage | 6:00-6:35 | about 3 | placed | 4 + 0 |
| 7 Spotlight corridor | 6:40-7:25 | about 4-5 | they step out of the side doors (7:00-7:25) | 5 + 3 door + 4 vent |
| 8 Locker room | 8:00-8:35 | about 4-5 | placed | 3 + 0 |
| 9 Break room | 9:40-10:30 | about 4 | placed | 5 + 0 |
| 10 Control tower | 10:35-10:50 | about 3 on screen, end of a running fight | the fight carries in | 7 + 4 door + 9 wave |

Map totals in build zu (placed + backup, build zq onward): 2, 3 + 1, 3, 5 + 2, 4, 3, 3 + 2, 3, 4, 4 + 1. The last column above is the older build zl.

## Ten rounds (finished 2026-11-14)

Ten improvement rounds (builds zk to zs), a spawn fix (zt) and an exit-bar fix (zu); the owner played zu and it was released to the Workshop the same day. madnesswright 1.0.0 came out of round 10. Scorecard at the end: coherence 4, the Playground room 5, details and textures 3, difficulty and reinforcements 4, care and appeal 3. Every round's entry, the end test list and the older states are in [archive/handoff-ten-rounds-2026-11-14.md](archive/handoff-ten-rounds-2026-11-14.md); the toggles are in [notes/2026-11-14-toggle-table.md](notes/2026-11-14-toggle-table.md). Much of what those rounds changed was never checked in play; the archive says which.

Also open:
Also open:
- The requester's test of Workshop item 3837705778 and their notes.
- The item has only the `playground` tag: propose search-menu tags to the owner on the next upload.
- Workshop comments (notes/2026-11-11-nexus-workshop-comments.md): some hostile about AI authorship; one says basing the map on a community project is wrong, which the owner may want to answer.
- Look proposals: [notes/2026-11-11-nexus-overlaps-and-look.md](notes/2026-11-11-nexus-overlaps-and-look.md), one at a time between plays.
- Level-technique harvest leads 12-19 live in madnesswright (ai-docs/notes/2026-11-11-level-technique-harvest.md); they need an in-game test.

Wiki (2026-11-11): https://github.com/m4bwav/nexus-prominence/wiki is live; how to update it is in notes/2026-11-11-github-wiki.md.
