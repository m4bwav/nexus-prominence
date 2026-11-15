"""Tint the seamless surface tiles for Nexus Prominence (round 3, build zm).

    python make_textures.py <raw render dir> <SceneTools>/Assets/Templates/NexusProminence/Tex

Recipe (full-size sources stay in the lab, madnesswright-lab/nexus/tex-zm, never in this repo):
1. comfyui-gen comfy_gen.py, Z-Image Turbo (Apache-2.0; never Krea 2), 1024x1024, 9 steps, cfg 1,
   workflow = building-sprite template with node 4 text replaced by the prompt alone. Seeds 3101 (wall), 3102 (floor).
   Prompts: "flat 2D cartoon texture, front view of an industrial wall made of large rectangular metal panels,
   thin black ink outlines on every panel seam, ..." and "... top-down view of an industrial concrete floor of
   large square slabs ..." (full text in tex-zm/RECIPE.md in the lab).
2. crop each raw render to whole panels or slabs (the boxes below), which tiles cleanly; polywright
   pw.py texture --seamless blends offset copies and ghosts a doubled grid on this kind of pattern.
3. this script: keep the drawn detail (luminance around its mean, contrast squeezed) and lay it over the map's
   own flat colours, so the gray-and-red look and the black outline shader stay as they were.
Each output is 512 px, about 0.7 MB in the asset bundle at most.
"""
import os
import sys

from PIL import Image, ImageOps

# name: (source, crop box in the 1024 px render, base colour as in NexusProminenceBuilder.MakeMaterials, contrast)
TILES = {
    "tile_wall": ("wall_raw.png", (340, 340, 1016, 1016), (0.25, 0.20, 0.21), 0.9),
    "tile_panel": ("wall_raw.png", (340, 340, 1016, 1016), (0.31, 0.26, 0.27), 0.9),
    "tile_floor": ("floor_raw.png", (339, 267, 1022, 767), (0.34, 0.29, 0.30), 1.3),
    "tile_asphalt": ("floor_raw.png", (339, 267, 1022, 767), (0.22, 0.21, 0.22), 1.0),
}


def tint(src, box, base, contrast, size=512):
    g = ImageOps.grayscale(Image.open(src).convert("RGB").crop(box)).resize((size, size), Image.LANCZOS)
    px = list(g.tobytes())
    mean = sum(px) / len(px)
    out = []
    for v in px:
        k = max(0.0, 1.0 + contrast * (v - mean) / 255.0)
        out.append(tuple(min(255, int(c * 255 * k)) for c in base))
    im = Image.new("RGB", (size, size))
    im.putdata(out)
    return im


def main():
    src, dst = sys.argv[1], sys.argv[2]
    os.makedirs(dst, exist_ok=True)
    for name, (f, box, base, con) in TILES.items():
        p = os.path.join(dst, name + ".png")
        tint(os.path.join(src, f), box, base, con).save(p)
        print(p)


if __name__ == "__main__":
    main()
