# Handoff

Live chain: film geometry and arrivals. Prompt: [plans/2026-11-15-film-geometry-continue-prompt.md](plans/2026-11-15-film-geometry-continue-prompt.md), brief: [plans/2026-11-15-film-geometry-brief.md](plans/2026-11-15-film-geometry-brief.md).

## Film geometry and arrivals

Status: open. Set up 2026-11-15; nothing studied or built yet.

- The ask (2026-11-15): the owner sees ramps, steps, higher areas and pillars in the film that the rooms do not show, and wants a closer inspection and rooms close to the film. A player also reported that the backup is only met when walking back through finished rooms and that the level can be finished without seeing any. Both are quoted in the brief.
- The owner's five screenshots are in the lab's `frames/` folder as `owner-2026-11-15-*.png` (8:49 pillar and sloped blocks, garage pylons and generator, garage wide with floor edges, admin room, sniper balcony with ladder). The brief holds a first reading of each; the room each belongs to is still a guess.
- Build players have now: zu (Workshop item 3837705778, released 2026-11-14). Next build letter: zv.
- Next: the reinforcements (prompt, part A): find the cause first. Leads are in the brief; no cause is written down yet.
- Reinforcement fix: no build yet. When there is one, name it here; it stays the deployed build until the owner has played it.
- Structure sheets (prompt, part B): none yet. Rooms built (part C): none. How the game treats a floor at a second height: not yet researched.
- Askwright set for this chain: none yet. First question to put on it: may a session start the game itself to time the backup (the grant below says no)?
- Waits on the owner: nothing yet.
- Owner's test list: empty.

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
