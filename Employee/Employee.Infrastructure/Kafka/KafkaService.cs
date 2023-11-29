using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Models;
using KafkaFlow.Producers;

namespace Employee.Infrastructure.Kafka
{
    public class KafkaService : IKafkaService
    {
        private readonly IProducerAccessor producerAccessor;

        public KafkaService(IProducerAccessor producerAccessor)
        {
            this.producerAccessor = producerAccessor;
        }

        public async Task<bool> Send(OperationMessage operationMessage)
        {
            try
            {
                var producer = producerAccessor.GetProducer("employee-producer");
                await producer.ProduceAsync("employee-topic", Guid.NewGuid().ToString(), operationMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
