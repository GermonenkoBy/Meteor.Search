using System.ComponentModel.DataAnnotations;

namespace Meteor.Search.Core.Services.Validators;

public interface IValidator<TModel>
{
    Task<bool> ValidateAsync(TModel model, ICollection<ValidationResult> validationResults);
}