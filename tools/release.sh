#!/usr/bin/env bash
# Release a played build to Workshop item 3837705778: preview + gallery from the lab shots, upload, gallery replace.
# Usage: tools/release.sh <build-letter e.g. zj> <change-notes file in workshop/>
# Needs: ../madnesswright/examples/WorkshopUpload/out/wsup.exe (built), Steam running, lab shots-<b>/ from rebuild.sh.
# DRY=1 builds the preview and gallery only (no upload).
set -euo pipefail
B=$1; N=$2
AI=$(cygpath -m "$(cd "$(dirname "$0")/../.." && pwd)")
NP=$AI/nexus-prominence; L=$AI/madnesswright-lab/nexus; W=$AI/madnesswright/examples/WorkshopUpload/out/wsup.exe
[ -d "$L/shots-$B" ] || { echo "no $L/shots-$B"; exit 2; }
[ -s "$NP/workshop/$N" ] || { echo "no notes $NP/workshop/$N"; exit 2; }
D=$(date +%F); PV=$L/workshop-preview-$D$B.jpg
rm -rf "$L/pv-$B"; mkdir -p "$L/pv-$B" "$L/gal-$B"; cp -r "$L/shots-$B" "$L/pv-$B/shots"
(cd "$L/pv-$B" && python "$NP/tools/make_preview.py") && cp "$L/pv-$B/preview.jpg" "$PV"
GAL=(01_StreetCheckpoint 02_WantedHall 04_CargoBay 06_ShutterGarage 07_SpotlightCorridor 10_ControlTower)
python - "$L" "$B" "${GAL[@]}" <<'P'
import sys; from PIL import Image
L,B,*names=sys.argv[1:]
for n in names:
    Image.open(f"{L}/shots-{B}/Room_{n}.png").convert("RGB").resize((960,540),Image.LANCZOS).save(f"{L}/gal-{B}/{n}.jpg",quality=90)
im=Image.open(f"{L}/pv-{B}/preview.jpg"); im.thumbnail((480,480)); im.save(sys.argv[1].replace("madnesswright-lab/nexus","nexus-prominence/workshop")+"/preview.jpg",quality=85)
P
[ "${DRY:-}" ] || "$W" "$NP/NexusProminence" "$PV" "Nexus Prominence (Hank Raid map)" "$NP/workshop/description.txt" Playground 3837705778 "$NP/workshop/$N" 2>&1 | grep result
[ "${DRY:-}" ] || "$W" previews 3837705778 "$NP/workshop/$N" --clear 7 $(for g in "${GAL[@]}"; do echo "$L/gal-$B/$g.jpg"; done) youtube:v0suvMY3GgM 2>&1 | grep result
