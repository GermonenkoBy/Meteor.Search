using Meteor.Search.Core.Models.Enums;

namespace Meteor.Search.Core.Dtos;

public record EmployeesFilter
{
    public int Offset { get; set; }

    public int Limit { get; set; }

    public string? Query { get; set; }

    public List<EmployeeStatuses> Statuses { get; set; } = new();

    public List<Order> Order { get; set; } = new();
}