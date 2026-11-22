namespace JRPG.Core.Messaging;

public sealed class EventBus
{
	private readonly Dictionary<Type, List<Subscription>> _handlers = [];

	private readonly Lock _lock = new();

	public static Lazy<EventBus> Instance { get; } = new(() => new EventBus());

	public IDisposable Subscribe<TEvent>(Action<TEvent> handler = null)
	{
		ArgumentNullException.ThrowIfNull(handler);

		var subscription = new Subscription(this, typeof(TEvent), e => handler((TEvent)e));

		lock (_lock)
		{
			if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
			{
				handlers = [];
				_handlers[typeof(TEvent)] = handlers;
			}

			handlers.Add(subscription);
		}

		return subscription;
	}

	public void Publish<TEvent>(TEvent @event)
	{
		ArgumentNullException.ThrowIfNull(@event);

		Subscription[] snapshot;

		lock (_lock)
		{
			if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
			{
				return;
			}

			snapshot = [.. handlers];
		}

		foreach (var subscription in snapshot)
		{
			subscription.Invoke(@event);
		}
	}

	internal void Remove(Type eventType, Subscription subscription)
	{
		lock (_lock)
		{
			if (!_handlers.TryGetValue(eventType, out var handlers))
			{
				return;
			}

			handlers.Remove(subscription);

			if (handlers.Count == 0)
			{
				_handlers.Remove(eventType);
			}
		}
	}
}
