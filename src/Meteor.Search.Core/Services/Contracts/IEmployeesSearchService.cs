using Meteor.Search.Core.Dtos;
using Meteor.Search.Core.Models;

namespace Meteor.Search.Core.Services.Contracts;

public interface IEmployeesSearchService
{
    Task<(ICollection<Employee> Items, int Total)> SearchAsync(EmployeesFilter filter);
}