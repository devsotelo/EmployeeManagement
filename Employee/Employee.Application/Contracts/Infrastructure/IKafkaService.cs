using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Models;

namespace Employee.Application.Contracts.Infrastructure
{
    public interface IKafkaService
    {
        Task<bool> Send(OperationMessage operationMessage);
    }
}
