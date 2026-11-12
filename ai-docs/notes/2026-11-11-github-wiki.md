# GitHub wiki (2026-11-11)

Wiki: https://github.com/m4bwav/nexus-prominence/wiki, written with the wikiwright skill for Nexus Prominence build-zj. Working copy: a sibling clone `nexus-prominence.wiki` (master). Published by a plain push over GitHub's placeholder Home; `wikiwright.py live` passed for every page, sidebar and footer.

Pages: Home, Install-and-Play, Rooms, Build-from-Source, Workshop-Updates, FAQ-and-Troubleshooting, Versions, _Sidebar, _Footer.

Verification: Pages checked against README, generator entry points, workshop/description.txt and change notes. No game run. Kind: a game mod, not a package, so there is no wiki-verify script or saved output.

Public rules followed: no person named except the author credit, no machine names, no absolute local paths, no private projects; anyone who asked for features is "the requester". The pages credit madnesswright as the toolkit this was split from.

Inaccuracies found in the shipped docs (not fixed here):
1. The Workshop description still links the old madnesswright folder (already in HANDOFF).

Update procedure: pull the wiki clone, update the pages that name the version (Home, Versions, _Footer) plus the feature pages for what changed, run `wikiwright.py check <wiki dir> --version X`, commit, push, `wikiwright.py live m4bwav/nexus-prominence <wiki dir>`.
