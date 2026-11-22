using FluentAssertions;
using JRPG.Core.Messaging;

namespace JRPG.Core.Tests.Messaging;

[TestClass]
public sealed class SubscriptionTests
{
	private sealed record TestEvent(int Value);

	[TestMethod]
	public void ShouldNotThrowWhenDisposedTwice()
	{
		var bus = new EventBus();
		var count = 0;
		var subscription = bus.Subscribe<TestEvent>(_ => count++);

		var act = () =>
		{
			subscription.Dispose();
			subscription.Dispose();
		};

		act.Should().NotThrow();

		bus.Publish(new TestEvent(1));

		count.Should().Be(0);
	}

	[TestMethod]
	public void ShouldRemoveOnlyDisposedSubscription()
	{
		var bus = new EventBus();
		var first = 0;
		var second = 0;
		var firstSubscription = bus.Subscribe<TestEvent>(_ => first++);
		using var secondSubscription = bus.Subscribe<TestEvent>(_ => second++);

		firstSubscription.Dispose();
		bus.Publish(new TestEvent(1));

		first.Should().Be(0);
		second.Should().Be(1);
	}
}
