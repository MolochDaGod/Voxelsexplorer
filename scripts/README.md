# Scripts

| Command | Role |
|---------|------|
| `node scripts/vox-fleet.mjs validate [dir]` | MagicaVoxel `VOX ` magic + version ≥ 150 (no C#) |
| `node scripts/vox-fleet.mjs convert file.vox --height 1.1` | Delegates to ObjectStore `vox2glb` |

Voxlap / `.qb` validate needs the C# CLI (`--validate`).  
Do not add a second GLB baker here.