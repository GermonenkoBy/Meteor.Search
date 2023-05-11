using EnumsNET;
using Mapster;
using Meilisearch;
using Meteor.Search.Core.Dtos;

namespace Meteor.Search.Infrastructure.Mapping;

public class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EmployeesFilter, SearchQuery>()
            .Map(
                query => query.Sort,
                query => query.Order.Select(order =>
                    $"{order.Field.AsString(EnumFormat.Description)}:" +
                    $"{order.Direction.AsString(EnumFormat.Description)}"
                )
            )
            .Map(
                query => query.Filter,
                filter => $"status IN [{string.Join(',', filter.Statuses.Select(s => s.ToString("D")))}]"
            )
            .IgnoreIf((filter, query) => filter.Statuses.Count == 0, query => query.Filter)
            .IgnoreIf((filter, query) => filter.Order.Count == 0, query => query.Sort);
    }
}