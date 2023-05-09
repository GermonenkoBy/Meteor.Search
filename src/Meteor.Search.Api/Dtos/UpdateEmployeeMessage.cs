using Mapster;
using Meteor.Search.Core.Models;
using Meteor.Search.Core.Models.Enums;

namespace Meteor.Search.Api.Dtos;

public record struct UpdateEmployeeMessage
{
    [AdaptMember(nameof(Employee.Id))]
    public int EmployeeId;

    [AdaptIgnore]
    public int CustomerId;

    public string FirstName;

    public string LastName;

    public string EmailAddress;

    public string PhoneNumber;

    public EmployeeStatuses Status;
}