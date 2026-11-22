namespace JRPG.Core.Messaging;

internal sealed class Subscription(EventBus bus, Type eventType, Action<object> handler) : IDisposable
{
	private int _isDisposed;

	public void Invoke(object @event) => handler(@event);

	public void Dispose()
	{
		if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
		{
			return;
		}

		bus.Remove(eventType, this);
	}
}
