#!/usr/bin/env bash
# Daybreak Linux launcher wrapper
# Sets environment variables BEFORE the .NET runtime loads, so native
# libraries (Mesa, EGL, libwayland, WebKitGTK) see them at init time.

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

# Force X11 backend — Photino/WebKitGTK/GTK3 doesn't support Wayland natively
export GDK_BACKEND="${GDK_BACKEND:-x11}"

# Prevent WebKitGTK from using DMA-BUF (crashes on some Nvidia/Mesa combos)
export WEBKIT_DISABLE_DMABUF_RENDERER="${WEBKIT_DISABLE_DMABUF_RENDERER:-1}"

# Disable GPU compositing (blank windows on some drivers)
export WEBKIT_DISABLE_COMPOSITING_MODE="${WEBKIT_DISABLE_COMPOSITING_MODE:-1}"

exec "$SCRIPT_DIR/Daybreak" "$@"
