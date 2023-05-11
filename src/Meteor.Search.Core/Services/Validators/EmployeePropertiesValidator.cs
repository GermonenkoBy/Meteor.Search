using System.ComponentModel.DataAnnotations;
using Meteor.Search.Core.Models;

namespace Meteor.Search.Core.Services.Validators;

public class EmployeePropertiesValidator : IValidator<Employee>
{
    public Task<bool> ValidateAsync(Employee model, ICollection<ValidationResult> validationResults)
    {
        var valid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);
        return Task.FromResult(valid);
    }
}