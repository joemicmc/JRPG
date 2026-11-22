using System.Numerics;

namespace JRPG.Core.Views;

public class ButtonModel : IButtonModel
{
	public string Text { get; set; }

	public Action Click { get; set; }

	public Vector2 Position { get; set; }
}
