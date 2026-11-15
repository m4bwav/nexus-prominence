# Nexus Prominence: ten improvement rounds continue prompt

Run in the nexus-prominence clone. The madnesswright clone and the lab folder (`../madnesswright-lab/nexus`) sit next to it. This chain has two goals that feed each other: make the map answer a player's criticism over ten rounds while staying true to the film, and leave madnesswright (and the plugins it works with) better after every round. The owner plays nothing until all ten rounds are done.

The goal, the criticism, what is already known about each point and which plugin helps where are in [the brief](2026-11-14-ten-rounds-brief.md). It does not change. Read it before anything else.

## 1. Load state

1. Read the top of `ai-docs/HANDOFF.md` (the "Ten rounds" block: round counter, scorecard, grants, open questions) and the end of `ai-docs/log.md`. Compare their age with the lab's newest `build-*.log` and the commits on main, and trust the newer evidence.
2. Read `AGENTS.md` and `README.md` here, and `AGENTS.md` in madnesswright. Their rules apply; this file does not repeat them.
3. Load the chainwright skill (this chain runs subagents, and its ledger and report steps come from there). Read the madnesswright-scenes skill and the madnesswright hub skill from the madnesswright clone. Load any other skill only when a round uses it (the brief maps skills to concerns; parked skills are found through the parked-skills index named in the user's global instructions).
4. For any askwright set HANDOFF.md names, run `ask.py read`, act on answered questions, close them, then `ask.py consume <set> --note "<what you did>"`. "Not sure" and unanswered questions stay open and are never filled with a guess.

## 2. What a round is

One round is one theme, one rebuild and one judged result. The session that runs this prompt is the master: it picks the theme, hands the round to one subagent with a brief (round number, theme, the files to read, the skills to load, the gates below), and keeps only the subagent's short report. Rounds run one at a time, never in parallel: there is one SceneTools project and one render queue.

Each round, in order:

1. **Pick the theme.** Take the concern with the lowest score on the HANDOFF scorecard that the last two rounds did not work on. Every concern in the brief gets at least one round before any concern gets a third. One round late in the chain is a whole-map coherence pass. The last round changes nothing risky: it fixes what the earlier rounds left rough, prepares the owner's test and prepares madnesswright 1.0 (version bump, changelog, README and wiki, unverified claims marked or proved, budget and tests green).
2. **Plan with the plugin that owns the question** (brief, "Which plugin helps where"). Write the plan as a few lines in the round's HANDOFF entry before building. Check each idea against the film frames: a change the film does not support is dropped or reshaped.
3. **Build.** Edit the generator in the session's own worktree. Every new feature sits behind its own toggle constant, as the existing ones do, so the owner can switch a round off. Rebuild with the lab's `rebuild.sh <next letter>`; it reads the generator from a fixed clone path, so make it take the worktree as an argument the first time this bites, and record that in the log.
4. **Gates, all of them:** the generator compiles; every camera corner is on screen; `mpn.py scene-lint` reports 0 errors; `mpn.py check-mod` is clean; the overlap scan shows only the clashes already listed as harmless in `ai-docs/notes/2026-11-11-nexus-overlaps-and-look.md`. A round that fails a gate is fixed or reverted before the next one starts.
5. **Judge.** Render the rooms the round touched and compare them blind against the previous round's renders and the film frames with artwright-judge. A look change the judge does not prefer is reworked or reverted. Recreate the film's shots every round: the lab's `film_match.py <letter>` pairs a film frame with the map render per room in `film-match-<letter>/`; that sheet is the judge's reference and a source of gallery candidates, and the enemy counts stay within the brief's film table. For play changes (enemies, waves, difficulty, routes) the evidence is the scene data and the game's decompiled code read with madnesswright-modtools, plus ludowright's read of the difficulty curve; say plainly that no one has played it.
6. **Keep a way back.** Copy the round's built `.sobj` into the lab as `sobj-<letter>/` and note the commit, so any round can be restored without a rebuild.
7. **Record.** Add the round's entry to HANDOFF.md (round n of 10, build letter, theme, what changed, the evidence, what stays unverified in game), rescore the scorecard with one line of evidence per score, append to the log, add a line to the running change-notes draft in `workshop/`, and keep `workshop/description.txt` and `README.md` in step. Add to the owner's end-of-chain test list in HANDOFF.md: one concrete thing to look at or do per change. Screenshots and descriptions updated for every change: README, wiki, gallery candidates in the lab (`gal-<letter>/`, never uploaded during the chain); repository copies small (480 px wide). (owner rule)
8. **Improve the toolkit.** Each round leaves at least one concrete improvement in madnesswright that came from the round: a lint rule in `mpn.py`, a script, a reference page, a corrected claim, a learning. Follow madnesswright's AGENTS.md for tests, budget, changelog, version and tag, and name the sibling plugins a change works with in its README and release notes. A gap found in another plugin (ludowright, polywright, artwright, comfyui-gen, game-juice, cinewright, askwright, chainwright) is fixed in that plugin's own repository, in its own worktree, under that repository's rules. Update the installed plugin after a release.
9. **Land it.** Commit, open the pull request and merge it under the standing rules in the user's global instructions (merge by default, worktree per session, tags and GitHub releases without asking). A wiki page the round makes wrong is corrected with wikiwright.

## 3. Limits for this chain

- **No Workshop upload of a build during the ten rounds.** AGENTS.md already forbids uploading a build the owner has not played; here that means the whole chain is local until the end test. The only Workshop action allowed earlier is the one HANDOFF.md grants by date and scope.
- **Do not start the game** unless HANDOFF.md records the owner's permission for it. Without that, all evidence is editor-side and every play claim is marked unverified.
- **Leave the played feel alone.** The camera, zoom and room sizes were tuned over many plays and one attempt to change several at once was reverted (madnesswright's HANDOFF, the build n to r entries). Change them only behind a toggle that is off by default, and list it for the owner's test.
- **The film's art is not copied.** Everything is built or generated fresh in the film's gray-and-red look; a generated texture or model keeps its recipe and licence record (comfyui-gen, polywright).
- **Shared machines.** Renders and generations queue behind other work under the comfyui-gen rules; kill nothing by image name.
- **Size.** Renders, frames, generated textures at full size and built `.sobj` files stay in the lab, not in either repository; madnesswright's `scripts/budget.py` must pass.

## 4. Side task: links on every Madness mod

The owner asked that every Madness mod link the madnesswright plugin and that mod's own repository. The mods and where their text lives are in the brief. Do it in the first round that has room: the Workshop description file, README and wiki Home of each mod carry both links, no link points at a path that has moved, and madnesswright's README lists each mod with its repository and Workshop page. What may go live on the Workshop pages, and when, is the grant in HANDOFF.md.

## 5. Questions and stop points

Questions for the owner go on one askwright page for this chain; name the set in HANDOFF.md and carry on with other work. Small creative calls are decided, flagged in the round's entry and put on the end test list. Things that wait on the owner: playing any build, every Workshop upload outside the HANDOFF grant, starting the game, anything a repository's rules reserve for them.

After round ten, build the end test as an askwright page: the build to play, a short list per round of what to look at (from the test list), which toggle or `sobj-<letter>` undoes each round, the renders side by side with the first round's, and the questions. Give full paths for anything the owner must open. Then set `Status: waiting on the owner's test` in HANDOFF.md.

Stop early when HANDOFF.md says `Status: finished` or `Status: waiting on the owner's test` with no answers in, or when nothing can move without the owner; say what waits. After the owner's test: read `mpn.py log` and their answers, fix what they found, and release with `tools/release.sh` only on their word; then set `Status: finished`.

## 6. Hand over

Hand over when the context is running low, at a stop point, or after round ten. A round is never left half built: finish or revert it first. Order: record the session's agent tree with chainwright's `chain_report.py from-session <session> --out ../madnesswright-lab/nexus/chain/chains.json` (never by hand), build the tier report and name it in the final reply; then the wrap-up (evergreen-wrapup if installed; a failure is reported and never blocks), then the log and HANDOFF.md, then print the same start command, `Execute ai-docs/plans/2026-11-14-ten-rounds-continue-prompt.md`, in the nexus-prominence folder (give its absolute path in the reply, not in this public file). A subagent that ran a round wraps up from its own transcript. State goes in HANDOFF.md, never in this file.
