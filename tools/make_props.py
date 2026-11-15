"""Nexus Prominence modelled props (round 5, polywright code route).

Writes one OBJ per prop into the folder given (the lab's prop-zo/), in metres, Y up, centred on the
origin at the exact size of the box it replaces, so the builder's ModelSwap scales by ~1 and frames keep
their thickness. Each prop is a union of boxes with flat face normals; the in-game outline shader draws
their edges, which is what the film's props look like (framed panels, slats, kick recesses).

Usage: python tools/make_props.py <out dir>
"""
import os
import sys


def box(cx, cy, cz, sx, sy, sz):
    return (cx, cy, cz, sx, sy, sz)


FACES = [  # normal, four corner signs (x, y, z) wound counter-clockwise seen from outside
    ((1, 0, 0), [(1, -1, -1), (1, 1, -1), (1, 1, 1), (1, -1, 1)]),
    ((-1, 0, 0), [(-1, -1, 1), (-1, 1, 1), (-1, 1, -1), (-1, -1, -1)]),
    ((0, 1, 0), [(-1, 1, -1), (-1, 1, 1), (1, 1, 1), (1, 1, -1)]),
    ((0, -1, 0), [(-1, -1, 1), (-1, -1, -1), (1, -1, -1), (1, -1, 1)]),
    ((0, 0, 1), [(1, -1, 1), (1, 1, 1), (-1, 1, 1), (-1, -1, 1)]),
    ((0, 0, -1), [(-1, -1, -1), (-1, 1, -1), (1, 1, -1), (1, -1, -1)]),
]


def write_obj(path, title, boxes):
    lines = ["# Nexus Prominence prop: " + title + " (tools/make_props.py, code route, metres, Y up)"]
    vi = 1
    for n, (nx, ny, nz) in enumerate(f[0] for f in FACES):
        lines.append("vn %d %d %d" % (nx, ny, nz))
    faces = []
    for (cx, cy, cz, sx, sy, sz) in boxes:
        for fi, (_, corners) in enumerate(FACES):
            idx = []
            for (a, b, c) in corners:
                lines.append("v %.4f %.4f %.4f" % (cx + a * sx / 2, cy + b * sy / 2, cz + c * sz / 2))
                idx.append(vi)
                vi += 1
            # standard OBJ counter-clockwise winding; Unity's importer converts it (as nx_crate.obj)
            faces.append("f " + " ".join("%d//%d" % (i, fi + 1) for i in idx))
    with open(path, "w", newline="\n") as fh:
        fh.write("\n".join(lines + faces) + "\n")


def counter(w, h, d, bays):
    """Service counter / desk: body set back, a kick recess, framed front bays standing 3 cm proud."""
    t, kick = 0.06, 0.12
    out = [box(0, kick / 2, 0.06, w - 0.1, kick, d - 0.12),          # kick plinth, set back
           box(0, kick + (h - kick) / 2, 0, w, h - kick, d)]          # body
    fz = -d / 2 - 0.015                                             # front face (front is -z)
    bw = w / bays
    for i in range(bays):
        x0 = -w / 2 + i * bw
        cxb = x0 + bw / 2
        out.append(box(x0 + t / 2, kick + (h - kick) / 2, fz, t, h - kick, 0.03))   # stile
        out.append(box(cxb, kick + 0.1, fz, bw - 0.4, 0.04, 0.03))                # bay bottom rail
        out.append(box(cxb, h - 0.12, fz, bw - 0.4, 0.04, 0.03))                  # bay top rail
    out.append(box(w / 2 - t / 2, kick + (h - kick) / 2, fz, t, h - kick, 0.03))
    return out


def bench(w, h, d, slats):
    gap = 0.09  # round 7: 4 cm gaps between four slats did not read from the game camera; three slats, 9 cm gaps
    sd = (d - gap * (slats - 1)) / slats
    return [box(0, 0, -d / 2 + sd / 2 + i * (sd + gap), w, h, sd) for i in range(slats)]


def locker(w, h, d):
    """Tall locker: door inset in a frame that stands 5 cm proud (round 7: the 3 cm frame read as faint seams),
    a top cap. The vents and handle are builder boxes in front of the frame (round 5 rework)."""
    fz = -d / 2 - 0.025
    out = [box(0, 0, 0, w, h, d),
           box(0, h / 2 - 0.04, -0.01, w + 0.04, 0.08, d + 0.04)]           # cap
    for sx in (-1, 1):
        out.append(box(sx * (w / 2 - 0.05), 0, fz, 0.1, h, 0.05))          # frame sides
    for sy in (-1, 1):
        out.append(box(0, sy * (h / 2 - 0.05), fz, w, 0.1, 0.05))          # frame top/bottom
    return out


def fridge(w, h, d):
    """Tall fridge: freezer and main doors split by a gap line, two long handles, a plinth."""
    fz = -d / 2 - 0.02
    split = 0.7                                                          # at the builder's FridgeLine (y 2.7 on a 4 m fridge)
    out = [box(0, 0, 0, w, h, d),
           box(0, split, fz, w, 0.06, 0.04),                                # door split
           box(0, -h / 2 + 0.06, fz, w - 0.1, 0.12, 0.04)]                  # plinth line
    out.append(box(w / 2 - 0.2, split + 0.55, fz - 0.03, 0.07, 0.6, 0.06))  # freezer handle
    out.append(box(w / 2 - 0.2, split - 0.9, fz - 0.03, 0.07, 1.2, 0.06))   # main handle
    return out


