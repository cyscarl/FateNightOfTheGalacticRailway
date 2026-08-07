#!/usr/bin/env python3
"""Update the Dependencies line and dependencies[] of mod_manifest.json files
from the NuGet-resolved package versions in project.assets.json.

Run by MSBuild (UpdateModManifestDependencies target) before the manifest is
copied/exported, so the manifest's dependency info always tracks the csproj's
actual resolved versions (e.g. after bumping the RitsuLib/BaseLib packages).

Usage:
    update_mod_manifest.py <project.assets.json> <manifest.json> [...more manifests]
"""

import json
import os
import re
import sys

RITSU_PKG = "STS2.RitsuLib"
BASELIB_PKG = "Alchyr.Sts2.BaseLib"
RITSU_LABEL = "RitsuLib (STS2-RitsuLib)"
BASELIB_LABEL = "BaseLib"


def read_resolved_versions(assets_path):
    """Return {package_name: version} from the first target framework."""
    if not assets_path or not os.path.exists(assets_path):
        return {}
    with open(assets_path, encoding="utf-8") as f:
        data = json.load(f)
    for deps in data.get("targets", {}).values():
        versions = {}
        for pkg_key in deps:
            name, _, ver = pkg_key.rpartition("/")
            versions[name] = ver
        return versions
    return {}


def main():
    if len(sys.argv) < 3:
        print("usage: update_mod_manifest.py <project.assets.json> <manifest.json> [...]")
        return 1

    assets_path = sys.argv[1]
    manifest_paths = sys.argv[2:]

    versions = read_resolved_versions(assets_path)
    ritsu = versions.get(RITSU_PKG)
    baselib = versions.get(BASELIB_PKG)

    if not ritsu or not baselib:
        print(f"[manifest] WARNING: could not resolve versions (ritsu={ritsu}, baselib={baselib}) "
              f"from {assets_path}; leaving manifests unchanged.")
        return 0

    dep_line = f"Dependencies: {RITSU_LABEL} v{ritsu}, {BASELIB_LABEL} v{baselib}"
    any_change = False

    for path in manifest_paths:
        with open(path, encoding="utf-8") as f:
            doc = json.load(f)

        changed = False

        # Strip any existing dependency suffix, keep the flavor-text base.
        desc = doc.get("description", "")
        base = re.split(r"\n\s*Dependencies:\s*", desc)[0].rstrip()
        new_desc = base + "\n\n" + dep_line
        if new_desc != desc:
            doc["description"] = new_desc
            changed = True

        deps = doc.get("dependencies")
        if deps is None:
            doc["dependencies"] = deps = []
        for dep in deps:
            if dep.get("id") == "STS2-RitsuLib" and dep.get("min_version") != ritsu:
                dep["min_version"] = ritsu
                changed = True
            if dep.get("id") == "BaseLib" and dep.get("min_version") != baselib:
                dep["min_version"] = baselib
                changed = True

        if changed:
            with open(path, "w", encoding="utf-8", newline="\n") as f:
                json.dump(doc, f, indent=2, ensure_ascii=False)
                f.write("\n")
            print(f"[manifest] updated {os.path.relpath(path)}: {dep_line}")
            any_change = True

    if not any_change:
        print("[manifest] dependencies already up to date")
    return 0


if __name__ == "__main__":
    sys.exit(main())
