#!/usr/bin/env python3
# =============================================================================
# generate_projections.py — DISABLED (2026-08-09)
#
# 平衡 v0.3.4 之后，所有投影卡都是手改的（削弱数值 + 无升级 + 部分重写 OnPlay），
# 与原卡完全分叉。再跑本脚本会把所有手改投影覆盖回「原卡副本」，导致投影效果错乱，
# 因此整段代码已注释停用。
#
# 若未来需要新增卡的投影，请先把手改投影文件加入 keep 集合，
# 再基于 git 历史中的原版重写本脚本：
#   git show HEAD:scripts/generate_projections.py
# =============================================================================
# 原代码（已注释，仅存档参考）：
# #!/usr/bin/env python3
# """Generate projection cards + registry + localization for all concrete cards.

# Each projection card is a FULL COPY of its original card's effect code (CanonicalVars,
# OnPlay, OnUpgrade, portrait, fields, helpers — everything except the constructor), so
# effects can diverge per projection later without touching the original.

# Emitted for each non-GoldenSlash card:
#   - core/cards/projection/Projection<X>.cs
# Plus:
#   - core/cards/projection/ProjectionRegistry.cs  (original -> projection map, generic fallback)
#   - projection entries appended to localization/{eng,zhs}/cards.json

# GoldenSlash* cards are emitted as stubs (their shared chain logic lives in
# GoldenSlashBase and is hand-written in the projection files).

# Run after adding/removing cards:
#     python scripts/generate_projections.py
# """

# import json
# import os
# import re
# import sys

# ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
# CARDS_DIR = os.path.join(ROOT, "core", "cards")
# OUT_DIR = os.path.join(CARDS_DIR, "projection")
# LOC_DIR = os.path.join(ROOT, "FateNightOfTheGalacticRailway", "localization")
# CARDS_NS = "FateNightOfTheGalacticRailway.Core.Cards"
# PROJ_NS = CARDS_NS + ".Projection"
# MOD_PREFIX = "FATENIGHTOFTHEGALACTICRAILWAY-"

# SUFFIX = {"eng": " Exhaust.", "zhs": "消耗。"}
# TITLE_PREFIX = {"eng": "Pseudo ", "zhs": "（伪）"}

# class_re = re.compile(r"^public\s+class\s+(\w+)\s*:\s*(\w+)", re.MULTILINE)
# ns_re = re.compile(r"^using\s+[^;]+;", re.MULTILINE)


# def parse_ctor_args(text, class_name):
#     """Parse `public X() : base(...)` into `CardType.A, CardRarity.B, TargetType.C`."""
#     m = re.search(rf"public\s+{class_name}\s*\(\s*\)\s*:\s*base\(([^)]*)\)", text)
#     if not m:
#         return None
#     type_ = rarity = target = None
#     for p in m.group(1).split(","):
#         p = p.strip()
#         if p.startswith("CardType."):
#             type_ = p.split(".")[1]
#         elif p.startswith("CardRarity."):
#             rarity = p.split(".")[1]
#         elif p.startswith("TargetType."):
#             target = p.split(".")[1]
#     if type_ and rarity and target:
#         return f"CardType.{type_}, CardRarity.{rarity}, TargetType.{target}"
#     if target and not type_ and not rarity:  # GoldenSlashBase subclasses
#         return f"CardType.Attack, CardRarity.Uncommon, TargetType.{target}"
#     return None


# def find_cards():
#     """Return list of (class_name, base_name, ctor_args). GoldenSlash* marked as stubs."""
#     cards = []
#     for fname in sorted(os.listdir(CARDS_DIR)):
#         if not fname.endswith(".cs"):
#             continue
#         path = os.path.join(CARDS_DIR, fname)
#         with open(path, encoding="utf-8") as f:
#             text = f.read()
#         for m in class_re.finditer(text):
#             cls, base = m.groups()
#             if cls in {"GoldenSlashBase", "GoldenSlashTracker"}:
#                 continue
#             if base not in {"CustomCardModel", "GoldenSlashBase"}:
#                 continue
#             args = parse_ctor_args(text, cls)
#             if args is None:
#                 print(f"  WARN: could not parse ctor of {cls} ({fname})")
#                 continue
#             cards.append((cls, base, args))
#     seen = set()
#     return [c for c in cards if not (c[0] in seen or seen.add(c[0]))]


