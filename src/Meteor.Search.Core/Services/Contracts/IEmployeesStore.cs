using Meteor.Search.Core.Models;

namespace Meteor.Search.Core.Services.Contracts;

public interface IEmployeesStore
{
    Task SaveEmployeeAsync(Employee employee);

    Task RemoveEmployeeAsync(int employeeId);
}