namespace JRPG.Core.Data;

public readonly record struct Colour
{
	public static readonly Colour Black = new() { R = 0, G = 0, B = 0, A = 255 };

	public static readonly Colour Grey = new() { R = 132, G = 130, B = 132, A = 255 };

	public static readonly Colour LightGrey = new() { R = 198, G = 195, B = 198, A = 255 };

	public static readonly Colour White = new() { R = 255, G = 251, B = 255, A = 255 };

	public static readonly Colour Blue = new() { R = 0, G = 0, B = 255, A = 255 };

	public static readonly Colour Magenta = new() { R = 198, G = 0, B = 198, A = 255 };

	public static readonly Colour Cyan = new() { R = 0, G = 251, B = 255, A = 255 };

	public static readonly Colour Brown = new() { R = 132, G = 97, B = 0, A = 255 };

	public static readonly Colour Vermilion = new() { R = 198, G = 65, B = 33, A = 255 };

	public static readonly Colour Peach = new() { R = 255, G = 195, B = 132, A = 255 };

	public static readonly Colour Red = new() { R = 255, G = 0, B = 0, A = 255 };

	public static readonly Colour Orange = new() { R = 255, G = 130, B = 0, A = 255 };

	public static readonly Colour Yellow = new() { R = 255, G = 251, B = 0, A = 255 };

	public static readonly Colour Green = new() { R = 0, G = 130, B = 66, A = 255 };

	public static readonly Colour Lime = new() { R = 0, G = 251, B = 0, A = 255 };

	public int R { get; init; }

	public int G { get; init; }

	public int B { get; init; }

	public int A { get; init; }
}