# def extract_class_body(text, class_name):
#     """Return the text between the class's opening `{` and its matching `}`."""
#     m = re.search(rf"public\s+class\s+{class_name}[^{{]*\{{", text)
#     if not m:
#         return None
#     i = m.end() - 1  # position of the `{`
#     depth = 0
#     n = len(text)
#     while i < n:
#         if text[i] == '{':
#             depth += 1
#         elif text[i] == '}':
#             depth -= 1
#             if depth == 0:
#                 return text[m.end():i]
#         i += 1
#     return None


# def split_members(body):
#     """Split a class body into top-level members (handles {..} blocks and `;` lines)."""
#     members = []
#     i, n = 0, len(body)
#     while i < n:
#         while i < n and body[i] in ' \t\r\n':
#             i += 1
#         if i >= n:
#             break
#         start = i
#         depth = 0
#         while i < n:
#             c = body[i]
#             if c == '{':
#                 depth += 1
#             elif c == '}':
#                 depth -= 1
#                 if depth == 0:
#                     i += 1
#                     j = i
#                     while j < n and body[j] in ' \t\r\n':
#                         j += 1
#                     if j < n and body[j] == ';':
#                         i = j + 1
#                     break
#             elif c == ';' and depth == 0:
#                 i += 1
#                 break
#             i += 1
#         members.append(body[start:i].strip())
#     return members


# def gen_projection_file(name, base, ctor_args, src_text, is_stub=False):
#     usings = ns_re.findall(src_text)
#     extra = [
#         "using BaseLib.Abstracts;",
#         "using MegaCrit.Sts2.Core.Entities.Cards;",
#         f"using {CARDS_NS};",
#     ]
#     for u in extra:
#         if u not in usings:
#             usings.append(u)

#     lines = usings + ["", f"namespace {PROJ_NS};", "",
#                       "// This file is auto-generated by scripts/generate_projections.py.",
#                       f"/// <summary>（伪）{name} — projection (weakened) copy of {name}.</summary>",
#                       "[Pool(typeof(WeakenedCardPool))]",
#                       f"public class Projection{name} : ProjectionCardBase",
#                       "{",
#                       f"    public Projection{name}() : base({ctor_args}) {{ }}",
#                       ""]
#     if is_stub:
#         lines += [
#             "    // TODO(hand-written): GoldenSlash chain logic lives in GoldenSlashBase;",
#             "    // this stub will be replaced with a simplified effect.",
#         ]
#     else:
#         body = extract_class_body(src_text, name) or ""
#         for member in split_members(body):
#             if re.match(rf"^public\s+{name}\s*\(", member):  # skip the ctor
#                 continue
#             # ProjectionCardBase already declares [Exhaust, 投影]; copying the
#             # original's CanonicalKeywords would override and drop them.
#             if "public override IEnumerable<CardKeyword> CanonicalKeywords" in member:
#                 continue
#             lines.append("    " + member)
#     lines.append("}")
#     lines.append("")
#     return "\n".join(lines)


# def gen_registry(cards):
#     lines = [
#         "using System;",
#         "using System.Collections.Generic;",
#         f"using {CARDS_NS};",
#         "",
#         f"namespace {PROJ_NS};",
#         "",
#         "// This file is auto-generated by scripts/generate_projections.py.",
#         "/// <summary>Maps each original card type to its projection card type.",
#         "/// Cards without a projection map to the generic 伪卡牌.</summary>",
#         "public static class ProjectionRegistry",
#         "{",
#         "    private static readonly Dictionary<Type, Type> _map = new()",
#         "    {",
#     ]
#     for name, _, _ in cards:
#         lines.append(f"        [typeof({name})] = typeof(Projection{name}),")
#     lines += [
#         "    };",
#         "",
#         "    /// <summary>Projection type for an original card, or the generic 伪卡牌 fallback.</summary>",
#         "    public static Type GetProjectionType(Type originalType) =>",
#         "        _map.TryGetValue(originalType, out Type? t) ? t : typeof(GenericProjectionCard);",
#         "}",
#         "",
#     ]
#     return "\n".join(lines)


