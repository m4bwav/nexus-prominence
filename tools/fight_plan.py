"""Draw each room's fight from a build log: one panel per room, seen from above, the way in on the left.

Reads the NX_POST lines StagedFights writes (round 6) and draws every placed enemy as a dot (white = unaware,
amber = hunting, red = combat) with a tick for its facing and a short tag for its weapon or behaviour.
Usage: python tools/fight_plan.py <build-log> <out.png> [width]   Standard library plus Pillow.
"""
import re
import sys

from PIL import Image, ImageDraw

NAMES = ["1 street: ambush", "2 hall: walk-in", "3 office: desks", "4 bay: shield wall", "5 loft: sniper",
         "6 garage: breather", "7 corridor: gauntlet", "8 lockers: close", "9 break: false calm", "10 tower: last stand"]
COL = {"Unaware": (235, 235, 235), "Hunting": (240, 170, 40), "Combat": (220, 40, 40)}
FACE = {"Entry": (-1, 0), "Exit": (1, 0), "Back": (0, -1), "Front": (0, 1)}
TAG = {"Camper": "S", "HoldCover": "c", "Defensive": "D", "Hunter": "H", "Runner": "R"}


def main(log, out, width=960):
    posts = {}
    pat = re.compile(r"NX_POST (\d+)\.(\d+) (\S+) u ([\d.]+) v ([\d.]+) face (\w+) alert (\w+) held ?(\S*) add ?(\S*)")
    walk = None
    for line in open(log, encoding="utf-8", errors="replace"):
        m = pat.search(line)
        if m:
            posts.setdefault(int(m.group(1)), []).append(m.groups()[1:])
        if "NX_WALKIN" in line:
            walk = line
    pw, ph, pad = 200, 150, 8
    img = Image.new("RGB", (5 * pw, 2 * ph), (40, 40, 44))
    d = ImageDraw.Draw(img)
    for r in range(10):
        x0, y0 = (r % 5) * pw, (r // 5) * ph
        d.rectangle([x0 + pad, y0 + 22, x0 + pw - pad, y0 + ph - pad], outline=(130, 130, 130), fill=(70, 70, 76))
        d.rectangle([x0 + pad - 2, y0 + 60, x0 + pad + 2, y0 + 100], fill=(120, 200, 255))  # way in
        d.rectangle([x0 + pw - pad - 2, y0 + 60, x0 + pw - pad + 2, y0 + 100], fill=(200, 60, 60))  # way on
        d.text((x0 + pad, y0 + 5), NAMES[r], fill=(240, 240, 240))
        for n, typ, u, v, face, alert, held, add in posts.get(r, []):
            cx = x0 + pad + float(u) * (pw - 2 * pad)
            cy = y0 + ph - pad - float(v) * (ph - 30 - pad)  # back wall at the top
            fx, fy = FACE[face]
            d.line([cx, cy, cx + fx * 12, cy + fy * 12], fill=(255, 255, 255), width=2)
            d.ellipse([cx - 5, cy - 5, cx + 5, cy + 5], fill=COL.get(alert, (200, 200, 200)), outline=(0, 0, 0))
            tag = "".join(TAG.get(a, "") for a in add.split("+") if a) + ("/" if "Sword" in held else "") + ("F" if "FAL" in held else "")
            if typ.startswith("Nexus4"):
                tag = "[" + tag + "]"
            if r == 1 and n == "3" and walk:
                tag += " door"
            d.text((cx + 7, cy - 6), tag, fill=(255, 255, 160))
    if width != img.width:
        img = img.resize((width, int(img.height * width / img.width)), Image.LANCZOS)
    img.save(out)
    print(out, img.size, sum(len(v) for v in posts.values()), "enemies")


if __name__ == "__main__":
    main(sys.argv[1], sys.argv[2], int(sys.argv[3]) if len(sys.argv) > 3 else 1000)
