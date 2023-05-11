using MapsterMapper;
using Meilisearch;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Dtos;
using Meteor.Search.Core.Models;

namespace Meteor.Search.Infrastructure.Contracts;

public class EmployeesRepository : IEmployeesRepository
{
    private const string EmployeeIndexName = "employees";

    private readonly MeilisearchClient _meilisearchClient;

    private readonly IMapper _mapper;

    public EmployeesRepository(MeilisearchClient meilisearchClient, IMapper mapper)
    {
        _meilisearchClient = meilisearchClient;
        _mapper = mapper;
    }

    public async Task<(IReadOnlyCollection<Employee> Items, int Total)> SearchAsync(EmployeesFilter filter)
    {
        var searchQuery = _mapper.Map<SearchQuery>(filter);

        var response = await _meilisearchClient.Index(EmployeeIndexName)
            .SearchAsync<Employee>(filter.Query ?? "", searchQuery);

        if (response is SearchResult<Employee> searchResult)
        {
            return (searchResult.Hits, searchResult.EstimatedTotalHits);
        }

        return (response.Hits, response.Hits.Count);
    }

    public Task SaveEmployeeAsync(Employee employee)
    {
        return _meilisearchClient
            .Index(EmployeeIndexName)
            .UpdateDocumentsAsync(new []{employee});
    }

    public Task RemoveEmployeeAsync(int employeeId)
    {
        return _meilisearchClient
            .Index(EmployeeIndexName)
            .DeleteDocumentsAsync(new[] { employeeId });
    }
}