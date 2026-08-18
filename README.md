# Voxelsexplorer

Windows Explorer thumbnails + CLI for MagicaVoxel / Qubicle / Voxlap.  
Fork: [MolochDaGod/Voxelsexplorer](https://github.com/MolochDaGod/Voxelsexplorer) · upstream [Arlorean/Voxels](https://github.com/Arlorean/Voxels).

**This is ingest, not a playable game.** Play GLBs, Railway bag, and GRUDOX explorer stay in fleet SSOT.

| Doc | What |
|-----|------|
| [docs/FLEET_VOX_SSOT.md](docs/FLEET_VOX_SSOT.md) | CDN / Railway / convert law |
| [docs/GAPS.md](docs/GAPS.md) | Gaps → solutions |

## Clone

```text
git clone --recurse-submodules https://github.com/MolochDaGod/Voxelsexplorer.git
cd Voxelsexplorer
# if you already cloned without submodules:
git submodule update --init --recursive
```

## Validate + convert

```text
# Magic check (no C#):
node scripts/vox-fleet.mjs validate Voxels.CommandLine

# After building Voxels.CommandLine:
Voxels.CommandLine.exe --validate wizard.vox
Voxels.CommandLine.exe --obj wizard.vox

# Production GLB (fleet baker — not this sln):
cd path/to/ObjectStore
npm run convert -- vox2glb path/to/file.vox -o dist/file.glb --height 1.1
```

### CommandLine flags

| Flag | Meaning |
|------|---------|
| *(default)* | PNG + SVG |
| `--png` `--svg` `--gif` | Raster outputs |
| `--obj` | Wavefront OBJ, Y-up (same axis as `vox2glb`) |
| `--validate` | Magic + flatten; exit 1 on fail |
| `--vox` | PNG → `.vox` |
| `--3D` | Unity 3D texture atlas PNG |
| `-w` `-y` `-x` | Size / yaw / pitch |
| `--recursive` | Walk directories |

Never load raw `.vox` in the browser. Player roster / bag = Railway (`?era=voxel`), not D1.

The [Voxels.Setup.exe](https://github.com/Arlorean/Voxels/releases/latest) provides Windows Explorer Thumbnails for:
- [MagicaVoxel](https://ephtracy.github.io/) [**.vox** files](https://github.com/ephtracy/voxel-model/blob/master/MagicaVoxel-file-format-vox.txt)
- [Voxlap Engine](http://advsys.net/ken/voxlap.htm) [**.vox** files](http://advsys.net/ken/build.htm)
- [Qubicle](http://minddesk.com) [**.qbcl** project files](http://minddesk.com/learn/article.php?id=100)
- [Qubicle](http://minddesk.com) [**.qb** binary files](http://minddesk.com/learn/article.php?id=22)

Here are the MagicaVoxel sample file thumbnails:
![Windows Explorer Thumbnails](Voxels.Website/WindowsExplorer.png)

Here are some of [Mike Judge's mmmm](https://mikelovesrobots.github.io/mmmm/) collections file thumbnails:
![mmmm Thumbnails](Voxels.Website/mmmm.png)

Special thanks to Voxel Artist [Zachary Soares](https://www.zsinked.com/) for a screenshot of his [Qubicle](http://minddesk.com) project file thumbnails:
![ZacharySoares Thumbnails](Voxels.Website/ZacharySoares.png)

The library uses [SkiaSharp](https://github.com/mono/SkiaSharp#using-skiasharp) which requires [Visual C++ Redistributable for Visual Studio 2015](https://www.microsoft.com/en-us/download/details.aspx?id=48145) to be installed. The exe setup does this for you.

# PNG and SVG output

The Voxels.CommandLine.exe tool converts .vox files to .png and .svg (512x512). Here is my example [wizard.vox](Voxels.CommandLine/wizard.vox) file converted:

PNG             |  SVG
----------------|-------------------------
![PNG](Voxels.Website/wizard.png)  |  ![SVG](https://cdn.rawgit.com/Arlorean/Voxels/df6f605a/Voxels.Website/wizard.svg)

# Command Line Build

1. Install Visual Studio **2022** (or 2019) with .NET desktop + .NET Framework 4.8
1. `git submodule update --init --recursive`
1. Open ``Voxels.sln``
1. Set ``Voxels.CommandLine`` as the startup project
1. Arguments: ``wizard.vox`` or ``--validate wizard.vox`` or ``--obj wizard.vox``
1. Press Start
1. Output: ``wizard.png`` / ``wizard.svg`` (default), or ``wizard.obj`` with ``--obj``

# Setup Build

1. Visual Studio **2022** + WiX v3.11 (or current WiX that still builds this wixproj)
1. Open ``Voxels.sln``
1. Restore NuGet (SkiaSharp 1.59)
1. Build ``Voxels.Setup`` for the installer exe (upstream release still listed below) 

# Third Party Credits

1. [SkiaSharp](https://github.com/mono/SkiaSharp) - Xamarin C# wrapper for Google's Skia 2D rendering library
1. [SharpShell](https://github.com/dwmkerr/sharpshell) - Dave Kerr's ShellExtensions Library for .NET
1. [SharpShellTools](https://github.com/dwmkerr/sharpshell) - Dave Kerr's ShellExtensions Tools for .NET
1. [WiX Toolset](http://wixtoolset.org/) - ~~Simple~~ XML based windows installer scripting
1. [Ambient Occlusion](https://0fps.net/2013/07/03/ambient-occlusion-for-minecraft-like-worlds/) algorithm by [Mikola Lysenko](https://github.com/mikolalysenko).
1. [Wix VC++ 2015 Setup](https://gist.github.com/nathancorvussolis/6852ba282647aeb0c5c00e742e28eb48) gist for installing VC++ 2015 dlls.

The 3x3x3.vox, 8x8x8.vox files are directly from the [MagicaVoxel](https://ephtracy.github.io/) distribution for authentic testing.

# TODO

* Add shell context menus to export PNG/SVG interactively 
