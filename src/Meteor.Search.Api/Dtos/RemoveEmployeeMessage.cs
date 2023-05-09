namespace Meteor.Search.Api.Dtos;

public record RemoveEmployeeMessage
{
    public int EmployeeId;

    public int CustomerId;
}