using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.Contracts.Persistence;
using Employee.Application.Features.Permissions.Commands.CreatePermission;
using FluentValidation;

namespace Employee.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IPermissionTypeRepository permissionTypeRepository;
        private readonly IEmployeeRepository employeeRepository;

        public UpdatePermissionCommandValidator(IPermissionRepository permissionRepository, IPermissionTypeRepository permissionTypeRepository, IEmployeeRepository employeeRepository)
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

        private async Task<bool> PermissionUnique(UpdatePermissionCommand e, CancellationToken token)
        {
            return !(await permissionRepository.IsPermissionUnique(e.EmployeeId, e.PermissionTypeId));
        }

        private async Task<bool> EmployeeExists(UpdatePermissionCommand e, CancellationToken token)
        {
            return (await employeeRepository.GetByIdAsync(e.EmployeeId)) != null;
        }

        private async Task<bool> TypeExists(UpdatePermissionCommand e, CancellationToken token)
        {
            return (await permissionTypeRepository.GetByIdAsync(e.PermissionTypeId)) != null;
        }
    }
}
