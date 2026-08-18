# Fleet use of Voxelsexplorer (MagicaVoxel ingest)

This repo is **Windows Explorer thumbnails + CLI raster/mesh for `.vox` / `.qb`**.  
It is **not** GRUDOX play, not a second bag DB, not a second Three.js voxel engine.

## What this owns vs fleet

| Layer | Owner | Not this repo |
|-------|--------|----------------|
| Parse / preview `.vox` `.qb` | **Voxels.Core** + CommandLine | — |
| Production GLB + SI height | ObjectStore **`vox2glb`** (`grudge-asset-convert`) | Do not add a second GLB baker here |
| Binaries | `assets.grudge-studio.com` R2 | — |
| Asset index | D1 `asset_registry` | — |
| Player / bag / heroes | Railway `/api/characters` `/api/account/*` | Never store roster in D1 or local JSON |
| Play explorer | GRUDOX `TvsExplorerRig` + Mixamo | Do not ship Toon/grudge6 as voxel hero |

## Convert (validated)

```text
# 1. Validate magic + flatten (this repo)
Voxels.CommandLine.exe --validate pack/*.vox

# 2. Optional OBJ (Y-up) for inspect
Voxels.CommandLine.exe --obj wizard.vox

# 3. Production GLB (fleet SSOT)
cd ObjectStore
npm run convert -- vox2glb path/to/anvil.vox -o dist/anvil.glb --height 1.1
# then R2 put + D1 register (grudge-d1-r2)
```

Node helper (no C#): `node scripts/vox-fleet.mjs validate Voxels.CommandLine`

## API / database

| Need | Call |
|------|------|
| Login | `id.grudge-studio.com` JWT |
| Roster | `GET /api/characters?era=voxel` |
| Progress / attrs | `POST /api/characters/:uuid/progress` |
| Bag / mats | `/api/account/resources` |
| Unique items | `/api/uuid/*` + ledger |

Voxel **meshes** are CDN keys. Voxel **ownership** is Railway.

## Game improvements (GRUDOX, not this sln)

- Explorer: one mixer, dodge Q / dash V (`voxel-explorer-combat-hud`)
- Physics: Rapier only
- Ground: one `getGroundY` / BVH
- Convert every author `.vox` before play — never load raw VOX in the browser
