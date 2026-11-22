using JRPG.Core.Data;
using System.Numerics;

namespace JRPG.Core.Views;

public interface ITween
{
	Task MoveTo(Vector2 position, float duration, Easing easing);
}
