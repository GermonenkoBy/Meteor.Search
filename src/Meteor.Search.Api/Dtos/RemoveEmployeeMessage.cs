namespace Meteor.Search.Api.Dtos;

public record RemoveEmployeeMessage : CustomerBasedMessage
{
    public int EmployeeId { get; set; }
}