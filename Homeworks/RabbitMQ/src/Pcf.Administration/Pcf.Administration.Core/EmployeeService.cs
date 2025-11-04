using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.Administration.Core
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> IncrementAppliedPromocodesAsync(Guid employeeId, CancellationToken ct)
        {
            Employee employee = _employeeRepository.GetByIdAsync(employeeId).Result;
            if (employee == null) return false;

            employee.AppliedPromocodesCount++;
            await _employeeRepository.UpdateAsync(employee);
            return true;
        }
    }
}
