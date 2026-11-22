using Godot;
using JRPG.Core.Views;

namespace JRPG.Godot.Views;

public class GodotButtonFactory : IButtonFactory
{
	public Node Parent { get; init; }

	public IButton Create(IButtonModel model)
	{
		var button = new GodotButton
		{
			Text = model.Text,
			Position = new Vector2(model.Position.X, model.Position.Y),
			Click = model.Click,
		};

		Parent.AddChild(button);

		return button;
	}
}
