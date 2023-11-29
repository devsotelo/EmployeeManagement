using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Models;
using KafkaFlow;

namespace Employee.Infrastructure.Kafka
{
    public class MessageHandler : IMessageHandler<OperationMessage>
    {
        public Task Handle(IMessageContext context, OperationMessage message)
        {
            Console.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message.Uuid);

            return Task.CompletedTask;
        }
    }
}
