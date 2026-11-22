#if LIBGODOT_ENABLED

namespace GodotPlugins.Game;

public static class TwoDogWebBoot
{
	public static IntPtr PluginsInitializer() => Main.TwoDogGetInitializePointer();
}
#endif
