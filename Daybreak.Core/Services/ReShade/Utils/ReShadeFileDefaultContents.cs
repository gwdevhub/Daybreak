namespace Daybreak.Services.ReShade.Utils;

/// <summary>
/// Obtained by running a fresh ReShade install
/// </summary>
internal static class ReShadeFileDefaultContents
{
    public const string ReShadePresetsIni = @"";

    public const string ReShadeLog = @"
If you are reading this after launching the game at least once, it likely means ReShade was not loaded by the game.

In that event here are some steps you can try to resolve this:

1) Make sure this file and the related DLL are really in the same directory as the game executable.
   If that is the case and it does not work regardless, check if there is a 'bin' directory, move them there and try again.

2) Try running the game with elevated user permissions by doing a right click on its executable and choosing 'Run as administrator'.

3) If the game crashes, try disabling all game overlays (like Origin), recording software (like Fraps), FPS displaying software,
   GPU overclocking and tweaking software and other proxy DLLs (like ENB, Helix or Umod).

4) If none of the above helps, you can get support on the forums at https://reshade.me/forum. But search for your problem before
   creating a new topic, as somebody else may have already found a solution.
";

    public const string ReShadeIni = @"[APP]
ForceDefaultRefreshRate=0
ForceFullscreen=0
ForceVsync=0
ForceWindowed=0

[GENERAL]
PreprocessorDefinitions=RESHADE_DEPTH_LINEARIZATION_FAR_PLANE=1000.0,RESHADE_DEPTH_INPUT_IS_UPSIDE_DOWN=0,RESHADE_DEPTH_INPUT_IS_REVERSED=0,RESHADE_DEPTH_INPUT_IS_LOGARITHMIC=0
EffectSearchPaths=.\reshade-shaders\Shaders\**
TextureSearchPaths=.\reshade-shaders\Textures\**

[INPUT]
GamepadNavigation=1
KeyOverlay=36,0,0,0


";
}
