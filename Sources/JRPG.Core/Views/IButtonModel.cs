using System.Numerics;

namespace JRPG.Core.Views;

public interface IButtonModel
{
	string Text { get; set; }

	Action Click { get; set; }

	Vector2 Position { get; set; }
}
