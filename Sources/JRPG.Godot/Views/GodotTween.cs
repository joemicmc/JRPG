using Godot;
using JRPG.Core.Data;
using JRPG.Core.Views;
using NumericsVector2 = System.Numerics.Vector2;

namespace JRPG.Godot.Views;

public sealed class GodotTween : ITween
{
	public Node Target { get; init; }

	public Task MoveTo(NumericsVector2 position, float duration, Easing easing)
	{
		var completion = new TaskCompletionSource();

		var tween = Target.CreateTween();

		tween.TweenProperty(Target, "position", new Vector2(position.X, position.Y), duration)
			.SetTrans(easing == Easing.Linear ? Tween.TransitionType.Linear : Tween.TransitionType.Sine)
			.SetEase(ToGodotEase(easing));

		tween.Finished += () => completion.TrySetResult();
		Target.TreeExiting += () => completion.TrySetCanceled();

		return completion.Task;
	}

	private static Tween.EaseType ToGodotEase(Easing easing) => easing switch
	{
		Easing.EaseIn => Tween.EaseType.In,
		Easing.EaseOut => Tween.EaseType.Out,
		_ => Tween.EaseType.InOut,
	};
}