def vending(w, h, d):
    """Vending machine: a framed window bay, a coin panel and a delivery slot below."""
    fz = -d / 2 - 0.025
    out = [box(0, 0, 0, w, h, d), box(0, h / 2 - 0.05, 0, w + 0.06, 0.1, d + 0.06)]
    for sx in (-1, 1):
        out.append(box(sx * (w / 2 - 0.05), 0.15, fz, 0.1, h - 0.6, 0.05))   # bay sides
    out.append(box(0, -h / 2 + 0.6, fz, w, 0.1, 0.05))                      # bay sill
    out.append(box(-w * 0.15, -h / 2 + 0.3, fz - 0.01, w * 0.5, 0.18, 0.06))  # delivery slot lip
    out.append(box(w * 0.32, -h / 2 + 0.32, fz - 0.01, 0.2, 0.3, 0.06))     # coin panel
    return out


def console(w, h, d):
    """Work console: sloped-looking stepped top (a raised back deck), front panel bays, a kick recess."""
    kick = 0.12
    fz = -d / 2 - 0.015
    out = [box(0, kick / 2, 0.06, w - 0.1, kick, d - 0.12),
           box(0, kick + (h * 0.8 - kick) / 2, 0, w, h * 0.8 - kick, d),         # body
           box(0, h * 0.9, d * 0.2, w, h * 0.2, d * 0.6)]                       # raised back deck
    bays = 4  # stiles fall between the builder's four console lamps
    bw = w / bays
    for i in range(bays + 1):
        out.append(box(-w / 2 + i * bw + (0.03 if i == 0 else -0.03 if i == bays else 0), kick + (h * 0.8 - kick) / 2, fz, 0.06, h * 0.8 - kick, 0.03))
    return out


def cabinet(w, h, d):
    fz = -d / 2 - 0.015
    out = [box(0, 0, 0, w, h, d), box(0, 0, fz, 0.04, h - 0.1, 0.03)]       # body, door split
    for sx in (-1, 1):
        out.append(box(sx * (w / 2 - 0.03), 0, fz, 0.06, h, 0.03))
        out.append(box(sx * 0.12, 0.1, fz - 0.03, 0.05, 0.3, 0.04))           # handles
    for sy in (-1, 1):
        out.append(box(0, sy * (h / 2 - 0.03), fz, w, 0.06, 0.03))
    return out


PROPS = {
    # name: (title, boxes); sizes are the boxes they replace in NexusProminenceBuilder.cs
    "nx_counter": ("office counter 12 x 1.2 x 1.0", counter(12.0, 1.2, 1.0, 6)),
    "nx_servecounter": ("break room counter 10 x 1.4 x 1.4", counter(10.0, 1.4, 1.4, 5)),
    "nx_desk": ("tower desk 15 x 1.5 x 1.4", counter(15.0, 1.5, 1.4, 6)),
    "nx_bench": ("locker bench seat 5 x 0.18 x 0.7", bench(5.0, 0.18, 0.7, 3)),
    "nx_locker": ("locker 1.1 x 3.4 x 1.0", locker(1.1, 3.4, 1.0)),
    "nx_cabinet": ("cabinet 2.4 x 2.2 x 1.0", cabinet(2.4, 2.2, 1.0)),
    # round 7 (build zq): the next most visible boxes the film shows as framed props
    "nx_fridge": ("break room fridge 1.6 x 4.0 x 1.2", fridge(1.6, 4.0, 1.2)),
    "nx_vending": ("vending machine 1.6 x 3.8 x 1.2", vending(1.6, 3.8, 1.2)),
    "nx_console": ("garage console 7 x 1.6 x 0.9", console(7.0, 1.6, 0.9)),
}


def recentre(boxes):
    xs = [b[0] - b[3] / 2 for b in boxes] + [b[0] + b[3] / 2 for b in boxes]
    ys = [b[1] - b[4] / 2 for b in boxes] + [b[1] + b[4] / 2 for b in boxes]
    zs = [b[2] - b[5] / 2 for b in boxes] + [b[2] + b[5] / 2 for b in boxes]
    c = ((min(xs) + max(xs)) / 2, (min(ys) + max(ys)) / 2, (min(zs) + max(zs)) / 2)
    return [(b[0] - c[0], b[1] - c[1], b[2] - c[2]) + b[3:] for b in boxes], (max(xs) - min(xs), max(ys) - min(ys), max(zs) - min(zs))


def main():
    out = sys.argv[1] if len(sys.argv) > 1 else "."
    os.makedirs(out, exist_ok=True)
    for name, (title, boxes) in PROPS.items():
        boxes, size = recentre(boxes)
        write_obj(os.path.join(out, name + ".obj"), title, boxes)
        print("%s %.3f x %.3f x %.3f" % (name, *size))


if __name__ == "__main__":
    main()
