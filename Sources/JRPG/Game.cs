using JRPG.Core;
using JRPG.Core.Data;
using JRPG.Core.Events;
using JRPG.Core.Messaging;
using JRPG.Core.Views;
using System.Numerics;

namespace JRPG;

public class Game
{
	public IButtonFactory ButtonFactory { get; init; }

	public static void OnActionPressed(InputAction inputAction)
	{
		EventBus.Instance.Value.Publish(new ActionPressed { InputAction = inputAction });

		Log.WriteLine(inputAction);
	}

	public async Task Ready()
	{
		var model = new ButtonModel
		{
			Text = "Click me!",
			Click = () => Log.WriteLine("Button clicked!"),
			Position = new Vector2(0, 0),
		};

		var button = ButtonFactory.Create(model);

		await button.Tween.MoveTo(new Vector2(100, 100), 1f, Easing.EaseInOut);

		button.Position = new Vector2(0, 0);
	}
}
