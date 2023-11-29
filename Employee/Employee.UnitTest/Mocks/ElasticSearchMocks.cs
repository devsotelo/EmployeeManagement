using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Contracts.Persistence;
using Employee.Application.Models;
using Moq;

namespace Employee.UnitTest.Mocks
{
    public class ElasticSearchMocks
    {
        public static Mock<IElasticService> GetElasticService()
        {
            var mock = new Mock<IElasticService>();

            mock.Setup(repo => repo.Send(It.IsAny<PermissionDocument>())).ReturnsAsync(
                (PermissionDocument permissionDocument) =>
                {
                    return true;
                });

            return mock;
        }
    }
}
