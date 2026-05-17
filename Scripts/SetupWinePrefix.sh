#!/usr/bin/env bash
#
# One-time setup for building the x86 Windows NativeAOT components
# (Daybreak.Injector, Daybreak.API) on Linux via Wine.
#
# What this installs into the wine prefix (default ~/.wine-daybreak):
#   C:\dotnet  — the .NET SDK for Windows (downloaded zip, no installer)
#   C:\llvm    — LLVM Windows binaries (we use lld-link.exe and llvm-lib.exe)
#   C:\xwin    — MSVC CRT + Windows SDK x86 lib files (extracted by xwin)
#   C:\wine-aot.props — MSBuild properties consumed by PublishLinuxWineX86.sh
#
# Requires on the host: wine (>= 9), curl, unzip, tar, 7z, python3.
# `xwin` is downloaded automatically into the wine prefix if not on PATH.
#
# Re-running this script is safe: it skips components that are already present.
# Use --force to wipe and reinstall.

set -euo pipefail

# ─── configuration ────────────────────────────────────────────────────────────
DOTNET_SDK_CHANNEL="${DOTNET_SDK_CHANNEL:-10.0}"
LLVM_VERSION="${LLVM_VERSION:-22.1.5}"
XWIN_VERSION="${XWIN_VERSION:-0.9.0}"
WINEPREFIX_DEFAULT="$HOME/.wine-daybreak"
export WINEPREFIX="${WINEPREFIX:-$WINEPREFIX_DEFAULT}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$REPO_ROOT"

FORCE=0
for arg in "$@"; do
    case "$arg" in
        --force) FORCE=1 ;;
        -h|--help)
            sed -n '2,18p' "$0"
            exit 0
            ;;
        *) echo "Unknown argument: $arg" >&2; exit 2 ;;
    esac
done

CACHE_DIR="${XDG_CACHE_HOME:-$HOME/.cache}/daybreak-wine-setup"
mkdir -p "$CACHE_DIR"

log()  { printf '\033[1;36m[setup]\033[0m %s\n' "$*"; }
warn() { printf '\033[1;33m[setup]\033[0m %s\n' "$*" >&2; }
die()  { printf '\033[1;31m[setup]\033[0m %s\n' "$*" >&2; exit 1; }

for tool in wine curl unzip tar 7z python3; do
    command -v "$tool" >/dev/null 2>&1 || die "Missing required tool: $tool"
done

if [[ $FORCE -eq 1 && -d "$WINEPREFIX" ]]; then
    log "Removing existing prefix $WINEPREFIX (--force)"
    rm -rf "$WINEPREFIX"
fi

# ─── 1. wine prefix ───────────────────────────────────────────────────────────
if [[ ! -f "$WINEPREFIX/system.reg" ]]; then
    log "Initializing wine prefix at $WINEPREFIX"
    mkdir -p "$WINEPREFIX"
    WINEARCH=win64 wineboot --init >/dev/null 2>&1 || true
    # wait for wineboot to settle
    wineserver -w 2>/dev/null || true
else
    log "Wine prefix already initialized at $WINEPREFIX"
fi

