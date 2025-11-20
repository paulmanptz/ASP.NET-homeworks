using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using System.Collections.Generic;
using System.Linq;

namespace Pcf.Administration.DataAccess.Data
{
    public class MongoDbInitializer : IDbInitializer
    {
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IRepository<Role> _roleRepo;

        public MongoDbInitializer(
            IRepository<Employee> employeeRepo,
            IRepository<Role> roleRepo)
        {
            _employeeRepo = employeeRepo;
            _roleRepo = roleRepo;
        }

        public async void InitializeDb()
        {
            await _roleRepo.DeleteAllAsync();
            await _employeeRepo.DeleteAllAsync();

            // Загрузка ролей
            IEnumerable<Role> existingRoles = await _roleRepo.GetAllAsync();
            if (!existingRoles.Any())
            {
                foreach (Role role in FakeDataFactory.Roles)
                    await _roleRepo.CreateAsync(role);
            }

            // Загрузка сотрудников
            IEnumerable<Employee> existingEmployees = await _employeeRepo.GetAllAsync();
            if (!existingEmployees.Any())
            {
                List<Role> dbRoles = (await _roleRepo.GetAllAsync()).ToList();
                foreach (Employee emp in FakeDataFactory.Employees)
                {
                    Role roleInDb = dbRoles.First(r => r.Name == emp.Role?.Name);

                    Employee employeeWithValidRole = new Employee
                    {
                        Id = emp.Id,
                        Email = emp.Email,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        Role = roleInDb,
                        AppliedPromocodesCount = emp.AppliedPromocodesCount
                    };

                    await _employeeRepo.CreateAsync(employeeWithValidRole);
                }
            }
        }
    }
}
