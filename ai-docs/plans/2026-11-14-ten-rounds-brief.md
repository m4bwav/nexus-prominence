# Ten rounds: the brief

The fixed brief for [the ten-rounds continue prompt](2026-11-14-ten-rounds-continue-prompt.md). State lives in [HANDOFF.md](../HANDOFF.md), not here.

## The ask

The owner, 2026-11-14: use and improve madnesswright by improving the Nexus Prominence map, addressing the concerns below while staying true to the video; chain the work, using ludowright, polywright, artwright, the ComfyUI plugin and the other plugins, for ten rounds of improvement. The owner tests only at the end. Also: every Madness mod links the madnesswright plugin and its own repository.

Owner goal (2026-11-14): by the end of the ten rounds madnesswright gets a 1.0 release, in round 10 or after the owner's end test if it depends on in-game proof.

## The criticism

A comment on Workshop item 3837705778, 2026-11-13, quoted as written with the author's name left out:

> ignoring that this was done with A.I, this is just boring and incoherent. The map have a playground room for no reason, the details an textures are poor, and it is not really dificult cause of the absence of reinforcements, which makes everything even more ridiculous, because the rooms have entrances for enemies that are not used.
>
> This thing is just bad, there is no care or effort here. Even other people using A.I for making games give more dedication for their projects and try to instruct the A.I for doing something that is visually appealing at least.

Set the tone aside and take the claims as a player's report. Nobody replies to the comment from this chain; any reply is the owner's.

## The concerns, and what is known

These are the scorecard rows. Each lead below is where to start looking, not a finding: check it against the generator and the game before building on it.

| # | Concern | What the player said | Known so far |
|---|---|---|---|
| 1 | Coherence and interest | "boring and incoherent" | Ten rooms share one shell and one kind of fight (enter, shoot what stands there). An earlier look pass found the rooms run together. Nothing gives the route an arc beyond rising alert levels. |
| 2 | The Playground room | "a playground room for no reason" | The map is built on the Playground sample room: the walls are its mesh (`room_pg_room2_*`), and room 1 keeps Player_Start and the Playground terminal. First find out what the player actually saw (sample geometry showing, the terminal standing in the street checkpoint, a stray room in the chain), then either give it a reason inside the fiction or remove it if the map can still be loaded and played without it. |
| 3 | Details and textures | "the details an textures are poor" | Every surface is the game's `Madness/Terrain - General` shader over a solid 4x4 colour texture, and nearly every prop is a box. Decals come from `tools/make_decals.py`. No surface has a real texture and no prop is modelled. |
| 4 | Difficulty and reinforcements | "absence of reinforcements", "entrances for enemies that are not used" | `Encounters()` gives spawner waves only to rooms 4, 7 and 10, and the Workshop description itself says they are not confirmed in play. The spare doors of the sample room are left in every room. Work out from the scene data and the decompiled game whether the existing spawners can fire at all, then make every visible enemy entrance do something or take it away. |
| 5 | Care and appeal | "no care or effort", "visually appealing at least" | The sum of the others, plus the page: description, preview and gallery are part of what the player judged. |

Earlier comment review, with faults already fixed and a few left open: [notes/2026-11-11-nexus-workshop-comments.md](../notes/2026-11-11-nexus-workshop-comments.md). Earlier look pass: [notes/2026-11-11-nexus-overlaps-and-look.md](../notes/2026-11-11-nexus-overlaps-and-look.md).

## Staying true to the video

The film is the fan collab NEXUS: HANK RAID, https://www.youtube.com/watch?v=v0suvMY3GgM. The room table in the README was taken from its frames. Contact sheets and owner-sent frames are in the lab's `frames/` folder; there are few, so pull more with cinewright-analyze when a round needs a closer look at a room, and keep them in the lab. True to the video means: the route and order of rooms, what each room is and holds, the gray-and-red look, who the enemies are and how they arrive, and the pace of the raid. It does not mean copying the film's drawings. Where the film and a fix pull apart, the film wins and the fix is reshaped; where the film is silent, the game's own style decides.

Film count per room (standing table; owner 2026-11-14: "Don't betray the movie: the first room is only 2 enemies"). Read from the film at one frame per 5 s (lab `film-frames/sheet5s_*.jpg`), so counts are approximate except room 1; the film often cuts between short fights, mapped here to the README rooms. "Walk in" means enemies are seen entering through a door mid-fight. A round may not push a room past this table; where the film shows no one walking in, the spare doors stay shut (`DisableSpawn`).

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

## Which plugin helps where

| Need | Plugin or skill |
|---|---|
| Building, linting, deploying the map; SceneTools objects, spawners, doors, events | madnesswright-scenes, the madnesswright hub (`mpn.py`) |
| Reading the game's code to learn what a component really does | madnesswright-modtools (`decomp.py`); never publish decompiled code |
| Pacing, difficulty curve, one idea per room, encounter design, what makes it boring | ludowright |
| Feedback, impact, things that react to the player | game-juice (juice-principles, juice-apply); record changes in `juice-changes.md` |
| Breaking the film into rooms, sets, blocking and a level brief | cinewright-analyze |
| Judging renders blind against the film frames and the last round; critique of light and composition | artwright, artwright-judge |
| Tiling textures, decals, signs in the game's flat outlined style | comfyui-gen for the image, polywright-references (`pw.py texture`) for seamless tiles and maps |
| Hero props that should stop being boxes | polywright (generate or code, then finish and export); the import scale into SceneTools is still unverified in madnesswright, so prove one prop before making many |
| Overlap checks on room boxes | polywright (`pw.py overlaps`, already in `rebuild.sh`) |
| Sound, if a round reaches it | sfxwright; madnesswright's FMOD notes are unverified in game |
| Workshop description and README wording | everwrite |
| Wiki pages | wikiwright |
| The owner's questions and the end test | askwright |
| The chain itself | chainwright (`chain.py check` on the prompt after any edit) |

When a plugin falls short of what a round needs, that gap is a finding: fix it in the plugin (prompt, step 8).

## The Madness mods (links side task)

| Mod | Repository | Workshop item | Description file | Known link fault |
|---|---|---|---|---|
| Nexus Prominence | https://github.com/m4bwav/nexus-prominence | https://steamcommunity.com/sharedfiles/filedetails/?id=3837705778 | `workshop/description.txt` | No link to madnesswright or to the repository. |
| Director | https://github.com/m4bwav/mpn-director | https://steamcommunity.com/sharedfiles/filedetails/?id=3822329165 | `workshop-description.txt` | Source link points at the old `examples/SwapUnaware` folder in madnesswright. |

The plugin: https://github.com/m4bwav/madnesswright. Check the account for any Madness mod repository missing from this table before calling the task done. The uploader (`examples/WorkshopUpload` in madnesswright) always sends content with the description; a description-only mode would be a fair toolkit improvement, and without it a page update re-sends the files of the release that is already live.
