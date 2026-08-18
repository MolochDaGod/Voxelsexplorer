# Gaps → solutions (Voxelsexplorer + fleet)

**Repo:** https://github.com/MolochDaGod/Voxelsexplorer · parent [README](../README.md)

| Gap | Solution | Status |
|-----|----------|--------|
| Clone without submodules = empty Core | `git submodule update --init --recursive` | Required |
| CLI only PNG/SVG/GIF/Unity 3D tex — no mesh | `--obj` via `MeshBuilder` | Added |
| No file validation | `--validate` + `scripts/vox-fleet.mjs validate` | Added |
| VS2017 docs | VS 2022 + .NET 4.8 still builds; production mesh = Node `vox2glb` | Doc |
| Dual GLB bakers | **One** baker: ObjectStore `vox2glb` + `--height` SI | Law |
| Browser loads raw `.vox` | Convert → R2 GLB; GRUDOX loads CDN | Law |
| Player data in this repo / D1 | Railway character UUID + account bag | Law |
| MagicaVoxel n-TRN scenes | Core already Flatten(); validate after flatten | Use Flatten |
| Shell context menu PNG | Still TODO upstream | Not this pass |
| 100× voxel props | `--height` 1.1 m props / human 1.8 m | SI |
