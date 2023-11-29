using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.IntegrationTests.Base;
using System.Text.Json;
using Employee.Application.Features.Permissions.Queries.GetPermissionsList;

namespace Employee.IntegrationTests.Controllers
{
    public class PermissionControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public PermissionControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetPermissionsTest()
        {
            var client = _factory.GetAnonymousClient();

            var response = await client.GetAsync("/GetPermissions");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<PermissionVm>>(responseString);

            Assert.IsType<List<PermissionVm>>(result);
            Assert.NotEmpty(result);
        }
    }
}
