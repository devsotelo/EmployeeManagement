using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Infrastructure;
using Employee.Application.Models;
using Employee.Domain.Entities;
using Nest;

namespace Employee.Infrastructure.ElasticSearch
{
    public class ElasticSearchService : IElasticService
    {
        private readonly ElasticClient elasticClient;

        public ElasticSearchService(ElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task<bool> Send(PermissionDocument permissionDocument)
        {
            var indexResponseAsync = await elasticClient.IndexDocumentAsync(permissionDocument);
            return indexResponseAsync.IsValid;
        }
    }
}
