#!/usr/bin/env bash
#
# Linux equivalent of Scripts/PublishWindowsX86.ps1.
#
# Publishes Daybreak.Injector (x86) and Daybreak.API (x86) for Windows by
# running the Windows .NET SDK under Wine, using lld-link as the platform
# linker and the MSVC CRT + Windows SDK x86 libraries extracted by xwin.
#
# Run Scripts/SetupWinePrefix.sh once before using this.
#
# Usage:  Scripts/PublishLinuxWineX86.sh [Configuration]
# Env vars:
#   WINEPREFIX  — wine prefix to use (default: ~/.wine-daybreak)

set -euo pipefail

CONFIGURATION="${1:-Release}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$REPO_ROOT"

export WINEPREFIX="${WINEPREFIX:-$HOME/.wine-daybreak}"
DOTNET_EXE="$WINEPREFIX/drive_c/dotnet/dotnet.exe"
PROPS_FILE='C:\wine-aot.props'

if [[ ! -f "$DOTNET_EXE" ]]; then
    echo "Wine prefix is not set up. Run Scripts/SetupWinePrefix.sh first." >&2
    exit 1
fi
if [[ ! -f "$WINEPREFIX/drive_c/wine-aot.props" ]]; then
    echo "Missing C:\\wine-aot.props in wine prefix. Re-run Scripts/SetupWinePrefix.sh." >&2
    exit 1
fi

# Map repo into wine as R:\
ln -sfn "$REPO_ROOT" "$WINEPREFIX/dosdevices/r:"

# Environment for wine + dotnet
export WINEDLLOVERRIDES="mscoree=b"
export WINEDEBUG="${WINEDEBUG:--all}"
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_GENERATE_ASPNET_CERTIFICATE=false
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

INJECTOR_OUT='R:\Publish\Injector'
API_OUT='R:\Publish\Api'

# Clean previous outputs so MSBuild incremental can't get confused. The
# generator + shared projects are also wiped because their bin/Debug is
# shared with the Linux dotnet build (same netstandard2.0 / net10.0 TFM),
# which can leave the wine build half-incremental and CSC then fails to
# locate the generator dll mid-publish.
rm -rf "$REPO_ROOT/Publish/Injector" "$REPO_ROOT/Publish/Api"
for p in Daybreak.Injector Daybreak.API Daybreak.Generators Daybreak.Shared; do
    rm -rf "$REPO_ROOT/$p/obj" "$REPO_ROOT/$p/bin"
done

# Kill any stale daemons from a previous wine session (these wedge stdout)
pkill_pat() {
    pgrep -af "$1" 2>/dev/null | awk '{print $1}' | xargs -r kill -9 2>/dev/null || true
}
pkill_pat 'VBCSCompiler'
pkill_pat 'MSBuild.dll.*nodemode'

dotnet_publish() {
    local proj="$1" out="$2" tag="$3"
    local log="/tmp/wine-${tag}-publish.log"
    echo "=== Publishing $proj → $out (log: $log) ===" >&2
    if ! wine "$DOTNET_EXE" publish "$proj" \
        -c "$CONFIGURATION" -r win-x86 \
        -o "$out" \
        -nodeReuse:false -m:1 \
        -p:UseSharedCompilation=false \
        -p:EnableSourceLink=false \
        -p:CustomAfterMicrosoftCommonProps="$PROPS_FILE" \
        -v:m 2>&1 | tee "$log"; then
        local rc=${PIPESTATUS[0]}
        echo "!!! publish failed (exit $rc). Full log: $log" >&2
        return "$rc"
    fi
    # MSBuild can exit 0 even when a build error was reported via Exec'd subprocess
    if grep -qE '^[^[:space:]].*: error |error MSB[0-9]+|Build FAILED|build failed' "$log"; then
        echo "!!! publish reported errors (see $log)" >&2
        grep -E ': error |error MSB[0-9]+|Build FAILED' "$log" >&2 | head -20
        return 1
    fi
}

dotnet_publish 'R:\Daybreak.Injector\Daybreak.Injector.csproj' "$INJECTOR_OUT" injector
dotnet_publish 'R:\Daybreak.API\Daybreak.API.csproj'           "$API_OUT"      api

# wine spawns long-lived build-server processes that keep the parent shell's
# stdout pipe open even after publish returns. Reap them so this script exits.
pkill_pat 'VBCSCompiler'
pkill_pat 'MSBuild.dll.*nodemode'
wineserver -k 2>/dev/null || true

echo "=== Linux/Wine x86 publish complete ==="
echo "Injector: $REPO_ROOT/Publish/Injector"
echo "API:      $REPO_ROOT/Publish/Api"
