// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Tests.Configuration
{
	using Magnum.Extensions;
	using Magnum.TestFramework;
	using Messages;
	using TestFramework;

	[Scenario]
	public class When_subscribing_a_consumer_to_the_bus
	{
		IServiceBus _bus;
		PingMessage _ping;

		[When]
		public void Subscribing_a_consumer_to_the_bus()
		{
			_bus = ServiceBusFactory.New(x =>
				{
					x.ReceiveFrom("loopback://localhost/mt_test");

					x.Subscribe(s => s.Consumer<ConsumerOf<PingMessage>>());
				});

			_ping = new PingMessage();
			_bus.Publish(_ping);
		}

		[Finally]
		public void Finally()
		{
			_bus.Dispose();
		}

		[Then]
		public void Should_have_subscribed()
		{
			_bus.ShouldHaveRemoteSubscriptionFor<PingMessage>();
		}

		[Then]
		public void Should_have_received_the_message()
		{
			ConsumerOf<PingMessage>.OnlyOneShouldHaveReceivedMessage(_ping, 8.Seconds());
		}
	}
}