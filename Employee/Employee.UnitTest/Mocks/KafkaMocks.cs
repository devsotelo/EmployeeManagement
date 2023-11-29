using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Models;
using Moq;

namespace Employee.UnitTest.Mocks
{
    public class KafkaMocks
    {
        public static Mock<IKafkaService> GetKafkaService()
        {
            var mock = new Mock<IKafkaService>();

            mock.Setup(repo => repo.Send(It.IsAny<OperationMessage>())).ReturnsAsync(
                (OperationMessage operationMessage) =>
                {
                    return true;
                });

            return mock;
        }
    }
}
