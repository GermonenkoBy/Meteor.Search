using System.ComponentModel.DataAnnotations;
using Meteor.Common.Core.Exceptions;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Models;
using Meteor.Search.Core.Services.Contracts;
using Meteor.Search.Core.Services.Validators;

namespace Meteor.Search.Core.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;

    private readonly IEnumerable<IValidator<Employee>> _validators;

    public EmployeesService(IEmployeesRepository employeesRepository, IEnumerable<IValidator<Employee>> validators)
    {
        _employeesRepository = employeesRepository;
        _validators = validators;
    }

    public async Task SaveEmployeeAsync(Employee employee)
    {
        var employeeIsValid = true;
        var validationResults = new List<ValidationResult>(10);
        foreach (var validator in _validators)
        {
            if (!await validator.ValidateAsync(employee, validationResults))
            {
                employeeIsValid = false;
            }
        }

        if (!employeeIsValid)
        {
            throw new MeteorValidationException("Employee is invalid")
            {
                Errors = validationResults
            };
        }

        await _employeesRepository.SaveEmployeeAsync(employee);
    }

    public Task RemoveEmployeeAsync(int employeeId)
    {
        return _employeesRepository.RemoveEmployeeAsync(employeeId);
    }
}