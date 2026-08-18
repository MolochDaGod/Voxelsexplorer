#!/usr/bin/env node
/**
 * Fleet helper — validate MagicaVoxel .vox without C#.
 * Convert stays ObjectStore vox2glb (do not invent a second baker).
 *
 *   node scripts/vox-fleet.mjs validate [dir]
 *   node scripts/vox-fleet.mjs convert file.vox --height 1.1
 */
import { execFileSync } from "node:child_process";
import { existsSync, readdirSync, readFileSync, statSync } from "node:fs";
import { dirname, extname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const root = resolve(dirname(fileURLToPath(import.meta.url)), "..");
const cmd = process.argv[2] || "validate";
const target = resolve(process.argv[3] || join(root, "Voxels.CommandLine"));

function walkVox(dir, out = []) {
  if (!existsSync(dir)) return out;
  const st = statSync(dir);
  if (st.isFile()) {
    if (/\.vox$/i.test(dir)) out.push(dir);
    return out;
  }
  for (const name of readdirSync(dir)) {
    if (name === "node_modules" || name === ".git") continue;
    walkVox(join(dir, name), out);
  }
  return out;
}

function validateOne(path) {
  const buf = readFileSync(path);
  if (buf.length < 8 || buf.toString("ascii", 0, 4) !== "VOX ") {
    return { ok: false, path, error: "missing VOX magic (or Voxlap — use C# --validate)" };
  }
  const version = buf.readInt32LE(4);
  return { ok: version >= 150, path, version, bytes: buf.length, error: version < 150 ? "version < 150" : null };
}

if (cmd === "validate") {
  const files = walkVox(target);
  let fail = 0;
  for (const f of files) {
    const r = validateOne(f);
    if (!r.ok) {
      fail++;
      console.log(`FAIL\t${f}\t${r.error}`);
    } else {
      console.log(`OK\tv${r.version}\t${r.bytes}b\t${f}`);
    }
  }
  if (!files.length) console.log("no .vox files under", target);
  process.exit(fail ? 1 : 0);
}

if (cmd === "convert") {
  const file = target;
  const heightIdx = process.argv.indexOf("--height");
  const height = heightIdx >= 0 ? process.argv[heightIdx + 1] : "1.1";
  const convertCli = resolve(
    process.env.USERPROFILE || "",
    "Documents",
    "ObjectStore",
    "tools",
    "grudge-convert",
  );
  const alt = "F:\\GitHub\\ObjectStore\\tools\\grudge-convert";
  const cwd = existsSync(join(convertCli, "package.json"))
    ? convertCli
    : existsSync(join(alt, "package.json"))
      ? alt
      : null;
  if (!cwd) {
    console.error("ObjectStore grudge-convert not found — convert is fleet SSOT, not this repo.");
    process.exit(2);
  }
  const out = file.replace(/\.vox$/i, ".glb");
  execFileSync(
    "npm",
    ["run", "convert", "--", "vox2glb", file, "-o", out, "--height", String(height)],
    { cwd, stdio: "inherit", shell: true },
  );
  process.exit(0);
}

console.error("usage: node scripts/vox-fleet.mjs validate|convert …");
process.exit(2);