# def normalize(name):
#     s = re.sub(r"(?<!^)(?=[A-Z])", "_", name)
#     return s.upper()


# def gen_loc(cards):
#     for lang, path in (("eng", os.path.join(LOC_DIR, "eng", "cards.json")),
#                        ("zhs", os.path.join(LOC_DIR, "zhs", "cards.json"))):
#         if not os.path.exists(path):
#             print(f"  WARN: missing {path}, skipping {lang}")
#             continue
#         with open(path, encoding="utf-8") as f:
#             loc = json.load(f)
#         added, missing = 0, []
#         for name, _, _ in cards:
#             key = normalize(name)
#             proj = "PROJECTION_" + key
#             t = loc.get(f"{MOD_PREFIX}{key}.title")
#             if t is None:
#                 missing.append(name)
#                 continue
#             loc[f"{MOD_PREFIX}{proj}.title"] = TITLE_PREFIX[lang] + t
#             for field, base in (("description", loc.get(f"{MOD_PREFIX}{key}.description", "")),
#                                 ("upgradedDescription", loc.get(f"{MOD_PREFIX}{key}.upgradedDescription", "")),
#                                 ("smartDescription", loc.get(f"{MOD_PREFIX}{key}.smartDescription", ""))):
#                 if base:
#                     text = base.rstrip()
#                     if lang == "zhs":
#                         if not text.endswith("。"):
#                             text += "。"
#                         text += SUFFIX[lang]
#                     else:
#                         text += SUFFIX[lang]
#                     loc[f"{MOD_PREFIX}{proj}.{field}"] = text
#             added += 1
#         with open(path, "w", encoding="utf-8", newline="\n") as f:
#             json.dump(loc, f, indent=2, ensure_ascii=False)
#             f.write("\n")
#         if missing:
#             print(f"  WARN ({lang}): no localization entry for: {', '.join(missing)}")
#         print(f"  {lang}: added {added} projection entries -> {path}")


# def main():
#     cards = find_cards()
#     if not cards:
#         print("No cards found!")
#         return 1
#     os.makedirs(OUT_DIR, exist_ok=True)

#     # Clear old generated per-card files (keep hand-written ones: ProjectionCardBase,
#     # GenericProjectionCard, and the hand-written GoldenSlash stubs).
#     keep = {"ProjectionCardBase.cs", "ProjectionUtil.cs", "GenericProjectionCard.cs",
#             "ProjectionGoldenSlash1.cs", "ProjectionGoldenSlash2.cs", "ProjectionGoldenSlash3.cs"}
#     for fname in os.listdir(OUT_DIR):
#         if fname not in keep and fname.startswith("Projection") and fname.endswith(".cs"):
#             os.remove(os.path.join(OUT_DIR, fname))

#     for name, base, args in cards:
#         if base == "GoldenSlashBase":
#             continue  # GoldenSlash projections are hand-written (chain logic in GoldenSlashBase)
#         src = os.path.join(CARDS_DIR, f"{name}.cs")
#         with open(src, encoding="utf-8") as f:
#             text = f.read()
#         content = gen_projection_file(name, base, args, text)
#         with open(os.path.join(OUT_DIR, f"Projection{name}.cs"), "w", encoding="utf-8", newline="\n") as f:
#             f.write(content)

#     with open(os.path.join(OUT_DIR, "ProjectionRegistry.cs"), "w", encoding="utf-8", newline="\n") as f:
#         f.write(gen_registry(cards))

#     print(f"Generated {len(cards)} projection cards:")
#     print("  " + ", ".join(name for name, _, _ in cards))
#     print("Localization:")
#     gen_loc(cards)
#     return 0


# if __name__ == "__main__":
#     sys.exit(main())
