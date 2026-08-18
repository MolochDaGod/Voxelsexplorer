# Production convert verify (ran 2026-08-18)

**Tool:** `ObjectStore/tools/grudge-convert` `vox2glb` → glTF-Transform optimize + `--height` + y-hip ground.  
**Doctor:** node 24, FBX2glTF, Blender, sharp — backends ready.  
**Not assumed:** every file below was converted and scored from `*.manifest.json` AABB (written **before** meshopt quantize).

## Verdict

| Set | Files | Result |
|-----|-------|--------|
| Explorer samples | 9 | **PASS** — `glTF` magic, feet `min.y=0`, height as requested |
| Blacksmith pack | 14 | **PASS** — 1.1 m props |
| `Spaceships.vox` fused | 1 | **CHECK** — native height **28 m** (1 cell=1 m). Forced `--height 6` is a squash, not isolate. Do **not** ship as one play mesh. |

Raw `.vox` is **not** browser-ready. These `.glb` are.

## Measured (manifest AABB)

| Asset | Baked H (m) | min.y | GLB bytes | Flag |
|-------|-------------|-------|-----------|------|
| wizard | 1.8 | 0 | 32160 | character sample |
| cars | 1.6 | 0 | 109928 | vehicle sample |
| 1x1x1 … 8x8x8, Axes | 1.1 | 0 | 3–35 KB | test voxels |
| anvil … workbench (14) | 1.1 | 0 | 26–122 KB | blacksmith props |
| Spaceships `--height 6` | 6.0 | 0 | 848 KB | fused pack — isolate before play |
| Spaceships native | 28.0 | 0 | 847 KB | too tall as one entity |

## Browser pack

Each PASS file is `glTF` binary, 1 mesh, vertex colors, no textures, meshopt-quantized, `.collider.json` + `.manifest.json` beside it.

Collider extras still stamp `character_root` capsules on **props** — use the **box** halfExtents for benches/anvils, not the 0.35 r hero capsule.

## Reproduce

```text
cd F:\GitHub\ObjectStore\tools\grudge-convert
node ./bin/grudge-convert.mjs doctor
node ./bin/grudge-convert.mjs vox2glb path\to\file.vox -o out.glb --height 1.1
node ./bin/grudge-convert.mjs inspect out.glb
node ./bin/grudge-convert.mjs check-vox voxDir -o glbDir
```

Outputs on this machine: `F:\GitHub\Voxelsexplorer\dist\prod\`
