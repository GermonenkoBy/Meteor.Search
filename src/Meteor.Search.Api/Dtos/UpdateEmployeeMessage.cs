using Mapster;
using Meteor.Search.Core.Models;
using Meteor.Search.Core.Models.Enums;

namespace Meteor.Search.Api.Dtos;

public record UpdateEmployeeMessage : CustomerBasedMessage
{
    [AdaptMember(nameof(Employee.Id))]
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public EmployeeStatuses Status { get; set; }
}