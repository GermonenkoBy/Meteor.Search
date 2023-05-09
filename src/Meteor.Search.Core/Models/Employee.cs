using System.ComponentModel.DataAnnotations;
using Meteor.Search.Core.Models.Enums;

namespace Meteor.Search.Core.Models;

public class Employee
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "Max first name length is 50.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Max last name length is 50.")]
    public string LastName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Email address is required.")]
    [StringLength(250, ErrorMessage = "Max email address length is 250.")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required.")]
    public string PhoneNumber { get; set; } = string.Empty;

    public EmployeeStatuses Status { get; set; }
}