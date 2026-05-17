#!/usr/bin/env bash
#
# Build the x86 NativeAOT components via wine and copy them into the
# Daybreak.Linux output tree so `dotnet run --project Daybreak.Linux` (or
# easy-dot-net's "Run Daybreak.Linux") picks them up.
#
# Layout produced (matches VerifyNativeComponentsAction / DaybreakInjector):
#   Daybreak.Linux/bin/<Config>/net10.0/linux-x64/Injector/Daybreak.Injector.exe
#   Daybreak.Linux/bin/<Config>/net10.0/linux-x64/Api/Daybreak.API.dll
#   Daybreak.Linux/bin/<Config>/net10.0/linux-x64/Api/gwca.dll
#
# Usage:  Scripts/StageX86ForDebug.sh [Configuration]
#   Configuration: Debug (default) or Release
#
# NativeAOT is built in Debug by default to match the Windows dev workflow
# (Scripts/PublishWindowsX86.ps1 stages Debug AOT into the Daybreak.Linux
# output tree). Debug AOT keeps PDBs and emits a console window on injection,
# which is great for live log output and gdb/lldb debugging.
#
# Override with STAGE_AOT_CONFIG=Release for a smaller, stripped binary.

set -euo pipefail

DAYBREAK_CONFIG="${1:-Debug}"
STAGE_AOT_CONFIG="${STAGE_AOT_CONFIG:-Debug}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$REPO_ROOT"

DEST_ROOT="$REPO_ROOT/Daybreak.Linux/bin/$DAYBREAK_CONFIG/net10.0/linux-x64"
mkdir -p "$DEST_ROOT/Injector" "$DEST_ROOT/Api"

echo "=== Building x86 components ($STAGE_AOT_CONFIG) via wine ==="
"$SCRIPT_DIR/PublishLinuxWineX86.sh" "$STAGE_AOT_CONFIG"

SRC_INJECTOR="$REPO_ROOT/Publish/Injector"
SRC_API="$REPO_ROOT/Publish/Api"

echo "=== Staging into $DEST_ROOT ==="
# rsync gives clean delete-extras + permission control; cp would leave orphans
if command -v rsync >/dev/null 2>&1; then
    rsync -a --delete "$SRC_INJECTOR/" "$DEST_ROOT/Injector/"
    rsync -a --delete "$SRC_API/"      "$DEST_ROOT/Api/"
else
    rm -rf "$DEST_ROOT/Injector"/* "$DEST_ROOT/Api"/*
    cp -a "$SRC_INJECTOR/." "$DEST_ROOT/Injector/"
    cp -a "$SRC_API/."      "$DEST_ROOT/Api/"
fi

echo
echo "=== Restoring solution for Roslyn LSP ==="
# The wine publish leaves Daybreak.{API,Shared,Generators}/obj/ restored
# *only* for win-x86 (a Windows TFM), which Roslyn LSP can't reconcile when
# opening Daybreak.slnx — package references like ZLinq, ILogger<>, and
# SystemExtensions.* go unresolved in editor diagnostics. A plain Linux
# `dotnet restore` of the slnx rewrites each project's obj/ back to a
# multi-target shape Roslyn understands, without invalidating the staged
# binaries (those are physical files under Daybreak.Linux/bin/, not obj/).
dotnet restore "$REPO_ROOT/Daybreak.slnx" --nologo --verbosity quiet

echo
echo "=== Staged ==="
ls -la "$DEST_ROOT/Injector/" | sed 's/^/  /'
echo
ls -la "$DEST_ROOT/Api/" | sed 's/^/  /'
echo
echo "Now: dotnet run --project Daybreak.Linux -c $DAYBREAK_CONFIG"
