using System.Globalization;
using System.IO;
using System.Text;

namespace Voxels {
    /// <summary>
    /// Wavefront OBJ from <see cref="MeshBuilder"/> (culled faces).
    /// MagicaVoxel Z-up → OBJ Y-up (x, z, −y) — same as fleet vox2glb.
    /// 1 cell = 1 unit; SI height is applied by grudge-convert --height.
    /// </summary>
    public static class ObjExport {
        public static string ToObj(VoxelData voxelData, string objectName = "vox") {
            var mesh = new MeshBuilder(voxelData, new MeshSettings {
                Yaw = 45f,
                Pitch = -26f,
                MeshType = MeshType.Triangles,
                FakeLighting = false,
                FloorShadow = false,
            });
            var sb = new StringBuilder();
            sb.AppendLine("# Voxels.Core OBJ — MagicaVoxel Z-up → Y-up");
            sb.AppendLine("o " + Sanitize(objectName));
            var inv = CultureInfo.InvariantCulture;
            for (var i = 0; i < mesh.Vertices.Length; i++) {
                var v = mesh.Vertices[i];
                sb.AppendFormat(inv, "v {0} {1} {2}\n", v.X, v.Z, -v.Y);
            }
            for (var i = 0; i < mesh.Normals.Length; i++) {
                var n = mesh.Normals[i];
                sb.AppendFormat(inv, "vn {0} {1} {2}\n", n.X, n.Z, -n.Y);
            }
            var faces = mesh.Faces;
            for (var i = 0; i + 2 < faces.Length; i += 3) {
                var a = faces[i] + 1;
                var b = faces[i + 1] + 1;
                var c = faces[i + 2] + 1;
                sb.AppendFormat(inv, "f {0}//{0} {1}//{1} {2}//{2}\n", a, b, c);
            }
            return sb.ToString();
        }

        public static void Write(VoxelData voxelData, string path) {
            File.WriteAllText(path, ToObj(voxelData, Path.GetFileNameWithoutExtension(path)));
        }

        static string Sanitize(string name) {
            if (string.IsNullOrWhiteSpace(name)) return "vox";
            var sb = new StringBuilder();
            foreach (var ch in name) {
                sb.Append(char.IsLetterOrDigit(ch) || ch == '_' ? ch : '_');
            }
            return sb.Length > 0 ? sb.ToString() : "vox";
        }
    }
}
