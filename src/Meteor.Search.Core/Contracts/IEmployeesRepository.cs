using Meteor.Search.Core.Dtos;
using Meteor.Search.Core.Models;

namespace Meteor.Search.Core.Contracts;

public interface IEmployeesRepository
{
    Task<(IReadOnlyCollection<Employee> Items, int Total)> SearchAsync(EmployeesFilter filter);

    Task SaveEmployeeAsync(Employee employee);

    Task RemoveEmployeeAsync(int employeeId);
}