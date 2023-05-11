using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Dtos;
using Meteor.Search.Core.Models;
using Meteor.Search.Core.Services.Contracts;

namespace Meteor.Search.Core.Services;

public class EmployeesSearchService : IEmployeesSearchService
{
    private readonly IEmployeesRepository _employeesRepository;

    public EmployeesSearchService(IEmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public Task<(IReadOnlyCollection<Employee> Items, int Total)> SearchAsync(EmployeesFilter filter)
        => _employeesRepository.SearchAsync(filter);
}