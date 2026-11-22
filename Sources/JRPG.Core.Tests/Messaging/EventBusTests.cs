using FluentAssertions;
using JRPG.Core.Messaging;

namespace JRPG.Core.Tests.Messaging;

[TestClass]
public sealed class EventBusTests
{
	private sealed record FirstEvent(int Value);

	private sealed record SecondEvent(string Text);

	[TestMethod]
	public void ShouldInvokeSubscribedHandlerWhenEventPublished()
	{
		var bus = new EventBus();
		var received = new List<FirstEvent>();
		bus.Subscribe<FirstEvent>(e => received.Add(e));

		bus.Publish(new FirstEvent(42));

		received.Should().ContainSingle().Which.Value.Should().Be(42);
	}

	[TestMethod]
	public void ShouldNotInvokeHandlerWhenEventTypeDiffers()
	{
		var bus = new EventBus();
		var invoked = false;
		bus.Subscribe<SecondEvent>(_ => invoked = true);

		bus.Publish(new FirstEvent(1));

		invoked.Should().BeFalse();
	}

	[TestMethod]
	public void ShouldPropagateHandlerException()
	{
		var bus = new EventBus();
		bus.Subscribe<FirstEvent>(_ => throw new InvalidOperationException());

		FluentActions.Invoking(() => bus.Publish(new FirstEvent(1))).Should().ThrowExactly<InvalidOperationException>();
	}

	[TestMethod]
	public void ShouldNotThrowWhenPublishingWithoutSubscribers()
	{
		var bus = new EventBus();

		bus.Publish(new FirstEvent(1));
	}

	[TestMethod]
	public void ShouldRemoveHandlerWhenSubscriptionDisposed()
	{
		var bus = new EventBus();
		var count = 0;
		var subscription = bus.Subscribe<FirstEvent>(_ => count++);

		subscription.Dispose();
		bus.Publish(new FirstEvent(1));

		count.Should().Be(0);
	}

	[TestMethod]
	public void ShouldInvokeHandlerTwiceWhenSubscribedTwice()
	{
		var bus = new EventBus();
		var count = 0;
		using var first = bus.Subscribe<FirstEvent>(_ => count++);
		using var second = bus.Subscribe<FirstEvent>(_ => count++);

		bus.Publish(new FirstEvent(1));

		count.Should().Be(2);
	}

	[TestMethod]
	public void ShouldThrowWhenHandlerIsNull()
	{
		var bus = new EventBus();

		FluentActions.Invoking(() => bus.Subscribe<FirstEvent>(null)).Should().ThrowExactly<ArgumentNullException>();
	}

	[TestMethod]
	public void ShouldThrowWhenPublishedEventIsNull()
	{
		var bus = new EventBus();
		bus.Subscribe<SecondEvent>(_ => { });

		FluentActions.Invoking(() => bus.Publish<SecondEvent>(null)).Should().ThrowExactly<ArgumentNullException>();
	}

	[TestMethod]
	public void ShouldReturnSameInstanceWhenAccessedRepeatedly()
	{
		EventBus.Instance.Value.Should().BeSameAs(EventBus.Instance.Value);
	}

	[TestMethod]
	public void ShouldInvokeHandlerForEachPublishedEvent()
	{
		var bus = new EventBus();
		var count = 0;
		using var subscription = bus.Subscribe<FirstEvent>(_ => count++);

		bus.Publish(new FirstEvent(1));
		bus.Publish(new FirstEvent(2));

		count.Should().Be(2);
	}
}
