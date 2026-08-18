using System;
using System.IO;

namespace Voxels {
    public sealed class VoxelValidateResult {
        public bool Ok;
        public string Path;
        public string Format;
        public int Version;
        public XYZ Size;
        public int VoxelCount;
        public int PaletteColors;
        public string Error;
        public long Bytes;
    }

    /// <summary>Magic + flatten check. Does not write player data.</summary>
    public static class VoxelValidate {
        public static VoxelValidateResult Check(string path) {
            var r = new VoxelValidateResult { Path = path };
            try {
                if (!File.Exists(path)) {
                    r.Error = "missing file";
                    return r;
                }
                r.Bytes = new FileInfo(path).Length;
                using (var stream = File.OpenRead(path)) {
                    var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
                    if (ext == ".qb") {
                        var qb = QbFile.ReadAndFlatten(stream);
                        if (qb == null) { r.Error = "qb parse failed"; return r; }
                        r.Format = "qubicle-qb";
                        r.Ok = true;
                        r.Size = qb.Size;
                        r.VoxelCount = qb.Count;
                        return r;
                    }
                    var magica = new MagicaVoxel();
                    if (magica.Read(stream)) {
                        var flat = magica.Flatten();
                        r.Format = "magica";
                        r.Version = (int)magica.Version;
                        r.Ok = flat != null && flat.Count > 0;
                        r.Size = flat != null ? flat.Size : XYZ.Zero;
                        r.VoxelCount = flat != null ? flat.Count : 0;
                        r.PaletteColors = magica.Palette != null ? magica.Palette.Length : 0;
                        if (!r.Ok) r.Error = "empty flatten";
                        return r;
                    }
                    stream.Seek(0, SeekOrigin.Begin);
                    var voxlap = Voxlap.Read(stream);
                    if (voxlap != null) {
                        r.Format = "voxlap";
                        r.Ok = voxlap.Count > 0;
                        r.Size = voxlap.Size;
                        r.VoxelCount = voxlap.Count;
                        if (!r.Ok) r.Error = "empty voxlap";
                        return r;
                    }
                }
                r.Error = "unrecognized voxel file";
            }
            catch (Exception ex) {
                r.Error = ex.Message;
            }
            return r;
        }

        public static string ToLine(VoxelValidateResult r) {
            if (!r.Ok) return $"FAIL\t{r.Path}\t{r.Error}";
            return $"OK\t{r.Format}\tv{r.Version}\t{r.Size.X}x{r.Size.Y}x{r.Size.Z}\tvoxels={r.VoxelCount}\t{r.Bytes}b\t{r.Path}";
        }
    }
}
