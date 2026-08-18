# Audit — missed / should already use (2026-08-18)

Ran after VERIFY_RUN. No assumptions: HEAD/GET on live CDN, catalog files, leftover author formats.

## Should already use (do not invent paths)

| Existing SSOT | Status | Note |
|---------------|--------|------|
| `https://assets.grudge-studio.com/models/voxel/content/anvil.glb` | **LIVE** 17 024 328 B `model/gltf-binary` | Open catalog `voxel-last30` `r2Key` |
| Other `models/voxel/content/*` in that catalog | 8/9 HEAD OK | See below |
| `bench-mesh-catalog.json` **uMMORPG benches preferred** | **LIVE** legion-workbench 8 644 724 B | “prefer over voxel GLB props” |
| Railway characters / bag | unchanged | Do not put roster in this repo |
| `vox2glb` + `--height` | used | Now wired in CLI |
| GRUDOX `TvsExplorerRig` | play | Still uses `buildVoxChar` fallback — our bakes are **not** wired into play |

## Live `models/voxel/content` HEAD

| Key | Result |
|-----|--------|
| `anvil.glb` | OK (17 MB) |
| `queen_annes_revenge.glb` | OK |
| `desert_portal.glb` | OK |
| `brick_modular_kit.glb` | OK |
| `xyz_buildings_draft.glb` | OK |
| `warning_bell.glb` | OK |
| `balloons.glb` | OK |
| `t0_crossbow.glb` | OK |
| `phantom_seal_altar.glb` (**r2Key**) | **OK 200** |
| `phantom_in_seal_altar.glb` (catalog `src`) | **404** — typo vs r2Key |

Catalog row `anvil_station` lists **16.24 MB**. Our MagicaVoxel bake is **25 772 B**. Different mesh. **Use the catalog URL for Open voxels.** Slim bake is a fallback, not a silent overwrite of the 17 MB object.

## We missed

| Miss | Why it matters | Next (existing tools) |
|------|----------------|------------------------|
| **R2 put / D1 register** | Local `dist/prod` is not production until CDN | `grudge-d1-r2` + deploy.md — only if we *intend* to replace catalog keys |
| **Nerds.qb / Nerds.qbcl** | Author formats in this repo; `vox2glb` is `.vox` only | C# `VoxelImport` + `--obj`, or skip (sample art) |
| **C# CLI not built** | NuGet SkiaSharp missing on this machine | Restore + `--validate` / `--obj` |
| **`--draco`** | Optional smaller GLB | `vox2glb … --draco` if decoders already in play loaders |
| **Spaceships isolate** | 28 m fused pack | Isolate meshes before `--height` |
| **Prop `character_root` capsule** | Convert stamps hero CCT on benches | Use manifest **box** for props |
| **GRUDOX play** | Still procedural cubes | Load catalog CDN GLBs; do not invent a second explorer |
| **uMMORPG benches** | Warlords craft stations | Prefer `legion-workbench` / fabled over voxel anvil |
| **CLI help text** | Doctor lists `vox2glb` via NAMED; CONVERSIONS blurb still omits `.vox` | Copy-edit help |
| **`D:\Games\Models`** | Not scanned | Out of this repo’s set |
| **Fat CDN anvil (17 MB)** | Browser-heavy vs 26 KB bake | `glb2glb` the **live** file if we keep that art; don’t swap keys blindly |
| **Inspect vs meshopt** | Raw POSITION ints look like 65k m | Trust `manifest.json` AABB / `grudgeColliders` |

## Do not

- Invent `models/blacksmith/anvil.glb` (those HEADs 404)
- Replace live 17 MB anvil with the 26 KB MagicaVoxel bake without an explicit product call
- Store player bag on D1
- Treat Voxelsexplorer as a playable GRUDOX game
