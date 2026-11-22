using GodotPlugins.Game;
using Engine = twodog.Engine;

internal static class Program
{
	private static int Main(string[] args)
	{
		Engine.RegisterWebPluginsInitializer(TwoDogWebBoot.PluginsInitializer());

		var engine = new Engine("JRPG.Godot", args: args);
		engine.Start();
		engine.Run();

		return 0;
	}
}
