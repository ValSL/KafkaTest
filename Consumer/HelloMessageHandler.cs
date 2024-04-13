using KafkaFlow;
using Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public class HelloMessageHandler : IMessageHandler<HelloMessage>
    {
        public Task Handle(IMessageContext context, HelloMessage message)
        {
            Console.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message.Text);

            return Task.CompletedTask;
        }
    }
}
