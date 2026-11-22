using System.Diagnostics;
using Godot;
using JRPG.Core.Data;
using JRPG.Godot.Views;

namespace JRPG.Godot;

public partial class Program : Node
{
	private Game _game;

	public override async void _Ready()
	{
		base._Ready();

		if (!Trace.Listeners
			.OfType<DefaultTraceListener>()
			.Any())
		{
			Trace.Listeners.Add(new DefaultTraceListener());
		}

		_game = new Game
		{
			ButtonFactory = new GodotButtonFactory { Parent = this },
		};

		await _game.Ready();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventKey { Pressed: true, Echo: false } keyEvent)
		{
			var inputAction = keyEvent.Keycode switch
			{
				Key.Enter => InputAction.Accept,
				Key.Escape => InputAction.Cancel,
				Key.F1 => InputAction.Debug,
				Key.F12 => InputAction.Mute,
				Key.F5 => InputAction.Reset,
				_ => (InputAction?)null,
			};

			if (inputAction is not null)
			{
				Game.OnActionPressed(inputAction.Value);
			}
		}
	}
}
