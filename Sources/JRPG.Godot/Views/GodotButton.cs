using Godot;
using JRPG.Core.Views;

namespace JRPG.Godot.Views;

public partial class GodotButton : global::Godot.Button, IButton
{
	private Action _click;

	private ITween _tween;

	public Action Click
	{
		get => _click;
		set => _click = value;
	}

	string IButtonModel.Text
	{
		get => Text;
		set => Text = value;
	}

	System.Numerics.Vector2 IButtonModel.Position
	{
		get => new(Position.X, Position.Y);
		set => Position = new Vector2(value.X, value.Y);
	}

	ITween IButton.Tween => _tween ??= new GodotTween { Target = this };

	public override void _Ready()
	{
		base._Ready();

		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		_click?.Invoke();
	}
}
