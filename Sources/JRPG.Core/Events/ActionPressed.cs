using JRPG.Core.Data;

namespace JRPG.Core.Events;

public sealed record ActionPressed
{
	required public InputAction InputAction { get; init; }
}
