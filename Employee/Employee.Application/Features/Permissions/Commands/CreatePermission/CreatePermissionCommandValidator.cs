using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Persistence;
using FluentValidation;

namespace Employee.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IPermissionTypeRepository permissionTypeRepository;
        private readonly IEmployeeRepository employeeRepository;

        public CreatePermissionCommandValidator(IPermissionRepository permissionRepository, IPermissionTypeRepository permissionTypeRepository, IEmployeeRepository employeeRepository)
        {
            this.permissionRepository = permissionRepository;
            this.permissionTypeRepository = permissionTypeRepository;
            this.employeeRepository = employeeRepository;

            RuleFor(e => e)
                .MustAsync(EmployeeExists)
                .WithMessage("The employee not exists.");
            
            RuleFor(e => e)
                .MustAsync(TypeExists)
                .WithMessage("The permission type not exists.");
            
            RuleFor(e => e)
                .MustAsync(PermissionUnique)
                .WithMessage("An permission with the same user already exists.");
        }

        private async Task<bool> PermissionUnique(CreatePermissionCommand e, CancellationToken token)
        {
            return !(await permissionRepository.IsPermissionUnique(e.IdEmployee, e.IdPermissionType));
        }

        private async Task<bool> EmployeeExists(CreatePermissionCommand e, CancellationToken token)
        {
            return (await employeeRepository.GetByIdAsync(e.IdEmployee)) != null;
        }

        private async Task<bool> TypeExists(CreatePermissionCommand e, CancellationToken token)
        {
            return (await permissionTypeRepository.GetByIdAsync(e.IdPermissionType)) != null;
        }
    }
}
