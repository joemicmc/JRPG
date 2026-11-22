namespace JRPG.Core;

public static class Log
{
	public static void WriteLine(object value)
	{
		Console.WriteLine(value);

		System.Diagnostics.Debug.WriteLine(value);
	}
}