# ─── 2. .NET SDK ──────────────────────────────────────────────────────────────
DOTNET_DIR="$WINEPREFIX/drive_c/dotnet"
if [[ ! -f "$DOTNET_DIR/dotnet.exe" ]]; then
    log "Resolving latest .NET $DOTNET_SDK_CHANNEL SDK"
    SDK_INFO=$(curl -fsSL "https://builds.dotnet.microsoft.com/dotnet/release-metadata/${DOTNET_SDK_CHANNEL}/releases.json" | python3 -c "
import json,sys
d=json.load(sys.stdin)
v=d['latest-sdk']
for r in d['releases']:
    if r['sdk']['version']==v:
        for f in r['sdk']['files']:
            if f['rid']=='win-x64' and f['name'].endswith('.zip'):
                print(v); print(f['url']); sys.exit(0)
sys.exit('No win-x64 SDK zip found')
")
    SDK_VER=$(echo "$SDK_INFO" | sed -n '1p')
    SDK_URL=$(echo "$SDK_INFO" | sed -n '2p')
    SDK_ZIP="$CACHE_DIR/dotnet-sdk-${SDK_VER}-win-x64.zip"

    if [[ ! -f "$SDK_ZIP" ]]; then
        log "Downloading .NET SDK $SDK_VER (~280 MB)"
        curl -fSL --progress-bar -o "$SDK_ZIP.tmp" "$SDK_URL"
        mv "$SDK_ZIP.tmp" "$SDK_ZIP"
    else
        log "Using cached SDK zip: $SDK_ZIP"
    fi

    mkdir -p "$DOTNET_DIR"
    log "Extracting SDK into $DOTNET_DIR"
    (cd "$DOTNET_DIR" && unzip -q "$SDK_ZIP")
else
    log ".NET SDK already present in $DOTNET_DIR"
fi

# ─── 3. LLVM Windows binaries (lld-link.exe, llvm-lib.exe) ───────────────────
LLVM_DIR="$WINEPREFIX/drive_c/llvm"
if [[ ! -f "$LLVM_DIR/bin/lld-link.exe" ]]; then
    LLVM_EXE="$CACHE_DIR/LLVM-${LLVM_VERSION}-win64.exe"
    if [[ ! -f "$LLVM_EXE" ]]; then
        log "Downloading LLVM ${LLVM_VERSION} for Windows (~435 MB)"
        curl -fSL --progress-bar \
            -o "$LLVM_EXE.tmp" \
            "https://github.com/llvm/llvm-project/releases/download/llvmorg-${LLVM_VERSION}/LLVM-${LLVM_VERSION}-win64.exe"
        mv "$LLVM_EXE.tmp" "$LLVM_EXE"
    else
        log "Using cached LLVM installer: $LLVM_EXE"
    fi
    mkdir -p "$LLVM_DIR"
    log "Extracting LLVM into $LLVM_DIR"
    (cd "$LLVM_DIR" && 7z x -y "$LLVM_EXE" >/dev/null)
else
    log "LLVM already present in $LLVM_DIR"
fi

# ─── 4. xwin: MSVC CRT + Windows SDK x86 libs ─────────────────────────────────
XWIN_DIR="$WINEPREFIX/drive_c/xwin"
if [[ ! -d "$XWIN_DIR/sdk/lib/um/x86" ]]; then
    XWIN_BIN="$(command -v xwin || true)"
    if [[ -z "$XWIN_BIN" ]]; then
        XWIN_BIN="$CACHE_DIR/xwin"
        if [[ ! -x "$XWIN_BIN" ]]; then
            log "Downloading xwin ${XWIN_VERSION}"
            curl -fsSL "https://github.com/Jake-Shadle/xwin/releases/download/${XWIN_VERSION}/xwin-${XWIN_VERSION}-x86_64-unknown-linux-musl.tar.gz" \
                | tar -xz -C "$CACHE_DIR" --strip-components=1 "xwin-${XWIN_VERSION}-x86_64-unknown-linux-musl/xwin"
            chmod +x "$XWIN_BIN"
        fi
    fi

    log "Running xwin to fetch MSVC x86 libraries (~1 GB on disk; this takes a few minutes)"
    mkdir -p "$XWIN_DIR"
    "$XWIN_BIN" --accept-license --arch x86 \
        --cache-dir "$CACHE_DIR/xwin-cache" \
        splat --copy --include-debug-libs --output "$XWIN_DIR"
else
    log "xwin output already present in $XWIN_DIR"
fi

# ─── 5. wine-aot.props ────────────────────────────────────────────────────────
PROPS_SRC="$SCRIPT_DIR/wine-aot.props"
PROPS_DST="$WINEPREFIX/drive_c/wine-aot.props"
[[ -f "$PROPS_SRC" ]] || die "Missing $PROPS_SRC (should be next to this script)"
cp "$PROPS_SRC" "$PROPS_DST"
log "Installed $PROPS_DST"

# ─── done ─────────────────────────────────────────────────────────────────────
log ""
log "Setup complete. Build the x86 components with:"
log "    Scripts/PublishLinuxWineX86.sh"
log ""
log "Disk usage:"
du -sh "$DOTNET_DIR" "$LLVM_DIR" "$XWIN_DIR" 2>/dev/null | sed 's/^/    /'
