using Meteor.Search.Core.Dtos.Enums;

namespace Meteor.Search.Core.Dtos;

public record Order
{
    public EmployeeOrderFields Field { get; set; }

    public OrderDirections Direction { get; set; }
}