using Meteor.Search.Core.Dtos.Enums;
using Meteor.Search.Core.Models.Enums;

namespace Meteor.Search.Core.Dtos;

public record EmployeesFilter
{
    public int Offset { get; set; }

    public int Limit { get; set; }

    public string? Query { get; set; }

    public EmployeeStatuses? Status { get; set; }

    public List<OrderDirections> Order { get; set; } = new();
}