$ErrorActionPreference = "Stop"
$RepoRoot = Resolve-Path "$PSScriptRoot/.."

# Force X11 backend — Photino/WebKitGTK/GTK3 doesn't support Wayland natively
if (-not $env:GDK_BACKEND) { $env:GDK_BACKEND = "x11" }

# Prevent WebKitGTK from using DMA-BUF (crashes on some Nvidia/Mesa combos)
if (-not $env:WEBKIT_DISABLE_DMABUF_RENDERER) { $env:WEBKIT_DISABLE_DMABUF_RENDERER = "1" }

# Disable GPU compositing (blank windows on some drivers)
if (-not $env:WEBKIT_DISABLE_COMPOSITING_MODE) { $env:WEBKIT_DISABLE_COMPOSITING_MODE = "1" }

dotnet run --project Daybreak.Linux

