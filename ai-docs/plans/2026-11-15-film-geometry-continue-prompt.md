# Nexus Prominence: film geometry and arrivals continue prompt

Run in the nexus-prominence clone. The madnesswright clone and the lab folder (`../madnesswright-lab/nexus`) sit next to it. The goal: bring every room's fixed structure (ramps, steps, raised floors, pillars, balconies, wall blocks) close to what the film shows, make the backup arrive when the film shows it, and leave madnesswright better for each thing learned on the way.

The owner's ask, the player's report on the reinforcements, a first reading of the owner's screenshots and which plugin helps where are in [the brief](2026-11-15-film-geometry-brief.md). It does not change. Read it before anything else.

## 1. Load state

1. Read the top of `ai-docs/HANDOFF.md` (the film geometry block, the grants and the film table) and the end of `ai-docs/log.md`. Compare their age with the lab's newest `build-*.log` and the commits on main, and trust the newer evidence.
2. Read `AGENTS.md` and `README.md` here, and `AGENTS.md` in madnesswright. Their rules apply; this file does not repeat them.
3. Load the chainwright skill. Read the madnesswright-scenes skill and the madnesswright hub skill from the madnesswright clone. Load any other skill only when a step uses it (the brief maps skills to needs; parked skills are found through the parked-skills index named in the user's global instructions).
4. For any askwright set HANDOFF.md names, run `ask.py read`, act on answered questions, close them, then `ask.py consume <set> --note "<what you did>"`. "Not sure" and unanswered questions stay open and are never filled with a guess.

## 2. Order of work

The session that runs this prompt is the master. It hands each study and each build round to a subagent with a brief (the files to read, the skills to load, the gates below) and keeps only the short report. Studies only read film frames, so several run at once. Build rounds run one at a time: there is one SceneTools project and one render queue.

### A. The reinforcements, first

The fault is in the build players have now, so it goes before everything else and ships alone.

1. **Find the cause before changing anything.** Read the owner's last play in `mpn.py log` if it shows squads. Read the game's code with madnesswright-modtools: when a squad's timer runs, the unit cap, what happens to a squad that has not spawned when its room is left and entered again, and what lets the player move on. Set that against each backup room's placed count, squad size, cap and delay from the newest build log. Write the cause, with the evidence, in HANDOFF.md.
2. **Fix it in the generator** behind a toggle, to the brief's meaning of fixed. Keep the counts inside the film table.
3. **Make the toolkit catch it.** Add a `mpn.py scene-lint` rule in madnesswright that would have flagged this map (a squad that cannot enter while its room's placed enemies stand, or that can be outrun), with its test, and correct whatever the scenes skill says about squads that the cause proves wrong.
4. Run the gates in C4, keep the build's `.sobj` in the lab, and ask for a play (section 4). Until the owner has played it, this build is the one deployed: after any later build, deploy this one again from its `sobj-<letter>/` so the owner never tests the fix mixed with unplayed structures. It is released alone, on the owner's word, with `tools/release.sh`.

### B. Study the film, room by room

One subagent per room. Each one:

1. Pulls frames for the room's film times with cinewright-analyze, much denser than the 5 s sheets and at full size, into the lab. The owner's screenshots in the lab's `frames/` folder come first as evidence; find where each falls in the film.
2. Writes a structure sheet, `ai-docs/notes/film-geometry/room-<nn>-<name>.md`: every fixed structure the film shows (the brief's list), with where it stands (across the room and out from the back wall), its size in character heights and in metres, what it does in the fight, the film times that show it and how sure the reading is. Then what the newest map render has in that place, and the gaps ranked by how much a viewer of the film would miss them.
3. Says which film set the sheet follows when the film shows more than one for that room, and why.

Which film set and time range belongs to each room is in [notes/film-geometry/film-room-map.md](../notes/film-geometry/film-room-map.md); use it instead of guessing, and correct it first when a study disagrees. Sheets hold words and numbers only; frames stay in the lab. The master lists the sheets in HANDOFF.md and orders the build rounds from them: rooms in the owner's screenshots first, then the widest gaps.

### C. Build, one room per round

1. **Before the first structure a player can stand on**, learn how the game treats a floor at a second height: the official documents, the sample scenes in the SceneTools project, and the game's movement and pathing code. Record it as a reference in the madnesswright-scenes skill with its sources. Until that is proven, a structure is solid and stands on the one floor: pillars, wall blocks, sloped barricades, balconies as backdrop, steps and ramps that read as built but are not a route. The first walkable one is a proof in a single room, and it is played before a second is built.
2. **Plan.** A few lines in the round's HANDOFF entry: which structures from the sheet, their sizes, what each does to cover, sight lines and the route (ludowright), and what was left out and why. A structure the film does not show is not added.
3. **Build** in the session's own worktree, each room's structures behind that room's toggle. Shapes that are more than a box are made in code as meshes on the path `tools/make_props.py` already proved. Rebuild with the lab's `rebuild.sh <next letter> <worktree>`.
4. **Gates, all of them:** the generator compiles; every camera corner is on screen; `mpn.py scene-lint` reports 0 errors; `mpn.py check-mod` is clean; the overlap scan shows only the clashes listed as harmless in `ai-docs/notes/2026-11-11-nexus-overlaps-and-look.md`; the player start has floor under it; no structure stands in a doorway or closes the route from the way in to the way on; every placed enemy and cover point still stands on open floor. A round that fails a gate is fixed or reverted before the next one starts.
5. **Judge.** Fit the room's line in the lab's `filmcams.txt` to the film frame, render with `filmshots.sh`, and pair film and render with `film_match.py`. A fresh subagent compares the round's renders blind against the previous build's and the film frames with artwright-judge, from the film camera and the game camera. A change the judge does not prefer for likeness to the film is reworked or reverted, and judged again after the rework.
6. **Keep a way back.** Copy the built `.sobj` into the lab as `sobj-<letter>/` and note the commit.
7. **Record.** The round's HANDOFF entry (room, build letter, what was built, evidence, what stays unverified in game), the log, a line in the running change-notes draft in `workshop/`, `workshop/description.txt` and `README.md` in step, the room's line on the owner's test list (what to look at, and the toggle or `sobj-<letter>` that undoes it). Screenshots for the README, the wiki and the lab's `gal-<letter>/`; repository copies small.
8. **Improve the toolkit.** Each round leaves one concrete thing in madnesswright that came from it: a scene-lint check or info line (structures per room and their heights, a map whose rooms are all flat, a structure in a doorway), a script, a reference, a corrected claim, a learning. Follow madnesswright's AGENTS.md for tests, budget, changelog, version and tag. A gap in another plugin is fixed in that plugin's repository, in its own worktree, under its rules. Update the installed plugin after a release.
9. **Land it.** Commit, open the pull request and merge it under the standing rules in the user's global instructions. A wiki page the round makes wrong is corrected with wikiwright.

## 3. Limits for this chain

- **Nothing goes to the Workshop before the owner has played that build and said so** (AGENTS.md).
- **Do not start the game** unless HANDOFF.md records the owner's permission for it. Without that, evidence is editor-side and every play claim is marked unverified.
- **The played feel:** the brief's rule on camera, zoom and room sizes.
- **The film's art is not copied.** Structures are built fresh in the film's gray-and-red look; a generated texture or model keeps its recipe and licence record.
- **Shared machines.** Renders and generations queue behind other work under the comfyui-gen rules; kill nothing by image name.
- **Size.** Frames, renders and built `.sobj` files stay in the lab, not in either repository; madnesswright's `scripts/budget.py` must pass.

## 4. Questions and stop points

Questions for the owner go on one askwright page for this chain; name the set in HANDOFF.md and carry on with other work. Small creative calls are decided, flagged in the round's entry and put on the test list. Things that wait on the owner: playing a build, every Workshop upload, starting the game, anything a repository's rules reserve for them.

A play is asked for three times, each as a short askwright test with the build to play, what to look at per room, what undoes it, and full paths for anything to open: the reinforcement fix; the first structure a player can stand on; the last build. No other work waits on a play: studies and solid structures in other rooms go on meanwhile.

The work is finished when every room has a sheet, each structure a sheet ranks as plain in the film is built or listed with the reason it was not, the owner has played the last build, and it is released on their word. Then set `Status: finished` in HANDOFF.md.

Stop early when HANDOFF.md says `Status: finished`, or when nothing can move without the owner; say what waits.

## 5. Hand over

Hand over when the context is running low, at a stop point, or when the work is finished. A round is never left half built: finish or revert it first. Order: record the session's agent tree with chainwright's `chain_report.py from-session <session> --out ../madnesswright-lab/nexus/chain/chains.json` (never by hand), build the tier report and name it in the final reply; then the wrap-up (evergreen-wrapup if installed; a failure is reported and never blocks), then the log and HANDOFF.md, then print the same start command, `Execute ai-docs/plans/2026-11-15-film-geometry-continue-prompt.md`, in the nexus-prominence folder (give its absolute path in the reply, not in this public file). A subagent that ran a study or a round wraps up from its own transcript. State goes in HANDOFF.md, never in this file.
