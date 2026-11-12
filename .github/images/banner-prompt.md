# Banner prompt

- Model: Z-Image Turbo (`z_image_turbo_bf16.safetensors`, CLIP `qwen_3_4b.safetensors` type lumina2, VAE `ae.safetensors`)
- Host: PC 127.0.0.1:8199 (ComfyUI)
- Workflow: comfyui-gen `building-sprite-workflow.api.json` with node 4 text set to the prompt alone and node 6 set to 1536x640
- Sampler: KSampler, 9 steps, cfg 1.0, `res_multistep`, scheduler `simple`, denoise 1.0, empty negative
- Seed: 2201 (picked from four seeds, 2201-2204, on a 2x2 contact sheet)
- Output: banner.jpg 1536x640 JPEG q88; social-preview.jpg is the 1280x640 centre crop
- Generated 2026-11-11. Original imagery; no official MADNESS Combat or Project Nexus art.

## Prompt

Wide cutaway cross-section illustration of a gritty industrial facility built from stacked bare grey concrete rooms, like a dollhouse view, comic style with bold black outlines and flat grey shading. Red and black diagonal hazard stripes band the walls and doorways. Bottom left a large garage room with a half-open corrugated roller shutter door, tapered concrete pylons and a bulky diesel generator with cables. Narrow corridors and stairwells connect rooms on two floors, caged ceiling lamps, pipes and vents. A few tiny faceless grey stick-figure men in black suits stand and walk in different rooms. Stark grey palette with red accents, no text, no letters.
