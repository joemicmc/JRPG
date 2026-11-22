#if LIBGODOT_ENABLED
using Godot.NativeInterop;

namespace GodotPlugins.Game;

internal static partial class Main
{
	internal static unsafe IntPtr TwoDogGetInitializePointer() =>
		(IntPtr)(delegate* unmanaged<IntPtr, IntPtr, IntPtr, int, godot_bool>)&InitializeFromGameProject;
}
#endif
